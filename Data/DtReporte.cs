using SistemaWebMedida.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using OfficeOpenXml.Utils;


namespace SistemaWebMedida.Data
{
    public class DtReporte
    {
        public List<ReporteDatos> RetornarPresion()
        {
            List<ReporteDatos> ObjLista =  new List<ReporteDatos>();

            using (SqlConnection oconexion = new SqlConnection("Data Source=DBMEDIDA.mssql.somee.com;Initial Catalog=DBMEDIDA; user id=ParzivalFair_SQLLogin_1; pwd=ol7mybg2d9; TrustServerCertificate=True;"))
            {
                string Query = "SP_ReportePresion";

                SqlCommand cmd = new SqlCommand(Query, oconexion);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();

                using(SqlDataReader dr =  cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ObjLista.Add(new ReporteDatos()
                        {
                            PresionAtm = Convert.ToDecimal(dr["PresionAtm"]),
                            Fecha = Convert.ToDateTime(dr["Fecha"]),
                        });
                    }
                }
            }                           

            return ObjLista;
        }

        public List<ReporteDatos> RetornarPresionDif()
        {
            List<ReporteDatos> ObjLista = new List<ReporteDatos>();

            using (SqlConnection oconexion = new SqlConnection("Data Source=DBMEDIDA.mssql.somee.com;Initial Catalog=DBMEDIDA; user id=ParzivalFair_SQLLogin_1; pwd=ol7mybg2d9; TrustServerCertificate=True;"))
            {
                string Query = "SP_ReportePresionDif";

                SqlCommand cmd = new SqlCommand(Query, oconexion);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ObjLista.Add(new ReporteDatos()
                        {
                            PresionDif = Convert.ToDecimal(dr["PresionDif"]),
                            Fecha = Convert.ToDateTime(dr["Fecha"]),
                        });
                    }
                }
            }

            return ObjLista;
        }

        public List<ReporteDatos> RetornarCo2()
        {
            List<ReporteDatos> ObjLista = new List<ReporteDatos>();

            using (SqlConnection oconexion = new SqlConnection("Data Source=DBMEDIDA.mssql.somee.com;Initial Catalog=DBMEDIDA; user id=ParzivalFair_SQLLogin_1; pwd=ol7mybg2d9; TrustServerCertificate=True;"))
            {
                string Query = "SP_ReporteCo2";

                SqlCommand cmd = new SqlCommand(Query, oconexion);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ObjLista.Add(new ReporteDatos()
                        {
                            Co2 = Convert.ToDecimal(dr["Co2"]),
                            Fecha = Convert.ToDateTime(dr["Fecha"]),
                        });
                    }
                }
            }

            return ObjLista;
        }

        public List<ReporteDatos> RetornarTemperatura()
        {
            List<ReporteDatos> ObjLista = new List<ReporteDatos>();

            using (SqlConnection oconexion = new SqlConnection("Data Source=DBMEDIDA.mssql.somee.com;Initial Catalog=DBMEDIDA; user id=ParzivalFair_SQLLogin_1; pwd=ol7mybg2d9; TrustServerCertificate=True;"))
            {
                string Query = "SP_ReporteTemperatura";

                SqlCommand cmd = new SqlCommand(Query, oconexion);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ObjLista.Add(new ReporteDatos()
                        {
                            Temperatura = Convert.ToDecimal(dr["Temperatura"]),
                            Fecha = Convert.ToDateTime(dr["Fecha"]),
                        });
                    }
                }
            }

            return ObjLista;
        }

        public List<ReporteDatos> RetornarOxigeno()
        {
            List<ReporteDatos> ObjLista = new List<ReporteDatos>();

            using (SqlConnection oconexion = new SqlConnection("Data Source=DBMEDIDA.mssql.somee.com;Initial Catalog=DBMEDIDA; user id=ParzivalFair_SQLLogin_1; pwd=ol7mybg2d9; TrustServerCertificate=True;"))
            {
                string Query = "SP_ReporteOxigeno";

                SqlCommand cmd = new SqlCommand(Query, oconexion);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ObjLista.Add(new ReporteDatos()
                        {
                            Oxigeno = Convert.ToDecimal(dr["Oxigeno"]),
                            Fecha = Convert.ToDateTime(dr["Fecha"]),
                        });
                    }
                }
            }

            return ObjLista;
        }

        public ReporteDatos UltimoRegistro()
        {
            ReporteDatos entidad = null; // Inicializamos la entidad como null

            using (SqlConnection oconexion = new SqlConnection("Data Source=DBMEDIDA.mssql.somee.com;Initial Catalog=DBMEDIDA;user id=ParzivalFair_SQLLogin_1;pwd=ol7mybg2d9;TrustServerCertificate=True;"))
            {
                string query = "[Sp_Datos]";

                SqlCommand cmd = new SqlCommand(query, oconexion);
                cmd.CommandType = CommandType.StoredProcedure;
                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read()) // Usamos 'if' en lugar de 'while' ya que solo esperamos un registro
                    {
                        entidad = new ReporteDatos()
                        {
                            Co2 = dr["Co2"] != DBNull.Value ? Convert.ToDecimal(dr["Co2"]) : 0,
                            PresionDif = dr["PresionDif"] != DBNull.Value ? Convert.ToDecimal(dr["PresionDif"]) : 0,
                            PresionAtm = dr["PresionAtm"] != DBNull.Value ? Convert.ToDecimal(dr["PresionAtm"]) : 0,
                            Oxigeno = dr["Oxigeno"] != DBNull.Value ? Convert.ToDecimal(dr["Oxigeno"]) : 0
                        };
                    }
                }
            }

            return entidad;
        }

        public DataTable LlenarDataTable()
        {
            DataTable dt = new DataTable(); // Crear un DataTable para almacenar los resultados

            using (SqlConnection oconexion = new SqlConnection("Data Source=DBMEDIDA.mssql.somee.com;Initial Catalog=DBMEDIDA;user id=ParzivalFair_SQLLogin_1;pwd=ol7mybg2d9;TrustServerCertificate=True;"))
            {
                string query = "[Sp_DataTable]";

                SqlCommand cmd = new SqlCommand(query, oconexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    // Abrir la conexión y llenar el DataTable
                    oconexion.Open();
                    da.Fill(dt);
                }
            }

            return dt;
        }

        public bool ActualizarDatos(decimal peso, decimal volumen, DateTime fechaHoraInicio, DateTime fechaHoraFin)
        {            

            using (SqlConnection oconexion = new SqlConnection("Data Source=DBMEDIDA.mssql.somee.com;Initial Catalog=DBMEDIDA;user id=ParzivalFair_SQLLogin_1;pwd=ol7mybg2d9;TrustServerCertificate=True;"))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[Sp_UpdateReporte]", oconexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Peso", peso);
                    cmd.Parameters.AddWithValue("@Volumen", volumen);
                    cmd.Parameters.AddWithValue("@FechaHoraInicio", fechaHoraInicio);
                    cmd.Parameters.AddWithValue("@FechaHoraFin", fechaHoraFin);

                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return true;
        }

    }
}
