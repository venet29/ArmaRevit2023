
using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinformentSymbol.DTO;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FAMILIA;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.Tipos
{


    public class PathReinforceSymbolDTO
    {

        public PathReinforceSymbolDTO(string _parametro, double _valorParametro)
        {
            this.parametro = _parametro;
            this.valorParametro = _valorParametro;
        }

        public string parametro { get; set; }
        public double valorParametro { get; set; }

    }
    class JtFamilyLoadOptions : IFamilyLoadOptions
    {
        public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
        {
            overwriteParameterValues = true;
            return true;
        }

        public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
        {
            source = FamilySource.Family;
            overwriteParameterValues = true;
            return true;
        }
    }

    public class ChangeFamilyPathReinforceSymbol
    {
        protected UIApplication _uiapp;
        protected readonly DatosNuevaBarraDTO _datosNuevaBarraDTO;
        protected Document _doc;
        protected View _view;

        protected string nombreFamiliaBase_conEscala;
        private int contador; // 29-05-2023 borrar solo para carcar familia   f10A
        //protected string nombreFamiliaCOnPata;
        public string nombreFamiliaCOnPata { get; set; }
        protected string _scala;
        protected int _scaleView;
        protected List<PathReinforceSymbolDTO> _listaPara_pathSymbol;
        public Element elemtoSymboloPath { get; set; }
       // protected double extrasDesfase_foot;

        public ChangeFamilyPathReinforceSymbol(UIApplication uiapp, DatosNuevaBarraDTO _DatosNuevaBarraDTO)
        {
            this._uiapp = uiapp;
            this._datosNuevaBarraDTO = _DatosNuevaBarraDTO;
            this._doc = uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            _listaPara_pathSymbol = new List<PathReinforceSymbolDTO>();
            _scala = _view.ObtenerEscalaPAraPAthSimbol().ToString();
            _scaleView = _view.Scale;
            nombreFamiliaBase_conEscala = _datosNuevaBarraDTO.nombreSimboloPathReinforcement + "_" + _scala;
            contador = 0;
          //  extrasDesfase_foot = ConstNH.CONST_EXTRADESDFASE_FOOT;
        }





        protected bool M2_CrearCOpiaDePAthsymbolSegunPata(List<ParametroNewTipoPathSymbol> ListaTipoFamilia_conpata)
        {
            try
            {
                // nose para que es este parametro   02-20-2023
                bool desactivarCopiarFamiliasGrupo = true;

                elemtoSymboloPath = TiposPathReinSpanSymbol.M1_GetFamilySymbol_nh(nombreFamiliaCOnPata, _doc);

                if (elemtoSymboloPath != null) return true;

                if (_datosNuevaBarraDTO.PathSymbol_REbarshape_FXXDTO_.CopiarFamiliasDiferentesPatas && desactivarCopiarFamiliasGrupo)
                {
                    var result = Util.InfoMsg_YesNo($"No se ha encontrado familia '{nombreFamiliaCOnPata}' en la vista actual con escala {_scaleView}. \n\nDesea cargar toda la familia de pathsymbol '{_datosNuevaBarraDTO.nombreSimboloPathReinforcement}_ {_scala}'?? \n\n " +
                        $"NOTA: al cargar el resto de la familia los el dibujo de barras sera mas rapido.");
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {

                        M2_1_ObtenerListaDefamiliaporPAtas_Contrans(nombreFamiliaBase_conEscala, ListaTipoFamilia_conpata);

                        elemtoSymboloPath = TiposPathReinSpanSymbol.M1_GetFamilySymbol_nh(nombreFamiliaCOnPata, _doc);
                    }
                }


                if (elemtoSymboloPath == null)
                {
                    return M2_2_ObtenerFamiliaConPAtas_Contrans();
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear '{nombreFamiliaCOnPata}'.  \n ex:{ex.Message}");
                return false;
            }
            return true;
        }

        //1)
        public bool M2_1_ObtenerListaDefamiliaporPAtas_Contrans(string nombreFamiliaBase_conescala, List<ParametroNewTipoPathSymbol> ListaTipoFamilia_conpata)
        {
            try
            {

                _datosNuevaBarraDTO.nombreSimboloPathReinforcement_escala = nombreFamiliaBase_conescala;


                FamilySymbol famSyb = (FamilySymbol)TiposPathReinSpanSymbol.M1_GetFamilySymbol_nh(nombreFamiliaBase_conescala, _doc);

                if (famSyb == null)
                {
                    Util.ErrorMsg($"Error no se encontro familia base:'{nombreFamiliaBase_conEscala}'  para obtener 'nombreFamiliaCOnPata'");
                    return false;
                }
                Family fam = famSyb.Family;


                if (fam != null && fam.IsValidObject)
                {
                    M2_3_SetDimensionPathReinforceSymbol_ConTrans(fam, _doc, ListaTipoFamilia_conpata);
                }


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener '1PathSymbolF'  \n ex:{ex.Message}");
                return false;
            }
            return true;
        }

        //2)
        private bool M2_2_ObtenerFamiliaConPAtas_Contrans()
        {
            try
            {
                FamilySymbol famSyb = (FamilySymbol)TiposPathReinSpanSymbol.M1_GetFamilySymbol_nh(nombreFamiliaBase_conEscala, _doc);

                if (famSyb == null)
                {
                    Util.ErrorMsg($"Error no se encontro familia base:'{nombreFamiliaBase_conEscala}'  para obtener 'nombreFamiliaCOnPata'");
                    return false;
                }

                Family fam = famSyb.Family;
                if (fam != null && fam.IsValidObject)
                {
                    List<ParametroNewTipoPathSymbol> ListaTipoFamilia_conpata_solo1caso = new List<ParametroNewTipoPathSymbol>() {
                            new ParametroNewTipoPathSymbol(nombreFamiliaCOnPata,_listaPara_pathSymbol) };

                    if (!M2_3_SetDimensionPathReinforceSymbol_ConTrans(fam, _doc, ListaTipoFamilia_conpata_solo1caso)) return false;

                    elemtoSymboloPath = TiposPathReinSpanSymbol.M1_GetFamilySymbol_nh(nombreFamiliaCOnPata, _doc);


                    //if (("M_Path Reinforcement Symbol_F11A_50" == nombreFamiliaBase_conEscala || "M_Path Reinforcement Symbol_F10AIzq_50" == nombreFamiliaBase_conEscala) 
                    //    && elemtoSymboloPath == null && contador < 1)
                    //{
                    //    ManejadorCargarFAmilias _ManejadorCargarFAmilias = new ManejadorCargarFAmilias(_uiapp);
                    //    _ManejadorCargarFAmilias.cargarFamilias_F10A();

                    //    Util.InfoMsg($"Se recargaron algunas familiar. Cargar nuevamente comando");
                    //    return false;

                    //}

                    if (elemtoSymboloPath == null)
                    {
                        Util.ErrorMsg($"No se pudo crear PathReinSpanSymbol.{nombreFamiliaCOnPata}");
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en ObtenerFamiliaConPAtas. \n ex:{ex.Message}");
                return false;
            }
            return true;
        }

      
        protected bool M2_3_SetDimensionPathReinforceSymbol_ConTrans(Family f, Document _doc, List<ParametroNewTipoPathSymbol> ListaTipoFamilia)
        {
            bool result = false;
            if (f == null) return false;
            Document famdoc = null;

            if (f.IsEditable)
            {
                try
                {
                    famdoc = _doc.EditFamily(f);
                    Debug.Write(famdoc.PathName );
                    FamilyManager familyManager = famdoc.FamilyManager;
                    using (Transaction tranew = new Transaction(famdoc))
                    {
                        tranew.Start("UpdatePathReinforceSymbol-NH");

                        foreach (ParametroNewTipoPathSymbol nameFamilia in ListaTipoFamilia)
                        {
                            var VerificarSIesta = TiposPathReinSpanSymbol.M1_GetFamilySymbol_nh(nameFamilia.nombreNewFamilyPathSYmbol, _doc);
                            if (VerificarSIesta != null) continue;

                            if (VerificarSITipoEstaDentroEstructuraFAmilia(familyManager, nameFamilia.nombreNewFamilyPathSYmbol)) continue;

                            FamilyType newFamilyType = familyManager.NewType(nameFamilia.nombreNewFamilyPathSYmbol);
                            foreach (PathReinforceSymbolDTO item in nameFamilia._listaPrametros)
                            {
                                FamilyParameter familyParam = familyManager.get_Parameter(item.parametro);
                                if (null != familyParam) familyManager.Set(familyParam, item.valorParametro);
                            }
                        }
                        tranew.Commit();
                    }
                    result = true;
                    IFamilyLoadOptions opt = new JtFamilyLoadOptions();
                    Family f2 = famdoc.LoadFamily(_doc, opt);

                }
                catch (System.Exception ex)
                {
                    Util.ErrorMsg($"Error al crear Lista pathsymbol '{f.Name}'  \nex:{ex.Message}");
                    result = false;
                }
                finally
                {

                }
            }

            // famdoc.Close();

            return result;
        }

        private bool VerificarSITipoEstaDentroEstructuraFAmilia(FamilyManager familyManager,string nombreTipo)
        {
           //ConstNH.CONST_CORTO;
            if (familyManager == null) return true; //para q no siga con la transaccion y quede con error
            if(nombreTipo=="") return true; //para q no siga con la transaccion y quede con error
            try
            {
                //***
                //esta seccion esta pq al crear y ahcer undo la familia que queda dentro de las estructura de la familia, PERO NO en el proyecto
                FamilyTypeSet familyTypes = familyManager.Types;
                FamilyTypeSetIterator familyTypesItor = familyTypes.ForwardIterator();
                familyTypesItor.Reset();
                while (familyTypesItor.MoveNext())
                {
                    FamilyType familyType = familyTypesItor.Current as FamilyType;
                    Debug.Print(familyType.Name);

                    if (nombreTipo == familyType.Name)
                        return true;
                }
                //**
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al validar si tipo {nombreTipo} existe dentro de la estructura de familia   \n ex:{ex.Message}");
            }
            return false;
        }

    }
}
