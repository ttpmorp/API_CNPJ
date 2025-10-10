using System;
using System.Collections.Generic;

namespace CnpjApi.DTOs
{
    /// <summary>
    /// DTO que representa a resposta da consulta de CNPJ
    /// </summary>
    public class CNPJResponseDto
    {
        /// <summary>
        /// Número do CNPJ formatado
        /// </summary>
        public string? CNPJ { get; set; }
        
        /// <summary>
        /// Razão social da empresa
        /// </summary>
        public string? RazaoSocial { get; set; }
        
        /// <summary>
        /// Nome fantasia da empresa
        /// </summary>
        public string? NomeFantasia { get; set; }
        
        /// <summary>
        /// Código da natureza jurídica
        /// </summary>
        public string? NaturezaJuridica { get; set; }
        
        /// <summary>
        /// Capital social da empresa
        /// </summary>
        public decimal CapitalSocial { get; set; }
        
        /// <summary>
        /// Porte da empresa
        /// </summary>
        public string? PorteEmpresa { get; set; }
        
        /// <summary>
        /// Situação cadastral da empresa
        /// </summary>
        public string? SituacaoCadastral { get; set; }
        
        /// <summary>
        /// Data da situação cadastral
        /// </summary>
        public DateTime? DataSituacaoCadastral { get; set; }
        
        /// <summary>
        /// Motivo da situação cadastral
        /// </summary>
        public string? MotivoSituacaoCadastral { get; set; }
        
        /// <summary>
        /// Data de início da atividade
        /// </summary>
        public DateTime? DataInicioAtividade { get; set; }
        
        /// <summary>
        /// Código do CNAE principal
        /// </summary>
        public string? CNAEPrincipal { get; set; }
        
        /// <summary>
        /// Lista de CNAEs secundários
        /// </summary>
        public List<string>? CNAESecundarios { get; set; }
        
        /// <summary>
        /// Informações de endereço
        /// </summary>
        public EnderecoDto? Endereco { get; set; }
        
        /// <summary>
        /// Informações de contato
        /// </summary>
        public ContatoDto? Contato { get; set; }
        
        /// <summary>
        /// Informações do Simples Nacional
        /// </summary>
        public SimplesNacionalDto? SimplesNacional { get; set; }
        
        /// <summary>
        /// Informações de tributação
        /// </summary>
        public TributacaoDto? Tributacao { get; set; }
        
        /// <summary>
        /// Lista de sócios
        /// </summary>
        public List<SocioDto>? Socios { get; set; }
    }

    /// <summary>
    /// DTO que representa informações de endereço
    /// </summary>
    public class EnderecoDto
    {
        /// <summary>
        /// Logradouro completo (tipo + nome)
        /// </summary>
        public string? LogradouroCompleto { get; set; }
        
        /// <summary>
        /// Número do endereço
        /// </summary>
        public string? Numero { get; set; }
        
        /// <summary>
        /// Complemento do endereço
        /// </summary>
        public string? Complemento { get; set; }
        
        /// <summary>
        /// Bairro
        /// </summary>
        public string? Bairro { get; set; }
        
        /// <summary>
        /// CEP formatado
        /// </summary>
        public string? CEP { get; set; }
        
        /// <summary>
        /// Município
        /// </summary>
        public string? Municipio { get; set; }
        
        /// <summary>
        /// Unidade da Federação (Estado)
        /// </summary>
        public string? UF { get; set; }
    }

    /// <summary>
    /// DTO que representa informações de contato
    /// </summary>
    public class ContatoDto
    {
        /// <summary>
        /// Primeiro telefone
        /// </summary>
        public string? Telefone1 { get; set; }
        
        /// <summary>
        /// Segundo telefone
        /// </summary>
        public string? Telefone2 { get; set; }
        
        /// <summary>
        /// Número do fax
        /// </summary>
        public string? Fax { get; set; }
        
        /// <summary>
        /// Endereço de e-mail
        /// </summary>
        public string? Email { get; set; }
    }

    /// <summary>
    /// DTO que representa informações do Simples Nacional
    /// </summary>
    public class SimplesNacionalDto
    {
        /// <summary>
        /// Indicador se é optante do Simples Nacional
        /// </summary>
        public string? OptanteSimples { get; set; }
        
        /// <summary>
        /// Data de opção pelo Simples Nacional
        /// </summary>
        public DateTime? DataOpcaoSimples { get; set; }
        
        /// <summary>
        /// Data de exclusão do Simples Nacional
        /// </summary>
        public DateTime? DataExclusaoSimples { get; set; }
        
        /// <summary>
        /// Indicador se é optante do MEI
        /// </summary>
        public string? OptanteMEI { get; set; }
        
        /// <summary>
        /// Data de opção pelo MEI
        /// </summary>
        public DateTime? DataOpcaoMEI { get; set; }
        
        /// <summary>
        /// Data de exclusão do MEI
        /// </summary>
        public DateTime? DataExclusaoMEI { get; set; }
    }

    /// <summary>
    /// DTO que representa informações de tributação
    /// </summary>
    public class TributacaoDto
    {
        /// <summary>
        /// Ano de referência
        /// </summary>
        public int Ano { get; set; }
        
        /// <summary>
        /// Forma de tributação
        /// </summary>
        public string? FormaTributacao { get; set; }
        
        /// <summary>
        /// Quantidade de escriturações
        /// </summary>
        public int QuantidadeEscrituracoes { get; set; }
    }

    /// <summary>
    /// DTO que representa informações de um sócio
    /// </summary>
    public class SocioDto
    {
        /// <summary>
        /// Nome do sócio
        /// </summary>
        public string? Nome { get; set; }
        
        /// <summary>
        /// Tipo do sócio
        /// </summary>
        public string? Tipo { get; set; }
        
        /// <summary>
        /// Documento do sócio (CPF ou CNPJ)
        /// </summary>
        public string? Documento { get; set; }
        
        /// <summary>
        /// Qualificação do sócio
        /// </summary>
        public string? Qualificacao { get; set; }
        
        /// <summary>
        /// Data de entrada na sociedade
        /// </summary>
        public DateTime? DataEntrada { get; set; }
        
        /// <summary>
        /// Representante legal (se aplicável)
        /// </summary>
        public string? RepresentanteLegal { get; set; }
    }
}