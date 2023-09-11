using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using ArmaduraLosaRevit.Model.UTILES;
using System;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB.Structure;
using ArmaduraLosaRevit.Model.Traslapo.DTO;
using ArmaduraLosaRevit.Model.EditarTipoPath.Verificar;
using ArmaduraLosaRevit.Model.EditarTipoPath.Ayuda;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.EditarPath.CAmbiosPAth;
using ArmaduraLosaRevit.Model.EditarTipoPath.WPF.Ayuda;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;

namespace ArmaduraLosaRevit.Model.EditarTipoPath
{





    public class CargarActualizarPathReinfoment_Wpf
    {
        public static SeleccionarPathReinfomentConPto SeleccionarPathReinfomentConPto { get; set; }
        //double angle = 16.3;
        //angle = 16.3;
        //angulo -9.22 indica pelota de losa esta girada -9.22grados, despues se hace la conversion grado radian
        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public static void ExecuteSoloActualizando_sinSeleccion(UIApplication _uiapp, UbicacionLosa tipolosa, TipoBarra tipobarra, string _TipoDireccionBarra, double diametro, double espaciamiento, SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto)
        {
            UIApplication uiapp = _uiapp;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;



            SeleccionarPathReinfomentConPto = _seleccionarPathReinfomentConPto;
            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            try
            {
                UtilitarioFallasDialogosCarga.Cargar(_uiapp);

                //cargardatos
                ActualizarDatosDeBarra(_uiapp, tipolosa, tipobarra, diametro, espaciamiento, _TipoDireccionBarra);

                //SeleccionarPathReinfomentConPto = _seleccionarPathReinfomentConPto;
                UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
                return;
            }
            catch (System.Exception ex)
            {
                UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
                string msje = ex.Message;
                return;
            }


        }


        public static void ExecuteSeleccionador_YActualizar(UIApplication _uiapp, UbicacionLosa tipolosa, TipoBarra tipobarra, double diametro, double espaciamiento, string idElement,bool isFundaciones = false)
        {
            UIApplication uiapp = _uiapp;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;




            //UtilitarioFallasAdvertencias utilfallas = new UtilitarioFallasAdvertencias();
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            try
            {
              
                SeleccionarPathReinfomentConPto = new SeleccionarPathReinfomentConPto(uiapp);

                if (!SeleccionarPathReinfomentConPto.InsertarPathReinforment(idElement, isFundaciones)) return;

                //cargardatos
                CopiarNuevosDatosSeleccionarPathReinfomentConPto(SeleccionarPathReinfomentConPto);
                //ActualizarDatosDeBarra(_uiapp, tipolosa, tipobarra, diametro, espaciamiento);
   
            }
            catch (System.Exception ex)
            {
    
                string msje = ex.Message;
            }
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            return;

        }
 
        private static bool ActualizarDatosDeBarra(UIApplication _uiapp, UbicacionLosa ubicacionLosa, TipoBarra tipobarra, double diametro, double espaciamiento, string _TipoDireccionBarra)
        {
            try
            {


                string tipoBarra_ = ObtenerCasoAoBDeAhorro.ConversortoS4(SeleccionarPathReinfomentConPto._tipobarra);

                VerificarTiposEncambiosDeBarra _VerificarTiposEncambiosDeBarra =  new VerificarTiposEncambiosDeBarra(tipobarra, EnumeracionBuscador.ObtenerEnumGenerico(TipoBarra.NONE, tipoBarra_));
                if (!_VerificarTiposEncambiosDeBarra.Verificar())
                    return false;

                TipoPathReinfDTO tipoPathReinf = new TipoPathReinfDTO(ubicacionLosa, tipobarra, _TipoDireccionBarra);
                PathReinformeCambioManejador pathReinformeTraslapo = new PathReinformeCambioManejador(_uiapp, SeleccionarPathReinfomentConPto);
                if (!pathReinformeTraslapo.M0_EjecutarCambioPath(tipoPathReinf, diametro, espaciamiento)) return false;

                CopiarNuevosDatosSeleccionarPathReinfomentConPto(tipobarra, diametro, espaciamiento, _TipoDireccionBarra, ubicacionLosa, pathReinformeTraslapo._pathReinforcement, pathReinformeTraslapo._pathReinforcementSymbol);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($" Error en '' ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private static void CopiarNuevosDatosSeleccionarPathReinfomentConPto(TipoBarra tipobarra,
            double diametro,
            double espaciamiento,
            string _TipoDireccionBarra,
            UbicacionLosa ubicacionLosa,
            PathReinforcement _pathReinforcement, PathReinSpanSymbol _pathReinforcementSymbol)
        {
   
            SeleccionarPathReinfomentConPto.PathReinforcement = _pathReinforcement;
            SeleccionarPathReinfomentConPto.PathReinforcementSymbol = _pathReinforcementSymbol;
            SeleccionarPathReinfomentConPto._diametro = diametro.ToString();
            SeleccionarPathReinfomentConPto._espaciamiento = espaciamiento.ToString();
         
            SeleccionarPathReinfomentConPto._tipobarra = tipobarra.ToString();
            SeleccionarPathReinfomentConPto._direccion = ubicacionLosa.ToString();
            SeleccionarPathReinfomentConPto._TipoDireccionBarra = _TipoDireccionBarra.ToString();
        }

        private static void CopiarNuevosDatosSeleccionarPathReinfomentConPto(SeleccionarPathReinfomentConPto _SeleccionarPathReinfomentConPto)
        {
            SeleccionarPathReinfomentConPto.PathReinforcement = _SeleccionarPathReinfomentConPto.PathReinforcement;
            SeleccionarPathReinfomentConPto.PathReinforcementSymbol = _SeleccionarPathReinfomentConPto.PathReinforcementSymbol;
            SeleccionarPathReinfomentConPto._diametro = _SeleccionarPathReinfomentConPto._diametro.ToString();
            SeleccionarPathReinfomentConPto._Prefijo_F = _SeleccionarPathReinfomentConPto._Prefijo_F.ToString();
            SeleccionarPathReinfomentConPto._espaciamiento = _SeleccionarPathReinfomentConPto._espaciamiento.ToString();
            SeleccionarPathReinfomentConPto._tipobarra = _SeleccionarPathReinfomentConPto._tipobarra.ToString();
            SeleccionarPathReinfomentConPto._direccion = _SeleccionarPathReinfomentConPto._direccion.ToString();
            SeleccionarPathReinfomentConPto._TipoDireccionBarra = _SeleccionarPathReinfomentConPto._TipoDireccionBarra.ToString();
        }


    }

}
