using System;
using System.ComponentModel.DataAnnotations;

namespace CnpjApi.Models
{
    /// <summary>
    /// Representa uma empresa cadastrada no sistema
    /// </summary>
    public class Empresa
    {
        /// <summary>
        /// Número do CNPJ (14 dígitos: 8 para raiz, 4 para ordem, 2 para dígito verificador)
        /// </summary>
        [Key]
        public string? CNPJ { get; set; }
        
        /// <summary>
        /// Razão social da empresa
        /// </summary>
        public string? RAZ_SOC { get; set; }
        
        /// <summary>
        /// Código da natureza jurídica
        /// </summary>
        public string? NAT_JUR { get; set; }
        
        /// <summary>
        /// Qualificação do responsável
        /// </summary>
        public string? QUAL_RESP { get; set; }
        
        /// <summary>
        /// Capital social da empresa
        /// </summary>
        public decimal CAP_SOCIAL { get; set; }
        
        /// <summary>
        /// Porte da empresa
        /// </summary>
        public string? PORTE_EMP { get; set; }
        
        /// <summary>
        /// Entidade federativa responsável
        /// </summary>
        public string? ENT_FED_RESP { get; set; }
    }
}