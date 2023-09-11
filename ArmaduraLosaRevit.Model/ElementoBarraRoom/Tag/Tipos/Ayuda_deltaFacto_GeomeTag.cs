using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    class Ayuda_deltaFacto_GeomeTag
    {
        public static double deltaFActoSUP { get; private set; }
        public static double deltaFActoInf { get; private set; }

        public static bool calcular(View _view)
        {

            try
            {
                deltaFActoSUP = 0;
                deltaFActoInf = 0;

                if (_view.ObtenerNombre_EscalaConfiguracion() == 50)
                {
                    if (_view.Scale == 50)
                    {
                        deltaFActoSUP = 0;
                        deltaFActoInf = 0;
                    }
                    else if (_view.Scale == 75)
                    {
                        deltaFActoSUP = -Util.CmToFoot(0);
                        deltaFActoInf = Util.CmToFoot(0);
                    }
                    else if (_view.Scale == 100)
                    {
                        deltaFActoSUP = -Util.CmToFoot(0);
                        deltaFActoInf = Util.CmToFoot(17);
                    }

                }
                //else if (_view.ObtenerNombre_EscalaConfiguracion() == 75)
                //{
                //    if (_view.Scale == 50)
                //    {
                //        deltaFActoSUP = 0;
                //        deltaFActoInf = 0;
                //    }
                //    else if (_view.Scale == 75)
                //    {
                //        deltaFActoSUP = 0;
                //        deltaFActoInf = 0;
                //    }
                //    else if (_view.Scale == 100)
                //    {
                //        deltaFActoSUP = -Util.CmToFoot(11);
                //        deltaFActoInf = Util.CmToFoot(15);
                //    }

                //}
                else if (_view.ObtenerNombre_EscalaConfiguracion() == 100)
                {
                    if (_view.Scale == 50)
                    {
                        deltaFActoSUP = Util.CmToFoot(0);
                        deltaFActoInf = Util.CmToFoot(0);
                    }

                    else if (_view.Scale == 75)
                    {
                        deltaFActoSUP = -Util.CmToFoot(0);
                        deltaFActoInf = Util.CmToFoot(0);
                    }
                    else if (_view.Scale == 100)
                    {
                        deltaFActoSUP = -Util.CmToFoot(11);
                        deltaFActoInf = Util.CmToFoot(12);
                    }

                }

            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
    }
}
