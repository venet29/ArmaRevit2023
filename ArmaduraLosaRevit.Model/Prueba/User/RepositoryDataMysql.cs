using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;


namespace ArmaduraLosaRevit.Model.Prueba.User
{
    public interface IRepositoryDataMysql
    {

        //3) select tabla
        DataTable conexiones_solicitud(string buscar, string tabla_refe);

        //***************************************************************
        //4) agregar
        int agregar(string Tabla, string Campos, string Condicion);
        //5) borrar
        int borrar(string Tabla, string Condicion);
        //6) actualizar
        int actualizar(string Tabla, string Campos, string Condicion);

    }
    public class RepositoryDataMysql : IRepositoryDataMysql
    {
        //0) PROPIEDADES ********************************************************************************************************************************
        public MySqlConnection conectarme2 { get; set; }
        public MySqlDataAdapter adapter2 { get; set; }
        public MySqlDataReader myReader { get; set; }
        public bool registrar_movimientos { get; set; }
        public static NombreServer servidero { get; set; }
        string proveedor { get; set; }


        //1) CONSTRUCTOR ********************************************************************************************************************************
        public RepositoryDataMysql()
        {
            conectarme2 = new MySqlConnection();
            adapter2 = new MySqlDataAdapter();
            proveedor = @"PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source =";
            registrar_movimientos = true;
        }

        public RepositoryDataMysql(NombreServer Servidero)
        {
            conectarme2 = new MySqlConnection();
            adapter2 = new MySqlDataAdapter();
            servidero = Servidero;
            proveedor = @"PROVIDER=Microsoft.Jet.OLEDB.4.0;Data Source =";
            registrar_movimientos = true;
        }

        //METODOS2)******************************************************************************************************************************************
        public bool conexiones_abrir2()
        {
            try
            {
                conectarme2.Close();
                
                //servidero = NombreServer.JHUERTA;
                if (Environment.UserName == "JHUERTA")
                {
                    if (servidero == NombreServer.EUGENIA)
                    {
                        conectarme2.ConnectionString = ConexionParaOficina();
                    }
                    else
                    {
                        //conectarme2.ConnectionString = ConexionParaPcPersonalLocalHost();   //JHUERTA"  
                        conectarme2.ConnectionString = ConexionParaPcPersonalLocalHost(); //web
                    }
                }

                else
                {
                    //conectarme2.ConnectionString = ConexionParaOficina();
                    conectarme2.ConnectionString = ConexionParaOficina();
                }


                conectarme2.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;

        }

        public string ConexionParaOficina()
        {       
            //return "server=192.168.0.102;Port=3306;database=exporta;Uid=euge;password=1234;Convert Zero Datetime=True;SslMode=none"; //' euge      
            return "server=192.168.0.100;Port=3306;database=exporta;Uid=euge;password=ZsHpvCx372ssb5QD;Convert Zero Datetime=True;SslMode=none"; //' euge     
        }

        public string ConexionParaPcPersonalLocalHost()
        {
            return "server=localhost;Port=3306;database=exporta;Uid=root;password=CDV2017ji;Convert Zero Datetime=True;SslMode=none";//' jhuerta local host
        }
   
        public string ConexionWebCleverCloud()
        {
            return "server=bwjsbk11ozqihdpntr8o-mysql.services.clever-cloud.com;Port=3306;database=bwjsbk11ozqihdpntr8o;Uid=u9pntyha2yz5mxjf;password=LcKNh6d4JMGdcWzx6inw;Convert Zero Datetime=True;SslMode=none";//' jhuerta ip

        }

        public void conexiones_cerrar2()
        {
            try
            {
                conectarme2.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //2)------------------------------------------------------------------------------------



        public DataTable conexiones_solicitud(string buscar, string tabla_refe)
        {
            DataTable tb_elecacion = new DataTable();
            DataSet ds_elevacion = new DataSet();

            conexiones_cerrar2();
            if (!conexiones_abrir2())
                return tb_elecacion;
      
            try
            {
                adapter2 = new MySqlDataAdapter(buscar, conectarme2);
                //   adapter2.FillSchema(ds_elevacion, SchemaType.Mapped);
                adapter2.Fill(ds_elevacion, tabla_refe);
                adapter2.Fill(tb_elecacion);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            conexiones_cerrar2();

            return tb_elecacion;
        }

        //***************************************************************************************************************
        //4) SELECCIONAR
        public MySqlDataReader Selecionar(string Tabla, string Campos, string Condicion)
        {
            MySqlCommand comando = new MySqlCommand();
#pragma warning disable CS0219 // The variable 'filas' is assigned but its value is never used
            int filas = 0;
#pragma warning restore CS0219 // The variable 'filas' is assigned but its value is never used
            string sql = null;

            if (conectarme2.ConnectionString == "") conexiones_abrir2();

            sql = Tabla;

            comando = new MySqlCommand(sql, conectarme2);
            try
            {
                myReader = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                //  MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                myReader = null;
            }

            conectarme2.Close();
            return myReader;
        }



        //***************************************************************************************************************
        //4) agregar
        public int agregar(string Tabla, string Campos, string Condicion)
        {
            MySqlCommand comando = new MySqlCommand();
            int filas = 0;
            string sql = null;

            if (conectarme2.ConnectionString == "") conexiones_abrir2();
            conectarme2.Close();
            conectarme2.Open();
            sql = "INSERT INTO " + Tabla + " (" + Campos + ") values (" + Condicion + ")";

            comando = new MySqlCommand(sql, conectarme2);
            try
            {
                filas = comando.ExecuteNonQuery();
                if (registrar_movimientos) guardar_registroMysql(sql, "agregar");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                //  MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (ex.Message == "Duplicate entry '-1' for key 'PRIMARY'")
                { filas = 0; }
                else
                { filas = -1; }

            }

            conectarme2.Close();
            return filas;
        }
        //5) borrar
        public int borrar(string Tabla, string Condicion)
        {
            MySqlCommand comando = new MySqlCommand();
            int filas = 0;
            string sql = null;

            if (conectarme2.ConnectionString == "") conexiones_abrir2();
            conectarme2.Close();
            conectarme2.Open();
            //Tabla = contabilidad
            //Condicion = `AÑO`='' and `NOBRA`=''
            sql = "DELETE FROM " + Tabla + " WHERE " + Condicion + "";
            comando = new MySqlCommand(sql, conectarme2);
            try
            {
                filas = comando.ExecuteNonQuery();
                if (registrar_movimientos) guardar_registroMysql(sql, "borrar");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                //  MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                filas = -1;
            }
            conectarme2.Close();
            return filas;




        }
        //6) actualizar
        public int actualizar(string Tabla, string Campos, string Condicion)
        {
            MySqlCommand comando = new MySqlCommand();
            int filas = 0;
            string sql = null;


            if (conectarme2.ConnectionString == "") conexiones_abrir2();
            conectarme2.Close();
            conectarme2.Open();
            sql = "UPDATE " + Tabla + " SET " + Campos + " where " + Condicion;


            comando = new MySqlCommand(sql, conectarme2);
            try
            {
                filas = comando.ExecuteNonQuery();
                if (registrar_movimientos) guardar_registroMysql(sql, "actualizar");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                //  MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                filas = -1;
            }

            conectarme2.Close();
            return filas;
        }

        //****************************************************************************************************************************



        public void guardar_registro(string texto_ini)
        {
            DateTime fecha2 = DateTime.Now;

            string ruta = @"\\Server-cdv\usuarios2\jose.huerta\programas\general- programa reporte C.txt";
            string texto = Environment.UserName + "--> " + fecha2 + " -  " + texto_ini;
            //ruta = @"D:\Documents\Escritorio\1\general- programa reporte C.txt";
            try
            {


                if (!File.Exists(ruta))
                {
                    // Create a file to write to.
                    using (StreamWriter outputFile = File.CreateText(ruta))
                    {
                        outputFile.WriteLine(texto);
                    }
                }
                else
                {
                    string content = File.ReadAllText(ruta);
                    // Write the string array to a new file named "WriteLines.txt".
                    using (StreamWriter outputFile = new StreamWriter(ruta))
                    {


                        texto = texto + "\n" + content;
                        outputFile.WriteLine(texto);

                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }



        }


        public void guardar_registroMysql(string texto_ini, string tipoTransaccion)
        {
            DateTime fecha2 = DateTime.Now;

            string texto = Environment.UserName + "--> " + fecha2;
            //ruta = @"D:\Documents\Escritorio\1\general- programa reporte C.txt";
            try
            {
                string result = "'" + texto + "','" + tipoTransaccion + "',\"" + texto_ini + "\"";
                //agregar("`exporta`.`logcontrol`", "`usuario`,`texto`", "'Jhuerta','mensaje'");
                agregar("`exporta`.`logcontrol`", "`usuario`,`tipoTransaccion`,`texto`", result);

            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }



        }
    }
}
