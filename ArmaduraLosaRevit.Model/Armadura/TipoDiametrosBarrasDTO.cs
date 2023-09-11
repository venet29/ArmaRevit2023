using ArmaduraLosaRevit.Model.Armadura.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Armadura
{
    public class TipoDiametrosBarrasDTO
    {

        private static List<DiametrosBarrasDTO> ListDiametrosBarrasDTO { get; set; }


        public static List<DiametrosBarrasDTO> ObtenerListaDiametro()
        {
            if (ListDiametrosBarrasDTO == null)
                CArgar();
            else if (ListDiametrosBarrasDTO.Count != 10)
                CArgar();

            return ListDiametrosBarrasDTO;
        }

        private static void CArgar()
        {
            ListDiametrosBarrasDTO = new List<DiametrosBarrasDTO>();
            ListDiametrosBarrasDTO.Add(new DiametrosBarrasDTO(6, 4));
            ListDiametrosBarrasDTO.Add(new DiametrosBarrasDTO(8, 4));
            ListDiametrosBarrasDTO.Add(new DiametrosBarrasDTO(10, 4));
            ListDiametrosBarrasDTO.Add(new DiametrosBarrasDTO(12, 4));
            ListDiametrosBarrasDTO.Add(new DiametrosBarrasDTO(16, 4));

            ListDiametrosBarrasDTO.Add(new DiametrosBarrasDTO(18, 6));
            ListDiametrosBarrasDTO.Add(new DiametrosBarrasDTO(22, 6));
            ListDiametrosBarrasDTO.Add(new DiametrosBarrasDTO(25, 6));
            ListDiametrosBarrasDTO.Add(new DiametrosBarrasDTO(28, 6));
            ListDiametrosBarrasDTO.Add(new DiametrosBarrasDTO(32, 6));
            ListDiametrosBarrasDTO.Add(new DiametrosBarrasDTO(36, 6));
        }

        public static void Limpiar() => ListDiametrosBarrasDTO = new List<DiametrosBarrasDTO>();


    }
}
