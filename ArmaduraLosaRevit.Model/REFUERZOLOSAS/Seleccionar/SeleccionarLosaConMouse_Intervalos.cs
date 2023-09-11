using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FILTROS;
using ArmaduraLosaRevit.Model.LosaArmadura.Geometria;
using ArmaduraLosaRevit.Model.RebarLosa.Servicio;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Intervalos;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.Seleccionar
{

    public class SeleccionarLosaConMouse_Intervalos : SeleccionarLosaConMouse
    {
        private XYZ ptoInicial;
        private XYZ ptoFinal;
        private List<DatosBordeLosaDTO> ListaDatosBordeLosaDTO;

        public SeleccionarLosaConMouse_Intervalos(UIApplication uiapp, bool isTest = false) : base(uiapp, isTest)
        {
            ListaDatosBordeLosaDTO = new List<DatosBordeLosaDTO>();
        }

        public List<DatosBordeLosaDTO> M5_ObtenerListaDatosBordeLosaDTO(DatosRefuerzoTipoBorde datosRefuerzoTipoBorde)
        {
            M5_1_OrdenarPto();
            try
            {
                if (datosRefuerzoTipoBorde.IsIntervalos)
                {
                    M5_2_GenerarIntervalos(datosRefuerzoTipoBorde);
                }
                else
                {
                    M5_3_GenerarUnIntervalos();
                }
            }
            catch (Exception)
            {
                ListaDatosBordeLosaDTO.Clear();
                return ListaDatosBordeLosaDTO;
            }
            return ListaDatosBordeLosaDTO;
        }
        //ordenar ptos  mayor y menor
        private void M5_1_OrdenarPto()
        {
            ptoInicial = _curvaBordeLosa.GetEndPoint(0);
            ptoFinal = _curvaBordeLosa.GetEndPoint(1);
            var result = Util.Ordena2Ptos(ptoInicial, ptoFinal);
            ptoInicial = result[0];
            ptoFinal = result[1];
        }
        private void M5_3_GenerarUnIntervalos()
        {
            DatosBordeLosaDTO _DatosBordeLosaDTO = new DatosBordeLosaDTO()
            {
                DireccionHaciaLosa = DireccionHaciaLosa,
                ptoInicial = _curvaBordeLosa.GetEndPoint(0),
                ptoFinal = _curvaBordeLosa.GetEndPoint(1)
            };
            ListaDatosBordeLosaDTO.Add(_DatosBordeLosaDTO);
        }

        private bool M5_2_GenerarIntervalos(DatosRefuerzoTipoBorde datosRefuerzoTipoBorde)
        {
            try
            {
                List<XYZ> ListaPtosPerimetroBarras = new List<XYZ>();
 
                ListaPtosPerimetroBarras.Add(ptoInicial);
                ListaPtosPerimetroBarras.Add(ptoFinal);
      
                XYZ PtoMouse = (ptoInicial + ptoFinal) / 2;

                ParametrosListaIntervalosDTo _ParametrosListaIntervalosDTo = new ParametrosListaIntervalosDTo()
                {
                    uiapp = _uiapp,

                    _ubicacionPtoMouse = UbicacionPtoMouse.centro,
                    _tipoPosicionMouse = TipoPosicionMouse.segunMouse,
                    _Iscaso_intervalo = true,
                    _diametroMM = (int)datosRefuerzoTipoBorde.diamtroBarraRefuerzo_MM,
                    _espaciamiento = Util.CmToFoot(datosRefuerzoTipoBorde.espacimientoEstribo_Cm),
                    PtoSeleccionMouse1 = PtoMouse,
                    ListaPtosPerimetroBarras = ListaPtosPerimetroBarras,
                    TipoBarra = "REFUERZO_BA_BORDE",
                    ubicacionEnlosa = UbicacionLosa.Inferior //valor colocado pro defecto
                };

                //NOTA FECHA:27-09-2021  CDV INDICO QUE LAS BARRAS MIENTRAS ES LA BARRA ESTE DENTRO DE LA LOSA TODO BIEN
                //SOLO PARA PENDIENTES MAS ALTAS ( 45°) SE DEBE HACER  TRASLAPO EN EL CAMBIO DE PENDIENTE
                GenerarListaIntervalosBordeLosa _GenerarListaIntervalos = new GenerarListaIntervalosBordeLosa(_ParametrosListaIntervalosDTo, DireccionHaciaLosa);
                if (!_GenerarListaIntervalos.M1_ObtenerIntervalosBordeLosa())
                    return false;


                //var dd = _GenerarListaIntervalos.ListaIntervalosDTO.Select(c => c.ListaIntervalos).ToList();
                ListaDatosBordeLosaDTO.AddRange(_GenerarListaIntervalos.ListaDatosIntervalosDTO);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener Lista de intervalos ex:{ex.Message}"); ;
                return false;
            }
            return true;
        }


        public bool M4_SeleccionarDosPtos()
        {
            try
            {
                IsSeleccionarConDosPtos = true;


                ObjectSnapTypes snapTypes = ObjectSnapTypes.Nearest;

                List<XYZ> lista2ptos = Util.Pick2Point(_uiapp.ActiveUIDocument, snapTypes);

                if (lista2ptos.Count != 2) return false;
                if (lista2ptos[0].DistanceTo(lista2ptos[1]) < Util.CmToFoot(10))
                {
                    Util.ErrorMsg("Distancia entre puntos de seleccion debe ser mayor a 10 cm");
                    return false;
                }
                var pto1 = ((Line)_curvaBordeLosa).ProjectExtendidaXY0(lista2ptos[0]);
                var pto2 = ((Line)_curvaBordeLosa).ProjectExtendidaXY0(lista2ptos[1]);

                _curvaBordeLosa = Line.CreateBound(pto2, pto1);

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }



    }
}
