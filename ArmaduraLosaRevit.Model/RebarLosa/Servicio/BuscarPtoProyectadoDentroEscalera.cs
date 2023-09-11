using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Stairsnh.Entidades;
using ArmaduraLosaRevit.Model.Stairsnh.Servicio;
using ArmaduraLosaRevit.Model.UTILES.CreaLine;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.RebarLosa.Servicio
{
    public  class BuscarPtoProyectadoDentroEscalera
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        private PlanarFace _faceInferior;
        private Stairs _starisEncontrado;

        public double anguloDeCaraInferior { get; set; }
        public BuscarPtoProyeccionEnEscalera _buscarPtbProyeccionEnEscalera { get; set; }

        public BuscarPtoProyectadoDentroEscalera(UIApplication uiapp)
        {
            this._uiapp = uiapp; 
            this._doc = uiapp.ActiveUIDocument.Document;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PtoBusuqedaInicial">XYZ PtoBusuqedaInicial = new XYZ(-15.40, -3.034, 26.25);</param>
        /// <param name="direccionBusqueda">new XYZ(1,0,0)</param>
        /// <returns></returns>
        public bool M1_BuscarPtoProyectadoEnCaraSuperiorEScalera(XYZ PtoBusuqedaInicial, XYZ direccionBusqueda)
        {

            //e) crear vector SOLO TEMA VISUAL
            //CrearLIneaAux _crearLIneaAux = new CrearLIneaAux(_doc, PtoBusuqedaInicial , PtoBusuqedaInicial+ direccionBusqueda*2);
            //_crearLIneaAux.CrearLinea();

            //a)buscar escaleray cara inferior
            if (!M3_buscarEscalerYCaraInf(PtoBusuqedaInicial, direccionBusqueda)) return false;

            //b) angulo
            M4_buscarAnguloCaraINferior();

            //c) obtener espesor de losa
            //faltaImplementar


            //d) proyectar pto
            double espesorEscaleraFoot = Util.CmToFoot(15);
            _buscarPtbProyeccionEnEscalera = new BuscarPtoProyeccionEnEscalera(_uiapp, _faceInferior);
            _buscarPtbProyeccionEnEscalera.BuscarProyeccionEnCaraInSuperior(PtoBusuqedaInicial, espesorEscaleraFoot);


            return true;
        }

 

        public bool M1_BuscarPtoProyectadoEnCaraInferiorEScalera(XYZ PtoBusuqedaInicial, XYZ direccionBusqueda)
        {

            //a)buscar escaleray cara inferior
            if (!M3_buscarEscalerYCaraInf(PtoBusuqedaInicial, direccionBusqueda)) return false;
            //b) angulo
            M4_buscarAnguloCaraINferior();

            //c) obtener espesor de losa
            //faltaImplementar

            //d) proyectar pto
            double espesorRecubrimientoRebar =  ConstNH.DESPLAZAMIENTO_BAJO_Z_REBAR_FOOT;
            _buscarPtbProyeccionEnEscalera = new BuscarPtoProyeccionEnEscalera(_uiapp, _faceInferior);
            _buscarPtbProyeccionEnEscalera.BuscarProyeccionEnCaraInferior(PtoBusuqedaInicial, espesorRecubrimientoRebar);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PtoBusuqedaInicial"></param>
        /// <param name="direccionBusqueda"></param>
        /// <returns></returns>
        private bool  M3_buscarEscalerYCaraInf(XYZ PtoBusuqedaInicial, XYZ direccionBusqueda)
        {

            View3D elem3d = TiposFamilia3D.Get3DBuscar(_doc);
            if (elem3d == null)
            {
                Util.ErrorMsg("Error, favor cargar configuracion inicial");
                return false;
            }

            BuscarEscaleraHorizontalmente _buscarEscaleraHorizontalmente = new BuscarEscaleraHorizontalmente(_uiapp);
            _starisEncontrado = _buscarEscaleraHorizontalmente.buscarEscaleraHorizontal(elem3d, PtoBusuqedaInicial, direccionBusqueda, false);

            if (_starisEncontrado == null) return false;

            //b)buscar panarface de menor 
            GeometrisStairAreaMax _geometrisStairMaxArea = new GeometrisStairAreaMax(_uiapp);
            //_geometrisStair.M1_GetGEom(staris);
            _geometrisStairMaxArea.M1_GetGEomPlanarFaceMAxiamaArea(_starisEncontrado);

            _faceInferior = _geometrisStairMaxArea.GetPlanarFaceMaxArea();
            if (_faceInferior == null) return false;

            return true;
        }

        private void M4_buscarAnguloCaraINferior()
        {
            if (_faceInferior == null)
            {
                anguloDeCaraInferior = 32.1;
                return;
            }

            XYZ direccioncaraInf = (Math.Abs(_faceInferior.XVector.Z) < 0.01 ? _faceInferior.YVector : _faceInferior.XVector);

            anguloDeCaraInferior= direccioncaraInf.GetAngleEnZ_respectoPlanoXY()* direccioncaraInf.Z/Math.Abs(direccioncaraInf.Z);
        }
    }
}
