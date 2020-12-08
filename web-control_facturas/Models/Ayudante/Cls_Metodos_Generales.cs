﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace web_cambios_procesos.Models.Ayudante
{
    public static class Cls_Metodos_Generales
    {
        public const string FORMATO_FECHA_HORA = "yyyy-MMM-dd HH:mm:ss";
        /// <summary> Metodo para formatear las fechas que se deben visualizar en un grid
        /// previene que la fecha puede ser nula para regresar un guion
        /// si no es nulo regresa la fecha en formato completo y estandar
        /// </summary>
        /// <param name="p_fecha"></param>
        /// <returns></returns>
        public static string Formatear_Fecha_To_Grid(DateTime? p_fecha)
        {
            return (p_fecha != null ? Convert.ToDateTime(p_fecha).ToString(FORMATO_FECHA_HORA) : "-");
        }
        /// <summary> Metodo encargado de serealizar los datatables
        /// </summary>
        /// <param name="_Dt">Indique la fuente de datos</param>
        /// <returns></returns>
        public static string Convertir_DataTable_toString(DataTable _Dt)
        {
            string resultado = string.Empty;

            if (Valida_Fuente_Datos(_Dt))
            {
                JavaScriptSerializer serializar = new JavaScriptSerializer();
                List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
                Dictionary<string, object> registro;

                foreach (DataRow dr in _Dt.Rows)
                {
                    registro = new Dictionary<string, object>();
                    foreach (DataColumn col in _Dt.Columns)
                    {
                        registro.Add(col.ColumnName, dr[col]);
                    }
                    rows.Add(registro);
                }
                resultado = serializar.Serialize(rows);
            }

            return resultado;
        }
        /// <summary> Metodo para convertir una lista a DataTable
        /// Se diseña para convertir el JsonString a List<apl_cat...>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Convertir_JsonString_To_DataTable`1
        public static DataTable Convertir_Lista_To_DataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
        ///*******************************************************************************************************
        /// <summary>
        /// Metodo para validar los datos obligatorios. Se considera como campo Obligatorio aquel de BackColor LavanderBlush
        /// De momento solo considera TextBox y Combo. Pinta el mensaje de erro en el control correspondiente.
        /// </summary>
        /// <param name="_Control"> Control padre donde estan contenidos los campos a validar</param>
        /// <returns>Booelan indicando si Hubo o no algun campo vacio </returns>
        /// <creo>Lorena Espinoza Cuevas</creo>
        /// <fecha_creo>13-Nov-2014</fecha_creo>
        /// <modifico></modifico>
        /// <fecha_modifico></fecha_modifico>
        /// <causa_modificacion></causa_modificacion>
        ///*******************************************************************************************************
        public static Boolean Valida_Fuente_Datos(DataTable _Dt)
        {
            Boolean Valida = false;
            if (_Dt != null)
            {
                if (_Dt.Rows.Count > 0)
                {
                    Valida = true;
                }
            }
            return Valida;
        }
        public static DataTable Agrupar_Fuente(string _Nombre_Campo, DataTable dt_Fuente)
        {
            DataTable dt_Resultado = new DataTable();
            if (!string.IsNullOrEmpty(_Nombre_Campo))
            {
                if (Cls_Metodos_Generales.Valida_Fuente_Datos(dt_Fuente))
                {
                    if (dt_Fuente.Columns.Contains(_Nombre_Campo))
                    {
                        dt_Resultado = dt_Fuente.AsEnumerable()
                               .GroupBy(r => new { Col1 = r[_Nombre_Campo] })
                               .Select(g => g.OrderBy(r => r[_Nombre_Campo]).First())
                               .CopyToDataTable();
                    }
                    else
                    {
                        ///Tratamos de regresar por lo menos el esquema
                        dt_Resultado = dt_Fuente.Clone();
                    }
                }
                else if (dt_Fuente.Columns != null)
                {
                    ///Tratamos de regresar por lo menos el esquema
                    dt_Resultado = dt_Fuente.Clone();
                }
                else
                {
                    //Cls_Metodos_Generales.Mensaje_Error_Parametros_NO_Validos(null);
                }
            }
            else
            {
                //Cls_Metodos_Generales.Mensaje_Error_Parametros_NO_Validos(null);
            }

            return dt_Resultado;
        }
        /// <summary>
        /// Funcion que genera una nueva estructura con el arreglo de columnas enviado
        /// ademas de importar los datos de la fuente envie a la nueva fuente con el nuevo esquema.
        /// Si una columna no existe creara la columna y los datos quedaran vacios.
        /// </summary>
        /// <param name="_Dt">Indique la fuente de datos de los cuales tomara la datos para la nueva fuente </param>
        /// <param name="_Columnas_Mostar">Indique el arreglo de columnas que requier importar de la fuente inicial a la nueva fuente</param>
        /// <returns>Retorna un DataTable con la nuevas columnas y con los datos importados de acuerdo a las coincidencias de columnas con la fuente inicial</returns>
        public static DataTable Mostrar_Columnas_Tabla(DataTable _Dt, String[] _Columnas_Mostar)
        {
            DataTable Dt_Nueva = new DataTable();
            if (_Dt != null)
            {
                if (_Dt.Columns.Count > 0)
                {
                    #region Creamos el nuevo esquema
                    if (Cls_Metodos_Generales.Valida_Arreglos(_Columnas_Mostar))
                    { //Resperara el orden del arreglo.

                        for (int iCol = 0; iCol < _Columnas_Mostar.Length; iCol++)
                        {
                            String Nombre_Columna = _Columnas_Mostar[iCol];
                            if (!Dt_Nueva.Columns.Contains(Nombre_Columna))
                            {
                                if (_Dt.Columns.Contains(Nombre_Columna))
                                { //Si la columna existe en la fuente enviada copia tal cual la columna

                                    DataColumn Columna = new DataColumn(Nombre_Columna, _Dt.Columns[Nombre_Columna].DataType);
                                    Dt_Nueva.Columns.Add(Columna);
                                }
                                else
                                {
                                    Dt_Nueva.Columns.Add(_Columnas_Mostar[iCol]); //Se agrega por el nombre del arreglo y por default es String
                                }
                            }
                        }
                    }
                    else
                    {
                        //Si no esta indicado ninguna columna a mostrar, se ocultaran los se sufijo "_ID" 
                        foreach (DataColumn Dc in _Dt.Columns)
                        {
                            if (!Dc.ColumnName.Contains("_ID"))
                            {
                                Dt_Nueva.Columns.Add(Dc);
                            }
                        }
                    }
                    #endregion Creamos el nuevo esquema

                    #region Importamos los datos a la nueva fuente
                    foreach (DataRow Dr in _Dt.Rows)
                    { //Leemos cada registro de la fuente para importarlos a la nueva fuente con el nuevo esquema

                        DataRow Dr_Nueva = Dt_Nueva.NewRow();
                        foreach (DataColumn Dc_Nueva in Dt_Nueva.Columns)
                        {
                            if (_Dt.Columns.Contains(Dc_Nueva.ColumnName))
                            {
                                Dr_Nueva[Dc_Nueva.ColumnName] = Dr[Dc_Nueva.ColumnName]; //pasamos el dato que corresponde a cada dato
                            }
                        }
                        Dt_Nueva.Rows.Add(Dr_Nueva); //Insertamos el renglon a la nueva fuente de datos.
                    }
                    #endregion Importamos los datos a la nueva fuente
                }
            }

            return Dt_Nueva;
        }
        ///*******************************************************************************************************
        /// <summary>
        /// Metodo para validar los datos obligatorios. Se considera como campo Obligatorio aquel de BackColor LavanderBlush
        /// De momento solo considera TextBox y Combo. Pinta el mensaje de erro en el control correspondiente.
        /// </summary>
        /// <param name="_Control"> Control padre donde estan contenidos los campos a validar</param>
        /// <returns>Booelan indicando si Hubo o no algun campo vacio </returns>
        /// <creo>Lorena Espinoza Cuevas</creo>
        /// <fecha_creo>13-Nov-2014</fecha_creo>
        /// <modifico></modifico>
        /// <fecha_modifico></fecha_modifico>
        /// <causa_modificacion></causa_modificacion>
        ///*******************************************************************************************************
        public static Boolean Valida_Arreglos(Array _Arreglo)
        {
            Boolean Valida = false;
            if (_Arreglo != null)
            {
                if (_Arreglo.Length > 0)
                {
                    Valida = true;
                }
            }
            return Valida;
        }
    }
}