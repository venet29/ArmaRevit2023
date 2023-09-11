using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.EstadoDesarrollo.Modelo
{
    public class Proyecto
    {

        public string NombreProyecto { get; set; }

        public string numero { get; set; }

  

        public List<viewNH> ListasView { get; set; }
 

        public Proyecto()
        {

            ListasView = new List<viewNH>();

        }

        public bool ObtenerDatosDeProyecto(Document _doc)
        {

            try
            {
                NombreProyecto = ParameterUtil.FindValueParaByName(_doc.ProjectInformation, "PROYECTO", _doc);
                //rutaProyecto= _doc.PathName;
                numero =ParameterUtil.FindValueParaByName(_doc.ProjectInformation, "OBRA N°", _doc);

         

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en ObtenerLista. ex:{ex.Message}");

                return false;
            }
            return true;
        }
    }
}
