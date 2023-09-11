using ArmaduraLosaRevit.Model.AnalisisRoom;
using ArmaduraLosaRevit.Model.AnalisisRoom.Utiles;
using ArmaduraLosaRevit.Model.EditarPath;
using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Update.Calculo;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.ExtStore;
using ArmaduraLosaRevit.Model.ExtStore.Factory;
using ArmaduraLosaRevit.Model.ExtStore.Tipos;
using ArmaduraLosaRevit.Model.Traslapo.Calculos;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Update.Tag
{
    public class ManejarEditTagUpdate_MoverMantenerSOLOExtremos : ManejarEditTagUpdate_MoverBase
    {

        public ManejarEditTagUpdate_MoverMantenerSOLOExtremos(UIApplication uiapp, PathReinforcement pathReinforcement, XYZ ptoPathSymbol, List<IndependentTag> listaIndependentTag)
            : base(uiapp, pathReinforcement, ptoPathSymbol, listaIndependentTag)
        {


        }


        public bool Ejecutar()
        {
            try
            {
                ListaCasosNoconsiderar.Add("M_Path Reinforcement Tag(ID_cuantia_largo)_F");
                ListaCasosNoconsiderar.Add("M_Path Reinforcement Tag(ID_cuantia_largo)_F2");
                ListaCasosNoconsiderar.Add("M_Path Reinforcement Tag(ID_cuantia_largo)_L");
                ListaCasosNoconsiderar.Add("M_Path Reinforcement Tag(ID_cuantia_largo)_L2");
                ListaCasosNoconsiderar.Add("M_Path Reinforcement Tag(ID_cuantia_largo)_C");
                ListaCasosNoconsiderar.Add("M_Path Reinforcement Tag(ID_cuantia_largo)_C2");

                if (!M1_ObtenerTipos()) return false;
               // M2_SeleccionarRoom();
                if (!M3_ObtenerBordeDepath()) return false;
                //if (!M4_ObtenerTag()) return false;
                M5_Mover();

            }
            catch (Exception ex)
            {

                Debug.WriteLine($" ex: {ex.Message}");
                return IsOK = false;
            }
            return IsOK;
        }


        private void M5_Mover()
        {
            try
            {
                //filtrar
                var _listaIndependentTagAnoy = _listaIndependentTag_Enview.Select(c => new TagPathReinProcesado() { tagPath = c, Isprocesado = false }).ToList();
                _CreadorExtStoreDTO = FactoryExtStore.ObtnerPosicionTagLosa();
                _CreadorExtStore = new CreadorExtStore(_uiapp, _CreadorExtStoreDTO);


                for (int i = 0; i < _listaIndependentTagAnoy.Count; i++)
                {
                    var item = _listaIndependentTagAnoy[i];
                    if (item.Isprocesado == true) continue;

                    if (ContieneTagCOrrecto(item.tagPath.Name))
                    {
                        //set
                        _CreadorExtStore.SET_DataInElement_XYZ_SInTrans(item.tagPath, item.tagPath.TagHeadPosition);
                        continue;
                    }
                    //get
                    if (_CreadorExtStore.GET_DataInElement_XYZ_SinTrans(item.tagPath, _CreadorExtStoreDTO.SchemaName))
                    {
                        if (_CreadorExtStore.retrievedData.IsAlmostEqualTo(XYZ.Zero)) continue;

                        XYZ posicionAnteriorGuardada = _CreadorExtStore.retrievedData;

                        XYZ DesltaDesplaiento =  ObtenerDesplazaminetoEnSentidoBarras.Ejecutar( posicionAnteriorGuardada, item.tagPath, _coordenadaPath) ;
                        ElementTransformUtils.MoveElement(_doc, item.tagPath.Id, DesltaDesplaiento.AsignarZ(0));
                    }
                    //set  no se registra pq despues se activa el reactior de los tag que agrega su direccion final
                   // _CreadorExtStore.SET_DataInElement_XYZ_SInTrans(item.tagPath, item.tagPath.TagHeadPosition);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($" ex: {ex.Message}");
            }
        }

    

 


    }

}
