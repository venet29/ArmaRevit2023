using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Model;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Calculos
{
    class GruposListasTraslapo_V
    {
        private UIApplication uiapp;
        private List<RebarDesglose> lista_RebarDesglose;
        public List<RebarDesglose_GrupoBarras_V> GruposRebarMismaLinea { get; set; }
        public GruposListasTraslapo_V(UIApplication uiapp, List<RebarDesglose> lista_RebarDesglose)
        {
            this.uiapp = uiapp;
            this.lista_RebarDesglose = lista_RebarDesglose;
            this.GruposRebarMismaLinea = new List<RebarDesglose_GrupoBarras_V>();
        }

        public bool ObtenerGruposTraslapos()
        {
            List<RebarDesglose_Barras_V> listaBArras = lista_RebarDesglose.Where(c => c._tipoBarraEspecifico == TipoRebar.ELEV_BA_V)
                                                                         .Select(c => new RebarDesglose_Barras_V(c)).ToList();
            //obtener RebarDesglose_Barras
            foreach (RebarDesglose_Barras_V item in listaBArras)
            {
                item.Ordenar_UltimaCurvaMayorZ();
                item.Ordenar_Analizar();
            }

            // ordenar de los inicial menor y solo verticales

            listaBArras = listaBArras.Where(c => c._direccion == Ayuda.direccionBarra.Vertical).OrderBy(c => c.ptoInicial.Z).ToList();

            try
            {
                for (int i = 0; i < listaBArras.Count; i++)
                {
                    RebarDesglose_Barras_V item = listaBArras[i];

                    RebarDesglose_Barras_V BarraAnalizada = item;
                    List<RebarDesglose_Barras_V> NuewGrupoBarras = new List<RebarDesglose_Barras_V>();
                    if (item.analizadasuperior) continue;

                    item.analizadasuperior = true;
                    NuewGrupoBarras.Add(item);
                    RebarDesglose_GrupoBarras_V _RebarDesglose_GrupoBarrasNew = null;
                    if (item.IsTraslapable == false || !item.SepuedeTraslaparSUperior())
                    {
                        _RebarDesglose_GrupoBarrasNew = RebarDesglose_GrupoBarras_V.Creador_RebarDesglose_GrupoBarras(NuewGrupoBarras);
                        GruposRebarMismaLinea.Add(_RebarDesglose_GrupoBarrasNew);
                        continue;
                    }

                    var listaGrupo = listaBArras
                        .Where(c => (!c.ptoInicial.IsAlmostEqualTo(item.ptoInicial)) &&
                                    c.IsTraslapable && 
                                    UtilDesglose.IsCollinear_barraDesglose(item.curvePrincipal, c.curvePrincipal, Util.MmToFoot( Math.Max(item.diametroMM,c.diametroMM))))
                        .OrderBy(c => c.ptoInicial.Z)
                        .ToList();


                    //busca dentro del gurpo colineal
                    for (int j = 0; j < listaGrupo.Count; j++)
                    {
                        RebarDesglose_Barras_V barra_colineales = listaGrupo[j];

                        // cuando el pto inicial de la sigueinte barra no esta contendia en la actual
                        if (!BarraAnalizada.curvePrincipal.Contains(barra_colineales.ptoInicial, 
                                                                    Util.MmToFoot(Math.Max(barra_colineales.diametroMM, item.diametroMM)))) break;

                        //cambiar barra sigueinte a actual
                        BarraAnalizada = barra_colineales;

                        barra_colineales.analizadasuperior = true;
                        NuewGrupoBarras.Add(barra_colineales);
                    }

                    _RebarDesglose_GrupoBarrasNew = RebarDesglose_GrupoBarras_V.Creador_RebarDesglose_GrupoBarras(NuewGrupoBarras);
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
    }
}
