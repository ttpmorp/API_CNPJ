using System;

namespace CnpjApi.Models
{
    /// <summary>
    /// Representa um sócio ou responsável por uma empresa
    /// </summary>
    public class Socio
    {
        /// <summary>
        /// Identificador único do sócio
        /// </summary>
        public int ID_SOCIO { get; set; }
        
        /// <summary>
        /// Número do CNPJ da empresa
        /// </summary>
        public string? CNPJ { get; set; }
        
        /// <summary>
        /// Nome do sócio
        /// </summary>
        public string? NOME { get; set; }
        
        /// <summary>
        /// Tipo de sócio
        /// </summary>
        public string? TIPO_SOCIO { get; set; }
        
        /// <summary>
        /// CPF ou CNPJ do sócio
        /// </summary>
        public string? CNPJ_CPF { get; set; }
        
        /// <summary>
        /// Qualificação do sócio
        /// </summary>
        public string? QUAL_SOCIO { get; set; }
        
        /// <summary>
        /// Data de entrada na sociedade
        /// </summary>
        public DateTime? DT_ENT_SOC { get; set; }
        
        /// <summary>
        /// País de origem (para sócio estrangeiro)
        /// </summary>
        public string? PAIS { get; set; }
        
        /// <summary>
        /// Representante legal (se aplicável)
        /// </summary>
        public string? REP_LEGAL { get; set; }
        
        /// <summary>
        /// Nome do representante legal
        /// </summary>
        public string? NOME_REPR { get; set; }
        
        /// <summary>
        /// Qualificação do representante legal
        /// </summary>
        public string? QUAL_REPR { get; set; }
        
        /// <summary>
        /// Faixa etária do sócio
        /// </summary>
        public string? FAIXA_ETA_SOC { get; set; }
    }
}