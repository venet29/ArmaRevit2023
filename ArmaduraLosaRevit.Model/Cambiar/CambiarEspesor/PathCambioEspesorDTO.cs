using ArmaduraLosaRevit.Model;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Cambiar.CambiarEspesor
{
    public class PathCambioEspesorDTO
    {
        private readonly bool isTest;

        public PathReinforcement PathRein { get; }
        public string TipoBarra { get; set; }
        public List<string> ListaEspesores { get; }
        public int cantidadError { get; set; }
        public bool IsCorrect { get; set; }
        public double Diametro_mm { get;  set; }

        public PathCambioEspesorDTO(PathReinforcement pathRein, bool isTest = false)
        {
            PathRein = pathRein;
            this.isTest = isTest;
            ListaEspesores = new List<string>();
            cantidadError = 0;
            IsCorrect = false;
            TipoBarra = "";


        }
        public bool CalculosIniciales()
        {
            try
            {
                Diametro_mm = PathRein.ObtenerDiametro_mm();
                TipoBarra = PathRein.ObtenerTipoBarra();

                ObtenerEspesorCambio();
                IsCorrect = true;
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                IsCorrect = false;
                return false;
            }
            return true;
        }
   

        private void ObtenerEspesorCambio()
        {


            //string aux_espesorCambio_ = ParameterUtil.FindValueParaByName(PathRein, "EspesorCambio", PathRein.Document);
            string aux_espesorCambio_ = TipoLetraCambiar.ObtenerLetra(TipoBarra);
            if (aux_espesorCambio_ == null)
            {
                IsCorrect = false;
                cantidadError += 1;
                if (!isTest) Util.ErrorMsg("Error eb Obtener Espesor-cambio  PathReinformen ID:" + PathRein.Id);
                return;
            }

            if (aux_espesorCambio_ == "")
            {
                IsCorrect = true;
                return;
            }

            string[] aux_espesorCambioArray = aux_espesorCambio_.Split(',');

            foreach (string letraEspesor in aux_espesorCambioArray)
            {
                if ((letraEspesor.Contains("A_") || letraEspesor.Contains("B_") || letraEspesor.Contains("C_") ||
                    letraEspesor.Contains("D_") || letraEspesor.Contains("E_")) && letraEspesor.Length == 2)
                {
                    IsCorrect = true;
                    ListaEspesores.Add(letraEspesor);
                }
                else
                {
                    cantidadError += 1;
                    IsCorrect = false;
                    if (!isTest) Util.ErrorMsg("Error eb Obtener Espesor-cambio  PathReinformen ID:" + PathRein.Id);
                }



            }

        }

    }
}
