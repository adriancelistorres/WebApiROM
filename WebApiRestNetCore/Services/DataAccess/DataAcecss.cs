using System.Data;
using System.Data.SqlClient;
using WebApiRestNetCore.Models;
using System.Security.Cryptography;
using System.Text;

namespace WebApiRestNetCore.Services.DataAccess
{
    public class DataAcecss
    {
        private readonly string _connectionString = "";

        public DataAcecss(IConfiguration configuracion)
        {
            _connectionString = configuracion.GetConnectionString("SQLconexion"); ;
        }

        //public void InsertData(List<AperturaPDV> BE)
        //{
        //    SqlConnection cn = new SqlConnection(_connectionString);
        //    string truncateQuery = "TRUNCATE TABLE AperturaPDV";
        //    string insertQuery = "INSERT INTO AperturaPDV (JefedeVenta, Supervisor, PDV, Fecha, HoraApertura, HoraCierre, FechaHoraTransaccion) VALUES (@JefedeVenta, @Supervisor, @PDV, @Fecha, @HoraApertura, @HoraCierre, @FechaHoraTransaccion)";
        //    try
        //    {
        //        cn.Open();

        //        // Truncar la tabla
        //        using (SqlCommand truncateCmd = new SqlCommand(truncateQuery, cn))
        //        {
        //            truncateCmd.ExecuteNonQuery();
        //        }

        //        // Insertar los nuevos datos
        //        foreach (var item in BE)
        //        {
        //            using (SqlCommand insertCmd = new SqlCommand(insertQuery, cn))
        //            {
        //                //insertCmd.Parameters.AddWithValue("@JefedeVenta", item.JefedeVenta);
        //                //insertCmd.Parameters.AddWithValue("@Supervisor", item.Supervisor);
        //                //insertCmd.Parameters.AddWithValue("@PDV", item.PDV);
        //                //insertCmd.Parameters.AddWithValue("@Fecha", item.Fecha);
        //                //insertCmd.Parameters.AddWithValue("@HoraApertura", item.HoraApertura);
        //                //insertCmd.Parameters.AddWithValue("@HoraCierre", item.HoraCierre);

        //                insertCmd.Parameters.Add("@JefedeVenta", SqlDbType.NVarChar).Value = item.JefedeVenta != null ? item.JefedeVenta : DBNull.Value;
        //                insertCmd.Parameters.Add("@Supervisor", SqlDbType.NVarChar).Value = item.Supervisor != null ? item.Supervisor : DBNull.Value;
        //                insertCmd.Parameters.Add("@PDV", SqlDbType.NVarChar).Value = item.PDV != null ? item.PDV : DBNull.Value;
        //                insertCmd.Parameters.Add("@Fecha", SqlDbType.NVarChar).Value = item.Fecha != null ? item.Fecha : DBNull.Value;
        //                insertCmd.Parameters.Add("@HoraApertura", SqlDbType.NVarChar).Value = item.HoraApertura != null ? item.HoraApertura : DBNull.Value;
        //                insertCmd.Parameters.Add("@HoraCierre", SqlDbType.NVarChar).Value = item.HoraCierre != null ? item.HoraCierre : DBNull.Value;
        //                insertCmd.Parameters.Add("@FechaHoraTransaccion", SqlDbType.DateTime).Value = item.FechaHoraTransaccion != null ? item.FechaHoraTransaccion : DBNull.Value; ;


        //                insertCmd.ExecuteNonQuery();
        //            }
        //        }
        //        BE.Clear();
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

        public void InsertData(List<AperturaPDV> BE)
        {
            SqlConnection cn = new SqlConnection(_connectionString);
            string selectQuery = "SELECT ID, JefedeVenta, Supervisor, PDV, Fecha, HoraApertura, HoraCierre, FechaHoraTransaccion FROM CoberturaAperturaPDV";
            string updateQuery = "UPDATE CoberturaAperturaPDV SET JefedeVenta = @JefedeVenta, Supervisor = @Supervisor, PDV = @PDV, Fecha = @Fecha, HoraApertura = @HoraApertura, HoraCierre = @HoraCierre, FechaHoraTransaccion = @FechaHoraTransaccion WHERE ID = @ID";
            string insertQuery = "INSERT INTO CoberturaAperturaPDV (JefedeVenta, Supervisor, PDV, Fecha, HoraApertura, HoraCierre, FechaHoraTransaccion) VALUES (@JefedeVenta, @Supervisor, @PDV, @Fecha, @HoraApertura, @HoraCierre, @FechaHoraTransaccion)";

            try
            {
                cn.Open();

                // Obtener registros existentes de la base de datos
                List<AperturaPDV> existingRecords = new List<AperturaPDV>();
                using (SqlCommand selectCmd = new SqlCommand(selectQuery, cn))
                {
                    using (SqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AperturaPDV existingRecord = new AperturaPDV
                            {
                                ID = reader.GetInt32(0),
                                JefedeVenta = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Supervisor = reader.IsDBNull(2) ? null : reader.GetString(2),
                                PDV = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Fecha = reader.IsDBNull(4) ? null : reader.GetString(4),
                                HoraApertura = reader.IsDBNull(5) ? null : reader.GetString(5),
                                HoraCierre = reader.IsDBNull(6) ? null : reader.GetString(6),
                                FechaHoraTransaccion = reader.IsDBNull(7) ? (DateTime?)null : reader.GetDateTime(7)
                            };

                            existingRecords.Add(existingRecord);
                        }
                    }
                }

                // Comparar y actualizar los datos existentes
                foreach (var newItem in BE)
                {
                    var existingItem = existingRecords.FirstOrDefault(e => e.ID == newItem.ID);

                    if (existingItem != null)
                    {
                        // El registro existe, verificar si hay cambios
                        bool hasChanges = false;

                        // Comparar los campos relevantes y marcar cambios si hay diferencias
                        if (existingItem.JefedeVenta != newItem.JefedeVenta)
                        {
                            existingItem.JefedeVenta = newItem.JefedeVenta;
                            hasChanges = true;
                        }

                        if (existingItem.Supervisor != newItem.Supervisor)
                        {
                            existingItem.Supervisor = newItem.Supervisor;
                            hasChanges = true;
                        }

                        if (existingItem.PDV != newItem.PDV)
                        {
                            existingItem.PDV = newItem.PDV;
                            hasChanges = true;
                        }

                        if (existingItem.Fecha != newItem.Fecha)
                        {
                            existingItem.Fecha = newItem.Fecha;
                            hasChanges = true;
                        }

                        if (existingItem.HoraApertura != newItem.HoraApertura)
                        {
                            existingItem.HoraApertura = newItem.HoraApertura;
                            hasChanges = true;
                        }

                        if (existingItem.HoraCierre != newItem.HoraCierre)
                        {
                            existingItem.HoraCierre = newItem.HoraCierre;
                            hasChanges = true;
                        }

                        // Verificar si la FechaHoraTransaccion debe mantenerse igual
                        DateTime? originalFechaHoraTransaccion = existingItem.FechaHoraTransaccion;
                        if (existingItem.FechaHoraTransaccion != newItem.FechaHoraTransaccion)
                        {
                            existingItem.FechaHoraTransaccion = newItem.FechaHoraTransaccion;
                            hasChanges = true;
                        }
                        else
                        {
                            // La FechaHoraTransaccion no ha cambiado, no es necesario marcar cambios
                            hasChanges = false;
                        }

                        // Si hay cambios, realizar la actualización
                        if (hasChanges)
                        {
                            // Restaurar el valor original de la FechaHoraTransaccion
                            existingItem.FechaHoraTransaccion = originalFechaHoraTransaccion;

                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, cn))
                            {
                                updateCmd.Parameters.AddWithValue("@JefedeVenta", existingItem.JefedeVenta != null ? (object)existingItem.JefedeVenta : DBNull.Value);
                                updateCmd.Parameters.AddWithValue("@Supervisor", existingItem.Supervisor != null ? (object)existingItem.Supervisor : DBNull.Value);
                                updateCmd.Parameters.AddWithValue("@PDV", existingItem.PDV != null ? (object)existingItem.PDV : DBNull.Value);
                                updateCmd.Parameters.AddWithValue("@Fecha", existingItem.Fecha != null ? (object)existingItem.Fecha : DBNull.Value);
                                updateCmd.Parameters.AddWithValue("@HoraApertura", existingItem.HoraApertura != null ? (object)existingItem.HoraApertura : DBNull.Value);
                                updateCmd.Parameters.AddWithValue("@HoraCierre", existingItem.HoraCierre != null ? (object)existingItem.HoraCierre : DBNull.Value);
                                updateCmd.Parameters.AddWithValue("@FechaHoraTransaccion", existingItem.FechaHoraTransaccion.HasValue ? (object)existingItem.FechaHoraTransaccion.Value : DBNull.Value);
                                updateCmd.Parameters.AddWithValue("@ID", existingItem.ID);

                                updateCmd.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {
                        // El registro no existe, insertarlo como nuevo
                        using (SqlCommand insertCmd = new SqlCommand(insertQuery, cn))
                        {
                            insertCmd.Parameters.AddWithValue("@JefedeVenta", newItem.JefedeVenta != null ? (object)newItem.JefedeVenta : DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@Supervisor", newItem.Supervisor != null ? (object)newItem.Supervisor : DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@PDV", newItem.PDV != null ? (object)newItem.PDV : DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@Fecha", newItem.Fecha != null ? (object)newItem.Fecha : DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@HoraApertura", newItem.HoraApertura != null ? (object)newItem.HoraApertura : DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@HoraCierre", newItem.HoraCierre != null ? (object)newItem.HoraCierre : DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@FechaHoraTransaccion", newItem.FechaHoraTransaccion.HasValue ? (object)newItem.FechaHoraTransaccion.Value : DBNull.Value);

                            insertCmd.ExecuteNonQuery();
                        }
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
