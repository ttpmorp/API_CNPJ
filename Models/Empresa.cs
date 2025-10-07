using System;
using System.ComponentModel.DataAnnotations;

namespace CnpjApi.Models
{
    public class Empresa
    {
        [Key]
        public string? CNPJ { get; set; }
        public string? RAZ_SOC { get; set; }
        public string? NAT_JUR { get; set; }
        public string? QUAL_RESP { get; set; }
        public decimal CAP_SOCIAL { get; set; }
        public string? PORTE_EMP { get; set; }
        public string? ENT_FED_RESP { get; set; }
    }
}