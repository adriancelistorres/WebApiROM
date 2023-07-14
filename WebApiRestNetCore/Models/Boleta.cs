namespace WebApiRestNetCore.Models
{
    public class Boleta
    {
        /*Entrada*/
        public string numRuc { get; set; }
        public string codComp { get; set; }
        public string numeroSerie { get; set; } /*"E001"*/
        public string numero { get; set; }/*CORRELATIVO*/
        public string fechaEmision { get; set; }
        public string monto { get; set; } //
        public string tramaQRCode { get; set; } //
        public bool ironBarCode { get; set; } //

        //public string archivoBase64 { get; set; } //


        /*Salida*/
        //public string estadoComprobante { get; set; }
        //public string estadoContribuyente { get; set; }
        //public string estadoDomicilio { get; set; }

        public string nombreBoletaArchivo { get; set; }

    }

    public class Archivo
    {
        /*Entrada*/
      
        public string archivoBase64 { get; set; } //

    }

}
