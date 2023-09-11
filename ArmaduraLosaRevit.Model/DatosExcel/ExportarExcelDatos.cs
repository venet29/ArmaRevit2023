using ArmaduraLosaRevit.Model.Cubicacion;
using ArmaduraLosaRevit.Model.TablasSchedule;
using OfficeOpenXml;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.DatosExcel
{


    public class ExportarExcelDatos
    {
        System.Windows.Forms.SaveFileDialog saveFileDialog;
        //  private string rutaOF;
        private string rutaOF;
#pragma warning disable CS0414 // The field 'ExportarExcelDatos.rutaPedr' is assigned but its value is never used
        private string rutaPedr;
#pragma warning restore CS0414 // The field 'ExportarExcelDatos.rutaPedr' is assigned but its value is never used
        private string rutaArchivo;
        private string RutaArchivo;
        private readonly string nombreArchivo;
        private readonly ManejadorCubDTO manejadorCubDTO;

        public ExportarExcelDatos(ManejadorCubDTO manejadorCubDTO)
        {
            saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.Filter = "Execl files(*.xlsx) | *.xlsx";
            rutaOF = @"\\SERVER-CDV\cubicacion de proyectos\respaldo resumen\20xx-0xx-NOMBRE 22-03-2022 (Delporte)_RV.xlsx";
            rutaPedr = @"J:\_revit\CUBICACION\20xx-0xx-NOMBRE 22-03-2022 (Delporte)_RV.xlsx";
            rutaArchivo = rutaOF;
            // saveFileDialog.FileName = $"Cubicacion_{(DateTime.Now).ToString("MM_dd_yyyy Hmmss")}";
            this.nombreArchivo = manejadorCubDTO.NombreArchivo;
            this.manejadorCubDTO = manejadorCubDTO;
        }



        public bool M4_GuardareExcelHormigon(List<object[]> lista, ManejadorSchedule _ManejadorSchedule)
        {
            if (!M1_ObtenerDatos()) return false;
            M2_ExportarListaConHormnigonesV2(lista, RutaArchivo, _ManejadorSchedule);
            return true;
        }

        private bool M1_ObtenerDatos()
        {
            try
            {
#pragma warning disable CS0219 // The variable 'result' is assigned but its value is never used
                bool result = false;
#pragma warning restore CS0219 // The variable 'result' is assigned but its value is never used
                if (nombreArchivo != "")
                    saveFileDialog.FileName = nombreArchivo;
                else
                    saveFileDialog.FileName = $"Cubicacion_{(DateTime.Now).ToString("MM_dd_yyyy Hmmss")}";

                if (saveFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return false;

                RutaArchivo = saveFileDialog.FileName;

                if ((RutaArchivo != null) && (RutaArchivo != "")) return true;

                string name = Path.GetFileName(RutaArchivo);
                string direc = Path.GetDirectoryName(RutaArchivo);

                if (Path.GetExtension(RutaArchivo) != ".xlsx")
                {
                    System.Windows.Forms.MessageBox.Show("Ruta de archivo no corresponde o formato incorrecto", "Mensaje");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Util.DebugDescripcion(ex);
                return false;
            }
            return true;
        }

        private void M2_ExportarListaConHormnigonesV2(List<object[]> lista, string nombreArchivo, ManejadorSchedule _ManejadorSchedule)
        {
            try
            {
                FileInfo excelFile = new FileInfo(rutaArchivo);

                if (!excelFile.Exists) return;
                // Create the package and make sure you wrap it in a using statement
                using (ExcelPackage excel = new ExcelPackage(excelFile))
                {
                    ExcelWorksheet worksheet_Resumen = default;
                    // 1) RESUMEN
                    if (lista != null)
                    {
                        //  ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(nombreArchivo);

                        worksheet_Resumen = excel.Workbook.Worksheets.Where(c => c.Name == "Resumen").FirstOrDefault(); //Add("losa");

                        if (worksheet_Resumen == null)
                        {
                            Util.ErrorMsg("No se encontro Pestaña 'Resumen'");
                            return;
                        }
                    }
                    else
                        Util.ErrorMsg($"Tabla de lista de barras no encontrado");

                    // ActualizarTabladinamica(excel);

                    // 3) barras
                    if (lista != null)
                    {
                        //  ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add(nombreArchivo);

                        ExcelWorksheet worksheet_elev = excel.Workbook.Worksheets.Where(c => c.Name == "Datos").FirstOrDefault(); //Add("losa");
                        if (worksheet_elev == null)
                        {
                            Util.ErrorMsg("No se encontro Pestaña 'Datos'");
                            return;
                        }

                        worksheet_elev.Cells[14, 1].LoadFromArrays(lista);
                    }

                    // 2) listaPLanos
                    ExcelWorksheet worksheet_ListaPlanios = excel.Workbook.Worksheets.Where(c => c.Name == "ListaPlano").FirstOrDefault(); //Add("losa");

                    if (worksheet_ListaPlanios == null)
                    {
                        Util.ErrorMsg("No se encontro Pestaña 'ListaPlano'");
                        return;
                    }
                    else
                    {
                        if (_ManejadorSchedule._scheduleNH_ListaPlanos != null)
                            worksheet_ListaPlanios.Cells[10, 3].LoadFromArrays(_ManejadorSchedule._scheduleNH_ListaPlanos.listaPtosObj);
                        else
                            Util.ErrorMsg($"Tabla de Lista de planos no encontrado");
                    }


                    //4) superficies m2
                    ExcelWorksheet worksheet_SUP_m2 = excel.Workbook.Worksheets.Where(c => c.Name == "Datos MOL_m2").FirstOrDefault(); //Add("losa");
                    if (worksheet_SUP_m2 == null)
                    {
                        Util.ErrorMsg("No se encontro Pestaña 'Datos MOL_m2'");
                        return;
                    }

                    //
                    if (_ManejadorSchedule._scheduleNH_SUPERFICIE != null)
                    {
                        worksheet_SUP_m2.Cells[42, 15].LoadFromArrays(_ManejadorSchedule._scheduleNH_SUPERFICIE.listaPtosObj);
                        // pasa los los numero a valor numerico
                        for (int i = 0; i < _ManejadorSchedule._scheduleNH_SUPERFICIE.listaPtosObj.Count; i++)
                        {
                            if (worksheet_SUP_m2.Cells[42 + i, 16].Value == null) continue;
                            if (Util.IsNumeric(worksheet_SUP_m2.Cells[42 + i, 16].Value.ToString()))
                            {
                                worksheet_SUP_m2.Cells[42 + i, 16].Value = Util.ConvertirStringInInteger(worksheet_SUP_m2.Cells[42 + i, 16].Value.ToString());
                            }
                        }

                        //contarniverles
                        var canitdadNiveles = _ManejadorSchedule._scheduleNH_SUPERFICIE.listaPtosObj.Select(x => x[1]).Distinct().Count();
                        canitdadNiveles = canitdadNiveles + 2;
                        for (int i = 13 + canitdadNiveles; i < 33; i++)
                        {
                            worksheet_Resumen.Row(i).Hidden = true;
                        }
                    }
                    else
                        Util.ErrorMsg($"Tabla de superficies no encontrado");

                    //5) moldaje m3
                    ExcelWorksheet worksheet_HO_m3 = excel.Workbook.Worksheets.Where(c => c.Name == "Datos HO_m3").FirstOrDefault(); //Add("losa");
                    if (worksheet_HO_m3 == null)
                    {
                        Util.ErrorMsg("No se encontro Pestaña 'Datos HO_m3'");
                        return;
                    }

                    if (_ManejadorSchedule._TablasHormigoYModaje != null)
                    {
                        worksheet_HO_m3.Cells[42, 2].LoadFromArrays(_ManejadorSchedule._TablasHormigoYModaje.listaPtosObjTodoshORMIGON);

                        // pasa los los numero a valor numerico
                        for (int i = 0; i < _ManejadorSchedule._TablasHormigoYModaje.listaPtosObjTodoshORMIGON.Count; i++)
                        {
                            if (worksheet_HO_m3.Cells[43 + i, 4].Value == null) continue;
                            if (Util.IsNumeric(worksheet_HO_m3.Cells[43 + i, 4].Value.ToString()))
                            {
                                worksheet_HO_m3.Cells[43 + i, 4].Value = Util.ConvertirStringInInteger(worksheet_HO_m3.Cells[43 + i, 4].Value.ToString());
                            }
                        }
                    }
                    else
                        Util.ErrorMsg($"Tabla de TablasHormigo y Modaje no encontrado");


                    //6) moldaje m2
                    ExcelWorksheet worksheet_MOL_m2 = excel.Workbook.Worksheets.Where(c => c.Name == "Datos MOL_m2").FirstOrDefault();
                    if (worksheet_MOL_m2 == null)
                    {
                        Util.ErrorMsg("No se encontro Pestaña 'Datos MOL_m2'");
                        return;
                    }

                    if (_ManejadorSchedule._TablasHormigoYModaje != null)
                    {
                        worksheet_MOL_m2.Cells[42, 2].LoadFromArrays(_ManejadorSchedule._TablasHormigoYModaje.listaPtosObjTodosMOldajes);
                        worksheet_MOL_m2.Cells[12, 11].Value = manejadorCubDTO.nombreProyecto;
                        worksheet_MOL_m2.Cells[13, 11].Value = manejadorCubDTO.numeroObra;
                        worksheet_MOL_m2.Cells[14, 11].Value = (DateTime.Now).ToString("dd-MM-yyyy");
                        worksheet_MOL_m2.Cells[8, 11].Value = ":" + (DateTime.Now).ToString("dd-MM-yyyy");

                        // pasa los los numero a valor numerico
                        for (int i = 0; i < _ManejadorSchedule._TablasHormigoYModaje.listaPtosObjTodosMOldajes.Count; i++)
                        {
                            if (worksheet_MOL_m2.Cells[43 + i, 4].Value == null) continue;
                            if (Util.IsNumeric(worksheet_MOL_m2.Cells[43 + i, 4].Value.ToString()))
                            {
                                worksheet_MOL_m2.Cells[43 + i, 4].Value = Util.ConvertirStringInInteger(worksheet_MOL_m2.Cells[43 + i, 4].Value.ToString());
                            }
                        }
                    }

                    FileInfo excelFile2 = new FileInfo(nombreArchivo);
                    excel.SaveAs(excelFile2);
                }
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error al crear el excel. ex:{ex.Message}");
            }
        }

        private void ActualizarTabladinamica(ExcelPackage excel)
        {
            var worksheet_Resumen = excel.Workbook.Worksheets.Where(c => c.Name == "Resumen").FirstOrDefault(); //Add("losa");
            if (worksheet_Resumen == null)
            {
                Util.ErrorMsg("No se encontro Pestaña 'Resumen'");
                return;
            }
            worksheet_Resumen.Calculate();
            var tabla = worksheet_Resumen.PivotTables[""];
        }
    }

}

