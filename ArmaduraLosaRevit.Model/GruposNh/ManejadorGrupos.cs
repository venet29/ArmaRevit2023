using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.GruposNh.Model;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using Group = Autodesk.Revit.DB.Group;

namespace ArmaduraLosaRevit.Model.GruposNh
{


    public class ManejadorGrupos
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;

        public List<MOdelGroupNH> LIStaMOdelGroupNH { get; private set; }
        public List<MOdelGroupNH> LIStaDetailGroupNH { get; private set; }

        public ManejadorGrupos(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
        }


        public bool BuscarGrupos(bool isExportar = true)
        {
            try
            {
                LIStaMOdelGroupNH = BuscarGroup.ObtenerGroupModel(_doc);
                LIStaDetailGroupNH = BuscarGroup.ObtenerArrayDetailGroup(_doc);
                if(isExportar)
                    CreadoJsonGrupo.ExportarAJson(LIStaMOdelGroupNH, "Grupos_"+_doc.Title);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'BuscarGrupos'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool DesHaceGrupos()
        {
            try
            {

                using (Transaction t = new Transaction(_doc, "Desagrupar Grupo-NH"))
                {
                    t.Start();
                    for (int i = 0; i < LIStaMOdelGroupNH.Count; i++)
                    {
                        var lista = LIStaMOdelGroupNH[i].GroupNH.UngroupMembers();
                    }
                    t.Commit();
                }
                Util.InfoMsg("Proceso 'DesAgrupar' terminado");
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'DesHaceGrupos'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool DesHaceDetailGrupos(string contenGanNombre)
        {
            try
            {
                var listaResultFiltro = LIStaDetailGroupNH.Where(c => c.Nombre.Contains(contenGanNombre)).ToList();
                using (Transaction t = new Transaction(_doc, "Desagrupar Grupo-NH"))
                {
                    t.Start();
                    for (int i = 0; i < listaResultFiltro.Count; i++)
                    {
                        var lista = listaResultFiltro[i].GroupNH.UngroupMembers();
                    }
                    t.Commit();
                }
                Util.InfoMsg("Proceso 'DesAgrupar' terminado");
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'DesHaceGrupos'. ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public bool CrearGrupos(bool usarArchivo = false)
        {
            UtilitarioFallasDialogosCarga.Cargar(_uiapp);
            bool result = true;
            try
            {
                if(usarArchivo)
                    LIStaMOdelGroupNH = CreadoJsonGrupo.LeerArchivoJsonING(_doc);
               
                List<ElementId> elementIdsToGroup = new List<ElementId>();
                List<GroupType> LISTAbORRAR= new List<GroupType>();
                // Iniciar una transacción
                using (Transaction t = new Transaction(_doc, "Crear Grupo-NH"))
                {
                    t.Start();

                    for (int i = 0; i < LIStaMOdelGroupNH.Count; i++)
                    {
                        var item1 = LIStaMOdelGroupNH[i];
                        List<ElementId > ListaId = new List<ElementId>();
                        foreach (var item in item1.ListaGruopId)
                        {
                            var elemnt=_doc.GetElement(item);

                            if (elemnt == null) continue;
                            if (!elemnt.IsValidObject) continue;
                            ListaId.Add(item);
                        }
                        if (ListaId.Count == 0) continue;

                        Group newGroup = _doc.Create.NewGroup(ListaId); ;
                        LISTAbORRAR.Add(newGroup.GroupType);
                        var tipoAntiguo = newGroup.GroupType;
                        //newGroup.Name = item1.Nombre;
                        newGroup.GroupType= item1.GroupTipo;
                    }
                    // Crear un grupo con los elementos

                    _doc.Delete(LISTAbORRAR.Select(c=> c.Id).ToList());

                    t.Commit();
                }

                Util.InfoMsg("Proceso 'Agrupar' terminado");
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'CrearGrupos'. ex:{ex.Message}");
                result=true; 
            }
            UtilitarioFallasDialogosCarga.DesCargar(_uiapp);
            return result;
        }

    }

}
