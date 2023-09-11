using ArmaduraLosaRevit.Model;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Cambiar.CambiarEspesor
{
    public interface IManejadorRoomCambioEspesor
    {
        
        void M6_ObtenerRoomDeLosa(Floor floor);

        int M7_CAmbiarPArametroEspesorDeRoom(double _espesorLosaFoot);
    }
    public class ManejadorRoomCambioEspesor : IManejadorRoomCambioEspesor
    {
   
        private List<Room> _listaroomEnLOsa;
        private Document _doc;
        private UIDocument _uidoc;
        private readonly View _view;
        private readonly bool isTest;
        private Level _Level;

        public int cantidadError { get; set; }
        public int cantidadPathCambiado { get; set; }

        public ManejadorRoomCambioEspesor(ExternalCommandData commandData, bool isTest = false)
        {
            this._doc = commandData.Application.ActiveUIDocument.Document;
            this._uidoc = commandData.Application.ActiveUIDocument;
            this._view = this._doc.ActiveView;
            this._Level = _doc.ActiveView.GenLevel;
            this._listaroomEnLOsa = new List<Room>();
            this.isTest = isTest;
            
            cantidadPathCambiado = 0;
            cantidadError =0;
        }
        public void M6_ObtenerRoomDeLosa(Floor floor)
        {
            _listaroomEnLOsa = SeleccionarRoom.SeleccionarRoomNivelYEnLosa(_doc,floor);
        }
        public int M7_CAmbiarPArametroEspesorDeRoom(double _espesorLosaFoot)

        {
            int contadorNUmerosRoomCambidos = 0;
            int idAnalizado = 0;
            try
            {
                using (Transaction trans = new Transaction(_uidoc.Document))
                {
                    trans.Start("CAmbiar Espesor room-NH");

                    for (int i = 0; i < _listaroomEnLOsa.Count; i++)
                    {
                        ParameterUtil.SetParaInt(_listaroomEnLOsa[i], "Espesor", _espesorLosaFoot);
                        contadorNUmerosRoomCambidos += 1; 
                    }
               
                    trans.Commit();
                }
            }
            catch (Exception)
            {
                cantidadError += 1;
                if (!isTest)Util.ErrorMsg("Error al cambiar paramatro espoesor Room:" + idAnalizado);

            }

            return contadorNUmerosRoomCambidos;
        }

    }




}
