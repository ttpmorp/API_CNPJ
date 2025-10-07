using System;
using System.Collections.Generic;

namespace CnpjApi.DTOs
{
    public class CNPJResponseDto
    {
        public string? CNPJ { get; set; }
        public string? RazaoSocial { get; set; }
        public string? NomeFantasia { get; set; }
        public string? NaturezaJuridica { get; set; }
        public string? QualificacaoResponsavel { get; set; }
        public string? Responsavel { get; set; }
        public decimal CapitalSocial { get; set; }
        public string? PorteEmpresa { get; set; }
        public string? SituacaoCadastral { get; set; }
        public DateTime? DataSituacaoCadastral { get; set; }
        public string? MotivoSituacaoCadastral { get; set; }
        public DateTime? DataInicioAtividade { get; set; }
        public string? CNAEPrincipal { get; set; }
        public string? DescricaoCNAE { get; set; }
        public List<string>? CNAESecundarios { get; set; }
        public EnderecoDto? Endereco { get; set; }
        public ContatoDto? Contato { get; set; }
        public SimplesNacionalDto? SimplesNacional { get; set; }
        public TributacaoDto? Tributacao { get; set; }
        public List<SocioDto>? Socios { get; set; }
    }

    public class EnderecoDto
    {
        public string? LogradouroCompleto { get; set; }
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Bairro { get; set; }
        public string? CEP { get; set; }
        public string? Municipio { get; set; }
        public string? UF { get; set; }
    }

    public class ContatoDto
    {
        public string? Telefone1 { get; set; }
        public string? Telefone2 { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
    }

    public class SimplesNacionalDto
    {
        public string? OptanteSimples { get; set; }
        public DateTime? DataOpcaoSimples { get; set; }
        public DateTime? DataExclusaoSimples { get; set; }
        public string? OptanteMEI { get; set; }
        public DateTime? DataOpcaoMEI { get; set; }
        public DateTime? DataExclusaoMEI { get; set; }
    }

    public class TributacaoDto
    {
        public int Ano { get; set; }
        public string? FormaTributacao { get; set; }
        public int QuantidadeEscrituracoes { get; set; }
    }

    public class SocioDto
    {
        public string? Nome { get; set; }
        public string? Tipo { get; set; }
        public string? Documento { get; set; }
        public string? Qualificacao { get; set; }
        public DateTime? DataEntrada { get; set; }
        public string? RepresentanteLegal { get; set; }
    }
}