using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
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

namespace ArmaduraLosaRevit.Model.RebarLosa.PathFalso
{
   public  class ManejadorPathFalso
    {
        private  UIApplication _uiapp;
#pragma warning disable CS0649 // Field 'ManejadorPathFalso._ListIRebarLosa' is never assigned to, and will always have its default value null
        List<IRebarLosa> _ListIRebarLosa;
#pragma warning restore CS0649 // Field 'ManejadorPathFalso._ListIRebarLosa' is never assigned to, and will always have its default value null

        public ManejadorPathFalso(UIApplication uiapp)
        {
            this._uiapp = uiapp;
        }

        public bool Ejecutar()
        {
            try
            {
                //seleccionar ptos
                List<XYZ> listaPtos= CrearListaPtos.M2_ListaPtoSimple(_uiapp);

                GenerarPtos generarPtos = new GenerarPtos(_uiapp,listaPtos);

                RebarInferiorDTO rebarInferiorDTO1=  generarPtos.Ejecutar();
                if (rebarInferiorDTO1.IsOK == false) return false;
                //generar geometria

                GenerarBarra_path(rebarInferiorDTO1);

                //dibujar path falso, texto , circulo y dimension

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }


        private bool GenerarBarra_path(RebarInferiorDTO rebarInferiorDTO1)
        {
            try
            {
                //3)tag
                IGeometriaTag _newIGeometriaTag =new  GeomeTagNull();

                //4)barra
                IRebarLosa rebarLosa = FactoryIRebarLosa.CrearIRebarLosa(_uiapp, rebarInferiorDTO1, _newIGeometriaTag);
                if (!rebarLosa.M1A_IsTodoOK()) return false;

                _ListIRebarLosa.Add(rebarLosa);

            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }


    }
}
