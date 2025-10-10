using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CnpjApi.Data;
using CnpjApi.DTOs;
using CnpjApi.Models;
using CnpjApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CnpjApi.Services
{
    public interface ICNPJService
    {
        Task<CNPJResponseDto?> ConsultarCNPJ(string cnpj);
        Task<bool> CNPJExiste(string cnpj);
        Task<List<CNPJResponseDto>> ConsultarPorFiltros(ConsultaFiltrosDto filtros);
    }

    public class CNPJService : ICNPJService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CNPJService> _logger;

        public CNPJService(AppDbContext context, ILogger<CNPJService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CNPJResponseDto?> ConsultarCNPJ(string cnpj)
        {
            try
            {
                var cnpjLimpo = LimparCNPJ(cnpj);
                _logger.LogInformation("Consultando CNPJ: {Cnpj}", cnpjLimpo);

                // Buscar dados das várias tabelas
                var empresa = await _context.Empresas
                    .FirstOrDefaultAsync(e => e.CNPJ == cnpjLimpo);

                if (empresa == null)
                {
                    _logger.LogInformation("CNPJ {Cnpj} não encontrado", cnpjLimpo);
                    return null;
                }

                var estabelecimento = await _context.Estabelecimentos
                    .FirstOrDefaultAsync(e => e.CNPJ == cnpjLimpo);

                var simples = await _context.Simples
                    .FirstOrDefaultAsync(s => s.CNPJ == cnpjLimpo);

                var tributacao = await _context.Tributacoes
                    .FirstOrDefaultAsync(t => t.CNPJ == cnpjLimpo);

                var socios = await _context.Socios
                    .Where(s => s.CNPJ == cnpjLimpo)
                    .ToListAsync();

                return MapToDto(empresa, estabelecimento, simples, tributacao, socios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar CNPJ: {Cnpj}", cnpj);
                throw;
            }
        }

        private CNPJResponseDto MapToDto(Empresa empresa, Estabelecimento? estabelecimento,
            Simples? simples, Tributacao? tributacao, List<Socio> socios)
        {
            return new CNPJResponseDto
            {
                CNPJ = FormatarCNPJ(empresa.CNPJ ?? ""),
                RazaoSocial = empresa.RAZ_SOC,
                NomeFantasia = estabelecimento?.RAZ_SOC,
                NaturezaJuridica = empresa.NAT_JUR,
                CapitalSocial = empresa.CAP_SOCIAL,
                PorteEmpresa = empresa.PORTE_EMP,
                SituacaoCadastral = estabelecimento?.SIT_CAD,
                DataSituacaoCadastral = estabelecimento?.DT_SIT_CAD,
                MotivoSituacaoCadastral = estabelecimento?.MOT_SIT_CAD,
                DataInicioAtividade = estabelecimento?.DT_INI_ATIV,
                CNAEPrincipal = estabelecimento?.CNAE_PRINCIPAL,
                CNAESecundarios = !string.IsNullOrEmpty(estabelecimento?.CNAE_SECUNDARIO) ? estabelecimento.CNAE_SECUNDARIO.Split(',').ToList() : new List<string>(),
                Endereco = new EnderecoDto
                {
                    LogradouroCompleto = !string.IsNullOrEmpty(estabelecimento?.TIPO_LOGRADOURO) && !string.IsNullOrEmpty(estabelecimento?.LOGRADOURO) ? 
                        $"{estabelecimento.TIPO_LOGRADOURO} {estabelecimento.LOGRADOURO}" : null,
                    Numero = estabelecimento?.NUMERO,
                    Complemento = estabelecimento?.COMPLEMENTO,
                    Bairro = estabelecimento?.BAIRRO,
                    CEP = FormatarCEP(estabelecimento?.CEP ?? ""),
                    Municipio = estabelecimento?.MUNICIPIO,
                    UF = estabelecimento?.UF
                },
                Contato = new ContatoDto
                {
                    Telefone1 = FormatarTelefone(estabelecimento?.DDD_1 ?? "", estabelecimento?.TEL_1 ?? ""),
                    Telefone2 = FormatarTelefone(estabelecimento?.DDD_2 ?? "", estabelecimento?.TEL_2 ?? ""),
                    Fax = FormatarTelefone(estabelecimento?.DDD_FAX ?? "", estabelecimento?.FAX ?? ""),
                    Email = estabelecimento?.EMAIL
                },
                SimplesNacional = new SimplesNacionalDto
                {
                    OptanteSimples = simples?.SIMPLES,
                    DataOpcaoSimples = simples?.DT_OPC_SIMPLES,
                    DataExclusaoSimples = simples?.DT_EXC_SIMPLES,
                    OptanteMEI = simples?.MEI,
                    DataOpcaoMEI = simples?.DT_OPC_MEI,
                    DataExclusaoMEI = simples?.DT_EXC_MEI
                },
                Tributacao = new TributacaoDto
                {
                    Ano = tributacao?.ANO ?? 0,
                    FormaTributacao = tributacao?.FORMA_TRIBUTACAO,
                    QuantidadeEscrituracoes = tributacao?.QTD_ESCR ?? 0
                },
                Socios = socios.Select(s => new SocioDto
                {
                    Nome = s.NOME,
                    Tipo = s.TIPO_SOCIO,
                    Documento = s.CNPJ_CPF,
                    Qualificacao = s.QUAL_SOCIO,
                    DataEntrada = s.DT_ENT_SOC,
                    RepresentanteLegal = s.REP_LEGAL
                }).ToList()
            };
        }

        public async Task<bool> CNPJExiste(string cnpj)
        {
            try
            {
                var cnpjLimpo = LimparCNPJ(cnpj);
                return await _context.Empresas.AnyAsync(e => e.CNPJ == cnpjLimpo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar existência do CNPJ: {Cnpj}", cnpj);
                throw;
            }
        }

        public async Task<List<CNPJResponseDto>> ConsultarPorFiltros(ConsultaFiltrosDto filtros)
        {
            try
            {
                var query = from empresa in _context.Empresas
                            join estabelecimento in _context.Estabelecimentos on empresa.CNPJ equals estabelecimento.CNPJ
                            where (string.IsNullOrEmpty(filtros.UF) || estabelecimento.UF == filtros.UF)
                                  && (filtros.Ano == 0 || _context.Tributacoes.Any(t => t.CNPJ == empresa.CNPJ && t.ANO == filtros.Ano))
                                  && (string.IsNullOrEmpty(filtros.FormaTributacao) || _context.Tributacoes.Any(t => t.CNPJ == empresa.CNPJ && t.FORMA_TRIBUTACAO == filtros.FormaTributacao))
                                  && (filtros.CapitalSocialMinimo == 0 || empresa.CAP_SOCIAL >= filtros.CapitalSocialMinimo)
                            select new { empresa, estabelecimento };

                var resultados = await query.Take(100).ToListAsync();

                var responseList = new List<CNPJResponseDto>();
                foreach (var result in resultados)
                {
                    var simples = await _context.Simples.FirstOrDefaultAsync(s => s.CNPJ == result.empresa.CNPJ);
                    var tributacao = await _context.Tributacoes.FirstOrDefaultAsync(t => t.CNPJ == result.empresa.CNPJ && t.ANO == filtros.Ano);
                    var socios = await _context.Socios.Where(s => s.CNPJ == result.empresa.CNPJ).ToListAsync();

                    responseList.Add(MapToDto(result.empresa, result.estabelecimento, simples, tributacao, socios));
                }

                return responseList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar por filtros");
                throw;
            }
        }

        /// <summary>
        /// Remove todos os caracteres não numéricos do CNPJ
        /// </summary>
        /// <param name="cnpj">CNPJ a ser limpo</param>
        /// <returns>CNPJ contendo apenas dígitos</returns>
        private string LimparCNPJ(string cnpj)
        {
            return new string(cnpj.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Formata o CNPJ no padrão brasileiro: XX.XXX.XXX/XXXX-XX
        /// O CNPJ deve ter 14 dígitos: 8 para a raiz, 4 para a ordem e 2 para o dígito verificador
        /// </summary>
        /// <param name="cnpj">CNPJ a ser formatado</param>
        /// <returns>CNPJ formatado ou o valor original se inválido</returns>
        private string FormatarCNPJ(string cnpj)
        {
            // Verifica se o CNPJ tem o tamanho correto
            if (string.IsNullOrEmpty(cnpj) || cnpj.Length != 14)
                return cnpj;

            // Formata no padrão brasileiro: XX.XXX.XXX/XXXX-XX
            return $"{cnpj.Substring(0, 2)}.{cnpj.Substring(2, 3)}.{cnpj.Substring(5, 3)}/{cnpj.Substring(8, 4)}-{cnpj.Substring(12, 2)}";
        }

        /// <summary>
        /// Formata o CEP no padrão brasileiro: XXXXX-XXX
        /// </summary>
        /// <param name="cep">CEP a ser formatado</param>
        /// <returns>CEP formatado ou o valor original se inválido</returns>
        private string FormatarCEP(string cep)
        {
            // Verifica se o CEP tem o tamanho correto
            if (string.IsNullOrEmpty(cep) || cep.Length != 8)
                return cep;

            // Formata no padrão brasileiro: XXXXX-XXX
            return $"{cep.Substring(0, 5)}-{cep.Substring(5, 3)}";
        }

        /// <summary>
        /// Formata um número de telefone no padrão brasileiro: (XX) XXXXXXXX
        /// </summary>
        /// <param name="ddd">DDD do telefone</param>
        /// <param name="numero">Número do telefone</param>
        /// <returns>Telefone formatado ou string vazia se inválido</returns>
        private string FormatarTelefone(string ddd, string numero)
        {
            // Verifica se ambos os valores estão presentes
            if (string.IsNullOrEmpty(ddd) || string.IsNullOrEmpty(numero))
                return "";

            // Formata no padrão brasileiro: (XX) XXXXXXXX
            return $"({ddd}) {numero}";
        }
    }
}