//using Bytescout.BarCodeReader;
using Microsoft.AspNetCore.Mvc;
using WebApiRestNetCore.Models;
using IronBarCode;
using System;
namespace WebApiRestNetCore.Properties
{
    [ApiController]
    [Route("Boleta")]
    public class BoletaController : Controller
    {
        /*1-Guardar ruta dell fichero en bd*/
        /*2-Guardar el fichero en disco c*/
        /*Desacoplar el guardado de ruta del romweb del sp que actualia lla venta
         * Hace rla mejora en romweb que capture el error si no guardo la ruta en la bd*/
        /**/
        [HttpPost("ObtenerDatosBoleta")]
        public async Task<Response<Boleta>> ObtenerDatosBoleta(Archivo archivo)
        {
            Boleta boleta = new Boleta();
            var archivoBase64 = archivo.archivoBase64;
            //var response = new Response<Boleta>();
            var response = new Response<Boleta>();
            try
            {
                IronBarCode.License.LicenseKey = "IRONBARCODE.ROMOUTSOURCINGSAC.IRO230201.3690.75142.102032-CDC396FAD0-HU4KEGZSDGG62-ZV32PAWC3F4Z-7YEZU26HSFTK-ZODNVVGVVDMN-DGAOCCPV4B46-L3QDXT-LP7NBCXT7KOLUA-PROFESSIONAL.SUB-4FI4AW.RENEW.SUPPORT.01.FEB.2024";
                var ruta = string.Empty;
                var nombreArchivo = Guid.NewGuid().ToString() + ".jpg";
                ruta = $"Images/{nombreArchivo}";
                //string archivoBase64 = boleta.archivoBase64;
                byte[] archivoBytes = Convert.FromBase64String(archivoBase64);
                /*WriteAllByte(ruta, archivoDecofificado)*/
                System.IO.File.WriteAllBytes(ruta, archivoBytes);
                if (archivoBase64.Length > 0)
                {
                    BarcodeResult Result = BarcodeReader.QuicklyReadOneBarcode(ruta, BarcodeEncoding.QRCode | BarcodeEncoding.Code128, true);
                    var resultCodeQr = Result;
                    {
                        boleta.tramaQRCode = Convert.ToString(resultCodeQr);
                        ///*Dividir Trama */
                        char[] caracteresSeparadores = { '|' };
                        string[] arreglo = boleta.tramaQRCode.Split(caracteresSeparadores);
                        for (int i = 0; i <= 1; i++)
                        {
                            boleta.numRuc = arreglo[0];
                            boleta.codComp = arreglo[1];
                            boleta.numeroSerie = arreglo[2];
                            boleta.numero = arreglo[3];
                            boleta.fechaEmision = arreglo[6];
                            boleta.monto = arreglo[5];
                            boleta.ironBarCode = true;
                        }
                        /*Elimnina archivo que se creo para ser leído por QR*/
                        System.IO.File.Delete(ruta);

                    }
                    if (boleta is not null)
                    {
                        response.IsSuccess = true;
                        response.Message = "Consulta exitosa";
                        response.Data = boleta;
                    }                   
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "No se obtuvo cadena byte64, no se adjunto boleta";
                    //response.Data = "No se adjunto Boleta";
                }
            }
            catch (Exception ex)
            {                
                response.IsSuccess = false;
                response.Message+= ex.ToString();
            }
            return response;
        }
    }
}