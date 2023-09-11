using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Fund.DTO;
using ArmaduraLosaRevit.Model.Fund.Servicios;
using ArmaduraLosaRevit.Model.Fund.WPFfund.DTO;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Fund.Intervalos
{
    public class GenerarListaIntervalosFund : GenerarListaIntervalos
    {
        private TipoPataFund tipoPataFun;
        private double LArgoPataHOokIzq_cm;
        private double LArgoPataHOokDere_cm;
        private RebarHookType hookIZ;
        private RebarHookType hookDere;

        //private XYZ PtoMouse;
        //private XYZ PtoOriegenDireztriz;
        //private XYZ PtoDireccionDireztriz;
        public List<DatosIntervalosFundDTO> ListaDatosIntervalosDTO { get; set; }

        public GenerarListaIntervalosFund(ParametrosListaIntervalosDTo _parametrosListaIntervalosDTo, TipoPataFund tipoPataFun)
            : base(_parametrosListaIntervalosDTo)
        {

            this.tipoPataFun = tipoPataFun;
            //PtoMouse = _datosNuevaBarraDTOIniciales.PtoMouse;
            //PtoOriegenDireztriz = _datosNuevaBarraDTOIniciales.PtoOriegenDireztriz;
            //PtoDireccionDireztriz = _datosNuevaBarraDTOIniciales.PtoDireccionDireztriz;
            ListaDatosIntervalosDTO = new List<DatosIntervalosFundDTO>();
            factor = 0;
        }

        public bool M1_ObtenerIntervalosFund()
        {
            try
            {

                (RebarHookType hookIZ, RebarHookType hookDere, double LArgoPataHOokIzq_cm, double LArgoPataHOokDere_cm, bool IsOK) =
                         AyudaOBtenerHookYLargo.ObtenerHookFundaciones(_uiapp.ActiveUIDocument.Document, diametroMM);
                if (!IsOK) return false;
                this.LArgoPataHOokIzq_cm = LArgoPataHOokIzq_cm;
                this.LArgoPataHOokDere_cm = LArgoPataHOokDere_cm;
                this.hookIZ = hookIZ;
                this.hookDere = hookDere;
                //se ejecuta en GenerarListaIntervalos
                if (!M1_ObtenerIntervalos()) return false;


                switch (tipoPataFun)
                {
                    case TipoPataFund.IzqInf:
                        CasoIzqInf();
                        break;
                    case TipoPataFund.DereSup:
                        CasoDereSup();
                        break;
                    case TipoPataFund.Ambos:
                        CasoAmbos();
                        break;
                    case TipoPataFund.Sin:
                        CasoSin();
                        break;

                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        private void CasoSin()
        {
            for (int i = 0; i < ListaIntervalosDTO.Count; i++)
            {
                GenerarListaIntervalosDTo item = ListaIntervalosDTO[i];
                if (item.ListaIntervalos.Count != 4) continue;

                DatosIntervalosFundDTO _DatosIntervalosDTO = new DatosIntervalosFundDTO();
                _DatosIntervalosDTO.tipoPataFun = TipoPataFund.Sin;
                Agregar_enLista(i, item, _DatosIntervalosDTO);
            }
        }



        private void CasoAmbos()
        {
            for (int i = 0; i < ListaIntervalosDTO.Count; i++)
            {
                GenerarListaIntervalosDTo item = ListaIntervalosDTO[i];

                if (item.ListaIntervalos.Count != 4) continue;
                DatosIntervalosFundDTO _DatosIntervalosDTO = new DatosIntervalosFundDTO();

                if (0 == i)
                {
                    _DatosIntervalosDTO.tipoPataFun = TipoPataFund.IzqInf;
                    _DatosIntervalosDTO.LargoPAtaIzqHook = LArgoPataHOokIzq_cm;
                    _DatosIntervalosDTO.LargoPAtaDereHook = 0;
                    _DatosIntervalosDTO.rebarHookTypePrincipal_end = hookIZ;
                    _DatosIntervalosDTO.rebarHookTypePrincipal_star = null;
                }
                else if (ListaIntervalosDTO.Count - 1 == i)
                {
                    _DatosIntervalosDTO.tipoPataFun = TipoPataFund.DereSup;
                    _DatosIntervalosDTO.LargoPAtaIzqHook = 0;
                    _DatosIntervalosDTO.LargoPAtaDereHook = LArgoPataHOokDere_cm;
                    _DatosIntervalosDTO.rebarHookTypePrincipal_end = null;
                    _DatosIntervalosDTO.rebarHookTypePrincipal_star = hookDere;
                }
                else
                    _DatosIntervalosDTO.tipoPataFun = TipoPataFund.Sin;
                Agregar_enLista(i, item, _DatosIntervalosDTO);
            }
        }

        private void CasoDereSup()
        {

            for (int i = 0; i < ListaIntervalosDTO.Count; i++)
            {
                GenerarListaIntervalosDTo item = ListaIntervalosDTO[i];

                if (item.ListaIntervalos.Count != 4) continue;
                DatosIntervalosFundDTO _DatosIntervalosDTO = new DatosIntervalosFundDTO();

                if (ListaIntervalosDTO.Count - 1 == i)
                {
                    _DatosIntervalosDTO.tipoPataFun = TipoPataFund.DereSup;
                    _DatosIntervalosDTO.LargoPAtaIzqHook = 0;
                    _DatosIntervalosDTO.LargoPAtaDereHook = LArgoPataHOokDere_cm;
                    _DatosIntervalosDTO.rebarHookTypePrincipal_end = null;
                    _DatosIntervalosDTO.rebarHookTypePrincipal_star = hookDere;
                }
                else
                    _DatosIntervalosDTO.tipoPataFun = TipoPataFund.Sin;
                Agregar_enLista(i, item, _DatosIntervalosDTO);
            }

        }

        private void CasoIzqInf()
        {
            for (int i = 0; i < ListaIntervalosDTO.Count; i++)
            {
                GenerarListaIntervalosDTo item = ListaIntervalosDTO[i];

                if (item.ListaIntervalos.Count != 4) continue;
                DatosIntervalosFundDTO _DatosIntervalosDTO = new DatosIntervalosFundDTO();

                if (0 == i)
                {
                    _DatosIntervalosDTO.tipoPataFun = TipoPataFund.IzqInf;
                    _DatosIntervalosDTO.LargoPAtaIzqHook = LArgoPataHOokIzq_cm;
                    _DatosIntervalosDTO.LargoPAtaDereHook = 0;
                    _DatosIntervalosDTO.rebarHookTypePrincipal_end = hookIZ;
                    _DatosIntervalosDTO.rebarHookTypePrincipal_star = null;

                }
                else
                    _DatosIntervalosDTO.tipoPataFun = TipoPataFund.Sin;
                Agregar_enLista(i, item, _DatosIntervalosDTO);
            }
        }

        private void Agregar_enLista(int i, GenerarListaIntervalosDTo item, DatosIntervalosFundDTO _datosIntervalosDTO)
        {
            XYZ direccionAbajo = -(item.ListaIntervalos[1] - item.ListaIntervalos[0]).Normalize();
            XYZ direccionDerecha = (item.ListaIntervalos[2] - item.ListaIntervalos[1]).Normalize();


            _datosIntervalosDTO._generarListaIntervalosDTo = item;
            _datosIntervalosDTO._listaptos = item.ListaIntervalos;

            XYZ auxPtoMouse = CrearListaPtos.ObtenerCentroPoligono(item.ListaIntervalos);
            _datosIntervalosDTO.PtoMouse = auxPtoMouse + (Util.IsImPar(i) ? Util.CmToFoot(3) * direccionAbajo : XYZ.Zero);

            _datosIntervalosDTO.LeaderEnd = auxPtoMouse + direccionDerecha * FactoresLargoLeader.FactorDesplazaminetoPotFree_foot * _view.Scale / 50;
            _datosIntervalosDTO.PtoCodoDireztriz = auxPtoMouse + direccionAbajo * FactoresLargoLeader.FactorLargoCOdo_foot*_view.Scale/50 + direccionDerecha * FactoresLargoLeader.FactorDesplazaminetoPotFree_foot * _view.Scale / 50;
            _datosIntervalosDTO.PtoDireccionDireztriz = _datosIntervalosDTO.PtoCodoDireztriz + direccionDerecha * Util.CmToFoot(10) * _view.Scale / 50;
            _datosIntervalosDTO.PtoTag = _datosIntervalosDTO.PtoCodoDireztriz + -direccionDerecha * FactoresLargoLeader.FactorDesplazaminetoPotFree_foot * _view.Scale / 50;
            _datosIntervalosDTO.LargoPAtaIzqHook = _datosIntervalosDTO.LargoPAtaIzqHook;
            _datosIntervalosDTO.LargoPAtaDereHook = _datosIntervalosDTO.LargoPAtaDereHook;

            ListaDatosIntervalosDTO.Add(_datosIntervalosDTO);
        }
    }
}
