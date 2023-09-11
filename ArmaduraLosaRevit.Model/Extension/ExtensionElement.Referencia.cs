using ArmaduraLosaRevit.Model.GEOM.Casos;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{
    public static partial class ExtensionElement
    {

        //******
        //rutinas genericas
        //se utiliza solo para caso obtener referencias de plnanar fase para dibujar dimensiones 
        public static List<List<PlanarFace>> ListaFace_Conferencias(this Element elemet, bool IsComputeReferences = false)
        {
            GeometriaParaReferecias _geometriaBase = new GeometriaParaReferecias(elemet.Document);
            _geometriaBase.M1_AsignarGeometriaObjecto(elemet, IsComputeReferences);
            return _geometriaBase.listaGrupoPlanarFace;
        }

    }
}
