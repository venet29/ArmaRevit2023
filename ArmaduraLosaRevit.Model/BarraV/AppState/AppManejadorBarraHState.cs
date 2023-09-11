using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.BarraV.AppState
{
    public class AppManejadorBarraHState
    {
        public static Element ElementoSelecionado { get; internal set; }
        public static ElementoSeleccionado TipoElementoSeleccionado { get; internal set; }
        public static bool IsOk { get; private set; }

        public static XYZ ptoAproxinadoBArra { get; set; }

        internal static void ReseteaEstados()
        {
            ElementoSelecionado = null;
            TipoElementoSeleccionado = ElementoSeleccionado.None;
            IsOk = false;
        }

        public static void VerificarEstado(SeleccionarElementosH SeleclemH)
        {
            ElementoSelecionado = SeleclemH.Element_pickobject_element;
            TipoElementoSeleccionado = SeleclemH._elementoSeleccionado;
            VerificarEstado();

            if (SeleclemH.PtoSeleccionMouseCentroCaraMuro6.Z > SeleclemH._PtoInicioBaseBordeViga6.Z)
                ptoAproxinadoBArra = SeleclemH._PtoInicioBaseBordeViga6 + XYZ.BasisZ * Util.CmToFoot(ConstNH.RECUBRIMIENTO_LOSA_INF_cm);
            else
                ptoAproxinadoBArra = SeleclemH._PtoInicioBaseBordeViga6 - XYZ.BasisZ * Util.CmToFoot(ConstNH.RECUBRIMIENTO_LOSA_INF_cm);

           
        }


        public static bool VerificarPtoDentroElemento()=> ElementoSelecionado.IsPtoDentroDelSOlido(ptoAproxinadoBArra);

        public static void VerificarEstado()
        {

            IsOk = true;
            if (ElementoSelecionado == null)
            {
                IsOk = false;
                return;
            }

            if (TipoElementoSeleccionado == ElementoSeleccionado.None)
            {
                IsOk = false;
                return;
            }

        }
    }
}
