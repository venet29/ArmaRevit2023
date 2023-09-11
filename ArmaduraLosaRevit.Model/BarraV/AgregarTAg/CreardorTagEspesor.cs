using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.BarraV.Seleccion;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using System;
using System.Collections.Generic;
using System.Linq;
namespace ArmaduraLosaRevit.Model.LosaEstructural
{

    public class CreardorTagEspesor
    {
        //double angle = 16.3;
        //angle = 16.3;
        //angulo -9.22 indica pelota de losa esta girada -9.22grados, despues se hace la conversion grado radian

        List<XYZ> ListaPtos = new List<XYZ>();

        private Document _doc;
#pragma warning disable CS0169 // The field 'CreardorTagEspesor._numbrePelotaLosa' is never used
        private string _numbrePelotaLosa;
#pragma warning restore CS0169 // The field 'CreardorTagEspesor._numbrePelotaLosa' is never used
#pragma warning disable CS0169 // The field 'CreardorTagEspesor._seleccionarLosaConMouse' is never used
        private SeleccionarLosaConMouse _seleccionarLosaConMouse;
#pragma warning restore CS0169 // The field 'CreardorTagEspesor._seleccionarLosaConMouse' is never used
#pragma warning disable CS0169 // The field 'CreardorTagEspesor._commandData' is never used
        private ExternalCommandData _commandData;
#pragma warning restore CS0169 // The field 'CreardorTagEspesor._commandData' is never used
        private UIApplication _uiapp;
        private UIDocument _uidoc;
#pragma warning disable CS0169 // The field 'CreardorTagEspesor._message' is never used
        private string _message;
#pragma warning restore CS0169 // The field 'CreardorTagEspesor._message' is never used
#pragma warning disable CS0169 // The field 'CreardorTagEspesor.nombrePelotaLosa' is never used
        private string nombrePelotaLosa;
#pragma warning restore CS0169 // The field 'CreardorTagEspesor.nombrePelotaLosa' is never used
#pragma warning disable CS0169 // The field 'CreardorTagEspesor.AngPelotaObteneridoPanelRad' is never used
        private double AngPelotaObteneridoPanelRad;
#pragma warning restore CS0169 // The field 'CreardorTagEspesor.AngPelotaObteneridoPanelRad' is never used
#pragma warning disable CS0169 // The field 'CreardorTagEspesor.espesor' is never used
        private string espesor;
#pragma warning restore CS0169 // The field 'CreardorTagEspesor.espesor' is never used
        private View _view;
        private int _escala;

        public CreardorTagEspesor(UIApplication uiapp)
        {
   
            this._uiapp = uiapp;
            this._uidoc = this._uiapp.ActiveUIDocument;
            this._doc = this._uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this._escala = _view.Scale;

        }

        public Result C1CargandoIteracionSeleccion(configTagNH _configTagNH)
        {

            bool seguir = true;

   

            while (seguir)
            {

                SeleccionarElementosV _seleccionarElementos = new SeleccionarElementosV(_uiapp);
                if (!_seleccionarElementos.M1_SeleccionarElementoHost())
                {
                    if (UtilBarras.IsConNotificaciones)
                    {
                        //Util.ErrorMsg("Error Al Selecciona muro de referencia");
                    }
                    else
                        // Debug.WriteLine("Error Al Selecciona muro de referencia");
                        return Result.Cancelled;
                }

                Element Wall = _seleccionarElementos._ElemetSelect;
                if (Wall == null) return Result.Succeeded;

               var lista= CrearListaPtos.M2_ListaPtoSimple(_uiapp, 1);
                if (lista.Count == 0) return Result.Cancelled; 
                XYZ posicionTag = lista[0];

                //obtiene el familysymbol 
                Element IndependentTagPath = TiposWallTagsEnView.cargarListaDetagWall2(_doc, _configTagNH.nombrefamilia, _configTagNH.namefamilyEspecifico);

                if (IndependentTagPath == null)
                {
                    Util.ErrorMsg("Error al obtener tag");
                    return Result.Failed;
                }

                try
                {
                    using (Transaction t = new Transaction(_doc))
                    {
                        t.Start("CrearPelotaLosaEstructural_aux-NH");
                        //agrega la annotation generico de pelota de losa 
                        IndependentTag independentTag = IndependentTag.Create(_doc, IndependentTagPath.Id, _view.Id, new Reference(Wall), _configTagNH.IsDIrectriz,
                                                  _configTagNH.tagOrientation, _configTagNH.desplazamientoPathReinSpanSymbol); //new XYZ(0, 0, 0)
                        independentTag.TagHeadPosition = posicionTag;

                        t.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Util.ErrorMsg($"  EX:{ex.Message}");
                }

            }
            return Result.Succeeded;
        }

    }


    public class configTagNH
    {
        public bool IsDIrectriz { get; set; }
        public TagOrientation tagOrientation { get; set; }
        public XYZ desplazamientoPathReinSpanSymbol { get; set; }
        public string nombrefamilia { get;  set; }
        public string namefamilyEspecifico { get;  set; }
    }
}
