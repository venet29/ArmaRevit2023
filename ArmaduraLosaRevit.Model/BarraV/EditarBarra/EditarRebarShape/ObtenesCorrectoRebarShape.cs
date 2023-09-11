using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ArmaduraLosaRevit.Model.BarraV.EditarBarra.EditarRebarShape
{
    public class ObtenesCorrectoRebarShape
    {

        public static RebarShape NombreRebarShape(RebarShape _newRebarShape, RebarShape _actualRebarShape, Document _doc)
        {
            RebarShape rebarshape = null;
            try
            {

                string nombre = "";
                if (_newRebarShape.Name != _actualRebarShape.Name) return _newRebarShape;

                if (_actualRebarShape.Name == "L")
                    nombre = "";
                else if (_actualRebarShape.Name == "LAI")
                    nombre = "LAS";
                else if (_actualRebarShape.Name == "LAS")
                    nombre = "LAI";
                else if (_actualRebarShape.Name == "LABI")
                    nombre = "LABS";
                else if (_actualRebarShape.Name == "LABS")
                    nombre = "LABI";
                else if (_actualRebarShape.Name == "LBI")
                    nombre = "LABS";
                else if (_actualRebarShape.Name == "LBS")
                    nombre = "LABI";

                rebarshape = TiposFormasRebarShape.getRebarShape(nombre, _doc);
            }
            catch (Exception)
            {
                rebarshape = null;
            }

            return rebarshape;

        }

    }
}
