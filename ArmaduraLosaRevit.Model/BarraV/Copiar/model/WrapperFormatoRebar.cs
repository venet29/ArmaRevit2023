using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras.Entidades;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Copiar.model
{
    public class WrapperFormatoRebar_final
    {
        private Document _doc;
        private List<ElementId> liscopaida;
        private int IdbarraIncial;

        public Color colorBarras { get; set; }
        public Rebar Barra { get; set; }
        public string nombreElevacion { get; set; }
        public string barraTipo { get; set; }
        public ElementId IdHostVIga { get;  set; }

        public WrapperFormatoRebar_final(Document _doc, List<ElementId> liscopaida, ElementoRebar_Elev wrapperBarrasElevaciones)
        {
            this._doc = _doc;
            this.liscopaida = liscopaida;
            this.IdbarraIncial = wrapperBarrasElevaciones._rebar.Id.IntegerValue;
            this.colorBarras = wrapperBarrasElevaciones.colorBarras;
            this.barraTipo = wrapperBarrasElevaciones.barraTipo;
            this.nombreElevacion = wrapperBarrasElevaciones.nombreElevacion;
        }



        public void Ejecutar()
        {
            var view3D_Visualizar = TiposFamilia3D.Get3DVisualizar(_doc);
            foreach (ElementId item in liscopaida)
            {
                var elem = _doc.GetElement(item);

                if (elem is Rebar)
                {
                    Barra = (Rebar)elem;
                    bool resultnombreElevacion = ParameterUtil.SetParaStringNH(Barra, "NombreVista", nombreElevacion);
                    bool resultBarraTipo = ParameterUtil.SetParaStringNH(Barra, "BarraTipo", barraTipo);
                    bool resultIdBarraCopiar = ParameterUtil.SetParaIntNH(Barra, "IdBarraCopiar", IdbarraIncial);

                    ((Rebar)elem).SetSolidInView(view3D_Visualizar, true);
                    //permite que la barra se vea en el 3d como sin interferecnias 
                    ((Rebar)elem).SetUnobscuredInView(view3D_Visualizar, true);

                    IdHostVIga = Barra.GetHostId();

                }

            }
        }

    }
}
