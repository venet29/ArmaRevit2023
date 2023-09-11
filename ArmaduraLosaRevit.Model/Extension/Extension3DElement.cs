using ArmaduraLosaRevit.Model.BarraV.Desglose.Ayuda;
using ArmaduraLosaRevit.Model.Elemento_Losa.Ayuda;
using ArmaduraLosaRevit.Model.Elementos_viga;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{

    public static class Extension3DElement
    {


        /// <summary>
        /// Return the curve from a Revit database Element 
        /// location curve, if it has one.
        /// </summary>
        public static bool IsPtoDentroDelSOlido(this Element selecFloor, XYZ ptoInterssicon)
        {
            try
            {


                //*******************
                //pto superior
                XYZ PtoSuperior = XYZ.Zero;
                PlanarFace face_superior = selecFloor.ObtenerCaraSuperior(ptoInterssicon, XYZ.BasisZ);

                if (face_superior != null)
                    PtoSuperior = face_superior.ObtenerPtosInterseccionFace(ptoInterssicon, XYZ.BasisZ, true);
                else
                {
                    var losa_Encontrada_RuledFace = AyudaLosa.obtenerFaceSuperiorLosa_RuledFace(selecFloor);
                    if (losa_Encontrada_RuledFace == null) return false;
                    PtoSuperior = losa_Encontrada_RuledFace.ReludfaceNH.ProjectNH(ptoInterssicon);
                }

                if (XYZ.Zero.IsAlmostEqualTo(PtoSuperior)) return false;

                //pto esta sobre cara superior''
                if (PtoSuperior.Z < ptoInterssicon.Z) return false;


                //*******
                //pto inferior
                XYZ Ptoinferior = XYZ.Zero;
                PlanarFace face_inferior = selecFloor.ObtenerCaraInferior(ptoInterssicon, XYZ.BasisZ);

                if (face_inferior != null)
                    Ptoinferior = face_inferior.ObtenerPtosInterseccionFace(ptoInterssicon, XYZ.BasisZ, true);
                else
                {
                    var losa_Encontrada_RuledFace = AyudaLosa.obtenerFaceSuperiorLosa_RuledFace(selecFloor);
                    if (losa_Encontrada_RuledFace == null) return false;
                    Ptoinferior = losa_Encontrada_RuledFace.ReludfaceNH.ProjectNH(ptoInterssicon);
                }
                if (XYZ.Zero.IsAlmostEqualTo(Ptoinferior)) return false;

                // PTO ESTA bajo face inferior
                if (Ptoinferior.Z > ptoInterssicon.Z) return false;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en buscar si pto esta dentro de elemento.\nex:{ex.Message}");
                return false;
            }

            return true;
        }
    }
}
