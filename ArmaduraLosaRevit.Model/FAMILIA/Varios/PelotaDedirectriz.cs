using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.FAMILIA.Varios
{
    public class PelotaDedirectriz
    {
        private readonly Document _doc;
        private readonly List<string> listaFAmilySymbol;
        private UIApplication uidoc;

        public PelotaDedirectriz(UIApplication uidoc)
        {
            this._doc = uidoc.ActiveUIDocument.Document;


            listaFAmilySymbol = new List<string>();

            listaFAmilySymbol.Add("MRA Rebar_F_50");
            listaFAmilySymbol.Add("MRA Rebar_FH_50");
            listaFAmilySymbol.Add("MRA Rebar_L_50");
            listaFAmilySymbol.Add("MRA Rebar_C_50");
            listaFAmilySymbol.Add("MRA Rebar_F_SIN_50");
            listaFAmilySymbol.Add("MRA Rebar_SIN_50");

            listaFAmilySymbol.Add("MRA Rebar_F_75");
            listaFAmilySymbol.Add("MRA Rebar_FH_75");
            listaFAmilySymbol.Add("MRA Rebar_L_75");
            listaFAmilySymbol.Add("MRA Rebar_C_75");
            listaFAmilySymbol.Add("MRA Rebar_F_SIN_75");
            listaFAmilySymbol.Add("MRA Rebar_SIN_75");

            listaFAmilySymbol.Add("MRA Rebar_F_100");
            listaFAmilySymbol.Add("MRA Rebar_FH_100");
            listaFAmilySymbol.Add("MRA Rebar_L_100");
            listaFAmilySymbol.Add("MRA Rebar_C_100");
            listaFAmilySymbol.Add("MRA Rebar_F_SIN_100");
            listaFAmilySymbol.Add("MRA Rebar_SIN_100");
            this.uidoc = uidoc;
        }

        public bool Ejecutar()
        {
            var pelotaLlena = Tipos_Arrow.FindAllArrowheads(_doc, "Filled Dot 2mm");

            if (pelotaLlena == null)
            {
                Util.ErrorMsg("No se encontro circulo de directriz");
                return false;
            }
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CreateNuevoRefuerzo-NH");
                    foreach (string item in listaFAmilySymbol) // item="MRA Rebar_F_SIN_50"
                    {
                        var familiaSymbol = TiposRebarTag.M1_GetRebarTag(item, _doc);
                        if (familiaSymbol == null) continue;

                        var patametroDeDona=familiaSymbol.get_Parameter(BuiltInParameter.LEADER_ARROWHEAD);
                        if (patametroDeDona == null) continue;
                        if (patametroDeDona.AsValueString() == "Filled Dot 2mm") continue;

                        ParameterUtil.SetParaElementId(familiaSymbol, BuiltInParameter.LEADER_ARROWHEAD, pelotaLlena[0].Id);//"Filled Dot 2mm");//
                    }

                    t.Commit();
                }

            }
            catch (Exception ex)
            {
                string msj = ex.Message;
                return false;
            }

            return true;

        }

    }
}
