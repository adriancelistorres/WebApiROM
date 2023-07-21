using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using WebApiRestNetCore.DTO.DtoIncentivo;
using WebApiRestNetCore.Services.ServicesIncentivos;

namespace WebApiRestNetCore.Controllers.ControllerIncentivos
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncentivosController : ControllerBase
    {
        private readonly IncentivosServices _incentivosService;

        public IncentivosController(IConfiguration configuracion)
        {
            _incentivosService = new IncentivosServices(configuracion);
        }

        [HttpGet]
        public ActionResult<IEnumerable<IncentivoPagoDTO>> Get(string dni)
        {
            var incentivosPagos = _incentivosService.GetIncentivosPagos(dni);
            return Ok(incentivosPagos);
        }

        [HttpPost("GeneralWithDNI")]
        public ActionResult<IEnumerable<IncentivoPagoDTO>> GetGeneralWithDNI([FromBody] IncentivoPagoRequestDTO request)
        {
            string dni = request.Dni;
            var incentivosPagos = _incentivosService.GetGeneralIncentivosPagosWithDNI(dni);
            return Ok(incentivosPagos);
        }

        [HttpPost("GeneralWithDNIConfirmationFalse")]
        public ActionResult<IEnumerable<IncentivoVistaDTO>> GetGeneralWithDNIConfirmationFalse([FromBody] IncentivoPagoRequestDTO request)
        {
            //string token=Request.Headers.Where(x=>x.Key=="Authorization").FirstOrDefault().Value;

            //if (token != "marco123.")
            //{
            //    return Ok("error");
            //}
            string dni = request.Dni;
            var incentivosVistas = _incentivosService.GetGeneralIncentivosVistasWithDNIConfirmationFalse(dni);
            //if (!incentivosVistas.Any())
            //{
            //    return NotFound("No hay datos disponibles.");
            //}

            return Ok(incentivosVistas);
        }

        [HttpPost("UpdateWithDNI")]
        public IActionResult UpdateConfirmacionEntrega([FromBody] IncentivoPagoRequestDTO request)
        {
            string dni = request.Dni;
            int id= (int)request.Id;

            _incentivosService.UpdateConfirmacionEntrega(dni, id);
            return Ok();
        }
    }
}



