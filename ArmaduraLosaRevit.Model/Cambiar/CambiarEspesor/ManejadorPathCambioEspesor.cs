using ArmaduraLosaRevit.Model;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
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
    public interface IManejadorPathCambioEspesor
    {
        int cantidadPathCambiado { get; set; }
        View _view { get; set; }
        List<PathReinforcement> M3_SelecconarTodoslosPAthDeLosaSeleccionado(Floor _selecFloor);

        void M4_ObtenerLosEspesoresCAmbiar(List<PathReinforcement> _listaDePathReinDelosa);

        void M5_CAmbiarPArametroEspesorPath(double _espesorLosaFoot);

        void M5B_CAmbiarREbarShapeCuadrado();
    }
    public class ManejadorPathCambioEspesor : IManejadorPathCambioEspesor
    {

        private readonly List<PathCambioEspesorDTO> _listaPathCambioEspesorDTO;
        private Document _doc;
        private UIDocument _uidoc;

        public View _view { get; set; }
        private readonly bool isTest;
        public int cantidadError { get; set; }
        public int cantidadPathCambiado { get; set; }

        public ManejadorPathCambioEspesor(ExternalCommandData commandData, bool isTest = false)
        {
            this._doc = commandData.Application.ActiveUIDocument.Document;
            this._uidoc = commandData.Application.ActiveUIDocument;
            this._view = this._doc.ActiveView;
            this._listaPathCambioEspesorDTO = new List<PathCambioEspesorDTO>();
            this.isTest = isTest;

            cantidadPathCambiado = 0;
            cantidadError = 0;
        }



        public List<PathReinforcement> M3_SelecconarTodoslosPAthDeLosaSeleccionado(Floor _selecFloor)
        {

            List<Element> _listaDePathReinfNivelActual = M3_1_buscarListaPathReinFamiliasEnBrowser();
            List<PathReinforcement> _listaDePathReinDelosa = _listaDePathReinfNivelActual.Cast<PathReinforcement>().Where(cc => cc.GetHostId().IntegerValue == _selecFloor.Id.IntegerValue).ToList();
            return _listaDePathReinDelosa;
        }

        private List<Element> M3_1_buscarListaPathReinFamiliasEnBrowser()
        {
            ElementClassFilter f1 = new ElementClassFilter(typeof(FamilyInstance));
            ElementCategoryFilter f2 = new ElementCategoryFilter(BuiltInCategory.OST_PathRein);
            LogicalAndFilter f3 = new LogicalAndFilter(f1, f2);
            FilteredElementCollector _collectorPathReinfNivelActual = new FilteredElementCollector(_uidoc.Document, _view.Id);
            //para las familias en el browser
            //_collectorPathReinfNivelActual.OfCategory(BuiltInCategory.OST_PathRein).WhereElementIsElementType();

            //para las instacias de familia en el planonhs
            _collectorPathReinfNivelActual.OfCategory(BuiltInCategory.OST_PathRein).WhereElementIsNotElementType();
            //int elem = _collectorPathReinfNivelActual.GetElementCount();

            List<Element> _listaDePathReinfNivelActual = _collectorPathReinfNivelActual.ToElements() as List<Element>;

            return _listaDePathReinfNivelActual;
        }



        public void M4_ObtenerLosEspesoresCAmbiar(List<PathReinforcement> _listaDePathReinDelosa)
        {
            foreach (PathReinforcement pathRein in _listaDePathReinDelosa)
            {
                PathCambioEspesorDTO pathCambioEspesorDTO = new PathCambioEspesorDTO(pathRein);
                pathCambioEspesorDTO.CalculosIniciales();
                _listaPathCambioEspesorDTO.Add(pathCambioEspesorDTO);
            }
        }
        public void M5_CAmbiarPArametroEspesorPath(double _espesorLosaFoot)

        {
            int idAnalizado = 0;
            try
            {
                using (Transaction trans = new Transaction(_uidoc.Document))
                {
                    trans.Start("CAmbiar Espesor-NH");
                    foreach (var PathDto in _listaPathCambioEspesorDTO)
                    {
                        if (PathDto.IsCorrect == false) continue;

                        cantidadPathCambiado += 1;
                        idAnalizado = PathDto.PathRein.Id.IntegerValue;

                        Parameter largoPrimaria = ParameterUtil.FindParaByName(PathDto.PathRein.Parameters, "EsPrincipal");
                        //si es principal devuelve 1 del valor boolano yes, se aprovecha para usar como el cm q se agerga al la barra principal
                        //double espesorAdicional_BarraPrincipal = Util.CmToFoot(largoPrimaria.AsInteger());


                        if (largoPrimaria.AsInteger() == 1)
                            ParameterUtil.SetParaStringNH(PathDto.PathRein, "TipoDireccionBarra",  ConstNH.NOMBRE_BARRA_PRINCIPAL);
                        else
                            ParameterUtil.SetParaStringNH(PathDto.PathRein, "TipoDireccionBarra", ConstNH.NOMBRE_BARRA_SECUNADARIA);

                        double espesorAdicional_BarraPrincipal = (largoPrimaria.AsInteger() == 1 ? 0 : ConstNH.CONST_DESPLAZA_PORLUZSECUNDARIO_FOOT);// Util.CmToFoot((PathDto.Diametro_mm / 10.0f)));

                        for (int i = 0; i < PathDto.ListaEspesores.Count; i++)
                        {
                            ParameterUtil.SetParaInt(PathDto.PathRein, PathDto.ListaEspesores[i], _espesorLosaFoot - espesorAdicional_BarraPrincipal);
                        }
                    }
                    trans.Commit();
                }
            }
            catch (Exception)
            {
                cantidadError += 1;
                if (!isTest) Util.ErrorMsg("Error al cambiar paramatro de pathID:" + idAnalizado);

            }
        }

        public void M5B_CAmbiarREbarShapeCuadrado()

        {
            int idAnalizado = 0;
            try
            {
                using (Transaction trans = new Transaction(_uidoc.Document))
                {
                    trans.Start("CAmbiar Espesor-NH");
                    foreach (var PathDto in _listaPathCambioEspesorDTO)
                    {
                        if (PathDto.IsCorrect == false) continue;

                       //  FactoryCambiarRebarShape.Ejecutar(PathDto.TipoBarra);

                        cantidadPathCambiado += 1;
                        idAnalizado = PathDto.PathRein.Id.IntegerValue;

                        Parameter largoPrimaria = ParameterUtil.FindParaByName(PathDto.PathRein.Parameters, "EsPrincipal");
                        //si es principal devuelve 1 del valor boolano yes, se aprovecha para usar como el cm q se agerga al la barra principal
                        //double espesorAdicional_BarraPrincipal = Util.CmToFoot(largoPrimaria.AsInteger());


                        if (largoPrimaria.AsInteger() == 1)
                            ParameterUtil.SetParaStringNH(PathDto.PathRein, "TipoDireccionBarra", ConstNH.NOMBRE_BARRA_PRINCIPAL);
                        else
                            ParameterUtil.SetParaStringNH(PathDto.PathRein, "TipoDireccionBarra", ConstNH.NOMBRE_BARRA_SECUNADARIA);

                        double espesorAdicional_BarraPrincipal = (largoPrimaria.AsInteger() == 1 ? 0 : ConstNH.CONST_DESPLAZA_PORLUZSECUNDARIO_FOOT);// Util.CmToFoot((PathDto.Diametro_mm / 10.0f)));

                        for (int i = 0; i < PathDto.ListaEspesores.Count; i++)
                        {
                            ParameterUtil.SetParaInt(PathDto.PathRein, PathDto.ListaEspesores[i], espesorAdicional_BarraPrincipal);
                        }
                    }
                    trans.Commit();
                }
            }
            catch (Exception)
            {
                cantidadError += 1;
                if (!isTest) Util.ErrorMsg("Error al cambiar paramatro de pathID:" + idAnalizado);

            }
        }

    }




}
