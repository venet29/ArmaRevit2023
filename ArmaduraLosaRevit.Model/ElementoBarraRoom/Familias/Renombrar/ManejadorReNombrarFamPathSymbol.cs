using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias.Renombrar
{

    public class ManejadorReNombrarDTO
    {


        public ManejadorReNombrarDTO(string prefijoAntiguo_, string prefijoNuevo_)
        {
            this.prefijoAntiguo = prefijoAntiguo_;
            this.prefijoNuevo = prefijoNuevo_;
        }

        public string prefijoAntiguo { get; set; }
        public string prefijoNuevo { get; set; }
    }

    public class ManejadorReNombrarFamPathSymbol
    {
        private readonly UIApplication _uiapp;
        private Document _doc;

        List<ManejadorReNombrarDTO> listaNombre;
        public ManejadorReNombrarFamPathSymbol(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            listaNombre = new List<ManejadorReNombrarDTO>();
        }


        public bool IsFamiliasAntiguas()
        {
            try
            {
               // return false;
                string nombreFamiliaCOnPata = "M_Path Reinforcement Symbol_F20A_Dere_Tras_50";
                var elemtoSymboloPath = TiposPathReinSpanSymbol.M1_GetFamilySymbol_nh(nombreFamiliaCOnPata, _doc);

                if (elemtoSymboloPath != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }

        }

        public bool Renombarra()
        {

            try
            {
                var listaPathSYmbol = TiposPathReinSpanSymbol.M1_2_BuscarListaEnColecctor(_doc, BuiltInCategory.OST_PathReinSpanSymbol);

                if (!cargarDiccionario()) return false;
                using (Transaction transGroup = new Transaction(_doc))
                {
                    transGroup.Start("RenomFam-NH");


                    for (int i = 0; i < listaNombre.Count; i++)
                    {
                        var casos = listaNombre[i];

                        var listaCO = listaPathSYmbol.Where(fam => fam.Name.Contains(casos.prefijoAntiguo)).ToList();

                        for (int j = 0; j < listaCO.Count; j++)
                        {
                            var famiPAthsyb = listaCO[j];
                            famiPAthsyb.Name = famiPAthsyb.Name.Replace(casos.prefijoAntiguo, casos.prefijoNuevo);
                        }



                    }

                    transGroup.Commit();
                }

            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }

        private bool cargarDiccionario()
        {
            try
            {
                listaNombre.Add(new ManejadorReNombrarDTO("Symbol_F20A_Dere_Tras_50", "Symbol_F20ADereTras"));
                listaNombre.Add(new ManejadorReNombrarDTO("Symbol_F16_Dere", "Symbol_F16Dere"));
                listaNombre.Add(new ManejadorReNombrarDTO("Symbol_F16_Izq", "Symbol_F16Izq"));
                listaNombre.Add(new ManejadorReNombrarDTO("Symbol_F17A_Tras", "Symbol_F17ATras"));
                listaNombre.Add(new ManejadorReNombrarDTO("Symbol_F17B_Tras", "Symbol_F17BTras"));
                listaNombre.Add(new ManejadorReNombrarDTO("Symbol_F19_Dere", "Symbol_F19Dere"));
                listaNombre.Add(new ManejadorReNombrarDTO("Symbol_F19_Izq", "Symbol_F19Izq"));

                listaNombre.Add(new ManejadorReNombrarDTO("Symbol_F20A_Dere_Tras", "Symbol_F20ADereTras"));
                listaNombre.Add(new ManejadorReNombrarDTO("Symbol_F20A_Izq_Tras", "Symbol_F20AIzqTras"));
                listaNombre.Add(new ManejadorReNombrarDTO("Symbol_F20B_Dere_Tras", "Symbol_F20BDereTras"));
                listaNombre.Add(new ManejadorReNombrarDTO("Symbol_F20B_Izq_Tras", "Symbol_F20BIzqTras"));



                listaNombre.Add(new ManejadorReNombrarDTO("Symbol_F21A_Dere_Tras", "Symbol_F21ADereTras"));
                listaNombre.Add(new ManejadorReNombrarDTO("Symbol_F21A_Izq_Tras", "Symbol_F21AIzqTras"));
                listaNombre.Add(new ManejadorReNombrarDTO("Symbol_F21B_Dere_Tras", "Symbol_F21BDereTras"));
                listaNombre.Add(new ManejadorReNombrarDTO("Symbol_F21B_Izq_Tras", "Symbol_F21BIzqTras"));

                listaNombre.Add(new ManejadorReNombrarDTO("Symbol_F22_Dere", "Symbol_F22Dere"));
                listaNombre.Add(new ManejadorReNombrarDTO("Symbol_F22_Izq", "Symbol_F22Izq"));

            }
            catch (Exception)
            {

                return false;
            }
            return true; ;
        }
    }
}
