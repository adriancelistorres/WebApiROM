using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using WebApiRestNetCore.DTO;

namespace WebApiRestNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncentivosController : ControllerBase
    {
        private readonly string _connectionString = "";

        public IncentivosController(IConfiguration configuracion)
        {
            _connectionString = configuracion.GetConnectionString("SQLconexion"); ;
        }
        [HttpGet]
        public ActionResult<IEnumerable<IncentivoPagoDTO>> Get(string dni)
        {
            List<IncentivoPagoDTO> incentivosPagos = new List<IncentivoPagoDTO>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT
                            i.Descripcion,
                            i.Empresa,
                            ip.Monto
                        FROM
                            Incentivos i
                            JOIN IncentivosPagos ip ON i.Id = ip.IncentivosId
                        WHERE
                            ip.DniPromotor = @dni AND
                            ip.Monto > 0";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@dni", dni);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string descripcion = reader.GetString(0);
                            string empresa = reader.GetString(1);
                            decimal monto = reader.GetDecimal(2);

                            IncentivoPagoDTO incentivoPago = new IncentivoPagoDTO
                            {
                                Descripcion = descripcion,
                                Empresa = empresa,
                                Monto = monto
                            };

                            incentivosPagos.Add(incentivoPago);
                        }
                    }
                }
            }

            return Ok(incentivosPagos);
        }

        [HttpPost("GeneralWithDNI")]
        public ActionResult<IEnumerable<IncentivoPagoDTO>> GetGeneralWithDNI([FromBody] IncentivoPagoRequestDTO request)
        {
            string dni = request.Dni;

            List<IncentivoPagoDTO> incentivosPagos = new List<IncentivoPagoDTO>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT
                            i.Descripcion,
                            i.Empresa,
                            ip.Monto,
	                        ip.ConfirmacionEntrega

                        FROM
                            Incentivo_prueba i
                            JOIN IncentivosPagos_prueba ip ON i.Id = ip.IncentivosId
                        WHERE
                            ip.DniPromotor = @dni AND
                            ip.Monto > 0";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@dni", dni);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string descripcion = reader.GetString(0);
                            string empresa = reader.GetString(1);
                            decimal monto = reader.GetDecimal(2);
                            Boolean confirmacionEntrega = reader.GetBoolean(3);


                            IncentivoPagoDTO incentivoPago = new IncentivoPagoDTO
                            {
                                Descripcion = descripcion,
                                Empresa = empresa,
                                Monto = monto,
                                ConfirmacionEntrega = confirmacionEntrega

                            };

                            incentivosPagos.Add(incentivoPago);
                        }
                    }
                }
            }

            return Ok(incentivosPagos);
        }

        [HttpPost("GeneralWithDNIConfirmationFalse")]
        public ActionResult<IEnumerable<IncentivoPagoDTO>> GetGeneralWithDNIConfirmationFalse([FromBody] IncentivoPagoRequestDTO request)
        {
            string dni = request.Dni;

            List<IncentivoPagoDTO> incentivosPagos = new List<IncentivoPagoDTO>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT
                            i.Descripcion,
                            i.Empresa,
                            ip.Monto,
                            ip.ConfirmacionEntrega
                        FROM
                            Incentivo_prueba i
                            JOIN IncentivosPagos_prueba ip ON i.Id = ip.IncentivosId
                        WHERE
                            ip.DniPromotor = @dni AND
                            ip.Monto > 0 AND
                            ip.ConfirmacionEntrega = 0";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@dni", dni);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string descripcion = reader.GetString(0);
                            string empresa = reader.GetString(1);
                            decimal monto = reader.GetDecimal(2);
                            Boolean confirmacionEntrega = reader.GetBoolean(3);

                            IncentivoPagoDTO incentivoPago = new IncentivoPagoDTO
                            {
                                Descripcion = descripcion,
                                Empresa = empresa,
                                Monto = monto,
                                ConfirmacionEntrega=confirmacionEntrega
                            };

                            incentivosPagos.Add(incentivoPago);
                        }
                    }
                }
            }

            return Ok(incentivosPagos);
        }


        [HttpPost("UpdateWithDNI")]
        public IActionResult UpdateConfirmacionEntrega([FromBody] IncentivoPagoRequestDTO request)
        {
            string dni = request.Dni;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"UPDATE IncentivosPagos_prueba
                                 SET ConfirmacionEntrega = 1
                                 WHERE DniPromotor = @dni
                                 AND Monto > 0";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@dni", dni);
                    command.ExecuteNonQuery();
                }
            }

            return Ok();
        }
    }
}



