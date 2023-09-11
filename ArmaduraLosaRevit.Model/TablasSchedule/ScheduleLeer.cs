using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.Cubicacion.Ayuda;
using ArmaduraLosaRevit.Model.Cubicacion.model;
using ArmaduraLosaRevit.Model.TablasSchedule.Dto;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.TablasSchedule
{
    public class ScheduleLeer
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        private readonly ViewSchedule _schedule;
        private List<LevelDTO> _lista_Level_Habilitados;
        private UIApplication uiapp;

        public List<DatosTablasDTO> listaPtos { get; set; }
        public List<ListaPlanosDTO> listaPlanosDTO { get; set; }
        public List<object[]> listaPtosObj { get; set; }

        public ScheduleLeer(UIApplication uiapp, ViewSchedule schedule, List<LevelDTO> _lista_Level_habilitados)
        {
            this._uiapp = uiapp;
            _doc = _uiapp.ActiveUIDocument.Document;
            this._schedule = schedule;
            this._lista_Level_Habilitados = _lista_Level_habilitados;
            listaPtos = new List<DatosTablasDTO>();
            listaPlanosDTO = new List<ListaPlanosDTO>();
            listaPtosObj = new List<object[]>();
        }

        public ScheduleLeer(UIApplication uiapp)
        {
            this.uiapp = uiapp;
            listaPtos = new List<DatosTablasDTO>();
            listaPlanosDTO = new List<ListaPlanosDTO>();

            listaPtosObj = new List<object[]>();
            _doc = uiapp.ActiveUIDocument.Document;
        }

        public bool ObtenerDatosTAblas()
        {

            try
            {
                //_schedule.Definition.ShowGrandTotal = false;
                //_schedule.Definition.ShowGrandTotalCount = false;
                //_schedule.Definition.IsItemized = false;

                TableData colTableData = _schedule.GetTableData();
                var sss = colTableData.GetSectionData(SectionType.Body);

                listaPtosObj.Add(new object[] { "posicion", "nivel", "area m2", "vol m3" });

#pragma warning disable CS0219 // The variable 'j' is assigned but its value is never used
                int i, j = 0;
#pragma warning restore CS0219 // The variable 'j' is assigned but its value is never used
                for (i = 0; i <= sss.NumberOfRows - 1; i++)
                {

                    var asd = sss.GetCellType(i, 1);
                    Debug.WriteLine($" Fila {i} :  {asd.ToString()}");
                    var nivel_ = _schedule.GetCellText(SectionType.Body, i, 0);

                    if (!(sss.GetCellType(i, 1).ToString().Contains("Parameter"))) continue;
                    if (nivel_ == "") continue;
                    if (nivel_ == null) continue;
                    if (nivel_.ToLower().Contains("total general")) continue;

                    nivel_ = nivel_.Replace("Up to level:", "").Trim();


                    string NivelCorregido = ObtenerNivelEstructura(nivel_);
                    DatosTablasDTO _newDatosTablasDTO = new DatosTablasDTO()
                    {
                        OrdeElevacion = (NombreDefinidoUsuario.ObtenerOrdenPorElevacion(nivel_, _lista_Level_Habilitados) ? NombreDefinidoUsuario.OrdeElevacion : 0f),
                        nivel = NivelCorregido,
                        area = Util.ConvertirStringInDouble_ParaCUb(sss.GetCellText(i, 1).Replace("m²", "")),
                        vol = Util.ConvertirStringInDouble_ParaCUb(sss.GetCellText(i, 2).Replace("m³", "")),
                    };



                    if (Util.IsSimilarValor(_newDatosTablasDTO.area, 0, 0.001) && Util.IsSimilarValor(_newDatosTablasDTO.vol, 0, 0.001)) continue;

                    listaPtos.Add(_newDatosTablasDTO);
                    listaPtosObj.Add(_newDatosTablasDTO.ObtenerDato_array());
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }
            return true;
        }


        public bool ObtenerListaPlanos()
        {

            try
            {
                TableData colTableData = _schedule.GetTableData();
                var sss = colTableData.GetSectionData(SectionType.Body);

#pragma warning disable CS0219 // The variable 'j' is assigned but its value is never used
                int i, j = 0;
#pragma warning restore CS0219 // The variable 'j' is assigned but its value is never used
                for (i = 0; i <= sss.NumberOfRows - 1; i++)
                {
                    var nivel_ = _schedule.GetCellText(SectionType.Body, i, 0);


                    if (!(sss.GetCellType(i, 1).ToString() == "Parameter")) continue;
                    if (nivel_ == "") continue;
                    if (nivel_ == "PLANO N°") continue;
                    if (nivel_ == null) continue;

                    string NivelCorregido = ObtenerNivelEstructura(nivel_);
                    ListaPlanosDTO _newDatosTablasDTO = new ListaPlanosDTO()
                    {
                        Plano = _schedule.GetCellText(SectionType.Body, i, 0).ToString(),
                        contenido = _schedule.GetCellText(SectionType.Body, i, 1).ToString(),
                        revision = _schedule.GetCellText(SectionType.Body, i, 2).ToString(),
                        descripcion = sss.GetCellText(i, 3).ToString(),
                        dibujo = sss.GetCellText(i, 4).ToString(),
                        reviso = sss.GetCellText(i, 5).ToString(),
                    };

                    listaPlanosDTO.Add(_newDatosTablasDTO);
                    listaPtosObj.Add(_newDatosTablasDTO.ObtenerDato_array());
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
            }
            return true;
        }

        private string ObtenerNivelEstructura(string nomb1)
        {


            if (NombreDefinidoUsuario.ObtenerNombreDefinidoUsuario(nomb1, _lista_Level_Habilitados))
                nomb1 = NombreDefinidoUsuario.nivelModificado;

            return nomb1;

        }

        #region Noo utilidos

        public void getScheduleData(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> collection = collector.OfClass(typeof(ViewSchedule)).ToElements();

            List<Element> ListaCollection = new List<Element>();
            ViewSchedule schedule_muro = TiposViewSchedule.ObtenerViewSchedule("CUBICACION DE MUROS POR MATERIAL", _doc);
            ListaCollection.Add(schedule_muro);

            string prompt = "ScheduleData :";
            prompt += Environment.NewLine;

            foreach (Element e in ListaCollection)
            {
                ViewSchedule viewSchedule = e as ViewSchedule;
                TableData table = viewSchedule.GetTableData();
                TableSectionData section = table.GetSectionData(SectionType.Body);
                int nRows = section.NumberOfRows;
                int nColumns = section.NumberOfColumns;

                if (nRows > 1)
                {
                    //valueData.Add(viewSchedule.Name);

                    List<List<string>> scheduleData = new List<List<string>>();
                    for (int i = 0; i < nRows; i++)
                    {
                        List<string> rowData = new List<string>();

                        for (int j = 0; j < nColumns; j++)
                        {
                            rowData.Add(viewSchedule.GetCellText(SectionType.Body, i, j));
                        }
                        scheduleData.Add(rowData);
                    }

                    List<string> columnData = scheduleData[0];
                    scheduleData.RemoveAt(0);

                    DataMapping(columnData, scheduleData);
                }
            }
        }

        public static void DataMapping(List<string> keyData, List<List<string>> valueData)
        {
            List<Dictionary<string, string>> items = new List<Dictionary<string, string>>();

            string prompt = "Key/Value";
            prompt += Environment.NewLine;

            foreach (List<string> list in valueData)
            {
                for (int key = 0, value = 0; key < keyData.Count && value < list.Count; key++, value++)
                {
                    Dictionary<string, string> newItem = new Dictionary<string, string>();

                    string k = keyData[key];
                    string v = list[value];
                    newItem.Add(k, v);
                    items.Add(newItem);
                }
            }

            foreach (Dictionary<string, string> item in items)
            {
                foreach (KeyValuePair<string, string> kvp in item)
                {
                    prompt += "Key: " + kvp.Key + ",Value: " + kvp.Value;
                    prompt += Environment.NewLine;
                }
            }

            Autodesk.Revit.UI.TaskDialog.Show("Revit", prompt);
        }
        #endregion
    }
}

