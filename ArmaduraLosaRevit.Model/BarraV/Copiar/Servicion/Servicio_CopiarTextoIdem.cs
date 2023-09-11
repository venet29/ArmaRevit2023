using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.BarraV.Copiar.model;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.TextoNoteNH;
using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.BarraV.Copiar.Servicion
{
    class Servicio_CopiarTextoIdem
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;
        private List<WrapperFormatoRebar_final> listaBArrasCOpiadas;
        private string _textoIdem;
        private readonly string tipoTexto;

        public Servicio_CopiarTextoIdem(UIApplication uiapp, List<WrapperFormatoRebar_final> listaBArrasCOpiadas, string textoIdem, string TipoTexto)
        {
            _uiapp = uiapp;
            _doc = _uiapp.ActiveUIDocument.Document;
            _view = _doc.ActiveView;
            this.listaBArrasCOpiadas = listaBArrasCOpiadas;
            this._textoIdem = textoIdem;
            tipoTexto = TipoTexto;
        }

        public List<XYZ> listaptoCentroCara { get; private set; }

        public bool ObtenerPtos()
        {
            try
            {
                List<ElementId> listaVigasId = listaBArrasCOpiadas.Select(c => c.IdHostVIga).Distinct().ToList();
                List<Element> listaVigasElem = listaVigasId.Select(c => _doc.GetElement(c) as Element).Distinct().ToList();

                listaptoCentroCara = new List<XYZ>();
                foreach (Element item in listaVigasElem)
                {
                    if (!(item is FamilyInstance)) continue;
                    (bool reult, PlanarFace caraVerticalVisible)   = item.ObtenerCaraVerticalVIsible(_view);
                    if (!reult) continue;
                    var ptoCentroCara = caraVerticalVisible.ObtenerCenterDeCara();
                    listaptoCentroCara.Add(ptoCentroCara);
                }

                //for
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return false;
            }
            return true;
        }
        public bool CrearTexto(List<double> listaLevelZ)
        {
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Crear TextoVigaIdem-NH");

                    CrearTexNote _CrearTexNote = new CrearTexNote(_uiapp, tipoTexto, TipoCOloresTexto.Amarillo);//"2.5mm Arial"
                    foreach (XYZ item in listaptoCentroCara)
                    {
                        foreach (var desnivel in listaLevelZ)
                        {
                            var text = _CrearTexNote.M1_CrearCSintrans(item.AsignarZ(desnivel) + new XYZ(0,0,Util.CmToFoot(20)), _textoIdem, 0);
                        }
                     

                    }
                    t.Commit();
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
