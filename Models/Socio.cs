using System;

namespace CnpjApi.Models
{
    public class Socio
    {
        public int ID_SOCIO { get; set; }
        public string CNPJ { get; set; }
        public string TIPO_SOCIO { get; set; }
        public string NOME { get; set; }
        public string CNPJ_CPF { get; set; }
        public string QUAL_SOCIO { get; set; }
        public string DESC_SOCIO { get; set; }
        public DateTime? DT_ENT_SOC { get; set; }
        public string PAIS { get; set; }
        public string REP_LEGAL { get; set; }
        public string NOME_REPR { get; set; }
        public string QUAL_REPR { get; set; }
        public string FAIXA_ETA_SOC { get; set; }
    }
}