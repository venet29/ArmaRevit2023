using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.BarraAreaPath.Busqueda;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.BarraAreaPath.Seleccion;

namespace ArmaduraLosaRevit.Model.BarraAreaPath.Intervalo
{
    public class InterrvalosPtosPathRebar
    {
        private readonly UIApplication _uiapp;
        private View _view;
        private readonly SeleccionMuroAreaPathDTO _seleccionMuroAreaPathDTO;
        protected DatosMallasAutoDTO _datosMallasDTO;
        private readonly List<Level> _listaLevel; //solo para dos ptos
        protected XYZ _OrigenView;
        protected XYZ _dIreccionMuroEnIgualSentidoRightDirection;
        protected List<XYZ> _listaPtos;
        private List<Level> ListaLevelIntervalo;
        private double _PtoInicioZ;
        private double _PtoFinalZ;
        private XYZ ptFin;
        private XYZ ptIni;
        private double DistanciaEntrePtosXY0;

        public XYZ _direccionMayor { get; set; }

        public List<List<Curve>> ListaCurvaPAthArea { get; set; }
        public List<IntervalosMallaDTO> ListaIntervalosMallaDTO { get; set; }
        public View3D _view3D_buscar { get; set; }


        public InterrvalosPtosPathRebar(UIApplication uiapp, List<Level> listaLevel, View3D view3D_buscar)
        {
            this._uiapp = uiapp;
            this._view = uiapp.ActiveUIDocument.ActiveView;
            this._view3D_buscar = view3D_buscar;
            this._listaLevel = listaLevel;

            _dIreccionMuroEnIgualSentidoRightDirection = _view.RightDirection;
            // _listaPtos = seleccionMuroAreaPathDTO.ListaPtosPAth;
            ListaCurvaPAthArea = new List<List<Curve>>();
            ListaIntervalosMallaDTO = new List<IntervalosMallaDTO>();
        }
        public InterrvalosPtosPathRebar(UIApplication uiapp, SeleccionMuroAreaPathDTO seleccionMuroAreaPathDTO, DatosMallasAutoDTO _datosMallasDTO, List<Level> listaLevel, View3D view3D_buscar)
        {
            this._uiapp = uiapp;
            this._view3D_buscar = view3D_buscar;
            this._seleccionMuroAreaPathDTO = seleccionMuroAreaPathDTO;
            this._datosMallasDTO = _datosMallasDTO;
            this._listaLevel = listaLevel;
            this._OrigenView = _seleccionMuroAreaPathDTO.OrigenView;
            _dIreccionMuroEnIgualSentidoRightDirection = seleccionMuroAreaPathDTO.DIreccionMuroEnIgualSentidoVIewSecction.Normalize();
            _listaPtos = seleccionMuroAreaPathDTO.ListaPtosPAth;
            ListaCurvaPAthArea = new List<List<Curve>>();
            ListaIntervalosMallaDTO = new List<IntervalosMallaDTO>();
        }

        public bool Ejecutar()
        {
            if (_listaPtos.Count < 2) return false;

            if (_listaPtos.Count == 2)
                M1_GenerarLista4Ptos();
            else
            {
                Util.ErrorMsg("Selecciona con mas de dos puntos aun no implementado");
                return false;
            }
            return true;
        }


        private bool M1_GenerarLista4Ptos()
        {
            //por mouse
            _PtoInicioZ = _listaPtos.Min(c => c.Z);
            _PtoFinalZ = _listaPtos.Max(c => c.Z);

            M1_1_ObtenerIntervalLevel();

            // nivel inferior
            if (_datosMallasDTO.tipoSeleccionInf == TipoSeleccionMouse.nivel)
            {//por nivel
                if (ListaLevelIntervalo.Count == 0) return false;
                _PtoInicioZ = ListaLevelIntervalo.Min(c => c.ProjectElevation);
            }

            //superior
            if (_datosMallasDTO.tipoSeleccionSup == TipoSeleccionMouse.nivel)
            {//por nivel
                if (ListaLevelIntervalo.Count == 0) return false;
                _PtoFinalZ = ListaLevelIntervalo.Max(c => c.ProjectElevation);
            }

            M1_2_ReordenarPuntosIniciales();

            M1_3_Generar4PtosMalla();

            return false;
        }

        private void M1_1_ObtenerIntervalLevel()
        {
            ListaLevelIntervalo = new List<Level>();
            try
            {
                ListaLevelIntervalo = _listaLevel.Where(ll => ll.ProjectElevation > _PtoInicioZ - Util.CmToFoot(50) &&
                                                                ll.ProjectElevation < _PtoFinalZ + Util.CmToFoot(50)).OrderBy(nn => nn.ProjectElevation).ToList();
            }
            catch (Exception)
            {
                ListaLevelIntervalo.Clear();
            }
        }
        private void M1_2_ReordenarPuntosIniciales()
        {


            XYZ aux_dire = (_listaPtos[1] - _listaPtos[0]).Normalize();
            if (Util.GetProductoEscalar(aux_dire, _dIreccionMuroEnIgualSentidoRightDirection) < 0)
            {
                ptFin = _listaPtos[0];
                ptIni = _listaPtos[1];
            }
            else
            {
                ptFin = _listaPtos[1];
                ptIni = _listaPtos[0];
            }

            DesplazarPtosPorEspesoresBArras();

            DistanciaEntrePtosXY0 = ptIni.GetXY0().DistanceTo(ptFin.GetXY0());
        }

        private void DesplazarPtosPorEspesoresBArras()
        {
            ptIni = ptIni - _dIreccionMuroEnIgualSentidoRightDirection * Util.MmToFoot(_datosMallasDTO.diametroH_mm);
            ptFin = ptFin + _dIreccionMuroEnIgualSentidoRightDirection * Util.MmToFoot(_datosMallasDTO.diametroH_mm);
        }

        #region Generar 4ptos mallas
        private void M1_3_Generar4PtosMalla()
        {
            if (ListaLevelIntervalo.Count() <= 2)
            {
                UnNivel();
            }
            else
            {
                DosoMasNivles();
            }
        }

        private void UnNivel()
        {
            double aux_PtoInicioZ = ptIni.Z;
            double aux_PtoFinalZ = ptFin.Z;

            BuscandoFundaciones buscandoFundaciones = new BuscandoFundaciones(_uiapp, _view3D_buscar, _datosMallasDTO.diametroV_mm);
            if (buscandoFundaciones.OBbtenerFundaciones(ptIni))
            {
                aux_PtoInicioZ = buscandoFundaciones.p1_porFundaciones.Z;
            }

            GenerarCurvaAreaPathTramoIntermedio(aux_PtoInicioZ, aux_PtoFinalZ);
        }

        private void DosoMasNivles()
        {
            TramosInicial();
            TramoIntermedio();
            TramosFinal();
        }


        private void TramosInicial()
        {

            double aux_PtoInicioZ = (_datosMallasDTO.tipoSeleccionSup == TipoSeleccionMouse.nivel
                                                  ? ListaLevelIntervalo[0].ProjectElevation
                                                  : ptIni.Z);
            double aux__PtoFinalZ = (_datosMallasDTO.tipoSeleccionSup == TipoSeleccionMouse.nivel
                                        ? ListaLevelIntervalo[0 + 1].ProjectElevation
                                        : Math.Min(ptFin.Z, ListaLevelIntervalo[0 + 1].ProjectElevation));

            XYZ p1 = new XYZ(ptIni.X, ptIni.Y, aux_PtoInicioZ);

            BuscandoFundaciones buscandoFundaciones = new BuscandoFundaciones(_uiapp, _view3D_buscar, _datosMallasDTO.diametroV_mm);
            if (buscandoFundaciones.OBbtenerFundaciones(p1))
            {
                p1 = buscandoFundaciones.p1_porFundaciones;
                aux_PtoInicioZ = p1.Z;
            }


            XYZ pto2_aux = p1 + DistanciaEntrePtosXY0 * _dIreccionMuroEnIgualSentidoRightDirection;
            XYZ p2 = new XYZ(pto2_aux.X, pto2_aux.Y, aux_PtoInicioZ);
            XYZ p3 = new XYZ(ptFin.X, ptFin.Y, aux__PtoFinalZ + UtilBarras.largo_L9_DesarrolloFoot_diamMM(_datosMallasDTO.diametroV_mm));
            XYZ pto4_aux = p3 + -DistanciaEntrePtosXY0 * _dIreccionMuroEnIgualSentidoRightDirection;
            XYZ p4 = new XYZ(pto4_aux.X, pto4_aux.Y, aux__PtoFinalZ + UtilBarras.largo_L9_DesarrolloFoot_diamMM(_datosMallasDTO.diametroV_mm));




            GenerarCurvaAreaPath2(p1, p2, p3, p4);
        }

        private void TramoIntermedio()
        {
            for (int i = 1; i < ListaLevelIntervalo.Count() - 2; i++)
            {
                double aux_PtoInicioZ = ListaLevelIntervalo[i].ProjectElevation;
                double aux__PtoFinalZ = ListaLevelIntervalo[i + 1].ProjectElevation;
                GenerarCurvaAreaPathTramoIntermedio(aux_PtoInicioZ, aux__PtoFinalZ);
            }
        }

        //ob1)Explicacion de los p1,p2,p3,p4
        private void GenerarCurvaAreaPathTramoIntermedio(double aux_PtoInicioZ, double aux__PtoFinalZ)
        {
            XYZ p1 = new XYZ(ptIni.X, ptIni.Y, aux_PtoInicioZ);
            XYZ pto2_aux = p1 + DistanciaEntrePtosXY0 * _dIreccionMuroEnIgualSentidoRightDirection;
            XYZ p2 = new XYZ(pto2_aux.X, pto2_aux.Y, aux_PtoInicioZ);
            XYZ p3 = new XYZ(ptFin.X, ptFin.Y, aux__PtoFinalZ + UtilBarras.largo_L9_DesarrolloFoot_diamMM(_datosMallasDTO.diametroV_mm));

            XYZ pto4_aux = p3 + -DistanciaEntrePtosXY0 * _dIreccionMuroEnIgualSentidoRightDirection;
            XYZ p4 = new XYZ(pto4_aux.X, pto4_aux.Y, aux__PtoFinalZ + UtilBarras.largo_L9_DesarrolloFoot_diamMM(_datosMallasDTO.diametroV_mm));

            GenerarCurvaAreaPath2(p1, p2, p3, p4);
        }

        private void TramosFinal()
        {

            double largoDesarrollo = 0;
            BuscarMuros BuscarMuros = new BuscarMuros(_uiapp, UtilBarras.largo_L9_DesarrolloFoot_diamMM(_datosMallasDTO.diametroV_mm));
            var (wallSeleccionado, espesor, ptoSobreMuro) = BuscarMuros.OBtenerRefrenciaMuro(_view3D_buscar, ptFin, new XYZ(0, 0, 1));


            if (wallSeleccionado == null)
            { largoDesarrollo = -ConstNH.RECUBRIMIENTO_PATA_BARRAV_Foot; }
            else
            { largoDesarrollo = UtilBarras.largo_L9_DesarrolloFoot_diamMM(_datosMallasDTO.diametroV_mm); }

            double aux_PtoInicioZ = ListaLevelIntervalo[ListaLevelIntervalo.Count - 2].ProjectElevation;

            double aux__PtoFinalZ = (_datosMallasDTO.tipoSeleccionSup == TipoSeleccionMouse.nivel
                                        ? ListaLevelIntervalo[ListaLevelIntervalo.Count - 1].ProjectElevation + largoDesarrollo
                                        : ptFin.Z - ConstNH.RECUBRIMIENTO_PATA_BARRAV_Foot);

            XYZ p1 = new XYZ(ptIni.X, ptIni.Y, aux_PtoInicioZ);
            XYZ pto2_aux = p1 + DistanciaEntrePtosXY0 * _dIreccionMuroEnIgualSentidoRightDirection;
            XYZ p2 = new XYZ(pto2_aux.X, pto2_aux.Y, aux_PtoInicioZ);
            XYZ p3 = new XYZ(ptFin.X, ptFin.Y, aux__PtoFinalZ);
            XYZ pto4_aux = p3 + -DistanciaEntrePtosXY0 * _dIreccionMuroEnIgualSentidoRightDirection;
            XYZ p4 = new XYZ(pto4_aux.X, pto4_aux.Y, aux__PtoFinalZ);

            GenerarCurvaAreaPath2(p1, p2, p3, p4);
        }

        private void GenerarCurvaAreaPath2(XYZ p1, XYZ p2, XYZ p3, XYZ p4)
        {

            SeleccionMuroAreaPath _seleccionMuroAreaPath = new SeleccionMuroAreaPath(_uiapp);
            if (!_seleccionMuroAreaPath.Ejecutar_SeleccionarMuroPtoAuto((p1 + p3) / 2, _view3D_buscar))
                return;

            List<Curve> SubListaCurvaPAthArea = new List<Curve>();
            SubListaCurvaPAthArea.Add(Line.CreateBound(p1, p2));
            SubListaCurvaPAthArea.Add(Line.CreateBound(p2, p3));
            SubListaCurvaPAthArea.Add(Line.CreateBound(p3, p4));
            SubListaCurvaPAthArea.Add(Line.CreateBound(p4, p1));
            ListaCurvaPAthArea.Add(SubListaCurvaPAthArea);

            _direccionMayor = (p2 - p1).GetXY0().Normalize();

            ListaIntervalosMallaDTO.Add(new IntervalosMallaDTO()
            {
                ListaCurvaPAthArea = SubListaCurvaPAthArea,
                _datosMallasDTO = this._datosMallasDTO,
                _muroSeleccionado = _seleccionMuroAreaPath._ElemetSelect,
                _muroSeleccionadoId = _seleccionMuroAreaPath._ElemetSelect.Id.IntegerValue
            });
        }
        #endregion

        public string GetInfo()
        {
            return $" p1:{ptIni.ToString()}  -  p2:{ptFin.ToString()}";
        }

    }
}
