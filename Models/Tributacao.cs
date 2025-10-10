using System;

namespace CnpjApi.Models
{
    /// <summary>
    /// Representa informações de tributação de uma empresa
    /// </summary>
    public class Tributacao
    {
        /// <summary>
        /// Número do CNPJ (14 dígitos: 8 para raiz, 4 para ordem, 2 para dígito verificador)
        /// </summary>
        public string? CNPJ { get; set; }
        
        /// <summary>
        /// Ano de referência
        /// </summary>
        public int ANO { get; set; }
        
        /// <summary>
        /// CNPJ da SCP (Sociedade em Conta de Participação)
        /// </summary>
        public string? CNPJ_SCP { get; set; }
        
        /// <summary>
        /// Forma de tributação
        /// </summary>
        public string? FORMA_TRIBUTACAO { get; set; }
        
        /// <summary>
        /// Quantidade de escriturações
        /// </summary>
        public int QTD_ESCR { get; set; }
    }
}