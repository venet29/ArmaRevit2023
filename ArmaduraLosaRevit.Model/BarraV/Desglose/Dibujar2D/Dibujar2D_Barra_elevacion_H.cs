using ArmaduraLosaRevit.Model.BarraV.Desglose.Calculos;
using ArmaduraLosaRevit.Model.BarraV.Desglose.DTO;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Model;
using ArmaduraLosaRevit.Model.BarraV.Desglose.Tag;
using ArmaduraLosaRevit.Model.BarraV.EditarBarraLargo.Entidades;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Dimensiones;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.RebarLosa.Barras;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Desglose.Dibujar2D
{
    public class Dibujar2D_Barra_elevacion_H: Dibujar2D_Barra_BASE
    {


        public Dibujar2D_Barra_elevacion_H(UIApplication _uiapp, GruposListasTraslapoIguales_H gruposListasTraslapoIguales, Config_EspecialElev _Config_EspecialElv)
            :base( _uiapp,  gruposListasTraslapoIguales,  _Config_EspecialElv)

        {

        }

        public bool PreDibujar(bool isId)
        {

            try
            {
                SeleccionarElementosV _SeleccionarElementosV = new SeleccionarElementosV(_uiapp,true);
                _SeleccionarElementosV.M1_1_CrearWorkPLane_EnCentroViewSecction();

                var lista = CrearListaPtos.M2_ListaPtoSimple(_uiapp, 1);
                if (lista.Count == 0) return false;

                var trasnf = _config_EspecialElv.Trasform_;

                posicionInicial = trasnf.EjecutarTransform( lista[0]);
                XYZ posicionAUX = XYZ.Zero;

                XYZ direccionMuevenBarrasFAlsa = new XYZ(0, 0, -1);
                _config_EspecialElv.direccionMuevenBarrasFAlsa = direccionMuevenBarrasFAlsa;


                foreach (RebarDesglose_GrupoBarras_H itemGRUOP in _GruposListasTraslapoIguales_H.soloListaPrincipales)
                {
                    //   var BarraTipo = item._GrupoRebarDesglose[0];
                    RebarElevDTO _RebarElevDTOANterior=null;
                    bool IsPrimero = true;
                    for (int i = 0; i < itemGRUOP._GrupoRebarDesglose.Count; i++)
                    {
                        if (Util.IsPar(i))
                            posicionAUX = posicionInicial;
                        else//FALTA:hay q mejorar esta parte para que segun la seleccion es la direeccion donde se extiende las barra
                            posicionAUX = posicionInicial + trasnf.EjecutarTransform(Util.CmToFoot(2)* direccionMuevenBarrasFAlsa);

                        RebarDesglose_Barras_H item1 = itemGRUOP._GrupoRebarDesglose[i];
                        item1.contBarra = itemGRUOP._ListaRebarDesglose_GrupoBarrasRepetidas.Count + 1;
                        RebarElevDTO _RebarElevDTO = item1.ObtenerRebarElevDTO(posicionAUX, _uiapp, isId, _config_EspecialElv);

                        GenerarBarra_2D(_RebarElevDTO);

                        if(!IsPrimero)
                             CrearDimensionENtreBArras(_RebarElevDTO, _RebarElevDTOANterior);

                        _RebarElevDTOANterior = _RebarElevDTO;
                        IsPrimero = false;
                    }




                    posicionInicial =   trasnf.EjecutarTransform( trasnf.EjecutarTransformInvertida(posicionInicial) + direccionMuevenBarrasFAlsa * Util.CmToFoot(50));
                }

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

                return false;
            }
            return true;
        }


    }
}
