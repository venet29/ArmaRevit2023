using ArmaduraLosaRevit.Model.COMUNES;
using ArmaduraLosaRevit.Model.FAMILIA;
using ArmaduraLosaRevit.Model.FAMILIA.Varios;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using formNH= System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.DocumentosNh
{
    class ManejadorSaveArchivos
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        private Application m_app;

        public ManejadorSaveArchivos(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = uiapp.ActiveUIDocument.Document;
            this.m_app = _doc.Application;
           
        }

        public bool Ejecutar()
        {
  
            try
            {
                string rutaRaiz = InfoSystema.ObtenerRutaDeFamilias();
                var lista= FactoryCargarFamilias.CrearDiccionarioRutasFamilias_TODAS(rutaRaiz+"\\2019\\");
                var lista2023 = FactoryCargarFamilias.CrearDiccionarioRutasFamilias_TODAS(rutaRaiz + "\\2023\\");

                ManejadorCargarFAmilias manejadorCargarFAmilias = new ManejadorCargarFAmilias(_uiapp);
                //manejadorCargarFAmilias.DuplicarFamilasReBarBarv2();
                manejadorCargarFAmilias.cargarFamilias_run();


                // Check worksharing mode of each document
                // Open Revit projects
                formNH.OpenFileDialog theDialogRevit = new formNH.OpenFileDialog();
                theDialogRevit.Title = "Select Revit Project Files";
                theDialogRevit.Filter = "RVT files|*.rvt";
                theDialogRevit.FilterIndex = 1;
                theDialogRevit.InitialDirectory = @"D:\";
                theDialogRevit.Multiselect = true;
                if (theDialogRevit.ShowDialog() == formNH.DialogResult.OK)
                {
                    string mpath = "";
                    string mpathOnlyFilename = "";
                    formNH.FolderBrowserDialog folderBrowserDialog1 = new formNH.FolderBrowserDialog();
                    folderBrowserDialog1.Description = "Select Folder Where Revit Projects to be Saved in Local";
                    folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
                    if (folderBrowserDialog1.ShowDialog() == formNH.DialogResult.OK)
                    {
                        mpath = folderBrowserDialog1.SelectedPath;
                        foreach (string projectPath in theDialogRevit.FileNames)
                        {
                            FileInfo filePath = new FileInfo(projectPath);
                            ModelPath mp = ModelPathUtils.ConvertUserVisiblePathToModelPath(filePath.FullName);
                            OpenOptions opt = new OpenOptions();
                            opt.DetachFromCentralOption = DetachFromCentralOption.DetachAndDiscardWorksets;
                            mpathOnlyFilename = filePath.Name;
                            Document openedDoc = m_app.OpenDocumentFile(mp, opt);
                            SaveAsOptions options = new SaveAsOptions();
                            options.OverwriteExistingFile = true;
                            ModelPath modelPathout = ModelPathUtils.ConvertUserVisiblePathToModelPath(mpath + "\\" + mpathOnlyFilename);
                            openedDoc.SaveAs(modelPathout, options);
                            openedDoc.Close(false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"error: {ex.Message}");
                return false;
            }
            return true;
        }

        public bool EjecutarVArios()
        {

            try
            {
                string rutaRaiz = InfoSystema.ObtenerRutaDeFamilias();

                string rutaRaiz2019 = rutaRaiz + AgregarVErsion.Ejecutar("2019");
                string rutaRaizActual= rutaRaiz + AgregarVErsion.Ejecutar(_uiapp.Application.VersionNumber);
                //string rutaRaizActual = rutaRaiz + AgregarVErsion.Ejecutar("2020");
                var lista2019 = FactoryCargarFamilias.CrearDiccionarioRutasFamilias_TODAS(rutaRaiz2019);
                var listaactual = FactoryCargarFamilias.CrearDiccionarioRutasFamilias_TODAS(rutaRaizActual);

                foreach (var item in lista2019)
                {
                    string nombre= item.Item1;
                    string projectPath = item.Item2;

                    if (!File.Exists(projectPath))
                    {
                        Util.ErrorMsg($"No se encuentra familia:{nombre}  \n ruta:{projectPath}");
                        continue;
                    }
                    FileInfo filePath = new FileInfo(projectPath);
                    ModelPath mp = ModelPathUtils.ConvertUserVisiblePathToModelPath(filePath.FullName);
                    OpenOptions opt = new OpenOptions();
                    opt.DetachFromCentralOption = DetachFromCentralOption.DetachAndDiscardWorksets;
                   // mpathOnlyFilename = filePath.Name;
                    Document openedDoc = m_app.OpenDocumentFile(mp,opt);
                    SaveAsOptions options = new SaveAsOptions();
                    options.OverwriteExistingFile = true;

                    string rutaActua=listaactual.Where(c => c.Item1 == nombre).Select(c=> c.Item2).First();
                    if (rutaActua == null) continue;
                    FileInfo filePathActual = new FileInfo(rutaActua);
                    if (!Directory.Exists(filePathActual.DirectoryName))
                    {
                        Directory.CreateDirectory(filePathActual.DirectoryName);
                    }

                     ModelPath modelPathout = ModelPathUtils.ConvertUserVisiblePathToModelPath(filePathActual.FullName);
                    openedDoc.SaveAs(modelPathout, options);
                    openedDoc.Close(false);
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"error: {ex.Message}");
                return false;
            }
            return true;
        }

        // con la version inicial se generar un error en revit 2022
        public bool EjecutarVArios_op2()
        {
            string nombre = "";
            string projectPath = "";
            try
            {
                string rutaRaiz = InfoSystema.ObtenerRutaDeFamilias();

                string rutaRaiz2019 = rutaRaiz + AgregarVErsion.Ejecutar("2019");
                string rutaRaizActual = rutaRaiz + AgregarVErsion.Ejecutar(_uiapp.Application.VersionNumber);
                //string rutaRaizActual = rutaRaiz + AgregarVErsion.Ejecutar("2020");
                var lista2019 = FactoryCargarFamilias.CrearDiccionarioRutasFamilias_TODAS(rutaRaiz2019);
                var listaactual = FactoryCargarFamilias.CrearDiccionarioRutasFamilias_TODAS(rutaRaizActual);

                foreach (var item in lista2019)
                {
                     nombre = item.Item1;
                     projectPath = item.Item2;

                    if (!File.Exists(projectPath))
                    {
                        Util.ErrorMsg($"No se encuentra familia:{nombre}  \n ruta:{projectPath}");
                        continue;
                    }

                    //mpathOnlyFilename = filePath.Name;
                    Document openedDoc = m_app.OpenDocumentFile(projectPath);
                    SaveAsOptions options = new SaveAsOptions();
                    options.OverwriteExistingFile = true;

                    string rutaActua = listaactual.Where(c => c.Item1 == nombre).Select(c => c.Item2).First();
                    if (rutaActua == null) continue;
                    FileInfo filePathActual = new FileInfo(rutaActua);
                    if (!Directory.Exists(filePathActual.DirectoryName))
                    {
                        Directory.CreateDirectory(filePathActual.DirectoryName);
                    }

                    ModelPath modelPathout = ModelPathUtils.ConvertUserVisiblePathToModelPath(filePathActual.FullName);
                    openedDoc.SaveAs(modelPathout, options);
                    openedDoc.Close(false);
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"error {nombre}    path:{projectPath}     \n ex: {ex.Message}");
                return false;
            }

            Util.InfoMsg("Copias de familias finalizadas");
            return true;
        }
    }
}
