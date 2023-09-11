using ArmaduraLosaRevit.Model.BarraV.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Model;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Calculos
{
    public class GruposListasEstribo_H
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;
        private List<RebarDesglose> lista_RebarDesglose;
    
        public DatosHost _DatosHost { get; set; }

        public List<RebarDesglose_GrupoBarras_H> GruposRebarMismaLinea { get; set; }
      //  public PlanarFace CaraInferior { get; private set; }
      //  public XYZ CentroHost { get; private set; }
       // public double LargoMAximoHost_foot { get; private set; }
        public List<XYZ> ListaPtoSeccion { get; private set; }
        public List<RebarDesglose_Barras_H> listaBArrasEnElev { get; private set; }

        public GruposListasEstribo_H(UIApplication uiapp, List<RebarDesglose> lista_RebarDesglose)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this.lista_RebarDesglose = lista_RebarDesglose;
            this.GruposRebarMismaLinea = new List<RebarDesglose_GrupoBarras_H>();
        }

   
        public bool ObtenerGruposEstribo()
        {

            List<RebarDesglose_Barras_H> listaBArras = lista_RebarDesglose.Where(c => c._tipoBarraEspecifico == TipoRebar.ELEV_ES_VT ||
                                                                                    c._tipoBarraEspecifico == TipoRebar.ELEV_ES_V)
                                                                         .Select(c => new RebarDesglose_Barras_H(c, _uiapp)).ToList();

            if (listaBArras.Count == 0) return true;
            //obtener RebarDesglose_Barras
            foreach (RebarDesglose_Barras_H item in listaBArras)
            {
                // item.Ordenar_UltimaCurvaMayorZ();
                // item.Ordenar_Analizar();
                item.Ordenar_AnalizarEstribo();
            }

            //

            _DatosHost = new DatosHost( _uiapp, listaBArras[0]._rebarDesglose);
            if (!_DatosHost.ObtenerPtoMedio_conestribo()) return false;
            if (!_DatosHost.ObtenerCentroPilarOmUro()) return false;

            //ObtenerCentroPilarOmUro();
            Obtener2PTOSCrearSeccion(listaBArras[0]);
            // ordenar de los inicial menor y solo verticales

            listaBArras = listaBArras.OrderBy(c => c.ptoInicial.Z).ToList();

            try
            {
                for (int i = 0; i < listaBArras.Count; i++)
                {
                    RebarDesglose_Barras_H item = listaBArras[i];

                    RebarDesglose_Barras_H BarraAnalizada = item;
                    List<RebarDesglose_Barras_H> NuewGrupoBarras = new List<RebarDesglose_Barras_H>();
                    if (item.analizadasuperior) continue;

                    item.analizadasuperior = true;
                    NuewGrupoBarras.Add(item);
                    RebarDesglose_GrupoBarras_H _RebarDesglose_GrupoBarrasNew = null;

                    //busca dentro del gurpo colineal
                    for (int j = 0; j < listaBArras.Count; j++)
                    {
                        RebarDesglose_Barras_H estriboAnalizado = listaBArras[j];
                        if (estriboAnalizado.analizadasuperior) continue;
                        // cuando el pto inicial de la sigueinte barra no esta contendia en la actual

                        if (item.ptoInicial.Z < estriboAnalizado.ptoMedio.Z && item.ptoFinal.Z > estriboAnalizado.ptoMedio.Z)
                        {
                            estriboAnalizado.analizadasuperior = true;
                            NuewGrupoBarras.Add(estriboAnalizado);
                        }
                    }

                    _RebarDesglose_GrupoBarrasNew = RebarDesglose_GrupoBarras_H.Creador_RebarDesglose_GrupoBarras(NuewGrupoBarras);
                    GruposRebarMismaLinea.Add(_RebarDesglose_GrupoBarrasNew);
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener grupos de barras  ex:{ ex.Message} ");
                return false;
            }
            return true;
        }

        public bool ObteneGruposDeBarraEnELev()
        {
            try
            {
                listaBArrasEnElev = lista_RebarDesglose.Where(c => c._tipoBarraEspecifico == TipoRebar.ELEV_BA_V)
                                                                         .Select(c => new RebarDesglose_Barras_H(c,_uiapp)).ToList();
                //obtener RebarDesglose_Barras
                foreach (RebarDesglose_Barras_H item in listaBArrasEnElev)
                {
                    item.Ordenar_UltimaCurvaMayorZ();
                    item.Ordenar_Analizar();
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                Util.ErrorMsg($"Error al obtener barras en elevacion de corte ex:{ex.Message}");
                return false;
            }
            return true;

        }




        private void Obtener2PTOSCrearSeccion(RebarDesglose_Barras_H _RebarDesglose_Barras)
        {



            XYZ ptoInicia = _DatosHost.CentroHost - _DatosHost.Direccion_ParalelaView * _DatosHost.LargoMAximoHost_foot * 1;
            XYZ ptoFin = _DatosHost.CentroHost + _DatosHost.Direccion_ParalelaView * _DatosHost.LargoMAximoHost_foot * 3;

            if (AyudaCurveRebar.GetMitadRebarCurves(_RebarDesglose_Barras._rebarDesglose._rebar))
            {
                List<Curve> listaCurva = AyudaCurveRebar.curvaMedia;

                XYZ centroSUma = XYZ.Zero;
                foreach (Curve item in listaCurva)
                {
                    centroSUma = (centroSUma + item.ComputeDerivatives(0.5, false).Origin);
                }
                centroSUma = centroSUma / listaCurva.Count;

                ptoInicia = centroSUma - _DatosHost.Direccion_ParalelaView * _DatosHost.LargoMAximoHost_foot * 1;
                ptoFin = centroSUma + _DatosHost.Direccion_ParalelaView * _DatosHost.LargoMAximoHost_foot * 1;

                // double zaltura = AyudaCurveRebar.curvaMedia[0].GetEndPoint(0).Z;
                //ptoInicia = ptoInicia.AsignarZ(zaltura);
                // ptoFin = ptoFin.AsignarZ(zaltura);
            }


            ListaPtoSeccion = new List<XYZ>();
            ListaPtoSeccion.Add(ptoInicia);
            ListaPtoSeccion.Add(ptoFin);

        }

    }
}
