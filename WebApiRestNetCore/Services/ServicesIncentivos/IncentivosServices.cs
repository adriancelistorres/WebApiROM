using System.Data.SqlClient;
using System.Data;
using WebApiRestNetCore.DTO.DtoIncentivo;
using System.Text;
using System.Security.Cryptography;

namespace WebApiRestNetCore.Services.ServicesIncentivos
{
    public class IncentivosServices
    {
        private readonly string _connectionString;
        private readonly string _connectionStringBI;


        public IncentivosServices(IConfiguration configuracion)
        {
            _connectionString = configuracion.GetConnectionString("SQLconexion");
            _connectionStringBI= configuracion.GetConnectionString("APP_BI");

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

        //public IEnumerable<IncentivoPagoDTO> GetGeneralIncentivosPagosWithDNI(string dni)
        //{
        //    List<IncentivoPagoDTO> incentivosPagos = new List<IncentivoPagoDTO>();

        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        connection.Open();

        //        string query = @"SELECT
        //                    i.Descripcion,
        //                    i.Empresa,
        //                    ip.Monto,
	       //                 ip.ConfirmacionEntrega

        //                FROM
        //                    Incentivos i
        //                    JOIN IncentivosPagos ip ON i.Id = ip.IncentivosId
        //                WHERE
        //                    ip.DniPromotor = @dni AND
        //                    ip.Monto > 0";

        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@dni", dni);

        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    string descripcion = reader.GetString(0);
        //                    string empresa = reader.GetString(1);
        //                    decimal monto = reader.GetDecimal(2);
        //                    bool confirmacionEntrega = reader.GetBoolean(3);

        //                    IncentivoPagoDTO incentivoPago = new IncentivoPagoDTO
        //                    {
        //                        Descripcion = descripcion,
        //                        Empresa = empresa,
        //                        Monto = monto,
        //                        ConfirmacionEntrega = confirmacionEntrega
        //                    };

        //                    incentivosPagos.Add(incentivoPago);
        //                }
        //            }
        //        }
        //    }

        //    return incentivosPagos;
        //}

        ///------------------------------------------lo que se usa----------------------------------------------------




        public UsuarioDTO ValidateUser(string dni, string password)
    {
        UsuarioDTO usuarioRetorno = null;

        using (SqlConnection connection = new SqlConnection(_connectionStringBI))
        {
            connection.Open();

            string storedProcedureName = "SEG_ValidateUser";

            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@USUARIO", dni);
                command.Parameters.AddWithValue("@CLAVE", password);

                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        usuarioRetorno = new UsuarioDTO
                        {
                            IDUSUARIO = int.Parse(rdr["IDUSUARIO"].ToString()),
                            NOMBRES = rdr["NOMBRES"].ToString(),
                            APELLIDOPATERNO = rdr["APELLIDOPATERNO"].ToString(),
                            APELLIDOMATERNO = rdr["APELLIDOMATERNO"].ToString(),
                            JERARQUIA = rdr["JERARQUIA"].ToString(),
                            IDJERARQUIA = int.Parse(rdr["IDJERARQUIA"].ToString()),
                            CORREO = rdr["CORREO"] != null ? rdr["CORREO"].ToString() : "",
                            USUARIO = rdr["USUARIO"].ToString(),
                            COD_NEGOCIO = rdr["COD_NEGOCIO"].ToString(),
                            COD_CUENTA = rdr["COD_CUENTA"].ToString(),
                            COD_PAIS = rdr["COD_PAIS"].ToString(),
                            CLAVE = rdr["CLAVE"].ToString(),
                            ES_ADMIN = rdr["ES_ADMIN"].ToString(),
                            TOKEN = rdr["TOKEN"].ToString()
                        };
                    }
                }
            }
        }

        return usuarioRetorno;
    }
        public bool IsDniPresent(string dni)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT COUNT(*) FROM dbo.IncentivosPagos WHERE DniPromotor = @dni AND Monto > 0 AND ConfirmacionEntrega = 0 AND IdEstadoAdministrativo = 1";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@dni", dni);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
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
                    command.Parameters.AddWithValue("@P_IDESTADO", 3);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string periodoIncentivo = reader.GetString(1);
                            DateTime fechainicio = reader.GetDateTime(2);
                            DateTime fechafin = reader.GetDateTime(3);
                            string dniPromotor = reader.GetString(4);
                            string nombreCompleto = reader.GetString(5);
                            string puntoventa = reader.GetString(6);
                            int idIncentivo = reader.GetInt32(7);
                            string nombreIncentivo = reader.GetString(8);
                            string empresa = reader.GetString(9);
                            decimal monto = reader.GetDecimal(10);
                            string estadoIncentivo = reader.GetString(11);

                            IncentivoVistaDTO incentivoVista = new IncentivoVistaDTO
                            {
                                id = id,
                                PeriodoIncentivo = periodoIncentivo,
                                FechaInicio=fechainicio,
                                FechaFin=fechafin,
                                DniPromotor = dniPromotor,
                                NombreCompleto = nombreCompleto,
                                PUNTOVENTA=puntoventa,
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
                         SET ConfirmacionEntrega = 3,
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

