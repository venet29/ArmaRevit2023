using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ArmaduraLosaRevit.Model.LosaEstructural
{

    public class CreardorPelotaLosaEstructural
    {
        //double angle = 16.3;
        //angle = 16.3;
        //angulo -9.22 indica pelota de losa esta girada -9.22grados, despues se hace la conversion grado radian

        List<XYZ> ListaPtos = new List<XYZ>();

        private Document _doc;
        private string _numbrePelotaLosa;
        private SeleccionarLosaConMouse _seleccionarLosaConMouse;
#pragma warning disable CS0169 // The field 'CreardorPelotaLosaEstructural._commandData' is never used
        private ExternalCommandData _commandData;
#pragma warning restore CS0169 // The field 'CreardorPelotaLosaEstructural._commandData' is never used
        private UIApplication _uiapp;
        private UIDocument _uidoc;
        private string _message;
        private string nombrePelotaLosa;
        private double AngPelotaObteneridoPanelRad;
        private string espesor;
        private View _view;
        private int _escala;
        private int _direV;
        private int _direH;

        public CreardorPelotaLosaEstructural(UIApplication uiapp)
        {
            this._seleccionarLosaConMouse = new SeleccionarLosaConMouse(uiapp);

            this._uiapp = uiapp;
            this._uidoc = this._uiapp.ActiveUIDocument;
            this._doc = this._uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            this._escala = _view.Scale;
            this._direV = -1;
            this._direH = -1;
        }



        public void Ejecutar_e(string _nombrePelotaLosa, string _AngPelotaObteneridoPanelGrado)
        {

            if (ConfiguracionInicial() != Result.Succeeded) return;

            nombrePelotaLosa = _nombrePelotaLosa;
            if (nombrePelotaLosa == "") return;

            double AngPelotaObteneridoPanelGrado = Util.ConvertirStringInDouble(_AngPelotaObteneridoPanelGrado);

            AngPelotaObteneridoPanelRad = Util.GradosToRadianes(AngPelotaObteneridoPanelGrado);
            if (AngPelotaObteneridoPanelRad == -1) return;

            _numbrePelotaLosa = ConstNH.CONST_TAGLOSAESTRUCTURAL + "_" + 50; // _escala;// + _view.Scale;
            //programa principal
            C1CargandoIteracionSeleccion(_numbrePelotaLosa);
        }

        internal void Ejecutar_e_Armadura(string _nombrePelotaLosa, string _AngPelotaObteneridoPanelGrado, string direV, string direH)
        {

            if (ConfiguracionInicial() != Result.Succeeded) return;

            nombrePelotaLosa = _nombrePelotaLosa;
            if (nombrePelotaLosa == "") return;

            _direV = Util.ConvertirStringInInteger( direV);
            _direH = Util.ConvertirStringInInteger(direH);
            if (_direV == -1) return;
            if (_direH == -1) return;

            double AngPelotaObteneridoPanelGrado = Util.ConvertirStringInDouble(_AngPelotaObteneridoPanelGrado);

            AngPelotaObteneridoPanelRad = Util.GradosToRadianes(AngPelotaObteneridoPanelGrado);
            if (AngPelotaObteneridoPanelRad == -1) return;

            _numbrePelotaLosa = ConstNH.CONST_TAGLOSAARMA + "_" + 50; // _escala;// + _view.Scale;
            //programa principal
            C1CargandoIteracionSeleccion(_numbrePelotaLosa);
        }

        public void EjecutarVAr(string _nombrePelotaLosa, string _AngPelotaObteneridoPanelRad, string _espesor)
        {

            if (ConfiguracionInicial() != Result.Succeeded) return;

            nombrePelotaLosa = _nombrePelotaLosa;
            if (nombrePelotaLosa == "") return;

            AngPelotaObteneridoPanelRad = Util.ConvertirStringInDouble(_AngPelotaObteneridoPanelRad);
            if (AngPelotaObteneridoPanelRad == -1) return;

            espesor = _espesor;

            _numbrePelotaLosa = ConstNH.CONST_TAGLOSAESTRUCTURAL + "Var_" + 50;// _escala;// + _view.Scale;
            //programa principal
            C1CargandoIteracionSeleccion(_numbrePelotaLosa, espesor);
        }
        private Result ConfiguracionInicial()
        {
            nombrePelotaLosa = "100";
            AngPelotaObteneridoPanelRad = 0.0;
            espesor = "15";


            View3D elem3d = _view as View3D;
            if (null != elem3d)
            {
                _message += "Only create dimensions in 2D";
                return Result.Failed;

            }
            return Result.Succeeded;
        }



        public Result C1CargandoIteracionSeleccion(string _numbrePelotaLosa, string espesor_ = "")
        {

            bool seguir = true;

            while (seguir)
            {

                //obtiene una referencia floor con la referencia r
              //  Floor selecFloor = _seleccionarLosaConMouse.M1_SelecconarFloor();
                //obtiene una referencia floor con la referencia r
                if (_seleccionarLosaConMouse.M1_SelecconarFloor() == null) return Result.Failed;

                Floor selecFloor = _seleccionarLosaConMouse.LosaSelecionado;

                if (selecFloor == null) return Result.Succeeded;


                if (espesor_ == "")
                {
                    espesor_ = selecFloor.ObtenerEspesorLosaCm(_doc).ToString();//  ObtenerEspesorLosa(selecFloor);
                }

                espesor = espesor_;
                espesor_ = "";
                //obtiene el nivel del la losa
                Level levelLOsa = _doc.GetElement(selecFloor.LevelId) as Level;
                //obtiene el pto de seleccion con el mouse sobre la losa
                XYZ pt_selec = new XYZ(_seleccionarLosaConMouse._ptoSeleccionEnLosa.X, _seleccionarLosaConMouse._ptoSeleccionEnLosa.Y, levelLOsa.ProjectElevation + Util.CmToFoot(2));
                // UV pt_selecUV = r.UVPoint;

                //View actual
                Autodesk.Revit.DB.View view = _doc.ActiveView;
                // busca el nivel del pisos analizado
                ElementId levelId = Util.FindLevelId(_doc, view.GenLevel.Name);



                //obtiene el familysymbol 
                FamilySymbol PelotaLosaFamilySymbol = TiposGenericAnnotation.M1_GetFamilySymbol_nh(_numbrePelotaLosa, _doc);

                if (PelotaLosaFamilySymbol == null)
                {
                    Autodesk.Revit.UI.TaskDialog.Show("Error", "No se encuentra familia :" + _numbrePelotaLosa);
                    return Result.Failed;
                }

                try
                {
                    using (Transaction t = new Transaction(_doc))
                    {
                        t.Start("CrearPelotaLosaEstructural_aux-NH");
                        //agrega la annotation generico de pelota de losa 
                        FamilyInstance familyInstance = _doc.Create.NewFamilyInstance(pt_selec, PelotaLosaFamilySymbol, view);
                        // agrega los parametros
                        Mc1AgregarParametos(familyInstance);

                        Mc1AgregarParametos_DIrecciones(familyInstance);


                        Mc1RotarPelota(pt_selec, familyInstance);

                        t.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Util.ErrorMsg($"  EX:{ex.Message}");
                }
                int aux = 0;

                string CeroInicial = BuscarCeroInicial(nombrePelotaLosa);

                if (!int.TryParse(nombrePelotaLosa, out aux))
                {
                    seguir = false;
                }
                else
                {
                    nombrePelotaLosa = CeroInicial+(aux + 1).ToString();
                }


            }


            return Result.Succeeded;
        }

        private string BuscarCeroInicial(string nombrePelotaLosa)
        {
            string resul = "";

            try
            {
               char  primerElem= nombrePelotaLosa.FirstOrDefault();

                if (primerElem.Equals('0'))
                    resul = "0";
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
            return resul;
        }

        private string ObtenerEspesorLosa(Floor selecFloor)
        {

            espesor = ParameterUtil.FindParaByBuiltInParameter(selecFloor, BuiltInParameter.FLOOR_ATTR_THICKNESS_PARAM, _doc);

            double aux_espesor = 0;
            if (double.TryParse(espesor, out aux_espesor))
            {
                espesor = Util.FootToCm(aux_espesor).ToString();

            }

            return espesor;
        }

        private void Mc1AgregarParametos(FamilyInstance familyInstance)
        {
            var nm = ParameterUtil.SetParaInt(familyInstance, "NUMERO", nombrePelotaLosa);
            var es = ParameterUtil.SetParaInt(familyInstance, "ESPESOR", espesor);
            var ANG = ParameterUtil.SetParaInt(familyInstance, "ANGULO", AngPelotaObteneridoPanelRad);
        }

        private void Mc1AgregarParametos_DIrecciones(FamilyInstance familyInstance)
        {
            if(_direV == -1) return;
            if (_direH == -1) return;

            var nm = ParameterUtil.SetParaIntNH(familyInstance, "DireH", _direH);
            var es = ParameterUtil.SetParaIntNH(familyInstance, "DireV", _direV);
            
        }

        private void Mc1RotarPelota(XYZ pt_selec, FamilyInstance familyInstance)
        {
            Line axis = Line.CreateBound(pt_selec, pt_selec + new XYZ(0, 0, 10));
            ElementTransformUtils.RotateElement(_doc, familyInstance.Id, axis, AngPelotaObteneridoPanelRad);
        }

        /// <summary>
        /// obtiene  el nombre de la losa del texbox que se encuentra en el panelRibbon
        /// </summary>
        /// <param name="nombrePelotaLosa"></param>
        /// <returns></returns>
        private string NombrePelotaDeLosaDesdePanelRibbon()
        {
            Autodesk.Windows.RibbonControl pRibbon = Autodesk.Windows.ComponentManager.Ribbon;
            foreach (var pTab in pRibbon.Tabs)
            {
                //titilo del tab
                if (pTab.Id == "Diseño Estructura")
                {

                    //lsita de las subdiviciones tab
                    var ListaPanel = pTab.Panels;


                    foreach (var item in ListaPanel)
                    {

                        var aux4 = item.Source.Items;

                        foreach (var item3 in aux4)
                        {
                            //if (item3.Id.ToString().Contains("TextBox_losas"))
                            if (item3 is RibbonRowPanel)
                            {
                                RibbonRowPanel item3_Aux = item3 as RibbonRowPanel;

                                foreach (var item4 in item3_Aux.Items)
                                {
                                    if (item4.Id.ToString().Contains("TextBox_losas"))
                                    {
                                        var sdf = (RibbonTextBox)item4;
                                        if (sdf.Value == null)
                                        {
                                            Autodesk.Revit.UI.TaskDialog.Show("Error", "No se asigno nombre a Losa ");
                                            return "";
                                        }

                                        if (sdf.Value.ToString() == "")
                                        {
                                            Autodesk.Revit.UI.TaskDialog.Show("Error", "No se asigno nombre a Losa ");
                                            return "";
                                        }

                                        else
                                        {
                                            nombrePelotaLosa = sdf.Value.ToString();

                                            return nombrePelotaLosa;

                                        }
                                    }
                                }
                            }
                        }

                        var nombre = item.Id;
                    }
                    // agrgea panel al tab
                    // pTab.Panels.Add(CreateCustomPanel());
                    break;
                }
            }
            return "";
        }

   
    }

}
