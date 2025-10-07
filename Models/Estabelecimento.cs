using System;

namespace CnpjApi.Models
{
    public class Estabelecimento
    {
        public string CNPJ { get; set; }
        public int ORDEM { get; set; }
        public string DV { get; set; }
        public string MAT_FIL { get; set; }
        public string RAZ_SOC { get; set; }
        public string SIT_CAD { get; set; }
        public DateTime? DT_SIT_CAD { get; set; }
        public string MOT_SIT_CAD { get; set; }
        public string NOME_CIDADE_EXT { get; set; }
        public string COD_PAIS { get; set; }
        public DateTime? DT_INI_ATIV { get; set; }
        public string CNAE_PRINCIPAL { get; set; }
        public string CNAE_SECUNDARIO { get; set; }
        public string TIPO_LOGRADOURO { get; set; }
        public string LOGRADOURO { get; set; }
        public string NUMERO { get; set; }
        public string COMPLEMENTO { get; set; }
        public string BAIRRO { get; set; }
        public string CEP { get; set; }
        public string UF { get; set; }
        public string COD_MUN { get; set; }
        public string MUNICIPIO { get; set; }
        public string DDD_1 { get; set; }
        public string TEL_1 { get; set; }
        public string DDD_2 { get; set; }
        public string TEL_2 { get; set; }
        public string DDD_FAX { get; set; }
        public string FAX { get; set; }
        public string EMAIL { get; set; }
        public string SIT_ESPECIAL { get; set; }
        public DateTime? DT_SIT_ESPEC { get; set; }
    }
}