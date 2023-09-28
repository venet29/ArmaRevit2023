using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias
{
    public class TiposFamilia3D
    {

        private static View3D View3D_BUSCAR { get; set; }
        private static View3D View3D_VISUALIZAR { get; set; }

        public static View3D Limpiar3DBuscar() => View3D_BUSCAR = null;


            public static View3D Get3DBuscar(Document _doc, string name="")
        {
            try
            {


                if (View3D_BUSCAR == null || (View3D_BUSCAR?.IsValidObject==false))
                {
                    //   Util.ErrorMsg($" ConstantesGenerales.CONST_NOMBRE_3D_PARA_BUSCAR :{ConstantesGenerales.CONST_NOMBRE_3D_PARA_BUSCAR} ");
                    View3D_BUSCAR = Util.GetFirstElementOfTypeNamed(_doc, typeof(View3D), (name != "" ? name: ConstNH.CONST_NOMBRE_3D_PARA_BUSCAR)) as View3D;

                    if (View3D_BUSCAR == null) Util.ErrorMsg("Error,No se encontro '3D_PARA_BUSCAR' favor cargar configuracion inicial");
                }

                if (View3D_BUSCAR?.IsLocked==true) Util.ErrorMsg($"Error :   { (name != "" ? name : ConstNH.CONST_NOMBRE_3D_PARA_BUSCAR)}  IsLocked=true. 'Get3DBuscar'");
            }
            catch (Exception)
            {
                View3D_BUSCAR = null;
            }
            return View3D_BUSCAR;
        }

        public static View3D Get3DVisualizar(Document _doc)
        {
            try
            {
                if (View3D_VISUALIZAR == null || (View3D_VISUALIZAR?.IsValidObject == false))
                {
                    View3D_VISUALIZAR = Util.GetFirstElementOfTypeNamed(_doc, typeof(View3D), ConstNH.CONST_NOMBRE_3D_VISUALIZAR) as View3D;

                    if (View3D_VISUALIZAR==null)
                    {
                        List<string> ListaView = new List<string>() { "{3D - masteringenieria.cl}", "{3D - delporteing@gmail.com}", "{3D - delporte.bim}", "3D" };

                        foreach (string itemview3d in ListaView)
                        {
                            View3D_VISUALIZAR = TiposView3D.ObtenerTiposView(itemview3d, _doc);
                            if (View3D_VISUALIZAR != null) break;
                        }
                    }           
                    if (View3D_VISUALIZAR == null) Util.ErrorMsg("Error, favor cargar configuracion inicial");
                }

                if (View3D_VISUALIZAR?.IsLocked== true) Util.ErrorMsg($"Error :   { ConstNH.CONST_NOMBRE_3D_VISUALIZAR}  IsLocked=true. 'Get3DVisualizar'");
            }
            catch (Exception)
            {
                View3D_BUSCAR = null;
            }
            return View3D_VISUALIZAR;
        }




        public static bool DesactivarSectionBox(Document _doc,View3D _View3D,bool estado)
        {


            try
            {

                using (Transaction tr = new Transaction(_doc, "NoneView Template"))
                {
                    tr.Start();
                    _View3D.IsSectionBoxActive = estado;
                    tr.Commit();
                }
           
            }
            catch (Exception)
            {
                Util.ErrorMsg($"Error al cambiar estado sectionBox de 3dVIew:{_View3D.Name}");
                return false;
            }

            return true;
        }
    }
}
