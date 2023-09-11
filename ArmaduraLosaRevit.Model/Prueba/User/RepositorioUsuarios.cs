


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.Prueba.User
{
    public interface IRepositorioUsuarios
    {
        List<UsuariosCDV> GetRolUsuarioS();
        UsuariosCDV GetRolUsuarioSPorMac(string mac);
    }
    public class RepositorioUsuarios : IRepositorioUsuarios
    {
        private string nombreTabla1;
        private RepositoryDataMysql repositoryDataMysql;
        private static List<UsuariosCDV> ListaUsuarios;

        public static UsuariosCDV User { get; private set; }

        public RepositorioUsuarios(NombreServer server)
        {
            nombreTabla1 = "usuariosarmadura";
            repositoryDataMysql = new RepositoryDataMysql(server);
            
        }


        public List<UsuariosCDV> GetRolUsuarioS()
        {
            if (ListaUsuarios == null)
                LoadRolUSuario();
            return ListaUsuarios;
        }

        public UsuariosCDV GetRolUsuarioSPorMac(string mac)
        {
            //Util.InfoMsg("Desactivar rol 'DIBUJANTE'.");
#if DEBUG
            //User = new UsuariosCDV() { RolCdv = rolCDV.DIBUJANTE };
#endif
            if (User != null)
            {
                return User;
            }


            if (ListaUsuarios == null)
                LoadRolUSuario();
            if (mac == "")
            {
                InfoSystema_validar _InfoSystema = new InfoSystema_validar();
                _InfoSystema.Ejecutar();
                mac = _InfoSystema.mac;
            }
            User= ListaUsuarios.Where(c => c.NumeroMac == mac).FirstOrDefault();
            return User;
        }


        private void LoadRolUSuario()
        {
#pragma warning disable CS0219 // The variable 'solicitud' is assigned but its value is never used
            string solicitud = @"SELECT* FROM exporta.";
#pragma warning restore CS0219 // The variable 'solicitud' is assigned but its value is never used

            
            //RepositoryDataMysql RepositorioNh = new RepositoryDataMysql();
            // var Result = RepositorioNh.conexiones_solicitud(solicitud+ nombreTabla, nombreTabla);
            var Result2 = repositoryDataMysql.conexiones_solicitud($"SELECT * FROM exporta.{nombreTabla1} order by  IdUsuariosArmadura", nombreTabla1);

            if (Result2 == null)
            {
                MessageBox.Show("Error al cargar presupuesto, revisr base de datos", "Error Base Datos Presupuesto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // int contador = 0;
            if (ListaUsuarios == null)
                ListaUsuarios = new List<UsuariosCDV>();

            foreach (DataRow item in Result2.AsEnumerable())
            {
                var NewPresupuesto = new UsuariosCDV(item);
                if (NewPresupuesto != null)
                    ListaUsuarios.Add(NewPresupuesto);
                // if (contador == 20) break;
                //  contador++;
            }
        }


        public int AddUsuarioSPorMac(UsuariosCDV _UsuariosCDV)
        {
            var userencontrado = GetRolUsuarioSPorMac(_UsuariosCDV.NumeroMac);
            if (userencontrado != null)
                return 0;

            if (_UsuariosCDV == null)
                LoadRolUSuario();

            if (ListaUsuarios == null)
                ListaUsuarios = new List<UsuariosCDV>();

            ListaUsuarios.Add(_UsuariosCDV);

            string Campos = DataObtenerNombreCampos.obtenerNombreCampos_UsuariosCDV();
            string Condicion = DataObtenerValoresDeCampo.obtenerValoresDeCampo_UsuariosCDV(_UsuariosCDV);
            return repositoryDataMysql.agregar($"`exporta`.`{nombreTabla1}`", Campos, Condicion);
        }

    }
}
