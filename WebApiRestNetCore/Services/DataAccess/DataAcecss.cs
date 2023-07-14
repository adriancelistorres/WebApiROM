using System.Data;
using System.Data.SqlClient;
using WebApiRestNetCore.Models;

namespace WebApiRestNetCore.Services.DataAccess
{
    public class DataAcecss
    {
        private readonly string _connectionString = "";

        public DataAcecss(IConfiguration configuracion)
        {
            _connectionString = configuracion.GetConnectionString("SQLconexion"); ;
        }

        public void InsertData(List<AperturaPDV>BE)
        {
            SqlConnection cn = new SqlConnection(_connectionString);
            string truncateQuery = "TRUNCATE TABLE AperturaPDV";
            string insertQuery = "INSERT INTO AperturaPDV (JefedeVenta, Supervisor, PDV, Fecha, HoraApertura, HoraCierre) VALUES (@JefedeVenta, @Supervisor, @PDV, @Fecha, @HoraApertura, @HoraCierre)";
            try
            {
                cn.Open();

                // Truncar la tabla
                using (SqlCommand truncateCmd = new SqlCommand(truncateQuery, cn))
                {
                    truncateCmd.ExecuteNonQuery();
                }

                // Insertar los nuevos datos
                foreach (var item in BE)
                {
                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, cn))
                    {
                        //insertCmd.Parameters.AddWithValue("@JefedeVenta", item.JefedeVenta);
                        //insertCmd.Parameters.AddWithValue("@Supervisor", item.Supervisor);
                        //insertCmd.Parameters.AddWithValue("@PDV", item.PDV);
                        //insertCmd.Parameters.AddWithValue("@Fecha", item.Fecha);
                        //insertCmd.Parameters.AddWithValue("@HoraApertura", item.HoraApertura);
                        //insertCmd.Parameters.AddWithValue("@HoraCierre", item.HoraCierre);

                        insertCmd.Parameters.Add("@JefedeVenta", SqlDbType.NVarChar).Value = item.JefedeVenta != null ? item.JefedeVenta : DBNull.Value;
                        insertCmd.Parameters.Add("@Supervisor", SqlDbType.NVarChar).Value = item.Supervisor != null ? item.Supervisor : DBNull.Value;
                        insertCmd.Parameters.Add("@PDV", SqlDbType.NVarChar).Value = item.PDV != null ? item.PDV : DBNull.Value;
                        insertCmd.Parameters.Add("@Fecha", SqlDbType.NVarChar).Value = item.Fecha != null ? item.Fecha : DBNull.Value;
                        insertCmd.Parameters.Add("@HoraApertura", SqlDbType.NVarChar).Value = item.HoraApertura != null ? item.HoraApertura : DBNull.Value;
                        insertCmd.Parameters.Add("@HoraCierre", SqlDbType.NVarChar).Value = item.HoraCierre != null ? item.HoraCierre : DBNull.Value;


                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                cn.Close();
            }





        }
    }
}
