using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Borrar
{
    public class BorrarRebarRectangulo
    {

        public static void Ejecutar(UIApplication _uiapp)
        {

            try
            {
                SeleccionarRebarRectangulo administrador_ReferenciaRoom = new SeleccionarRebarRectangulo(_uiapp);
                if (!administrador_ReferenciaRoom.GetRebarSeleccionadosConRectaguloYFiltros()) return;

                administrador_ReferenciaRoom.BorrarRebarSeleccionado3();
                //SeleccionarRebarRectanguloWrapperRebar
                //  visibilidad.AsignarVisibilityBuiltInCategory(bordeBarraHideActual);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" Error  ex:{ex.Message}");
            }

        }


        public static void EjecutarLosa(UIApplication _uiapp)
        {
            try
            {
                SeleccionarRebarRectanguloWrapperRebar administrador_ReferenciaRoom = new SeleccionarRebarRectanguloWrapperRebar(_uiapp, TipoBarraGeneral.Losa);
                if (!administrador_ReferenciaRoom.BorrarBarras()) return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" Error  ex:{ex.Message}");
            }
        }

        public static void EjecutarElevaciones(UIApplication _uiapp,string casoEspecifico)
        {
            try
            {
                SeleccionarRebarRectanguloWrapperRebar administrador_ReferenciaRoom = new SeleccionarRebarRectanguloWrapperRebar(_uiapp, TipoBarraGeneral.Elevacion, casoEspecifico);
                if (!administrador_ReferenciaRoom.BorrarBarras()) return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" Error  ex:{ex.Message}");
            }
        }
        public static void EjecutarEscalera(UIApplication _uiapp)
        {
            try
            {
                SeleccionarRebarRectanguloWrapperRebar administrador_ReferenciaRoom = new SeleccionarRebarRectanguloWrapperRebar(_uiapp, TipoBarraGeneral.Escalera);
                if (!administrador_ReferenciaRoom.BorrarBarras()) return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" Error  ex:{ex.Message}");
            }
        }

        public static void EjecutarFundaciones(UIApplication _uiapp)
        {
            try
            {
                SeleccionarRebarRectanguloWrapperRebar administrador_ReferenciaRoom = new SeleccionarRebarRectanguloWrapperRebar(_uiapp, TipoBarraGeneral.Fundaciones);
                if (!administrador_ReferenciaRoom.BorrarBarras()) return;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" Error  ex:{ex.Message}");
            }
        }
    }



}
