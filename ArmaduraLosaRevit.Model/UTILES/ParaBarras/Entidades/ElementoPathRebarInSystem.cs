using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades
{


    public class ElementoPathRebarInSystem : ElementoPath 
    {

        public RebarInSystem _rebarInSystem { get; set; }

        public static ElementoPathRebarInSystem CrearVisibilidadElementoRebarhDTO(Document _doc, RebarInSystem _rebar, PathReinforcement _createdPathReinforcement, List<Element> tagpath)
        {

            // tipo barra
            TipoBarra tipoBarra = M1_2_1_OBtenerTipoBarra(_createdPathReinforcement, _doc,"BarraTipo");
            TipoDireccionBarra tipoDireccionBarra_ = M1_2_2_OBtenerTipoDireccion(_createdPathReinforcement, _doc);
            //ubicacion
            UbicacionLosa ubicacionLosa = M1_2_3_OBtenerTipoBarraDireccion(_createdPathReinforcement, _doc, "BarraOrientacion");

            TipoConfiguracionBarra tipoConfiguracionBarra = ObtenerM1_2_4_TipoConfiguracionBarra(tipoBarra);
            int diametroBarra = _rebar.ObtenerDiametroMM();

            TipoBarraGeneral TipoBarraGeneral_ = Tipos_Barras.M2_Buscar_TipoGrupoBarras_pornombre(tipoBarra.ToString());
            ObtenerTipoBarra _tipoBarraGeneral = AyudaObtenerParametros.ObtenerTipoBarraRebra(_createdPathReinforcement);

            return new ElementoPathRebarInSystem(_rebar, tagpath, ubicacionLosa, tipoConfiguracionBarra, tipoBarra, tipoDireccionBarra_,  diametroBarra, TipoBarraGeneral_);
        }



        public ElementoPathRebarInSystem(RebarInSystem pathrein,List<Element> tagpath, UbicacionLosa orientacionBarra, TipoConfiguracionBarra tipoconfiguracionBarra,
                                        TipoBarra tipoBarra, TipoDireccionBarra tipoDireccionBarra_,  int diametroBarra, TipoBarraGeneral TipoBarraGeneral)
            : base(tagpath, orientacionBarra, tipoconfiguracionBarra, tipoBarra, tipoDireccionBarra_, diametroBarra, TipoBarraGeneral)
        {
            this._rebarInSystem = pathrein;
 
        }


#pragma warning disable CS0108 // 'ElementoPathRebarInSystem.ObtenerListaIdPath()' hides inherited member 'ElementoPath.ObtenerListaIdPath()'. Use the new keyword if hiding was intended.
        public List<ElementId> ObtenerListaIdPath()
#pragma warning restore CS0108 // 'ElementoPathRebarInSystem.ObtenerListaIdPath()' hides inherited member 'ElementoPath.ObtenerListaIdPath()'. Use the new keyword if hiding was intended.
        {
            List<ElementId> ele = new List<ElementId>();
            ele.Add(_rebarInSystem.Id);
            if (ListTagpath.Count > 0)
                ele.AddRange(ListTagpath.Select(c => c.Id).ToList());
            return ele;
        }
    }

}
