using LitJson;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace admin_cambios_procesos.Models.Ayudante
{
    public class Cls_Ayudante_Serializar_Json
    {
        /// <summary>
        /// Metodo para crear una tabla en cadena con formato json para combos
        /// </summary>
        /// <creo>Juan Alberto Hernandez Negrete</creo>
        /// <fecha_creo>01-Ago-2014</fecha_creo>
        /// <modifico></modifico>
        /// <fecha_modifico></fecha_modifico>
        /// <causa_modificacion></causa_modificacion>
        public static String Crear_Tabla_Formato_JSON_ComboBox(DataTable Dt_Datos)
        {
            StringBuilder Buffer = new StringBuilder();
            StringWriter Escritor = new StringWriter(Buffer);
            JsonWriter Escribir_Formato_JSON = new JsonWriter(Escritor);
            String Cadena_Resultado = String.Empty;

            try
            {
                Escribir_Formato_JSON.WriteArrayStart();

                if (Dt_Datos is DataTable)
                {
                    if (Dt_Datos.Rows.Count > 0)
                    {
                        foreach (DataRow FILA in Dt_Datos.Rows)
                        {
                            Escribir_Formato_JSON.WriteObjectStart();
                            foreach (DataColumn COLUMNA in Dt_Datos.Columns)
                            {
                                if (!String.IsNullOrEmpty(FILA[COLUMNA.ColumnName].ToString()))
                                {
                                    Escribir_Formato_JSON.WritePropertyName(COLUMNA.ColumnName);
                                    Escribir_Formato_JSON.Write(FILA[COLUMNA.ColumnName].ToString().Trim());
                                }
                            }
                            Escribir_Formato_JSON.WriteObjectEnd();
                        }
                    }
                }

                Escribir_Formato_JSON.WriteArrayEnd();
                Cadena_Resultado = Buffer.ToString();
            }
            catch (Exception)
            {
                throw new Exception("Error al crear la cadena json para cargar un combo.");
            }
            return Cadena_Resultado;
        }

        /// <summary>
        /// Metodo para crear una tabla en cadena con formato json para grids
        /// </summary>
        /// <creo>Juan Alberto Hernandez Negrete</creo>
        /// <fecha_creo>02-Ago-2014</fecha_creo>
        /// <modifico></modifico>
        /// <fecha_modifico></fecha_modifico>
        /// <causa_modificacion></causa_modificacion>
        public static String Crear_Tabla_Formato_JSON_DataGrid(DataTable Dt_Datos, Int32 Total_Registros, String Formato_Fecha)
        {
            StringBuilder Buffer = new StringBuilder();
            StringWriter Escritor = new StringWriter(Buffer);
            JsonWriter Escribir_Formato_JSON = new JsonWriter(Escritor);
            String Cadena_Resultado = String.Empty;

            try
            {
                Escribir_Formato_JSON.WriteObjectStart();
                Escribir_Formato_JSON.WritePropertyName("total");
                Escribir_Formato_JSON.Write(Total_Registros.ToString());
                Escribir_Formato_JSON.WritePropertyName(Dt_Datos.TableName);

                if (Dt_Datos is DataTable)
                {
                    if (Dt_Datos.Rows.Count > 0)
                    {
                        Escribir_Formato_JSON.WriteArrayStart();
                        foreach (DataRow FILA in Dt_Datos.Rows)
                        {
                            Escribir_Formato_JSON.WriteObjectStart();
                            foreach (DataColumn COLUMNA in Dt_Datos.Columns)
                            {
                                Escribir_Formato_JSON.WritePropertyName(COLUMNA.ColumnName);

                                //validamos si es de tipo datetime para formatear
                                if (COLUMNA.DataType.Name.Equals("DateTime"))
                                    Escribir_Formato_JSON.Write(String.Format(Formato_Fecha, FILA[COLUMNA.ColumnName]));
                                else if (COLUMNA.DataType.Name.Equals("Decimal"))
                                    Escribir_Formato_JSON.Write(String.Format("{0:n}", FILA[COLUMNA.ColumnName]));
                                //validamos si es de tipo decimal para formatear
                                else
                                    Escribir_Formato_JSON.Write(string.IsNullOrEmpty(FILA[COLUMNA.ColumnName].ToString()) ? " " : FILA[COLUMNA.ColumnName].ToString());
                            }
                            Escribir_Formato_JSON.WriteObjectEnd();
                        }

                        Escribir_Formato_JSON.WriteArrayEnd();
                        Escribir_Formato_JSON.WriteObjectEnd();
                        Cadena_Resultado = Buffer.ToString();
                    }
                    else Cadena_Resultado = "{\"total\":0,\"rows\":[]}";
                }
                else Cadena_Resultado = "{\"total\":0,\"rows\":[]}";
            }
            catch (Exception)
            {
                throw new Exception("Error al crear la cadena json para cargar un grid.");
            }
            return Cadena_Resultado;
        }

        /// <summary>
        /// Metodo para crear una tabla en cadena con formato json para grids
        /// </summary>
        /// <creo>Juan Alberto Hernandez Negrete</creo>
        /// <fecha_creo>02-Ago-2014</fecha_creo>
        /// <modifico></modifico>
        /// <fecha_modifico></fecha_modifico>
        /// <causa_modificacion></causa_modificacion>
        public static String Crear_Tabla_Formato_JSON_DataGrid(DataTable Dt_Datos, String Formato_Fecha, String Formato_Decimal, String Formato_Double)
        {
            StringBuilder Buffer = new StringBuilder();
            StringWriter Escritor = new StringWriter(Buffer);
            JsonWriter Escribir_Formato_JSON = new JsonWriter(Escritor);
            String Cadena_Resultado = String.Empty;

            try
            {
                if (Dt_Datos is DataTable)
                {
                    if (Dt_Datos.Rows.Count > 0)
                    {
                        Escribir_Formato_JSON.WriteArrayStart();
                        foreach (DataRow FILA in Dt_Datos.Rows)
                        {
                            Escribir_Formato_JSON.WriteObjectStart();
                            foreach (DataColumn COLUMNA in Dt_Datos.Columns)
                            {
                                Escribir_Formato_JSON.WritePropertyName(COLUMNA.ColumnName);

                                //validamos si es de tipo datetime para formatear
                                if (COLUMNA.DataType.Name.Equals("DateTime"))
                                    Escribir_Formato_JSON.Write(String.Format(Formato_Fecha, FILA[COLUMNA.ColumnName]));
                                else if (COLUMNA.DataType.Name.Equals("Decimal"))
                                    Escribir_Formato_JSON.Write(String.Format(Formato_Decimal, FILA[COLUMNA.ColumnName]));
                                else if (COLUMNA.DataType.Name.Equals("Double"))
                                    Escribir_Formato_JSON.Write(String.Format(Formato_Double, FILA[COLUMNA.ColumnName]));
                                //validamos si es de tipo decimal para formatear
                                else
                                    Escribir_Formato_JSON.Write(string.IsNullOrEmpty(FILA[COLUMNA.ColumnName].ToString()) ? " " : FILA[COLUMNA.ColumnName].ToString());
                            }
                            Escribir_Formato_JSON.WriteObjectEnd();
                        }

                        Escribir_Formato_JSON.WriteArrayEnd();
                        Cadena_Resultado = Buffer.ToString();
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Error al crear la cadena json para cargar un grid.");
            }
            return Cadena_Resultado;
        }
    }
}