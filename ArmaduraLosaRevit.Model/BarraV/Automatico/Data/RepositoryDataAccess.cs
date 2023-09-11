using System;
using System.Data.OleDb;
using System.Data;
using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.model.Data
{
    public class RepositoryDataAccess : IRepositoryDataAccess
    {
        private string proveedor;

        public RepositoryDataAccess()
        {
             proveedor = @"PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source =";
            proveedor = @"Provider = Microsoft.ACE.OLEDB.12.0;Data Source =";
        }

        // proveedor = @"PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source =";  access antigu
        // proveedor = @"Provider = Microsoft.ACE.OLEDB.12.0;Data Source ="; accese nuevo
       
        //1) crear tabla
        public int CrearTabla(string Tabla, string RutaArchivo, string queryString)
        {
            return EjecutarSentencia(queryString, proveedor + RutaArchivo);
        }

        //2) Borra tabla
        public int BorrarTabla(string Tabla, string RutaArchivo)
        {
            return EjecutarSentencia("DROP  TABLE " + Tabla + "", proveedor + RutaArchivo);
        }
        
        //3) select tabla
        public DataTable SeleccionTablaADatatble(string RutaArchivo, string queryString)
        {

            //  queryString = "SELECT * FROM Id=2";
            var dataTable = new DataTable();
            using (OleDbConnection connection = new OleDbConnection(proveedor + RutaArchivo))
            {
                try
                {
                    connection.Open();
                    var dataAdapter = new OleDbDataAdapter(queryString, connection);
                    dataAdapter.Fill(dataTable);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error obtener datos datatable.\n ex:{ex.Message}");
                }
            }
            return dataTable;
        }


        //*************************************************************************************************************
        

        //4) agregar
        public int AgregarDato(string RutaArchivo, string Tabla, string Campos, string Condicion)
        {
            return EjecutarSentencia("INSERT INTO " + Tabla + " (" + Campos + ") values (" + Condicion + ")", proveedor + RutaArchivo);
        }
        //5) borrar
        public int BorrarDato(string RutaArchivo, string Tabla, string Condicion)
        {
            return EjecutarSentencia("DELETE FROM " + Tabla + " WHERE " + Condicion + "", proveedor + RutaArchivo);
        }   
        //6) actualizar
        public int UpdateDato(string RutaArchivo, string Tabla, string sentencia, string Condicion)
        {
            return EjecutarSentencia("Update " + Tabla + " Set" + sentencia + " WHERE " + Condicion + "", proveedor + RutaArchivo);
        }

        //****************************************************************************************************************************
        //general 'EjecutarSentencia' se opcupa en los puntos 1,2,3,4,5
        public int EjecutarSentencia(string queryString, string proveedor_RutaArchivo)
        {

            int filas = 0;
            using (OleDbConnection connection = new OleDbConnection(proveedor_RutaArchivo))
            {
                try
                {
                    connection.Open();

                    OleDbCommand command = new OleDbCommand(queryString, connection);
                    filas = command.ExecuteNonQuery();

                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    filas = -1;
                }
            }
            return filas;
        }
        public bool Existetabla(string tabla, string RutaArchivo)
        {

            bool existe = false;
            using (OleDbConnection connection = new OleDbConnection(proveedor + RutaArchivo))
            {
                try
                {
                    connection.Open();
                    // busca si hay tabla

                    DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, tabla, "TABLE" });

                    if (schemaTable.Rows.Count != 0)
                    {
                        existe = true;
                    }
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    existe = false;
                }
            }
            return existe;
        }

    }
}
