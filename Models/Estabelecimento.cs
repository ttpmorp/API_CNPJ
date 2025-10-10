using System;

namespace CnpjApi.Models
{
    /// <summary>
    /// Representa um estabelecimento (matriz ou filial) de uma empresa
    /// </summary>
    public class Estabelecimento
    {
        /// <summary>
        /// Número do CNPJ (14 dígitos: 8 para raiz, 4 para ordem, 2 para dígito verificador)
        /// </summary>
        public string? CNPJ { get; set; }
        
        /// <summary>
        /// Número da ordem do estabelecimento (0001 para matriz, 0002+ para filiais)
        /// </summary>
        public int ORDEM { get; set; }
        
        /// <summary>
        /// Dígito verificador
        /// </summary>
        public string? DV { get; set; }
        
        /// <summary>
        /// Matriz ou filial
        /// </summary>
        public string? MAT_FIL { get; set; }
        
        /// <summary>
        /// Razão social do estabelecimento
        /// </summary>
        public string? RAZ_SOC { get; set; }
        
        /// <summary>
        /// Situação cadastral
        /// </summary>
        public string? SIT_CAD { get; set; }
        
        /// <summary>
        /// Data da situação cadastral
        /// </summary>
        public DateTime? DT_SIT_CAD { get; set; }
        
        /// <summary>
        /// Motivo da situação cadastral
        /// </summary>
        public string? MOT_SIT_CAD { get; set; }
        
        /// <summary>
        /// Nome da cidade no exterior (se aplicável)
        /// </summary>
        public string? NOME_CIDADE_EXT { get; set; }
        
        /// <summary>
        /// Código do país
        /// </summary>
        public string? COD_PAIS { get; set; }
        
        /// <summary>
        /// Data de início da atividade
        /// </summary>
        public DateTime? DT_INI_ATIV { get; set; }
        
        /// <summary>
        /// Código do CNAE principal
        /// </summary>
        public string? CNAE_PRINCIPAL { get; set; }
        
        /// <summary>
        /// Códigos dos CNAEs secundários
        /// </summary>
        public string? CNAE_SECUNDARIO { get; set; }
        
        /// <summary>
        /// Tipo de logradouro
        /// </summary>
        public string? TIPO_LOGRADOURO { get; set; }
        
        /// <summary>
        /// Nome do logradouro
        /// </summary>
        public string? LOGRADOURO { get; set; }
        
        /// <summary>
        /// Número do endereço
        /// </summary>
        public string? NUMERO { get; set; }
        
        /// <summary>
        /// Complemento do endereço
        /// </summary>
        public string? COMPLEMENTO { get; set; }
        
        /// <summary>
        /// Bairro
        /// </summary>
        public string? BAIRRO { get; set; }
        
        /// <summary>
        /// CEP
        /// </summary>
        public string? CEP { get; set; }
        
        /// <summary>
        /// Unidade da Federação (Estado)
        /// </summary>
        public string? UF { get; set; }
        
        /// <summary>
        /// Código do município
        /// </summary>
        public string? COD_MUN { get; set; }
        
        /// <summary>
        /// Nome do município
        /// </summary>
        public string? MUNICIPIO { get; set; }
        
        /// <summary>
        /// DDD do primeiro telefone
        /// </summary>
        public string? DDD_1 { get; set; }
        
        /// <summary>
        /// Primeiro telefone
        /// </summary>
        public string? TEL_1 { get; set; }
        
        /// <summary>
        /// DDD do segundo telefone
        /// </summary>
        public string? DDD_2 { get; set; }
        
        /// <summary>
        /// Segundo telefone
        /// </summary>
        public string? TEL_2 { get; set; }
        
        /// <summary>
        /// DDD do fax
        /// </summary>
        public string? DDD_FAX { get; set; }
        
        /// <summary>
        /// Número do fax
        /// </summary>
        public string? FAX { get; set; }
        
        /// <summary>
        /// Endereço de e-mail
        /// </summary>
        public string? EMAIL { get; set; }
        
        /// <summary>
        /// Situação especial
        /// </summary>
        public string? SIT_ESPECIAL { get; set; }
        
        /// <summary>
        /// Data da situação especial
        /// </summary>
        public DateTime? DT_SIT_ESPEC { get; set; }
    }
}