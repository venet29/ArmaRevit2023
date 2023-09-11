using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.DTO;
using ArmaduraLosaRevit.Model.BarraV.Horquilla.Creador;
using ArmaduraLosaRevit.Model.ElementoBarraRoom._tipoBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.TextoNoteNH;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Visibilidad;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.TipoBarra.Verticales
{
    public class ABarrasHorq_SinTrans : ABarrasElevV_SinTrans
    {
        List<ElementId> listaGrupo_LineaRebar = new List<ElementId>();
        protected List<DatosTExtoDTO> _listTextoElevacion;
        public ABarrasHorq_SinTrans(UIApplication uiapp, IntervaloBarrasDTO interBArraDto, IGeometriaTag _newGeometriaTag) : base(uiapp, interBArraDto, _newGeometriaTag)
        {
            _BarraTipo = interBArraDto.BarraTipo;
            _listTextoElevacion = new List<DatosTExtoDTO>();
        }



        public void M8_CrearPatSymbolFalso()
        {
            try
            {


                if (_listcurveElevacion.Count == 0) return;

                // M8_1_ObtenerLineStyle_Barra();
                Element line_styles_srv = Creador_TypoLine.ObtenerLineStyle_Horq(_doc);

                using (Transaction tx = new Transaction(_doc))
                {
                    tx.Start("Creata PatSymbolFalso-NH");
                    foreach (Curve item in _listcurveElevacion)
                    {
                        if (item is Line)
                        {
                            DetailLine lineafalsa = _doc.Create.NewDetailCurve(_view, item) as DetailLine;
                            lineafalsa.LineStyle = line_styles_srv;
                            if (lineafalsa != null) listaGrupo_LineaRebar.Add(lineafalsa.Id);
                        }
                        else if (item is Arc)
                        {
                            DetailArc lineafalsa = _doc.Create.NewDetailCurve(_view, item) as DetailArc;
                            lineafalsa.LineStyle = line_styles_srv;// line_styles_BARRA;
                            if (lineafalsa != null) listaGrupo_LineaRebar.Add(lineafalsa.Id);
                        }
                    }
                    tx.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear PathSymbol falso :{ex.Message}");
            }

        }

        public void M8_CrearPatSymbolFalso_SinTRans()
        {
            try
            {
                if (_listcurveElevacion.Count == 0) return;

                // M8_1_ObtenerLineStyle_Barra();
                Element line_styles_ = TiposLineaPattern.ObtenerTipoLinea("Horq", _doc);

                if (line_styles_ == null)
                    Util.ErrorMsg("No se encontro Linea tipo Horquilla");
                foreach (Curve item in _listcurveElevacion)
                {
                    if (item is Line)
                    {
                        DetailLine lineafalsa = _doc.Create.NewDetailCurve(_view, item) as DetailLine;
                        if (line_styles_ != null) lineafalsa.LineStyle = line_styles_;
                        if (lineafalsa != null) listaGrupo_LineaRebar.Add(lineafalsa.Id);
                    }
                    else if (item is Arc)
                    {
                        DetailArc lineafalsa = _doc.Create.NewDetailCurve(_view, item) as DetailArc;
                        if (line_styles_ != null) lineafalsa.LineStyle = line_styles_;// line_styles_BARRA;
                        if (lineafalsa != null) listaGrupo_LineaRebar.Add(lineafalsa.Id);
                    }
                }
                //***
                CrearTexNote _CrearTexNote = new CrearTexNote(_doc, FActoryTipoTextNote.TextoHorq, TipoCOloresTexto.Amarillo);//"2.5mm Arial"
                foreach (var item in _listTextoElevacion)
                {
                    var text= _CrearTexNote.M1_CrearCSintrans(item.pto, item.texto, 0);
                    if(text!=null)
                        listaGrupo_LineaRebar.Add(text.Id);
                }

                _doc.Create.NewGroup(listaGrupo_LineaRebar);

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al Crear PatSymbol Falso :{ex.Message}");
            }

        }
    }

    public class DatosTExtoDTO
    {
        public XYZ pto { get; set; }
        public string texto { get; set; }

    }
}
