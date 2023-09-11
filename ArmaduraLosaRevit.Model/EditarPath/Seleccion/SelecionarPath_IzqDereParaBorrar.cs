using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;

namespace ArmaduraLosaRevit.Model.EditarPath.Seleccion
{
    public class SelecionarPath_IzqDereParaBorrar
    {
#pragma warning disable CS0169 // The field 'SelecionarPath_IzqDereParaBorrar._uiapp' is never used
        private readonly UIApplication _uiapp;
#pragma warning restore CS0169 // The field 'SelecionarPath_IzqDereParaBorrar._uiapp' is never used
#pragma warning disable CS0169 // The field 'SelecionarPath_IzqDereParaBorrar._PathReinSpanSymbol' is never used
        private readonly PathReinSpanSymbol _PathReinSpanSymbol;
#pragma warning restore CS0169 // The field 'SelecionarPath_IzqDereParaBorrar._PathReinSpanSymbol' is never used
#pragma warning disable CS0169 // The field 'SelecionarPath_IzqDereParaBorrar._seleccionarPathReinfomentConPto' is never used
        private SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto;
#pragma warning restore CS0169 // The field 'SelecionarPath_IzqDereParaBorrar._seleccionarPathReinfomentConPto' is never used
        private UIDocument uidoc;
        private SeleccionPath seleccionPath_1ParaExtender;
        private SeleccionPath seleccionPath_2ParaBorrar;

        

        public DireccionEdicionPathRein _DireccionEdicionPathRein { get; set; }
        public SeleccionPath newSeleccionPath_2ParaBorrar { get; set; }
        public SeleccionPath newSeleccionPath_1ParaExtender { get; set; }

        public SelecionarPath_IzqDereParaBorrar(UIDocument uidoc, SeleccionPath seleccionPath_1ParaExtender, SeleccionPath seleccionPath_2ParaBorrar)
        {
            this.uidoc = uidoc;
            this.seleccionPath_1ParaExtender = seleccionPath_1ParaExtender;
            this.seleccionPath_2ParaBorrar = seleccionPath_2ParaBorrar;
            this.newSeleccionPath_2ParaBorrar = seleccionPath_2ParaBorrar;
        }

        public bool ObtenerLadoMAsLejano()
        {
            double angulo = Util.AnguloEntre2PtosGrados_enPlanoXY(seleccionPath_1ParaExtender._coordenadaPath.p2, seleccionPath_1ParaExtender._coordenadaPath.p3);
            CrearTrasformadaSobreEjeZ _crearTrasformada = new CrearTrasformadaSobreEjeZ(seleccionPath_1ParaExtender._coordenadaPath.p2, angulo);


            XYZ pto2_paraextender = _crearTrasformada.EjecutarTransform(seleccionPath_1ParaExtender._coordenadaPath.p2);
            XYZ pto2_paraborrar = _crearTrasformada.EjecutarTransform(seleccionPath_2ParaBorrar._coordenadaPath.p2);


            if (pto2_paraextender.X< pto2_paraborrar.X)
            {
                newSeleccionPath_2ParaBorrar._direccionEdicionPathRein = DireccionEdicionPathRein.Derecha;
                newSeleccionPath_2ParaBorrar._lineBordeSeleccionadoInicial = Line.CreateBound(seleccionPath_2ParaBorrar._coordenadaPath.p3.GetXY0(), seleccionPath_2ParaBorrar._coordenadaPath.p4.GetXY0());  
            }
            else
            {
                newSeleccionPath_2ParaBorrar._direccionEdicionPathRein = DireccionEdicionPathRein.Izquierda;
                newSeleccionPath_2ParaBorrar._lineBordeSeleccionadoInicial = Line.CreateBound(seleccionPath_2ParaBorrar._coordenadaPath.p1.GetXY0(), seleccionPath_2ParaBorrar._coordenadaPath.p2.GetXY0());
            }
            return true;
        }
    }
}
