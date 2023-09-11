using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraAreaPath.DTO
{
    public class DatosMallasAutoDTO
    {
        public double espesorFoot { get; set; } // en foot
        public int diametroH_mm { get; set; }
        public int diametroV_mm { get; set; }
        public double espaciemientoH_cm { get; set; } // en cm 
        public double espaciemientoV_cm { get; set; }  // en cm

        public TipoMAllaMuro tipoMallaV { get; set; }

        public TipoMAllaMuro tipoMallaH { get; set; }

        public TipoSeleccionMouse tipoSeleccionSup { get; set; }
        public TipoSeleccionMouse tipoSeleccionInf { get; set; }


        public string paraCantidadLineasV { get; internal set; }
        public string paraCantidadLineasH { get; internal set; }
        public CualMAllaDibujar CualMallaDibujar { get;  set; }
        public bool IsTipoViga { get;  set; }
        public bool IsBuscarCororonacion { get;  set; }
        public TipoPataMAlla Tipo_PataH { get;  set; }
        public TipoPataMAlla Tipo_PataV { get;  set; }
        public bool IsMallaH { get; internal set; } = true;
        public bool IsMallaV { get; internal set; } = true;

        public string ObtenerTExto()
        {
            if (IsTipoViga)
                  return ObtenerCuantiaViga();
            else
                 return ObtenerMalla();
        }

        private string ObtenerMalla()
        {
            var espesor_cm = Util.AproximarNumero(Util.FootToCm(espesorFoot),0.00001);

            if (diametroH_mm == 6 || diametroV_mm == 6)
            {


                switch (tipoMallaV)
                {
                    case TipoMAllaMuro.SM:
                        return $"M.H.A e={espesor_cm}\nMALLA CENTRAL\nACMA C188";
                    case TipoMAllaMuro.DM:
                        return $"M.H.A e={espesor_cm}\nD.M. ACMA C188";
                    case TipoMAllaMuro.TM:
                        return $"M.H.A e={espesor_cm}\nT.M. ACMA C188";
                    case TipoMAllaMuro.CM:
                        return $"M.H.A e={espesor_cm}\nC.M. ACMA C188";
                    default:
                        return $"M.H.A e={espesor_cm}\nMALLA ACMA C188";
                }
            }
            else if (diametroH_mm == diametroV_mm && espaciemientoH_cm == espaciemientoV_cm)
            {
         
                switch (tipoMallaV)
                {
                    case TipoMAllaMuro.SM:
                        return $"M.H.A e={espesor_cm}\nM. Ø{diametroH_mm}a{espaciemientoH_cm}";
                    case TipoMAllaMuro.DM:
                        return $"M.H.A e={espesor_cm}\nD.M. Ø{diametroH_mm}a{espaciemientoH_cm}";
                    case TipoMAllaMuro.TM:
                        return $"M.H.A e={espesor_cm}\nT.M. Ø{diametroH_mm}a{espaciemientoH_cm}";
                    case TipoMAllaMuro.CM:
                        return $"M.H.A e={espesor_cm}\nC.M. Ø{diametroH_mm}a{espaciemientoH_cm}";
                    default:
                        return $"M.H.A e={espesor_cm}\nD.M. Ø{diametroH_mm}a{espaciemientoH_cm}";
                }
            }
            else
            {
                switch (tipoMallaV)
                {
                    case TipoMAllaMuro.SM:
                        return $"M.H.A e={espesor_cm}\nM.H Ø{diametroH_mm}a{espaciemientoH_cm}\nM.V Ø{diametroV_mm}a{espaciemientoV_cm}";
                    case TipoMAllaMuro.DM:
                        return $"M.H.A e={espesor_cm}\nD.M.H Ø{diametroH_mm}a{espaciemientoH_cm}\nD.M.V Ø{diametroV_mm}a{espaciemientoV_cm}";
                    case TipoMAllaMuro.TM:
                        return $"M.H.A e={espesor_cm}\nT.M.H Ø{diametroH_mm}a{espaciemientoH_cm}\nD.M.V Ø{diametroV_mm}a{espaciemientoV_cm}";
                    case TipoMAllaMuro.CM:
                        return $"M.H.A e={espesor_cm}\nC.M.H Ø{diametroH_mm}a{espaciemientoH_cm}\nD.M.V Ø{diametroV_mm}a{espaciemientoV_cm}";
                    default:
                        return $"M.H.A e={espesor_cm}\nD.M.H Ø{diametroH_mm}a{espaciemientoH_cm}\nD.M.V Ø{diametroV_mm}a{espaciemientoV_cm}";
                }
               
            }
        }

        internal bool VerificarEspesor()
        {
            try
            {
                if (Util.CmToFoot(14) > espesorFoot)
                {
                    if (tipoMallaV == TipoMAllaMuro.SM || tipoMallaH == TipoMAllaMuro.SM || tipoMallaV == TipoMAllaMuro.DM || tipoMallaH == TipoMAllaMuro.DM)
                        return true;
                    else
                    {
                        Util.ErrorMsg($"NO se puede asignar {ObtenerMalla()} a muro de espesor {Util.FootToCm(espesorFoot)} cm");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private string ObtenerCuantiaViga()
        {
            if (diametroH_mm == diametroV_mm && espaciemientoH_cm == espaciemientoV_cm)
                return $"D.M.\n Ø{diametroH_mm}a{espaciemientoH_cm}";
            else
                return $"D.M.H Ø{diametroH_mm}a{espaciemientoH_cm}\nD.M.V Ø{diametroV_mm}a{espaciemientoV_cm}";
        }


        public string ObtenerNUmeroMallas_tipoMallaV() => tipoMalla_STRING(tipoMallaV);
        public string ObtenerNUmeroMallas_tipoMallaH() => tipoMalla_STRING(tipoMallaH);

        private string tipoMalla_STRING(TipoMAllaMuro tipoMalla)
        {
            switch (tipoMalla)
            {
                case TipoMAllaMuro.SM:
                    return "2";
                case TipoMAllaMuro.DM:
                    return "2+2";
                case TipoMAllaMuro.TM:
                    return "2+2+2";
                case TipoMAllaMuro.CM:
                    return "2+2+2+2";
                default:
                    return "2+2";
            }

        }

    }
}
