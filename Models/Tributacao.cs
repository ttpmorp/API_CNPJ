namespace CnpjApi.Models
{
    public class Tributacao
    {
        public int ANO { get; set; }
        public string CNPJ { get; set; }
        public string CNPJ_SCP { get; set; }
        public string FORMA_TRIBUTACAO { get; set; }
        public int QTD_ESCR { get; set; }
    }
}