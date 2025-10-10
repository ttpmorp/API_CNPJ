using System;

namespace CnpjApi.Models
{
    /// <summary>
    /// Representa informações sobre a adesão ao Simples Nacional
    /// </summary>
    public class Simples
    {
        /// <summary>
        /// Número do CNPJ (14 dígitos: 8 para raiz, 4 para ordem, 2 para dígito verificador)
        /// </summary>
        public string? CNPJ { get; set; }
        
        /// <summary>
        /// Indicador se é optante do Simples Nacional
        /// </summary>
        public string? SIMPLES { get; set; }
        
        /// <summary>
        /// Data de opção pelo Simples Nacional
        /// </summary>
        public DateTime? DT_OPC_SIMPLES { get; set; }
        
        /// <summary>
        /// Data de exclusão do Simples Nacional
        /// </summary>
        public DateTime? DT_EXC_SIMPLES { get; set; }
        
        /// <summary>
        /// Indicador se é optante do MEI
        /// </summary>
        public string? MEI { get; set; }
        
        /// <summary>
        /// Data de opção pelo MEI
        /// </summary>
        public DateTime? DT_OPC_MEI { get; set; }
        
        /// <summary>
        /// Data de exclusão do MEI
        /// </summary>
        public DateTime? DT_EXC_MEI { get; set; }
    }
}