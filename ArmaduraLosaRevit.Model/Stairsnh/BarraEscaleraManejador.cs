using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.RebarLosa.Geom;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.RebarLosa.Barras;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.RebarLosa.Barras.Tipo;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI.Selection;
using ArmaduraLosaRevit.Model.Stairsnh.Entidades;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Stairsnh.DTO;

namespace ArmaduraLosaRevit.Model.Stairsnh
{



    public class BarraEscaleraManejador

    {
        private readonly UIApplication _uiapp;
        private SeleccionarEscalera _seleccionarEscalera;
        private GeometrisStairAreaMax _geometrisStairMaxArea;

        public BarraEscaleraManejador(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            _seleccionarEscalera = new SeleccionarEscalera(_uiapp);
            _geometrisStairMaxArea = new GeometrisStairAreaMax(_uiapp);
        }
        public bool BarraInferioresEscalera(DatosFormularios _datosFormularios)
        {

            //obtiene geometria de los ptos
            //coordendas
            Stairs staris = _seleccionarEscalera.SeleccionarStairs(ObjectType.Element);

            //buscar panarface de menor rev
            if (staris != null) _geometrisStairMaxArea.M1_GetGEomPlanarFaceMAxiamaArea(staris);
            if (_geometrisStairMaxArea._servicioModificarCoordenadas== null)      return false;
         
            if (_geometrisStairMaxArea._servicioModificarCoordenadas.lista4ptos.Count != 4)
            {
                Util.ErrorMsg("Error al obtener las coordenadas de las barras");
                return false;
            }

            //longitud
            RebarInferiorDTO rebarInferiorDTO1 = _geometrisStairMaxArea.ObtenerGEometriaLong(TipoBarra.f3_incli_esc, UbicacionLosa.Izquierda);
            rebarInferiorDTO1.diametroMM = _datosFormularios.diaLongMM;
            rebarInferiorDTO1.espaciamientoFoot = Util.CmToFoot(_datosFormularios.espaciLongCm);
            rebarInferiorDTO1.AcortamientoEspesorSecundario =1;
            rebarInferiorDTO1.LargoPata = Util.CmToFoot(_datosFormularios.LargoPataEnLosaCm);

            if (rebarInferiorDTO1.IsOK == false) return false;

            //tag
            IGeometriaTag _newIGeometriaTag = new GeomeTagNull();

            //barra
            IRebarLosa rebarLosa = FactoryIRebarLosa.CrearIRebarLosa(_uiapp, rebarInferiorDTO1, _newIGeometriaTag);
            if (!rebarLosa.M1A_IsTodoOK()) return false;
            rebarLosa.M2A_GenerarBarra();

            //**********************************************************************************************************************
            //trasnversal
            RebarInferiorDTO rebarInferiorDTO2 = _geometrisStairMaxArea.ObtenerGEometriaTrasn(TipoBarra.f4_incli_esc, UbicacionLosa.Izquierda);
            rebarInferiorDTO2.diametroMM = _datosFormularios.diaTransMM;
            rebarInferiorDTO2.espaciamientoFoot = Util.CmToFoot(_datosFormularios.espaciTrasnCM);
            rebarInferiorDTO2.AcortamientoEspesorSecundario = 0;
            rebarInferiorDTO2.LargoPata = Util.CmToFoot(0);
            rebarInferiorDTO2.LargoPataF4 = Util.CmToFoot(25);

            if (rebarInferiorDTO2.IsOK == false) return false;

            //tag
            IGeometriaTag _newIGeometriaTag2 = new GeomeTagNull();

            //barra
            IRebarLosa rebarLosa2 = FactoryIRebarLosa.CrearIRebarLosa(_uiapp, rebarInferiorDTO2, _newIGeometriaTag2);
            if (!rebarLosa2.M1A_IsTodoOK()) return false;
            rebarLosa2.M2A_GenerarBarra();


            return true;
        }

        public bool BarraInferioresEscaleraF4(DatosFormularios _datosFormularios)
        {

            //obtiene geometria de los ptos
            //coordendas
            Stairs staris = _seleccionarEscalera.SeleccionarStairs(ObjectType.Element);

            //buscar panarface de menor 
            if (staris != null) _geometrisStairMaxArea.M1_GetGEomPlanarFaceMAxiamaArea(staris);

            if (_geometrisStairMaxArea._servicioModificarCoordenadas.lista4ptos.Count != 4)
            {
                Util.ErrorMsg("Error al obtener las coordenadas de las barras");
                return false;
            }

            //longitud
            //RebarInferiorDTO rebarInferiorDTO1 = _geometrisStairMaxArea.ObtenerGEometriaLong(TipoBarra.f3_incli_esc, UbicacionLosa.Izquierda);
            //rebarInferiorDTO1.diametroMM = _datosFormularios.diaLongMM;
            //rebarInferiorDTO1.espaciamientoFoot = Util.CmToFoot(_datosFormularios.espaciLongCm);
            //rebarInferiorDTO1.AcortamientoEspesorSecundario = 1;
            //rebarInferiorDTO1.LargoPata = Util.CmToFoot(_datosFormularios.LargoPataEnLosaCm);

            //if (rebarInferiorDTO1.IsOK == false) return false;

            ////tag
            //IGeometriaTag _newIGeometriaTag = new GeomeTagNull();

            ////barra
            //IRebarLosa rebarLosa = FactoryIRebarLosa.CrearIRebarLosa(_uiapp, rebarInferiorDTO1, _newIGeometriaTag);
            //if (!rebarLosa.M1A_IsTodoOK()) return false;
            //rebarLosa.M2A_GenerarBarra();

            //**********************************************************************************************************************
            //trasnversal
            RebarInferiorDTO rebarInferiorDTO2 = _geometrisStairMaxArea.ObtenerGEometriaTrasn(TipoBarra.f4_incli_esc, UbicacionLosa.Izquierda);
            rebarInferiorDTO2.diametroMM = _datosFormularios.diaTransMM;
            rebarInferiorDTO2.espaciamientoFoot = Util.CmToFoot(_datosFormularios.espaciTrasnCM);
            rebarInferiorDTO2.AcortamientoEspesorSecundario = 0;
            rebarInferiorDTO2.LargoPata = Util.CmToFoot(0);
            rebarInferiorDTO2.LargoPataF4 = Util.CmToFoot(25);

            if (rebarInferiorDTO2.IsOK == false) return false;

            //tag
            IGeometriaTag _newIGeometriaTag2 = new GeomeTagNull();

            //barra
            IRebarLosa rebarLosa2 = FactoryIRebarLosa.CrearIRebarLosa(_uiapp, rebarInferiorDTO2, _newIGeometriaTag2);
            if (!rebarLosa2.M1A_IsTodoOK()) return false;
            rebarLosa2.M2A_GenerarBarra();


            return true;
        }

    }
}
