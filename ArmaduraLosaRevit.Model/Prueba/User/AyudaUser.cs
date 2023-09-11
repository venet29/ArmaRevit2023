using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Prueba.User
{
    public enum NombreServer
    {
        EUGENIA,
        JHUERTA
    }


    public enum rolCDV
    {
        DIBUJANTE,
        INGENIERO,
        BIM,
        NONE
    }

    public class UsuariosCDV
    {
        private DataRow item;

        public UsuariosCDV() { }
        public UsuariosCDV(DataRow item)
        {
            this.item = item;
            IdUsuariosArmadura = (int)item["IdUsuariosArmadura"];
            Nombre = item["Nombre"].ToString().Trim();
            RolCdv = EnumeracionBuscador.ObtenerEnumGenerico(rolCDV.NONE, item["RolCdv"].ToString().Trim());
            NumeroMac = item["NumeroMac"].ToString().Trim();
        }
        public int IdUsuariosArmadura { get; set; }
        public string Nombre { get; set; }
        public rolCDV RolCdv { get; set; }
        public string NumeroMac { get; set; }
    }

    internal class DataObtenerNombreCampos
    {
        public static string obtenerNombreCampos_UsuariosCDV()
        {
            return "`Nombre`, `RolCDV`, `NumeroMac`, `DateCreated`";
        }
    }

    internal class DataObtenerValoresDeCampo
    {
        internal static string obtenerValoresDeCampo_UsuariosCDV(UsuariosCDV usuariosCDV)
        {
            return "'" + usuariosCDV.Nombre + "'," +
                   "'" + usuariosCDV.RolCdv + "'," +
                   "'" + usuariosCDV.NumeroMac + "'," +
                   "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
        }
    }


}
