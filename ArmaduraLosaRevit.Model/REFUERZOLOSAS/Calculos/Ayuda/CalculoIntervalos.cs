using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.Calculos.Ayuda
{
    public class CalculoIntervalos
    {
        private readonly int esp_borde_cm;
        private readonly int esp_cm;

        public int[] numbersSup { get; set; }
        public int[] numbersInf { get; set; }

        public CalculoIntervalos(int esp_borde_cm, int esp_cm)
        {
            this.esp_borde_cm = esp_borde_cm;
            this.esp_cm = esp_cm;
        }


        #region refuerzo de viga

        public void ObtenerIntervalos(int NumeroBArras)
        {
            if (NumeroBArras == 2)
            {
                numbersSup = new int[] { esp_borde_cm * 1 };
                numbersInf = new int[] { esp_borde_cm * 1 };
            }
            else if (NumeroBArras == 3)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1 };
                numbersInf = new int[] { esp_borde_cm * 1 };
            }
            else if (NumeroBArras == 4)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1 };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1 };
            }
            else if (NumeroBArras == 5)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2 };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1 };
            }
            else if (NumeroBArras == 6)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2 };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2 };
            }
            else if (NumeroBArras == 7)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3 };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2 };
            }
            else if (NumeroBArras == 8)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3 };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3 };
            }
            else if (NumeroBArras == 9)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4 };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3 };
            }
            else if (NumeroBArras == 10)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4 };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4 };
            }
            else if (NumeroBArras == 11)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5 };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4 , esp_borde_cm + esp_cm * 5 };
            }
            else if (NumeroBArras == 12)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6 };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6 };
            }
            else if (NumeroBArras == 13)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7 };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7 };
            }
            else if (NumeroBArras == 14)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8 };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8 };
            }
            else if (NumeroBArras == 15)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8, esp_borde_cm + esp_cm * 9 };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8, esp_borde_cm + esp_cm * 9};
            }
            else if (NumeroBArras == 16)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8, esp_borde_cm + esp_cm * 9, esp_borde_cm + esp_cm * 10};
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8, esp_borde_cm + esp_cm * 9, esp_borde_cm + esp_cm * 10 };
            }
            else
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1 };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1 };
            }

        }

        public void ObtenerIntervalosSup(int NumeroBArras)
        {
            if (NumeroBArras == 2)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1 };
                numbersInf = new int[] { };
            }
            else if (NumeroBArras == 3)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2 };
                numbersInf = new int[] { };
            }
            else if (NumeroBArras == 4)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3 };
                numbersInf = new int[] { };
            }
            else if (NumeroBArras == 5)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4 };
                numbersInf = new int[] { };
            }
            else if (NumeroBArras == 6)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5 };
                numbersInf = new int[] { };
            }
            else if (NumeroBArras == 7)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6 };
                numbersInf = new int[] { };
            }
            else if (NumeroBArras == 8)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7 };
                numbersInf = new int[] { };
            }
            else if (NumeroBArras == 9)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8 };
                numbersInf = new int[] { };
            }
            else if (NumeroBArras == 10)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8, esp_borde_cm + esp_cm * 9 };
                numbersInf = new int[] { };
            }
 
            else if (NumeroBArras == 11)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8, esp_borde_cm + esp_cm * 9, esp_borde_cm + esp_cm * 10 };
                numbersInf = new int[] { };
            }
            else if (NumeroBArras == 12)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8, esp_borde_cm + esp_cm * 9, esp_borde_cm + esp_cm * 10, esp_borde_cm + esp_cm * 11};
                numbersInf = new int[] { };
            }
            else if (NumeroBArras == 13)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8, esp_borde_cm + esp_cm * 9, esp_borde_cm + esp_cm * 10, esp_borde_cm + esp_cm * 11, esp_borde_cm + esp_cm * 12 };
                numbersInf = new int[] { };
            }
            else if (NumeroBArras == 14)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8, esp_borde_cm + esp_cm * 9, esp_borde_cm + esp_cm * 10, esp_borde_cm + esp_cm * 11, esp_borde_cm + esp_cm * 12, esp_borde_cm + esp_cm * 13};
                numbersInf = new int[] { };
            }
            else if (NumeroBArras == 15)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8, esp_borde_cm + esp_cm * 9, esp_borde_cm + esp_cm * 10, esp_borde_cm + esp_cm * 11, esp_borde_cm + esp_cm * 12, esp_borde_cm + esp_cm * 13, esp_borde_cm + esp_cm * 14};
                numbersInf = new int[] { };
            }
            else if (NumeroBArras == 16)
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8, esp_borde_cm + esp_cm * 9, esp_borde_cm + esp_cm * 10, esp_borde_cm + esp_cm * 11, esp_borde_cm + esp_cm * 12, esp_borde_cm + esp_cm * 13, esp_borde_cm + esp_cm * 14, esp_borde_cm + esp_cm * 15};
                numbersInf = new int[] { };
            }
            else
            {
                numbersSup = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3 };
                numbersInf = new int[] { };
            }
        }

        public void ObtenerIntervalosInf(int NumeroBArras)
        {
            if (NumeroBArras == 2)
            {
                numbersSup = new int[] { };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1 };
            }
            else if (NumeroBArras == 3)
            {
                numbersSup = new int[] { };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2 };
            }
            else if (NumeroBArras == 4)
            {
                numbersSup = new int[] { };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3 };
            }
            else if (NumeroBArras == 5)
            {
                numbersSup = new int[] { };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4 };
            }
            else if (NumeroBArras == 6)
            {
                numbersSup = new int[] { };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5 };
            }
            else if (NumeroBArras == 7)
            {
                numbersSup = new int[] { };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6 };
            }
            else if (NumeroBArras == 8)
            {
                numbersSup = new int[] { };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7 };
            }
            else if (NumeroBArras == 9)
            {
                numbersSup = new int[] { };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6 , esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8 };
            }
            else if (NumeroBArras ==10)
            {
                numbersSup = new int[] { };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8, esp_borde_cm + esp_cm * 9 };
            }
            else if (NumeroBArras == 11)
            {
                numbersSup = new int[] { };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8, esp_borde_cm + esp_cm * 9, esp_borde_cm + esp_cm *10 };
            }
            else if (NumeroBArras == 12)
            {
                numbersSup = new int[] { };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8, esp_borde_cm + esp_cm * 9, esp_borde_cm + esp_cm * 10, esp_borde_cm + esp_cm * 11 };
            }
            else if (NumeroBArras == 13)
            {
                numbersSup = new int[] { };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8, esp_borde_cm + esp_cm * 9, esp_borde_cm + esp_cm * 10, esp_borde_cm + esp_cm * 11, esp_borde_cm + esp_cm * 12 };
            }
            else if (NumeroBArras == 14)
            {
                numbersSup = new int[] { };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8, esp_borde_cm + esp_cm * 9, esp_borde_cm + esp_cm * 10, esp_borde_cm + esp_cm * 11, esp_borde_cm + esp_cm * 12, esp_borde_cm + esp_cm * 13 };
            }
            else if (NumeroBArras == 15)
            {
                numbersSup = new int[] { };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8, esp_borde_cm + esp_cm * 9, esp_borde_cm + esp_cm * 10, esp_borde_cm + esp_cm * 11, esp_borde_cm + esp_cm * 12, esp_borde_cm + esp_cm * 13, esp_borde_cm + esp_cm * 14 };
            }
            else if (NumeroBArras == 16)
            {
                numbersSup = new int[] { };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3, esp_borde_cm + esp_cm * 4, esp_borde_cm + esp_cm * 5, esp_borde_cm + esp_cm * 6, esp_borde_cm + esp_cm * 7, esp_borde_cm + esp_cm * 8, esp_borde_cm + esp_cm * 9, esp_borde_cm + esp_cm * 10, esp_borde_cm + esp_cm * 11, esp_borde_cm + esp_cm * 12, esp_borde_cm + esp_cm * 13, esp_borde_cm + esp_cm * 14, esp_borde_cm + esp_cm * 15 };
            }

            else
            {
                numbersSup = new int[] { };
                numbersInf = new int[] { esp_borde_cm * 1, esp_borde_cm + esp_cm * 1, esp_borde_cm + esp_cm * 2, esp_borde_cm + esp_cm * 3 };
            }

        }

        #endregion

        #region para refuerzo borde losa     
        public void ObtenerIntervalos_BordeLosa(int _numeroBArras)
        {
            if (_numeroBArras == 1)
            {
                numbersSup = new int[] { };
                numbersInf = new int[] { };
            }
            else if (_numeroBArras == 2)
            {
                numbersSup = new int[] { 0 };
                numbersInf = new int[] { 0 };
            }
            else if (_numeroBArras == 3)
            {
                numbersSup = new int[] { 0, 8 };
                numbersInf = new int[] { 0, 8 };
            }
            else if (_numeroBArras == 4)
            {
                numbersSup = new int[] { 0, 8, 16 };
                numbersInf = new int[] { 0, 8, 16 };
            }
            else if (_numeroBArras == 5)
            {
                numbersSup = new int[] { 0, 8, 16, 24 };
                numbersInf = new int[] { 0, 8, 16, 24 };
            }
            else if (_numeroBArras == 6)
            {
                numbersSup = new int[] { 0, 8, 16, 24, 32 };
                numbersInf = new int[] { 0, 8, 16, 24, 32 };
            }
            else if (_numeroBArras == 7)
            {
                numbersSup = new int[] { 0, 8, 16, 24, 32, 40 };
                numbersInf = new int[] { 0, 8, 16, 24, 32, 40 };
            }
            else
            {
                numbersSup = new int[] { 0 };
                numbersInf = new int[] { 0 };
            }


        }

        #endregion



    }
}
