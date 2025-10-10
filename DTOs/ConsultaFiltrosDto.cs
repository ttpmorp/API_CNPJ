namespace CnpjApi.DTOs
{
    /// <summary>
    /// DTO que representa os filtros para consulta de CNPJ
    /// </summary>
    public class ConsultaFiltrosDto
    {
        /// <summary>
        /// Unidade da Federação (Estado)
        /// </summary>
        public string? UF { get; set; }
        
        /// <summary>
        /// Ano de referência
        /// </summary>
        public int Ano { get; set; }
        
        /// <summary>
        /// Forma de tributação
        /// </summary>
        public string? FormaTributacao { get; set; }
        
        /// <summary>
        /// Capital social mínimo
        /// </summary>
        public decimal CapitalSocialMinimo { get; set; }
    }
}