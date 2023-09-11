﻿using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.DatosParaBarra;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinformentSymbol.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.FAMILIA;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.Tipos
{

    class PathSymbolF16A : ChangeFamilyPathReinforceSymbol, IPathSymbol
    {
        private readonly SolicitudBarraDTO solicitudDTO;
        private PathSymbol_REbarshape_FxxDTO pathSymbolFXXADTO;
        private List<ParametroNewTipoPathSymbol> ListaTipoFamilia_conpata;
#pragma warning disable CS0169 // The field 'PathSymbolF16A._PSD_eIzq' is never used
        private PathReinforceSymbolDTO _PSD_eIzq;
#pragma warning restore CS0169 // The field 'PathSymbolF16A._PSD_eIzq' is never used
#pragma warning disable CS0169 // The field 'PathSymbolF16A._PSD_eDere' is never used
        private PathReinforceSymbolDTO _PSD_eDere;
#pragma warning restore CS0169 // The field 'PathSymbolF16A._PSD_eDere' is never used
        private PathReinforceSymbolDTO _PSD_entre;

        public PathSymbolF16A(UIApplication uiapp, SolicitudBarraDTO solicitudDTO, DatosNuevaBarraDTO _DatosNuevaBarraDTO, PathSymbol_REbarshape_FxxDTO _PathSymbolF20ADTO) : base(uiapp, _DatosNuevaBarraDTO)
        {
            this.solicitudDTO = solicitudDTO;
            pathSymbolFXXADTO = _PathSymbolF20ADTO;
            ListaTipoFamilia_conpata = new List<ParametroNewTipoPathSymbol>();
        }


        public bool M1_ObtenerPArametros()
        {
            try
            {
                M1_1_OtenerPArametrosBAsePOrescala();

                double largoDesfase_foot = _datosNuevaBarraDTO.LargoPataFoot() + ConstNH.CONST_EXTRADESDFASE_FOOT;

                if (solicitudDTO.DireccionEditarDesplazamiento == TipoDireccion.Izquierda)
                {
                    PathReinforceSymbolDTO _pDS_DesIzqInf = new PathReinforceSymbolDTO("DesIzqInf", (pathSymbolFXXADTO.DesIzqInf_foot > -1 ? pathSymbolFXXADTO.DesIzqInf_foot: largoDesfase_foot) / _scaleView);
                    _listaPara_pathSymbol.Add(_pDS_DesIzqInf);

                    PathReinforceSymbolDTO _pDS_DesDereSup = new PathReinforceSymbolDTO("DesDereSup", (pathSymbolFXXADTO.DesDereSup_foot > -1 ? pathSymbolFXXADTO.DesDereSup_foot : largoDesfase_foot) / _scaleView);
                    _listaPara_pathSymbol.Add(_pDS_DesDereSup);

                    if(pathSymbolFXXADTO.DesIzqInf_foot > -1 && pathSymbolFXXADTO.DesDereSup_foot > -1)
                        nombreFamiliaCOnPata = _datosNuevaBarraDTO.nombreSimboloPathReinforcement + "_" + _scala + "_DA" + Util.FootToCm(pathSymbolFXXADTO.DesIzqInf_foot).ToString() + "_DB" + Math.Round( Util.FootToCm(pathSymbolFXXADTO.DesDereSup_foot),0).ToString();
                    else
                        nombreFamiliaCOnPata = _datosNuevaBarraDTO.nombreSimboloPathReinforcement + "_" + _scala + "_DA" + Util.FootToCm(pathSymbolFXXADTO.DesIzqInf_foot).ToString();

                }
                else
                {
                    PathReinforceSymbolDTO _pDS_DesIzqInf = new PathReinforceSymbolDTO("DesIzqInf", (pathSymbolFXXADTO.DesIzqInf_foot > -1 ? pathSymbolFXXADTO.DesIzqInf_foot : largoDesfase_foot) / _scaleView);
                    _listaPara_pathSymbol.Add(_pDS_DesIzqInf);

                    PathReinforceSymbolDTO _pDS_DesDereSup = new PathReinforceSymbolDTO("DesDereSup", (pathSymbolFXXADTO.DesDereSup_foot > -1 ? pathSymbolFXXADTO.DesDereSup_foot  : largoDesfase_foot) / _scaleView);
                    _listaPara_pathSymbol.Add(_pDS_DesDereSup);

                    if (pathSymbolFXXADTO.DesIzqInf_foot > -1 && pathSymbolFXXADTO.DesDereSup_foot > -1)
                        nombreFamiliaCOnPata = _datosNuevaBarraDTO.nombreSimboloPathReinforcement + "_" + _scala + "_DA" + Math.Round(Util.FootToCm(pathSymbolFXXADTO.DesIzqInf_foot),0).ToString() + "_DB" + Util.FootToCm(pathSymbolFXXADTO.DesDereSup_foot).ToString();
                    else
                        nombreFamiliaCOnPata = _datosNuevaBarraDTO.nombreSimboloPathReinforcement + "_" + _scala + "_DB" + Util.FootToCm(pathSymbolFXXADTO.DesDereSup_foot).ToString();
                }


                if (nombreFamiliaCOnPata == "")
                {
                    nombreFamiliaCOnPata = _datosNuevaBarraDTO.nombreSimboloPathReinforcement + "_" + _scala + "_D" + _datosNuevaBarraDTO.LargoPataFoot().ToString();
                }

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
            //    double eIzq = 18;
            //  double eDere = 18;
            double entre = 20;

            //  _PSD_eIzq = new PathReinforceSymbolDTO(_parametro: "eIzq", _valorParametro: Util.CmToFoot(eIzq / _scaleView));
            // _PSD_eDere = new PathReinforceSymbolDTO(_parametro: "eDere", _valorParametro: Util.CmToFoot(eDere / _scaleView));
            _PSD_entre = new PathReinforceSymbolDTO(_parametro: "entre", _valorParametro: Util.CmToFoot(entre / _scaleView));
            // _listaPara_pathSymbol.Add(_PSD_eIzq);
            // _listaPara_pathSymbol.Add(_PSD_eDere);
            _listaPara_pathSymbol.Add(_PSD_entre);
        }

        public bool M2_ejecutar(ParametroNewTipoPathSymbol TipoFamilia_conpata = null)
        {
            try
            {

                if (TipoFamilia_conpata == null && pathSymbolFXXADTO.CopiarFamiliasDiferentesPatas)
                    CAlcularCAsoEstadar();

                //List<ParametrosNewPathSymbol> ListaTipoFamilia_conpata = null; //_datosNuevaBarraDTO.Lista_ObtenerNombrePAthSymbolCOnpata();
                if (!M2_CrearCOpiaDePAthsymbolSegunPata(ListaTipoFamilia_conpata)) return false;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'PathSymbolF16'  \n ex:{ex.Message}");
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
                //  _listaPara_pathSymbol_casoEstandar.Add(_PSD_eIzq);
                //  _listaPara_pathSymbol_casoEstandar.Add(_PSD_eDere);
                _listaPara_pathSymbol_casoEstandar.Add(_PSD_entre);


                //
                PathReinforceSymbolDTO _PDS_pataIzq = new PathReinforceSymbolDTO("DesIzqInf", Util.CmToFoot(item / _scaleView));
                _listaPara_pathSymbol_casoEstandar.Add(_PDS_pataIzq);
                PathReinforceSymbolDTO _pDS_DesDereSup = new PathReinforceSymbolDTO("DesDereSup", Util.CmToFoot(item / _scaleView));
                _listaPara_pathSymbol_casoEstandar.Add(_pDS_DesDereSup);
                //PathReinforceSymbolDTO _pDS_DesDereInf = new PathReinforceSymbolDTO("DesDereInf",0);
                //_listaPara_pathSymbol_casoEstandar.Add(_pDS_DesDereInf);


                string nombreFamiliaBase_conEscala = _datosNuevaBarraDTO.nombreSimboloPathReinforcement + "_" + _scala + "_DI" + item.ToString() + "DD" + item.ToString();

                ParametroNewTipoPathSymbol _ParametrosNewPathSymbol = new ParametroNewTipoPathSymbol(nombreFamiliaBase_conEscala, _listaPara_pathSymbol_casoEstandar);

                ListaTipoFamilia_conpata.Add(_ParametrosNewPathSymbol);
            }

        }


    }
}
