using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinformentSymbol.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.Tipos
{
    class PathSymbolF20B : ChangeFamilyPathReinforceSymbol, IPathSymbol
    {
        private PathSymbol_REbarshape_FxxDTO pSymbFxxDTO;
        private List<ParametroNewTipoPathSymbol> ListaTipoFamilia_conpata;
        private PathReinforceSymbolDTO _PSD_eIzq;
        private PathReinforceSymbolDTO _PSD_eDere;
        private PathReinforceSymbolDTO _PSD_entre;


        public PathSymbolF20B(UIApplication uiapp, DatosNuevaBarraDTO _DatosNuevaBarraDTO, PathSymbol_REbarshape_FxxDTO _PathSymbolF20B) : base(uiapp, _DatosNuevaBarraDTO)
        {
            pSymbFxxDTO = _PathSymbolF20B;
            ListaTipoFamilia_conpata = new List<ParametroNewTipoPathSymbol>();
        }


        public bool M1_ObtenerPArametros()
        {
            try
            {
                M1_1_OtenerPArametrosBAsePOrescala();

                double largopata_foot = (pSymbFxxDTO.pataDere_foot == 0 ? _datosNuevaBarraDTO.LargoPataFoot() : pSymbFxxDTO.pataDere_foot);

                PathReinforceSymbolDTO _PDS_pataDere = new PathReinforceSymbolDTO("pataDere", largopata_foot / _scaleView);
                _listaPara_pathSymbol.Add(_PDS_pataDere);

                PathReinforceSymbolDTO _pDS_DesDereSup = new PathReinforceSymbolDTO("DesIzqSup", (pSymbFxxDTO.DesIzqSup_foot>0? pSymbFxxDTO.DesIzqSup_foot + ConstNH.CONST_EXTRADESDFASE_FOOT:0) / _scaleView);
                _listaPara_pathSymbol.Add(_pDS_DesDereSup);

                PathReinforceSymbolDTO _pDS_DesDereInf = new PathReinforceSymbolDTO("DesIzqInf", (pSymbFxxDTO.DesIzqInf_foot >0?pSymbFxxDTO.DesIzqInf_foot + ConstNH.CONST_EXTRADESDFASE_FOOT:0) / _scaleView);
                _listaPara_pathSymbol.Add(_pDS_DesDereInf);

                //nombre familia con pata
                nombreFamiliaCOnPata = _datosNuevaBarraDTO.ObtenerNombrePAthSymbol(_scala);

                // si pata es distinto  a desplazamiento
                double LargoDistintoCero_Foot = (Util.IsSimilarValor(pSymbFxxDTO.DesIzqSup_foot, 0, 0.001)
                                                            ? pSymbFxxDTO.DesIzqInf_foot
                                                            : pSymbFxxDTO.DesIzqSup_foot);

                if (!Util.IsSimilarValor(largopata_foot, LargoDistintoCero_Foot))
                {

                    if (Util.IsSimilarValor(pSymbFxxDTO.DesIzqInf_foot, 0, 0.001))
                        nombreFamiliaCOnPata = nombreFamiliaCOnPata + "_D" + Util.FootToCm(pSymbFxxDTO.DesIzqSup_foot).ToString();
                    else if (Util.IsSimilarValor(pSymbFxxDTO.DesIzqSup_foot, 0, 0.001))
                        nombreFamiliaCOnPata = nombreFamiliaCOnPata + "_DI" + Util.FootToCm(pSymbFxxDTO.DesIzqInf_foot).ToString();
                }
                else if (Util.IsSimilarValor(pSymbFxxDTO.DesIzqInf_foot, 0, 0.001))
                    nombreFamiliaCOnPata = nombreFamiliaCOnPata + "_D" + Util.FootToCm(pSymbFxxDTO.DesIzqSup_foot).ToString();
                else if (Util.IsSimilarValor(pSymbFxxDTO.DesIzqSup_foot, 0, 0.001))
                    nombreFamiliaCOnPata = nombreFamiliaCOnPata + "_DI" + Util.FootToCm(pSymbFxxDTO.DesIzqInf_foot).ToString();


                ListaTipoFamilia_conpata.Add(new ParametroNewTipoPathSymbol(nombreFamiliaCOnPata, _listaPara_pathSymbol));
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'PathSymbolF20'  \n ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private void M1_1_OtenerPArametrosBAsePOrescala()
        {
            double eIzq = 18;
            double eDere = 18;
            double entre = 20;

            _PSD_eIzq = new PathReinforceSymbolDTO(_parametro: "eIzq", _valorParametro: Util.CmToFoot(eIzq / _scaleView));
            _PSD_eDere = new PathReinforceSymbolDTO(_parametro: "eDere", _valorParametro: Util.CmToFoot(eDere / _scaleView));
            _PSD_entre = new PathReinforceSymbolDTO(_parametro: "entre", _valorParametro: Util.CmToFoot(entre / _scaleView));
            _listaPara_pathSymbol.Add(_PSD_eIzq);
            _listaPara_pathSymbol.Add(_PSD_eDere);
            _listaPara_pathSymbol.Add(_PSD_entre);
        }

        public bool M2_ejecutar(ParametroNewTipoPathSymbol TipoFamilia_conpata = null)
        {
            try
            {
                if (TipoFamilia_conpata == null && pSymbFxxDTO.CopiarFamiliasDiferentesPatas)
                    CAlcularCAsoEstadar();

                //List<ParametrosNewPathSymbol> ListaTipoFamilia_conpata = null; //_datosNuevaBarraDTO.Lista_ObtenerNombrePAthSymbolCOnpata();
                M2_CrearCOpiaDePAthsymbolSegunPata(ListaTipoFamilia_conpata);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'PathSymbolF20'  \n ex:{ex.Message}");
                return false;
            }
            return true;
        }

        private void CAlcularCAsoEstadar()
        {


            double[] listPAta = new double[] { 30f, 35f, 40f, 45f, 50f, 55f, 60f, 65f, 70f, 75f, 80f, 85f, 90f, 95f, 100f, 105f };

            ListaTipoFamilia_conpata.Clear();

            foreach (double item in listPAta)
            {
                List<PathReinforceSymbolDTO> _listaPara_pathSymbol_casoEstandar = new List<PathReinforceSymbolDTO>();
                _listaPara_pathSymbol_casoEstandar.Add(_PSD_eIzq);
                _listaPara_pathSymbol_casoEstandar.Add(_PSD_eDere);
                _listaPara_pathSymbol_casoEstandar.Add(_PSD_entre);


                //
                PathReinforceSymbolDTO _PDS_pataIzq = new PathReinforceSymbolDTO("pataDere", Util.CmToFoot(item / _scaleView));
                _listaPara_pathSymbol_casoEstandar.Add(_PDS_pataIzq);
                PathReinforceSymbolDTO _pDS_DesDereSup = new PathReinforceSymbolDTO("DesIzqSup", Util.CmToFoot(item / _scaleView));
                _listaPara_pathSymbol_casoEstandar.Add(_pDS_DesDereSup);
                PathReinforceSymbolDTO _pDS_DesDereInf = new PathReinforceSymbolDTO("DesIzqInf", 0);
                _listaPara_pathSymbol_casoEstandar.Add(_pDS_DesDereInf);


                string nombreFamiliaBase_conEscala = _datosNuevaBarraDTO.nombreSimboloPathReinforcement + "_" + _scala + "_P" + item.ToString();

                ParametroNewTipoPathSymbol _ParametrosNewPathSymbol = new ParametroNewTipoPathSymbol(nombreFamiliaBase_conEscala, _listaPara_pathSymbol_casoEstandar);

                ListaTipoFamilia_conpata.Add(_ParametrosNewPathSymbol);
            }

        }


    }
}
