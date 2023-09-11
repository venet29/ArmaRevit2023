using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.VisualBasic;
using System;
using System.IO;

namespace ArmaduraLosaRevit.Model.CopiaLocal
{
    public class BuscadorRutasLocal
    {
        private UIApplication _uiapp;
       // const string rutabase = @"\\SERVER-ADMIN2\id\ArchivosCopiasRevit\";
        const string rutabase = @"\\SERVER-CDV\Dibujo2\Proyectos\SaveRevit\";
       
        public BuscadorRutasLocal(UIApplication application)
        {
            this._uiapp = application;
        }

        public bool BuscarRutaLocalDeProyectoEnlaNube()
        {
            try
            {
                //https://knowledge.autodesk.com/es/support/revit/troubleshooting/caas/sfdcarticles/sfdcarticles/ESP/Collaboration-for-Revit-Finding-local-copies-of-files.html
                Document _doc = _uiapp.ActiveUIDocument.Document;
                var Logiun = _uiapp.Application.LoginUserId;
                string RutaArchivo = _doc.PathName;

                if (_uiapp.ActiveUIDocument.Document.IsWorkshared)
                {
                    var GUIDnH = _uiapp.ActiveUIDocument.Document.WorksharingCentralGUID;
                    var _guidDoc = GUIDnH.ToString();
                    var version = _uiapp.Application.VersionNumber;
                    var nameArchivo = _uiapp.ActiveUIDocument.Document.Title;

                    ModelPath path2 = _doc.GetCloudModelPath();
                    Guid _guidDoc2 = path2.GetModelGUID();
                    Guid ProyectGuid = path2.GetProjectGUID();


                    string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    path = path + $@"\Autodesk\Revit\Autodesk Revit {version}\CollaborationCache\{Logiun}\{ProyectGuid}";



                    if (Directory.Exists(path))
                    {
                        System.Diagnostics.Process.Start(path);
                        Util.InfoMsg($"Su respaldo del proyecto {nameArchivo} es el archivo con nombre\n\n {_guidDoc}.rvt");
                    }
                    else
                        Util.InfoMsg($"Ruta del archivos respaldo del  proyecto {nameArchivo} no fue encontrado");
                    return true;
                }

                else
                {

                    string RutaContenedor = Path.GetDirectoryName(RutaArchivo);
                    string NombreCarpeta = Path.GetFileNameWithoutExtension(RutaArchivo);
                    System.Diagnostics.Process.Start(RutaContenedor);

                    Util.InfoMsg("Modelo no es colaborartivo");
                    return false;
                }


            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'Buscar ruta local de Proyecto en la nube' ex:{ex.Message}");
            }
            return false;
        }


        public bool COpiarArchivoREspaldo()
        {
            try
            {
                string NombreCarpeta = "";
                string RutaContenedor = "";

                string versionRevit = UtilVersionesRevit.ObtenerVersionRevit(_uiapp);
                //https://knowledge.autodesk.com/es/support/revit/troubleshooting/caas/sfdcarticles/sfdcarticles/ESP/Collaboration-for-Revit-Finding-local-copies-of-files.html
                Document _doc = _uiapp.ActiveUIDocument.Document;
                var Logiun = _uiapp.Application.LoginUserId;

                var version = _uiapp.Application.VersionNumber;
                var nameArchivo = _uiapp.ActiveUIDocument.Document.Title;
                string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                string input = Interaction.InputBox("Ingrese numero de proyecto:", "Copia", "", 300, 300);
                if (input.Trim().ToLower() == "")
                {
                    Util.ErrorMsg($"Numero de referencia no puede ser vacio.");
                    return false;
                }

                string RutaArchivo = _doc.PathName;

                if (_uiapp.ActiveUIDocument.Document.IsWorkshared)
                {
                    var listaNombre = RutaArchivo.Split('/');
                    var GUIDnH = _uiapp.ActiveUIDocument.Document.WorksharingCentralGUID;
                    var _guidDoc = GUIDnH.ToString();
                    ModelPath path2 = _doc.GetCloudModelPath();
                    Guid _guidDoc2 = path2.GetModelGUID();
                    Guid ProyectGuid = path2.GetProjectGUID();
                    NombreCarpeta = listaNombre[listaNombre.Length - 2];
                    RutaContenedor = path + $@"\Autodesk\Revit\Autodesk Revit {version}\CollaborationCache\{Logiun}\{ProyectGuid}";

                    string rutaCarpetacopia = $@"{rutabase}{versionRevit}\{NombreCarpeta}_{ DateTime.Now.ToString("yyyyMMddHHmmss")}_{input}";
                    Directory.CreateDirectory(rutaCarpetacopia);

                    if (Directory.Exists(path))
                    {
                        DirectoryCopy(RutaContenedor, rutaCarpetacopia, true);
                        System.Diagnostics.Process.Start(rutaCarpetacopia);
                        Util.InfoMsg($"Su respaldo del proyecto {NombreCarpeta} fue copiada correctamente.");
                    }
                    else
                        Util.InfoMsg($"Ruta del archivos respaldo del  proyecto {nameArchivo} no fue encontrado");
                }
                else
                {
                    RutaContenedor = Path.GetDirectoryName(RutaArchivo);
                    NombreCarpeta = Path.GetFileNameWithoutExtension(RutaArchivo);

                    string rutaCarpetacopia = $@"{rutabase}{versionRevit}\{input}_{NombreCarpeta}_{ DateTime.Now.ToString("yyyyMMddHHmmss")}";
                    Directory.CreateDirectory(rutaCarpetacopia);

                    if (Directory.Exists(rutaCarpetacopia) && Directory.Exists(RutaContenedor))
                    {
                        System.IO.File.Copy(RutaArchivo, rutaCarpetacopia + $@"\{NombreCarpeta}.rvt", true);
                        System.Diagnostics.Process.Start(rutaCarpetacopia);
                        Util.InfoMsg($"Su respaldo del proyecto {NombreCarpeta} fue copiada correctamente.");
                    }
                    else
                        Util.InfoMsg($"Ruta del archivos respaldo del  proyecto {rutaCarpetacopia} no se puedo crear");
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'Buscar ruta local de Proyecto en la nube' ex:{ex.Message}");
            }
            return false;
        }



        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}