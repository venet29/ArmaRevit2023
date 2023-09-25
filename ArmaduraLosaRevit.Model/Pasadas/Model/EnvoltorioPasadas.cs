using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.ExtStore.Ayuda;
using ArmaduraLosaRevit.Model.ExtStore.Tipos;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.Pasadas.Model
{
    public class EnvoltorioPasadas
    {
        public Element Pasada { get; set; }
        public string EstadoPasada { get; set; }
        public string ComentarioPasada { get; set; }        
        public XYZ PtoInsercion { get; set; }
        public double Area { get; set; }
        public EnvoltorioPasadas(Element item)
        {
            this.Pasada = item;
        }



        internal bool OBtenerPArametro(UIApplication _uiapp, CreadorExtStoreComplejo _CreadorExtStore)
        {
            try
            {
   
                //a)
                var entityPtoInsercion= _CreadorExtStore.M3_OBtenerResultado_Entity(Pasada, "insertar");
                if (entityPtoInsercion != null)
                {

                    //PtoInsercion = AyudaCasosUnidades_2021Arriba.Obtener_DUT_DECIMAL_FEET(_uiapp, entityPtoInsercion, "SubFieldTest11");
                    if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2022))
                        PtoInsercion = AyudaCasosUnidades_2021Arriba.Obtener_DUT_DECIMAL_FEET(_uiapp,entityPtoInsercion, "SubFieldTest11");
                    else
                        PtoInsercion = AyudaCasosUnidades_2020Bajo.Obtener_DUT_DECIMAL_FEET_(_uiapp, entityPtoInsercion, "SubFieldTest11");
                }

                //b)
                var entityEstado = _CreadorExtStore.M3_OBtenerResultado_Entity(Pasada, "estado");
                if (entityEstado != null)
                    EstadoPasada = entityEstado.Get<string>("SubFieldTest21");


                //c)
                var entityArea = _CreadorExtStore.M3_OBtenerResultado_Entity(Pasada, "area");
                if (entityArea != null)
                {
                    //Area = AyudaCasosUnidades_2021Arriba.Obtener_DUT_Numero_FEET(_uiapp,entityArea, "SubFieldTest31");
                    if (UtilVersionesRevit.IsMAyorOIgual(_uiapp, VersionREvitNh.v2021))
                        Area = AyudaCasosUnidades_2021Arriba.Obtener_DUT_Numero_FEET(_uiapp, entityArea, "SubFieldTest31");
                    else
                        Area = AyudaCasosUnidades_2020Bajo.Obtener_DUT_NUMERO_FEET_(_uiapp, entityArea, "SubFieldTest31");
                }

                //d
                var entityComentario= _CreadorExtStore.M3_OBtenerResultado_Entity(Pasada, "comentario");
                if (entityComentario != null)
                    ComentarioPasada = entityComentario.Get<string>("SubFieldTest41");
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
