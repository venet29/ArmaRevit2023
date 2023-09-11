using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.ViewRang;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using ArmaduraLosaRevit.Model.ParametrosShare;

namespace ArmaduraLosaRevit.Model.ConfiguracionInicial
{
    public class ConfiguracionInicialParametros
    {
        private UIApplication _uiapp;
        private Document _doc;

        public ConfiguracionInicialParametros(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document; 
        }

        public void ReiniciarViewRange()
        {
             View view = _uiapp.ActiveUIDocument.Document.ActiveView;
            var scale = view.Scale;

            var  viewPlant= view as ViewPlan;
            if (null == viewPlant)
            {
                Util.ErrorMsg("Debe ejecutar comando en un ViewPlan");
                return;
            }
                View3D elem3d = view as View3D;
            if (null != elem3d)
            {
                Util.ErrorMsg("Only create dimensions in 2D");
             
                return ;
            }

            IViewRangleClase viewRangleClase = ViewRangleClase.CreatorNuevoViewRange(_doc, view);
            viewRangleClase.EditarParametroViewRange(ConstNH.VIEWRANGE_TOP, ConstNH.VIEWRANGE_CORTE, ConstNH.VIEWRANGE_BOTTON, ConstNH.VIEWRANGE_DEPTH);
         
        }

        public  bool AgregarParametrosShareLosa()
        {
            ManejadorCrearParametrosShare _definicionManejador = new ManejadorCrearParametrosShare(_uiapp, RutaArchivoCompartido: "ParametrosNH");
            if (!_definicionManejador.EjecutarLosa()) return false;
            return true;
        }
        public bool AgregarParametrosShareElevacion()
        {
            ManejadorCrearParametrosShare _definicionManejador = new ManejadorCrearParametrosShare(_uiapp, RutaArchivoCompartido: "ParametrosNH");
            if (!_definicionManejador.EjecutarElevacion()) return false;


            return true;
        }
        public bool AgregarParametrosShareElevacionElementos()
        {
            ManejadorCrearParametrosShare _definicionManejador = new ManejadorCrearParametrosShare(_uiapp, RutaArchivoCompartido: "ParametrosNH");
           
            if (!_definicionManejador.EjecutarElementos()) return false;


            return true;
        }

        // no se utilizo  para  no se aplica a todas los tipos shaft
        public bool AgregarParametrosSharePasadas()
        {
            ManejadorCrearParametrosShare _definicionManejador = new ManejadorCrearParametrosShare(_uiapp, RutaArchivoCompartido: "ParametrosNH");
            if (!_definicionManejador.EjecutarPasadas()) return false;
            return true;
        }

        public bool AgregarParametrosShareBIM()
        {
            ManejadorCrearParametrosShare _definicionManejador = new ManejadorCrearParametrosShare(_uiapp, RutaArchivoCompartido: "ParametrosNH");
            if (!_definicionManejador.EjecutarBIM()) return false;
            return true;
        }

        public bool AgregarParametrosShareDesglose()
        {
            ManejadorCrearParametrosShare _definicionManejador = new ManejadorCrearParametrosShare(_uiapp, RutaArchivoCompartido: "ParametrosNH");
            if (!_definicionManejador.EjecutarDesglose()) return false;
            return true;
        }

        public bool AgregarParametrosShareView()
        {
            ManejadorCrearParametrosShare _definicionManejador = new ManejadorCrearParametrosShare(_uiapp, RutaArchivoCompartido: "ParametrosNH");
            if (!_definicionManejador.EjecutarView()) return false;
            return true;
        }

        public bool AgregarParametrosShareEscalera()
        {
            ManejadorCrearParametrosShare _definicionManejador = new ManejadorCrearParametrosShare(_uiapp, RutaArchivoCompartido: "ParametrosNH");
            if (!_definicionManejador.EjecutarEscalera()) return false;
            return true;
        }
        public bool AgregarParametrosShareFundaciones()
        {
            ManejadorCrearParametrosShare _definicionManejador = new ManejadorCrearParametrosShare(_uiapp, RutaArchivoCompartido: "ParametrosNH");
            if (!_definicionManejador.EjecutarFundaciones()) return false;
            return true;
        }
    }


}
