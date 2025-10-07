using System;

namespace CnpjApi.DTOs
{
    public class ConsultaFiltrosDto
    {
        public string? UF { get; set; }
        public int Ano { get; set; }
        public string? FormaTributacao { get; set; }
        public decimal CapitalSocialMinimo { get; set; }
        public DateTime? DataInicioAtividadeInicio { get; set; }
        public DateTime? DataInicioAtividadeFim { get; set; }
    }
}