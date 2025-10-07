using CnpjApi.Data;
using CnpjApi.DTOSs;
using CnpjApi.Models;
using CnpjApi.Services;
using Microsoft.EntityFrameworkCore;

namespace CnpjApi.Services
{
    public interface ICNPJService
    {
        Task<CNPJResponseDto> ConsultarCNPJ(string cnpj);
        Task<bool> CNPJExiste(string cnpj);
        Task<List<CNPJResponseDto>> ConsultarPorFiltros(ConsultaFiltrosDto filtros);
    }

    public class CNPJService : ICNPJService
    {
        private readonly AppDbContext _context;

        public CNPJService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CNPJResponseDto> ConsultarCNPJ(string cnpj)
        {
            var cnpjLimpo = LimparCNPJ(cnpj);

            // Buscar dados das várias tabelas
            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(e => e.CNPJ == cnpjLimpo);

            if (empresa == null)
                return null;

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

        private CNPJResponseDto MapToDto(Empresa empresa, Estabelecimento estabelecimento,
            Simples simples, Tributacao tributacao, List<Socio> socios)
        {
            return new CNPJResponseDto
            {
                CNPJ = FormatCNPJ(empresa.CNPJ),
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
                CNAESecundarios = estabelecimento?.CNAE_SECUNDARIO?.Split(',').ToList(),
                Endereco = new EnderecoDto
                {
                    LogradouroCompleto = $"{estabelecimento?.TIPO_LOGRADOURO} {estabelecimento?.LOGRADOURO}",
                    Numero = estabelecimento?.NUMERO,
                    Complemento = estabelecimento?.COMPLEMENTO,
                    Bairro = estabelecimento?.BAIRRO,
                    CEP = FormatCEP(estabelecimento?.CEP),
                    Municipio = estabelecimento?.MUNICIPIO,
                    UF = estabelecimento?.UF
                },
                Contato = new ContatoDto
                {
                    Telefone1 = FormatTelefone(estabelecimento?.DDD_1, estabelecimento?.TEL_1),
                    Telefone2 = FormatTelefone(estabelecimento?.DDD_2, estabelecimento?.TEL_2),
                    Fax = FormatTelefone(estabelecimento?.DDD_FAX, estabelecimento?.FAX),
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
            var cnpjLimpo = LimparCNPJ(cnpj);
            return await _context.Empresas.AnyAsync(e => e.CNPJ == cnpjLimpo);
        }

        public async Task<List<CNPJResponseDto>> ConsultarPorFiltros(ConsultaFiltrosDto filtros)
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

        private string LimparCNPJ(string cnpj)
        {
            return new string(cnpj.Where(char.IsDigit).ToArray());
        }

        private string FormatCNPJ(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj) || cnpj.Length != 14)
                return cnpj;

            return $"{cnpj.Substring(0, 2)}.{cnpj.Substring(2, 3)}.{cnpj.Substring(5, 3)}/{cnpj.Substring(8, 4)}-{cnpj.Substring(12, 2)}";
        }

        private string FormatCEP(string cep)
        {
            if (string.IsNullOrEmpty(cep) || cep.Length != 8)
                return cep;

            return $"{cep.Substring(0, 5)}-{cep.Substring(5, 3)}";
        }

        private string FormatTelefone(string ddd, string numero)
        {
            if (string.IsNullOrEmpty(ddd) || string.IsNullOrEmpty(numero))
                return null;

            return $"({ddd}) {numero}";
        }
    }

    public class ConsultaFiltrosDto
    {
        public string UF { get; set; }
        public int Ano { get; set; }
        public string FormaTributacao { get; set; }
        public decimal CapitalSocialMinimo { get; set; }
        public DateTime? DataInicioAtividadeInicio { get; set; }
        public DateTime? DataInicioAtividadeFim { get; set; }
    }
}
