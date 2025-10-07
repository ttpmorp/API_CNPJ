using System;

namespace CnpjApi.Models
{
    public class Simples
    {
        public string? CNPJ { get; set; }
        public string? SIMPLES { get; set; }
        public DateTime? DT_OPC_SIMPLES { get; set; }
        public DateTime? DT_EXC_SIMPLES { get; set; }
        public string? MEI { get; set; }
        public DateTime? DT_OPC_MEI { get; set; }
        public DateTime? DT_EXC_MEI { get; set; }
    }
}