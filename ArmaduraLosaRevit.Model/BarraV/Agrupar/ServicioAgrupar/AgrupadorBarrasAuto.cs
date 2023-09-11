
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.DTO;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.model;
using ArmaduraLosaRevit.Model.BarraV.Agrupar.Model;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using ArmaduraLosaRevit.Model.BarraV.Contener;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Agrupar.ServicioAgrupar
{
    public class AgrupadorBarrasAuto : AgrupadorBarras
    {

        public int MyProperty { get; set; }

        public AgrupadorBarrasAuto(UIApplication uiapp, List<BarraIng> ListBarraIng , XYZ _ptoUbicacionTag, Orientacion OrientacionTagGrupoBarras) : base(uiapp, ListBarraIng, _ptoUbicacionTag, OrientacionTagGrupoBarras)
        {

        }

        #region 2)automatico
        public void M1_AgruparAuto()
        {
            IsOK = true;
            try
            {
                //aqui todos los barras tiene el mismo pto de partida P2
                var listaAgrupada = ListBarraIng.GroupBy(c => new { c.largoFoot, c.diametroInt });

                var confBarraTag =  ConfiguracionTAgBarraDTo.OBtenercasoVertical();

                foreach (var itemKey in listaAgrupada)
                {
                    List<BarraIng> list = itemKey.OrderBy(c => c.distaciaDesdeOrigen).ToList();
                    AdministradorGrupos newAdministradorGrupos = new AdministradorGrupos(_uiapp, itemKey.Key.diametroInt, itemKey.Key.largoFoot, list[0].P1.Z, list, confBarraTag);
                    ListAdministradorGrupos.Add(newAdministradorGrupos);
                }
            }
            catch (Exception)
            {
                IsOK = false; ;
            }
        }

        public void M2_ObtenerPtoInserccionPorPiso()
        {
            GenerarNuevaDirectizDTO _generarNuevaDirectizDTO = ObtenerGenerarNuevaDirectizDTO();

            foreach (AdministradorGrupos administradorGrupos in ListAdministradorGrupos)
            {
                //a)agregar tag
                administradorGrupos.M1_ObtenerDatosPreDibujoAuto(_generarNuevaDirectizDTO);             
            }
        }

        public void M3_DibujarTagDeGrupoAutomatico()
        { 
            foreach (AdministradorGrupos administradorGrupos in ListAdministradorGrupos)
            {
                //a)agregar tag
                administradorGrupos.M2_GenerarNuevaDirectrizVertical();

                //b)agregar contenedor  hay que agrupar
                //ContendorRebar ContendorRebar = new ContendorRebar(_uiapp);
                //Element muroVigaContanedor = _doc.GetElement(administradorGrupos.ListaBarraIng[0]._Rebar.GetHostId());
                //ContendorRebar.CrearContenedorSinTRAS(muroVigaContanedor, administradorGrupos.ListaBarraIng.Select(c => c._Rebar).ToList());
            }
        }
        #endregion
    }
}
