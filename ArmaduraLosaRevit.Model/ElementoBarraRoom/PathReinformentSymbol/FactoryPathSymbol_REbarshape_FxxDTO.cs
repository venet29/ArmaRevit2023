using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinformentSymbol
{
   public  class FactoryPathSymbol_REbarshape_FxxDTO
    {

        public static PathSymbol_REbarshape_FxxDTO Default()=> new PathSymbol_REbarshape_FxxDTO();

        public static PathSymbol_REbarshape_FxxDTO ConfigF1A(double DesIzqInf, double DesDereSup)
        {

            if (DesDereSup != 0 || DesIzqInf != 0)
                return new PathSymbol_REbarshape_FxxDTO() { DesDereSup_foot = DesDereSup, DesIzqInf_foot = DesIzqInf, IsOK = true };
            else
                return new PathSymbol_REbarshape_FxxDTO();
        }
        public static PathSymbol_REbarshape_FxxDTO ConfigF1B(double DesIzqInf, double DesDereSup)
        {

            if (DesDereSup != 0 || DesIzqInf != 0)
                return new PathSymbol_REbarshape_FxxDTO() { DesDereSup_foot = DesDereSup, DesIzqInf_foot = DesIzqInf, IsOK = true };
            else
                return new PathSymbol_REbarshape_FxxDTO();
        }
        public static PathSymbol_REbarshape_FxxDTO ConfigFf3()=> new PathSymbol_REbarshape_FxxDTO() { IsOK = true };

        public static PathSymbol_REbarshape_FxxDTO ConfigF4A(double DesIzqInf, double DesDereSup)
        {

            if (DesDereSup != 0 || DesIzqInf != 0)
                return new PathSymbol_REbarshape_FxxDTO() { DesDereSup_foot = DesDereSup, DesIzqInf_foot = DesIzqInf, IsOK = true };
            else
                return new PathSymbol_REbarshape_FxxDTO();
        }

        public static PathSymbol_REbarshape_FxxDTO ConfigF4B(double DesIzqInf, double DesDereSup)
        {

            if (DesDereSup != 0 || DesIzqInf != 0)
                return new PathSymbol_REbarshape_FxxDTO() { DesDereSup_foot = DesDereSup, DesIzqInf_foot = DesIzqInf, IsOK = true };
            else
                return new PathSymbol_REbarshape_FxxDTO();
        }

        public static PathSymbol_REbarshape_FxxDTO ConfigF11(double EspIzq_foot, double EspDere_foot)
        {
           return (EspIzq_foot != 0 || EspDere_foot != 0? new PathSymbol_REbarshape_FxxDTO() { EspIzq_foot = EspIzq_foot, EspDere_foot = EspDere_foot, IsOK = true }:            
                                                          new PathSymbol_REbarshape_FxxDTO());
        }

        public static PathSymbol_REbarshape_FxxDTO ConfigF12(double EspIzq_foot, double EspDere_foot)
        {
            if (EspIzq_foot != 0 || EspDere_foot != 0)
                return new PathSymbol_REbarshape_FxxDTO() { EspIzq_foot = EspIzq_foot, EspDere_foot = EspDere_foot, IsOK = true };
            else
                return new PathSymbol_REbarshape_FxxDTO();
        }

        public static PathSymbol_REbarshape_FxxDTO ConfigF16A(double DesIzqInf,double DesDereSup)
        {

            if (DesDereSup!=0 || DesIzqInf!=0)
              return  new PathSymbol_REbarshape_FxxDTO() { DesDereSup_foot = DesDereSup, DesIzqInf_foot = DesIzqInf, IsOK = true};
            else
                return new PathSymbol_REbarshape_FxxDTO();
        }
        public static PathSymbol_REbarshape_FxxDTO ConfigF16B(double DesDereInf, double DesIzqSup)
        {
            if (DesDereInf != 0 || DesIzqSup != 0)
                return  new PathSymbol_REbarshape_FxxDTO() { DesDereInf_foot = DesDereInf, DesIzqSup_foot = DesIzqSup, IsOK = true };
            else
                return new PathSymbol_REbarshape_FxxDTO();
        }
        //17
        public static PathSymbol_REbarshape_FxxDTO ConfigF17A( double DesIzqSup, double DesDereInf)
        {
            if ( DesIzqSup != 0 || DesDereInf != 0)
                return new PathSymbol_REbarshape_FxxDTO() { DesIzqSup_foot = DesIzqSup, DesDereInf_foot = DesDereInf, IsOK = true };
            else
                return new PathSymbol_REbarshape_FxxDTO();
        }

        public static PathSymbol_REbarshape_FxxDTO ConfigF17B(  double DesIzqInf, double DesDereSup_)

        {
            if ( DesDereSup_ != 0 || DesIzqInf != 0)
                return new PathSymbol_REbarshape_FxxDTO() { DesDereSup_foot = DesDereSup_, DesIzqInf_foot = DesIzqInf, IsOK = true };
            else
                return new PathSymbol_REbarshape_FxxDTO();
        }
        //18
        public static PathSymbol_REbarshape_FxxDTO ConfigF18(double DesDereSup, double DesIzqInf)
        {

            if (DesDereSup != 0 || DesIzqInf != 0)
                return new PathSymbol_REbarshape_FxxDTO() { DesDereSup_foot = DesDereSup, DesIzqInf_foot = DesIzqInf, IsOK = true };
            else
                return new PathSymbol_REbarshape_FxxDTO();
        }

        //19
        public static PathSymbol_REbarshape_FxxDTO ConfigF19(double DesDereSup, double DesIzqInf, double pataIzq)
        {

            if (DesDereSup != 0 || DesIzqInf != 0)
                return new PathSymbol_REbarshape_FxxDTO() { DesDereSup_foot = DesDereSup, DesIzqInf_foot = DesIzqInf,
                                                             pataIzq_foot = pataIzq, pataDere_foot= pataIzq, IsOK = true };
            else
                return new PathSymbol_REbarshape_FxxDTO();
        }


        //F20
        public static PathSymbol_REbarshape_FxxDTO ConfigF20A(double pataIzq, double DesDereSup, double DesDereInf)
        {
            if (pataIzq != 0 || DesDereSup != 0 || DesDereInf != 0)
                return new PathSymbol_REbarshape_FxxDTO() { pataIzq_foot = pataIzq, DesDereSup_foot = DesDereSup, DesDereInf_foot = DesDereInf, IsOK = true };
            else
                return new PathSymbol_REbarshape_FxxDTO();
        }

        public static PathSymbol_REbarshape_FxxDTO ConfigF20B(double pataDere_, double DesIzqSup_, double DesIzqInf)

        {
            if (pataDere_ != 0 || DesIzqSup_ != 0 || DesIzqInf != 0)
                return new PathSymbol_REbarshape_FxxDTO() { pataDere_foot = pataDere_, DesIzqSup_foot = DesIzqSup_, DesIzqInf_foot = DesIzqInf, IsOK = true };
            else
                return new PathSymbol_REbarshape_FxxDTO();
        }

        //F21
        public static PathSymbol_REbarshape_FxxDTO ConfigF21A(double pataIzq, double DesIzqSup, double DesDereInf)
        {
            if (pataIzq != 0 || DesIzqSup != 0 || DesDereInf != 0)
                return new PathSymbol_REbarshape_FxxDTO() { pataIzq_foot = pataIzq, DesIzqSup_foot = DesIzqSup, DesDereInf_foot = DesDereInf, IsOK = true };
            else
                return new PathSymbol_REbarshape_FxxDTO();
        }

        public static PathSymbol_REbarshape_FxxDTO ConfigF21B(double pataDere_, double DesIzqInf,double DesDereSup_)

        {
            if (pataDere_ != 0 || DesDereSup_ != 0 || DesIzqInf != 0)
                return new PathSymbol_REbarshape_FxxDTO() { pataDere_foot = pataDere_, DesIzqInf_foot = DesIzqInf, DesDereSup_foot = DesDereSup_, IsOK = true };
            else
                return new PathSymbol_REbarshape_FxxDTO();
        }

        // F22
        public static PathSymbol_REbarshape_FxxDTO ConfigF22A(double DesDereSup, double DesDereInf)
        {

            if (DesDereSup != 0 || DesDereInf != 0)
                return new PathSymbol_REbarshape_FxxDTO() { DesDereSup_foot = DesDereSup, DesDereInf_foot = DesDereInf, IsOK = true };
            else
                return new PathSymbol_REbarshape_FxxDTO();
        }
        public static PathSymbol_REbarshape_FxxDTO ConfigF22B(double DesIzqInf, double DesIzqSup)
        {
            if (DesIzqInf != 0 || DesIzqSup != 0)
                return new PathSymbol_REbarshape_FxxDTO() { DesIzqInf_foot = DesIzqInf, DesIzqSup_foot = DesIzqSup, IsOK = true };
            else
                return new PathSymbol_REbarshape_FxxDTO();
        }
    }
}
