using ArmaduraLosaRevit.Model.UTILES.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.TextoNoteNH
{
    public class FActoryTipoTextNote
    {

        public static string AcotarBarra { get; set; } = "AcotarBarra-NH";
        public static string TextoHorq { get; set; } = "TextoHorq";
        public static string TextoVigaIdem { get; set; } = "TextoVigaIdem";
        public static string TextoEntreLevel { get; set; } = "TextoEntreLevel";

        public static List<TipoTextoDTO> ObtenerLista()
        {
            List<TipoTextoDTO> _list = new List<TipoTextoDTO>();

            TipoTextoDTO _newACotar = new TipoTextoDTO(AcotarBarra, 255, 0, 0, _TEXT_FONT: "Arial", _TEXT_SIZE: 0.00656168, _TEXT_TAB_SIZE: 0.04166667); //0.00656168 = 2mm   //  0.04166667= 12.7 mm
            TipoTextoDTO _newTextoHorqy = new TipoTextoDTO(TextoHorq, 255, 255, 255, _TEXT_FONT: "Arial", _TEXT_SIZE: 0.00656168, _TEXT_TAB_SIZE: 0.04166667);
            TipoTextoDTO _newTextoVigaIdem = new TipoTextoDTO(TextoVigaIdem, 255, 255, 0, _TEXT_FONT: "Arial", _TEXT_SIZE: 0.00656168, _TEXT_TAB_SIZE: 0.04166667); // color amarrillo
            TipoTextoDTO _newTextoEntreLevel = new TipoTextoDTO(TextoEntreLevel, 255, 0, 255, _TEXT_FONT: "Arial", _TEXT_SIZE: 0.02624671, _TEXT_TAB_SIZE: 0.04166667); // 0.02624671 = 8mm     //  0.04166667= 12.7 mm

            _list.Add(_newACotar);
            _list.Add(_newTextoHorqy);
            _list.Add(_newTextoVigaIdem);
            _list.Add(_newTextoEntreLevel);

            return _list;
        }


        // texto para bim

        public static string TextoSumaLargoPipe { get; set; } = "TextoSumaLargoPipe";
        public static List<TipoTextoDTO> ObtenerLista_sumaLArgoPipe()
        {
            List<TipoTextoDTO> _list = new List<TipoTextoDTO>();

            TipoTextoDTO _newACotar = new TipoTextoDTO(TextoSumaLargoPipe, 255, 255, 255, _TEXT_FONT: "Arial", _TEXT_SIZE: 0.00656168, _TEXT_TAB_SIZE: 0.04166667); //0.00656168 = 2mm   //  0.04166667= 12.7 mm


            _list.Add(_newACotar);


            return _list;
        }
    }
}
