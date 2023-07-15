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

        public void InsertData(List<AperturaPDV> BE)
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
                BE.Clear();
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



        //public void InsertData(List<AperturaPDV> BE)
        //{
        //    SqlConnection cn = new SqlConnection(_connectionString);
        //    string selectQuery = "SELECT ID, JefedeVenta, Supervisor, PDV, Fecha, HoraApertura, HoraCierre FROM AperturaPDV";
        //    string updateQuery = "UPDATE AperturaPDV SET JefedeVenta = @JefedeVenta, Supervisor = @Supervisor, PDV = @PDV, Fecha = @Fecha, HoraApertura = @HoraApertura, HoraCierre = @HoraCierre WHERE ID = @ID";
        //    string insertQuery = "INSERT INTO AperturaPDV (JefedeVenta, Supervisor, PDV, Fecha, HoraApertura, HoraCierre) VALUES (@JefedeVenta, @Supervisor, @PDV, @Fecha, @HoraApertura, @HoraCierre)";
        //    try
        //    {
        //        cn.Open();

        //        // Obtener los datos existentes de la base de datos
        //        List<AperturaPDV> existingData = new List<AperturaPDV>();
        //        using (SqlCommand selectCmd = new SqlCommand(selectQuery, cn))
        //        {
        //            using (SqlDataReader reader = selectCmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    AperturaPDV data = new AperturaPDV
        //                    {
        //                        ID = reader.GetInt32(0),
        //                        JefedeVenta = reader.IsDBNull(1) ? null : reader.GetString(1),
        //                        Supervisor = reader.IsDBNull(2) ? null : reader.GetString(2),
        //                        PDV = reader.IsDBNull(3) ? null : reader.GetString(3),
        //                        Fecha = reader.IsDBNull(4) ? null : reader.GetString(4),
        //                        HoraApertura = reader.IsDBNull(5) ? null : reader.GetString(5),
        //                        HoraCierre = reader.IsDBNull(6) ? null : reader.GetString(6)
        //                    };
        //                    existingData.Add(data);
        //                }
        //            }
        //        }

        //        // Comparar y actualizar o insertar los nuevos datos
        //        foreach (var item in BE)
        //        {
        //            AperturaPDV existingItem = existingData.FirstOrDefault(x =>
        //                x.ID == item.ID &&
        //                x.JefedeVenta == item.JefedeVenta &&
        //                x.Supervisor == item.Supervisor &&
        //                x.PDV == item.PDV &&
        //                x.Fecha == item.Fecha);

        //            if (existingItem != null)
        //            {
        //                using (SqlCommand updateCmd = new SqlCommand(updateQuery, cn))
        //                {
        //                    updateCmd.Parameters.AddWithValue("@ID", existingItem.ID);
        //                    updateCmd.Parameters.AddWithValue("@JefedeVenta", item.JefedeVenta != null ? (object)item.JefedeVenta : DBNull.Value);
        //                    updateCmd.Parameters.AddWithValue("@Supervisor", item.Supervisor != null ? (object)item.Supervisor : DBNull.Value);
        //                    updateCmd.Parameters.AddWithValue("@PDV", item.PDV != null ? (object)item.PDV : DBNull.Value);
        //                    updateCmd.Parameters.AddWithValue("@Fecha", item.Fecha != null ? (object)item.Fecha : DBNull.Value);
        //                    updateCmd.Parameters.AddWithValue("@HoraApertura", item.HoraApertura != null ? (object)item.HoraApertura : DBNull.Value);
        //                    updateCmd.Parameters.AddWithValue("@HoraCierre", item.HoraCierre != null ? (object)item.HoraCierre : DBNull.Value);

        //                    updateCmd.ExecuteNonQuery();
        //                }
        //            }
        //            else
        //            {
        //                using (SqlCommand insertCmd = new SqlCommand(insertQuery, cn))
        //                {
        //                    insertCmd.Parameters.AddWithValue("@JefedeVenta", item.JefedeVenta != null ? (object)item.JefedeVenta : DBNull.Value);
        //                    insertCmd.Parameters.AddWithValue("@Supervisor", item.Supervisor != null ? (object)item.Supervisor : DBNull.Value);
        //                    insertCmd.Parameters.AddWithValue("@PDV", item.PDV != null ? (object)item.PDV : DBNull.Value);
        //                    insertCmd.Parameters.AddWithValue("@Fecha", item.Fecha != null ? (object)item.Fecha : DBNull.Value);
        //                    insertCmd.Parameters.AddWithValue("@HoraApertura", item.HoraApertura != null ? (object)item.HoraApertura : DBNull.Value);
        //                    insertCmd.Parameters.AddWithValue("@HoraCierre", item.HoraCierre != null ? (object)item.HoraCierre : DBNull.Value);

        //                    insertCmd.ExecuteNonQuery();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //    finally
        //    {
        //        cn.Close();
        //    }
        //}





        //private bool AreEqual(AperturaPDV obj1, AperturaPDV obj2)
        //{
        //    // Implementa la lógica de comparación para determinar si los objetos son iguales
        //    // Compara los campos relevantes y devuelve true si son iguales, de lo contrario, devuelve false
        //    // Puedes ajustar esta lógica según tus necesidades
        //    return obj1.JefedeVenta == obj2.JefedeVenta &&
        //           obj1.Supervisor == obj2.Supervisor &&
        //           obj1.PDV == obj2.PDV &&
        //           obj1.Fecha == obj2.Fecha &&
        //           obj1.HoraApertura == obj2.HoraApertura &&
        //           obj1.HoraCierre == obj2.HoraCierre;
        //}




    }
    }
