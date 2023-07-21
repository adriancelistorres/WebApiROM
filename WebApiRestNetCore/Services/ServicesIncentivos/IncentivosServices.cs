using System.Data.SqlClient;
using System.Data;
using WebApiRestNetCore.DTO.DtoIncentivo;

namespace WebApiRestNetCore.Services.ServicesIncentivos
{
    public class IncentivosServices
    {
        private readonly string _connectionString;

        public IncentivosServices(IConfiguration configuracion)
        {
            _connectionString = configuracion.GetConnectionString("SQLconexion");
        }

        public IEnumerable<IncentivoPagoDTO> GetIncentivosPagos(string dni)
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

            return incentivosPagos;
        }

        public IEnumerable<IncentivoPagoDTO> GetGeneralIncentivosPagosWithDNI(string dni)
        {
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
                            bool confirmacionEntrega = reader.GetBoolean(3);

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

            return incentivosPagos;
        }

        public IEnumerable<IncentivoVistaDTO> GetGeneralIncentivosVistasWithDNIConfirmationFalse(string dni)
        {
            List<IncentivoVistaDTO> incentivosVistas = new List<IncentivoVistaDTO>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string storedProcedureName = "SP_ListarIncentivos";

                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@P_DNIPROMOTOR", dni);
                    command.Parameters.AddWithValue("@P_IDESTADO", 1);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string periodoIncentivo = reader.GetString(1);
                            string dniPromotor = reader.GetString(2);
                            string nombreCompleto = reader.GetString(3);
                            int idIncentivo = reader.GetInt32(4);
                            string nombreIncentivo = reader.GetString(5);
                            string empresa = reader.GetString(6);
                            decimal monto = reader.GetDecimal(7);
                            string estadoIncentivo = reader.GetString(8);

                            IncentivoVistaDTO incentivoVista = new IncentivoVistaDTO
                            {
                                id = id,
                                PeriodoIncentivo = periodoIncentivo,
                                DniPromotor = dniPromotor,
                                NombreCompleto = nombreCompleto,
                                IdIncentivo = idIncentivo,
                                NombreIncentivo = nombreIncentivo,
                                Empresa = empresa,
                                Monto = monto,
                                EstadoIncentivo = estadoIncentivo
                            };

                            incentivosVistas.Add(incentivoVista);
                        }
                    }
                }
            }

            return incentivosVistas;
        }

        public void UpdateConfirmacionEntrega(string dni, int idIncentivo)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"UPDATE IncentivosPagos
                         SET ConfirmacionEntrega = 1,
                             IdEstadoAdministrativo = 4,
                             FechaConfirmacion = GETDATE()
                         WHERE DniPromotor = @dni
                         AND Monto > 0
                         AND Id = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@dni", dni);
                    command.Parameters.AddWithValue("@id", idIncentivo);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

