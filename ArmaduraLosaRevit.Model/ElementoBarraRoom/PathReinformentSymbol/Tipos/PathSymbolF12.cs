using ArmaduraLosaRevit.Model.Armadura;
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

    class PathSymbolF12 : ChangeFamilyPathReinforceSymbol, IPathSymbol
    {
        private readonly SolicitudBarraDTO solicitudDTO;
        private PathSymbol_REbarshape_FxxDTO pathSymbolFXXADTO;
        private List<ParametroNewTipoPathSymbol> ListaTipoFamilia_conpata;
        private PathReinforceSymbolDTO _PSD_eIzq;
        private PathReinforceSymbolDTO _PSD_eDere;
#pragma warning disable CS0169 // The field 'PathSymbolF12._PSD_entre' is never used
        private PathReinforceSymbolDTO _PSD_entre;
#pragma warning restore CS0169 // The field 'PathSymbolF12._PSD_entre' is never used

        public PathSymbolF12(UIApplication uiapp, SolicitudBarraDTO solicitudDTO, DatosNuevaBarraDTO _DatosNuevaBarraDTO, PathSymbol_REbarshape_FxxDTO _PathSymbolF20ADTO) : base(uiapp, _DatosNuevaBarraDTO)
        {
            this.solicitudDTO = solicitudDTO;
            pathSymbolFXXADTO = _PathSymbolF20ADTO;
            ListaTipoFamilia_conpata = new List<ParametroNewTipoPathSymbol>();
        }


        public bool M1_ObtenerPArametros()
        {
            try
            {
                nombreFamiliaCOnPata = _datosNuevaBarraDTO.nombreSimboloPathReinforcement + "_" + _scala;
                if (pathSymbolFXXADTO.EspIzq_foot != 25)
                {
                    _PSD_eIzq = new PathReinforceSymbolDTO(_parametro: "eIzq", _valorParametro: Util.CmToFoot(pathSymbolFXXADTO.EspIzq_foot / _scaleView));
                    _listaPara_pathSymbol.Add(_PSD_eIzq);
                    nombreFamiliaCOnPata = nombreFamiliaCOnPata + $"_PI{pathSymbolFXXADTO.EspIzq_foot}";
                }

                if (pathSymbolFXXADTO.EspDere_foot != 25)
                {
                    _PSD_eDere = new PathReinforceSymbolDTO(_parametro: "eDere", _valorParametro: Util.CmToFoot(pathSymbolFXXADTO.EspDere_foot / _scaleView));
                    _listaPara_pathSymbol.Add(_PSD_eDere);
                    nombreFamiliaCOnPata = nombreFamiliaCOnPata + $"_PD{pathSymbolFXXADTO.EspDere_foot}";
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



        public bool M2_ejecutar(ParametroNewTipoPathSymbol TipoFamilia_conpata = null)
        {
            try
            {
                if (!M2_CrearCOpiaDePAthsymbolSegunPata(ListaTipoFamilia_conpata)) return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'PathSymbolF16'  \n ex:{ex.Message}");
                return false;
            }
            return true;
        }




    }
}
