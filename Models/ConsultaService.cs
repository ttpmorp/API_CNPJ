using Cnpj_Consulta.Services.Interfaces;
using Cnpj_Consulta.Models;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data;
using System.Text.Json;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using StackExchange.Redis;

namespace Cnpj_Consulta.Services
{
    public class ConsultaService : IConsultaService
    {
        private readonly string _connectionString;
        private readonly ILogger<ConsultaService> _logger;
        private readonly string _cacheDirectory;
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _redisDb;

        public ConsultaService(ILogger<ConsultaService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("ConsultaCnpjConnection");
            _cacheDirectory = Path.Combine(Directory.GetCurrentDirectory(), "cache");
            
            // Create cache directory if it doesn't exist
            if (!Directory.Exists(_cacheDirectory))
            {
                Directory.CreateDirectory(_cacheDirectory);
            }
            
            // Try to connect to Redis (if available)
            try
            {
                _redis = ConnectionMultiplexer.Connect("localhost");
                _redisDb = _redis.GetDatabase();
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Could not connect to Redis. Using file-based caching instead. Error: " + ex.Message);
                _redis = null;
                _redisDb = null;
            }
        }

        public async Task<CnpjResult> ConsultarCnpjAsync(string cnpj)
        {
            try
            {
                cnpj = LimparCnpj(cnpj);
                
                // Try to get from cache first
                var cachedResult = await GetFromCacheAsync(cnpj);
                if (cachedResult != null)
                {
                    _logger.LogInformation($"Retrieved CNPJ {cnpj} from cache");
                    return cachedResult;
                }
                
                // Remove any extra characters and ensure we have just the base CNPJ
                if (cnpj.Length > 8)
                {
                    cnpj = cnpj.Substring(0, 8); // Use only the root CNPJ (first 8 digits)
                }
                
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                
                // SQL query based on the one you provided, corrected to use actual column names
                var sql = @"
                    SELECT 
                        A.CNPJ,B.ORDEM,B.DV,A.RAZ_SOC,A.NAT_JUR,I.DESCRICAO NATUREZA,A.QUAL_RESP,C.DESCRICAO RESPONSAVEL,A.CAP_SOCIAL,A.PORTE_EMP,D.DESCRICAO PORTE,A.ENT_FED_RESP,
                        B.MAT_FIL,E.DESCRICAO DESCRICAO,B.RAZ_SOC NOME_FANTASIA,B.SIT_CAD,F.DESCRICAO SITUACAO,B.DT_SIT_CAD,B.MOT_SIT_CAD,G.DESCRICAO MOTIVO,B.NOME_CIDADE_EXT,B.COD_PAIS,J.DESCRICAO PAIS,B.DT_INI_ATIV,B.CNAE_PRINCIPAL,H.DESCRICAO CNAE,
                        B.CNAE_SECUNDARIO,B.TIPO_LOGRADOURO,B.LOGRADOURO,B.NUMERO,B.COMPLEMENTO,B.BAIRRO,B.CEP,B.UF,B.COD_MUN,K.DESCRICAO MUNICIPIO,B.DDD_1,B.TEL_1,B.DDD_2,B.TEL_2,B.DDD_FAX,B.FAX,B.EMAIL,B.SIT_ESPECIAL,B.DT_SIT_ESPEC,
                        L.SIMPLES,L.DT_OPC_SIMPLES,L.DT_EXC_SIMPLES,L.MEI,L.DT_OPC_MEI,L.DT_EXC_MEI,
                        M.ANO,M.CNPJ CNPJ_TRIB,M.CNPJ_SCP,M.FORMA_TRIBUTACAO,M.QTD_ESCR,
                        N.ID_SOCIO,O.DESCRICAO TIPO_SOCIO,N.NOME,N.CNPJ_CPF,N.QUAL_SOCIO,P.DESCRICAO DESC_SOCIO,N.DT_ENT_SOC,N.PAIS,N.REP_LEGAL,N.NOME_REPR,N.QUAL_REPR,N.FAIXA_ETA_SOC
                    FROM EMPRESAS A WITH(NOLOCK) 
                    LEFT JOIN ESTABELECIMENTOS B WITH(NOLOCK) ON A.CNPJ=B.CNPJ
                    LEFT JOIN QUALIFICACOES    C WITH(NOLOCK) ON A.QUAL_RESP=C.CODIGO
                    LEFT JOIN PORTE            D WITH(NOLOCK) ON A.PORTE_EMP=D.CODIGO
                    LEFT JOIN MAT_FIL          E WITH(NOLOCK) ON B.MAT_FIL=E.CODIGO
                    LEFT JOIN SITUACAO         F WITH(NOLOCK) ON B.SIT_CAD=F.CODIGO
                    LEFT JOIN MOTIVOS          G WITH(NOLOCK) ON B.MOT_SIT_CAD=G.CODIGO
                    LEFT JOIN CNAES            H WITH(NOLOCK) ON B.CNAE_PRINCIPAL=H.CODIGO
                    LEFT JOIN NATUREZAS        I WITH(NOLOCK) ON A.NAT_JUR=I.CODIGO
                    LEFT JOIN PAISES           J WITH(NOLOCK) ON B.COD_PAIS=J.CODIGO
                    LEFT JOIN MUNICIPIOS       K WITH(NOLOCK) ON B.COD_MUN=K.CODIGO
                    LEFT JOIN SIMPLES          L WITH(NOLOCK) ON A.CNPJ=L.CNPJ
                    LEFT JOIN TRIBUTACAO       M WITH(NOLOCK) ON B.CNPJ+B.ORDEM+B.DV=M.CNPJ
                    LEFT JOIN SOCIOS           N WITH(NOLOCK) ON A.CNPJ=N.CNPJ
                    LEFT JOIN TIP_SOCIO        O WITH(NOLOCK) ON N.ID_SOCIO=O.CODIGO
                    LEFT JOIN QUALIFICACOES    P WITH(NOLOCK) ON N.QUAL_SOCIO=P.CODIGO
                    WHERE A.CNPJ LIKE @Cnpj + '%'";

                using var command = new SqlCommand(sql, connection);
                command.CommandTimeout = 900;
                command.Parameters.AddWithValue("@Cnpj", cnpj);
                
                using var reader = await command.ExecuteReaderAsync();
                
                CnpjResult resultado = null;
                var socios = new List<Socio>();
                
                while (await reader.ReadAsync())
                {
                    // Create the result object only once
                    if (resultado == null)
                    {
                        resultado = new CnpjResult
                        {
                            // DADOS BASICOS
                            CNPJ = reader["CNPJ"].ToString(),
                            ORDEM = reader["ORDEM"].ToString(),
                            DV = reader["DV"].ToString(),
                            RAZ_SOC = reader["RAZ_SOC"].ToString(),
                            NOME_FANTASIA = reader["NOME_FANTASIA"].ToString(),
                            NAT_JUR = reader["NAT_JUR"].ToString(),
                            NATUREZA = reader["NATUREZA"].ToString(),
                            QUAL_RESP = reader["QUAL_RESP"].ToString(),
                            RESPONSAVEL = reader["RESPONSAVEL"].ToString(),
                            CAP_SOCIAL = reader["CAP_SOCIAL"].ToString(),
                            PORTE_EMP = reader["PORTE_EMP"].ToString(),
                            PORTE = reader["PORTE"].ToString(),
                            ENT_FED_RESP = reader["ENT_FED_RESP"].ToString(),

                            // SITUAÇÃO CADASTRAL
                            SIT_CAD = reader["SIT_CAD"].ToString(),
                            SITUACAO = reader["SITUACAO"].ToString(),
                            DT_SIT_CAD = reader["DT_SIT_CAD"].ToString(),
                            MOT_SIT_CAD = reader["MOT_SIT_CAD"].ToString(),
                            MOTIVO = reader["MOTIVO"].ToString(),
                            NOME_CIDADE_EXT = reader["NOME_CIDADE_EXT"].ToString(),
                            COD_PAIS = reader["COD_PAIS"].ToString(),
                            PAIS = reader["PAIS"].ToString(),
                            DT_INI_ATIV = reader["DT_INI_ATIV"].ToString(),

                            // ATIVIDADES ECONÔMICAS
                            CNAE_PRINCIPAL = reader["CNAE_PRINCIPAL"].ToString(),
                            CNAE = reader["CNAE"].ToString(),
                            MAT_FIL = reader["MAT_FIL"].ToString(),
                            DESCRICAO = reader["DESCRICAO"].ToString(),

                            // ENDEREÇO 
                            TIPO_LOGRADOURO = reader["TIPO_LOGRADOURO"].ToString(),
                            LOGRADOURO = reader["LOGRADOURO"].ToString(),
                            NUMERO = reader["NUMERO"].ToString(),
                            COMPLEMENTO = reader["COMPLEMENTO"].ToString(),
                            BAIRRO = reader["BAIRRO"].ToString(),
                            CEP = reader["CEP"].ToString(),
                            UF = reader["UF"].ToString(),
                            COD_MUN = reader["COD_MUN"].ToString(),
                            MUNICIPIO = reader["MUNICIPIO"].ToString(),

                            // CONTATO
                            DDD_1 = reader["DDD_1"].ToString(),
                            TEL_1 = reader["TEL_1"].ToString(),
                            DDD_2 = reader["DDD_2"].ToString(),
                            TEL_2 = reader["TEL_2"].ToString(),
                            DDD_FAX = reader["DDD_FAX"].ToString(),
                            FAX = reader["FAX"].ToString(),
                            EMAIL = reader["EMAIL"].ToString(),

                            // SITUAÇÃO ESPECIAL
                            SIT_ESPECIAL = reader["SIT_ESPECIAL"].ToString(),
                            DT_SIT_ESPEC = reader["DT_SIT_ESPEC"].ToString(),

                            // REGIMES TRIBUTARIOS
                            SIMPLES = reader["SIMPLES"].ToString(),
                            DT_OPC_SIMPLES = reader["DT_OPC_SIMPLES"].ToString(),
                            DT_EXC_SIMPLES = reader["DT_EXC_SIMPLES"].ToString(),
                            MEI = reader["MEI"].ToString(),
                            DT_OPC_MEI = reader["DT_OPC_MEI"].ToString(),
                            DT_EXC_MEI = reader["DT_EXC_MEI"].ToString(),

                            // DADOS COMPLEMENTARES
                            ANO = reader["ANO"].ToString(),
                            CNPJ_SCP = reader["CNPJ_SCP"].ToString(),
                            FORMA_TRIBUTACAO = reader["FORMA_TRIBUTACAO"].ToString(),
                            QTD_ESCR = reader["QTD_ESCR"].ToString(),
                            
                            // Initialize empty lists
                            CNAE_SECUNDARIO = new List<string>(),
                            SOCIOS = new List<Socio>()
                        };
                        
                        // Handle CNAE_SECUNDARIO if it exists
                        if (!reader.IsDBNull("CNAE_SECUNDARIO"))
                        {
                            var cnaeSecundario = reader["CNAE_SECUNDARIO"].ToString();
                            if (!string.IsNullOrEmpty(cnaeSecundario))
                            {
                                resultado.CNAE_SECUNDARIO = new List<string> { cnaeSecundario };
                            }
                        }
                    }
                    
                    // Collect all partners/owners
                    if (!reader.IsDBNull("ID_SOCIO"))
                    {
                        socios.Add(new Socio
                        {
                            ID_SOCIO = reader["ID_SOCIO"].ToString(),
                            TIPO_SOCIO = reader["TIPO_SOCIO"].ToString(),
                            DESCRICAO_TIPO_SOCIO = reader["TIPO_SOCIO"].ToString(),
                            NOME = reader["NOME"].ToString(),
                            CNPJ_CPF = reader["CNPJ_CPF"].ToString(),
                            QUAL_SOCIO = reader["QUAL_SOCIO"].ToString(),
                            DESC_SOCIO = reader["DESC_SOCIO"].ToString(),
                            DT_ENT_SOC = reader["DT_ENT_SOC"].ToString(),
                            PAIS = reader["PAIS"].ToString(),
                            DESCRICAO_PAIS = reader["PAIS"].ToString(),
                            REP_LEGAL = reader["REP_LEGAL"].ToString(),
                            NOME_REPR = reader["NOME_REPR"].ToString(),
                            QUAL_REPR = reader["QUAL_REPR"].ToString(),
                            DESCRICAO_QUAL_REPR = reader["QUAL_REPR"].ToString(),
                            FAIXA_ETA_SOC = reader["FAIXA_ETA_SOC"].ToString()
                        });
                    }
                }
                
                if (resultado == null)
                {
                    throw new Exception("CNPJ não encontrado na base de dados");
                }
                
                // Assign partners to the result
                resultado.SOCIOS = socios;
                
                // Cache the result
                await SaveToCacheAsync(cnpj, resultado);
                
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao consultar CNPJ {cnpj}");
                throw;
            }
        }

        private async Task<CnpjResult> GetFromCacheAsync(string cnpj)
        {
            try
            {
                // Try Redis first if available
                if (_redisDb != null)
                {
                    var cachedData = await _redisDb.StringGetAsync($"cnpj:{cnpj}");
                    if (!cachedData.IsNullOrEmpty)
                    {
                        return JsonSerializer.Deserialize<CnpjResult>(cachedData);
                    }
                }
                
                // Fallback to file-based cache
                var fileName = GetCacheFileName(cnpj);
                var filePath = Path.Combine(_cacheDirectory, fileName);
                
                if (File.Exists(filePath))
                {
                    var json = await File.ReadAllTextAsync(filePath);
                    return JsonSerializer.Deserialize<CnpjResult>(json);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error reading from cache for CNPJ {cnpj}");
            }
            
            return null;
        }

        private async Task SaveToCacheAsync(string cnpj, CnpjResult result)
        {
            try
            {
                var json = JsonSerializer.Serialize(result);
                
                // Try Redis first if available
                if (_redisDb != null)
                {
                    await _redisDb.StringSetAsync($"cnpj:{cnpj}", json, TimeSpan.FromHours(24));
                    return;
                }
                
                // Fallback to file-based cache
                var fileName = GetCacheFileName(cnpj);
                var filePath = Path.Combine(_cacheDirectory, fileName);
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error saving to cache for CNPJ {cnpj}");
            }
        }

        private string GetCacheFileName(string cnpj)
        {
            using var sha256 = SHA256.Create();
            var hashed = sha256.ComputeHash(Encoding.UTF8.GetBytes(cnpj));
            return BitConverter.ToString(hashed).Replace("-", "").ToLower() + ".json";
        }

        private string LimparCnpj(string cnpj)
        {
            return string.IsNullOrEmpty(cnpj)
                ? throw new ArgumentNullException(nameof(cnpj))
                : new string(cnpj.Where(char.IsDigit).ToArray());
        }
    }
}