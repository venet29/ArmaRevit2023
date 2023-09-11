
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using ArmaduraLosaRevit.Model.ParametrosShare;
using System.Reflection;
using ArmaduraLosaRevit.Model.ParametrosShare.Buscar;
using ArmaduraLosaRevit.Model.UpdateGenerar;

namespace ArmaduraLosaRevit.Model.ParametrosShare
{

    public class DefinicionBorrarManejador
    {
        private UIApplication _uiapp;


        public Dictionary<string, ElementId> listaParameter;
        private List<ElementId> listaIdExistentes;

        public DefinicionBorrarManejador(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            listaIdExistentes = new List<ElementId>();
            listaParameter = new Dictionary<string, ElementId>();

        }

        public void EjecutarBorrar()
        {
            List<EntidadDefinition> lista = new List<EntidadDefinition>();

            lista.Add(FactoryEntidadDefinition22Up.AsignarNuevoParametroALista("AEC Código Interno"));
            lista.Add(FactoryEntidadDefinition22Up.AsignarNuevoParametroALista("AEC Código Interno"));
            //lista.Add(FactoryEntidadDefinition.AsignarNuevoParametroALista("AEC Rebar Uso"));



            lista.AddRange(FactoryEntidadDefinition22Up.CrearListaConParametrosElevaciones(_uiapp));
            lista.AddRange(FactoryEntidadDefinition22Up.CrearListaConParametrosLosa(_uiapp));
            lista.AddRange(FactoryEntidadDefinition22Up.CrearListaConParametrosView(_uiapp));
            //lista.AddRange(FactoryEntidadDefinition.CrearListaConParametrosFundaciones(_uiapp));
            // lista.AddRange(FactoryEntidadDefinition.CrearListaConParametrosEscalera(_uiapp));

            //UpdateGeneral _updateGeneral = new UpdateGeneral(_uiapp);
            UpdateGeneral.M5_DesCargarGenerar(_uiapp);

            string nameParametro = "";
            try
            {
                Document doc = _uiapp.ActiveUIDocument.Document;
                if (doc == null) return;


                    listaParameter = AyudaBuscaParametrerShared.ObtenerListaPArameterShare(doc);

                    foreach (var item in lista)
                    {
                        nameParametro = item.nombreParametro;
                        M1_ShareParameterExists(doc, item.nombreParametro);
                    }

                    if (listaIdExistentes.Count == 0 )
                    {
                        Util.InfoMsg("No se encontraron parametros para borrar");
                        return;
                    }
            

                    using (Transaction tran = new Transaction(_uiapp.ActiveUIDocument.Document, "Add shared param"))
                    {
                        tran.Start();
                        doc.Delete(listaIdExistentes);
                        tran.Commit();
                    }
                
                Util.InfoMsg("Datos parameros compartidos borrados");
            }
            catch (Exception ex)
            {
                Util.ErrorMsg("Parametro:  " + ex.Message);
            }

            UpdateGeneral.M4_CargarGenerar(_uiapp);
        }


        private void M1_ShareParameterExists(Document doc, string paramName)
        {
            BindingMap bindingMap = doc.ParameterBindings;
            DefinitionBindingMapIterator iter = bindingMap.ForwardIterator();
            iter.Reset();

            while (iter.MoveNext())
            {
                Definition tempDefinition = iter.Key;
                // find the definition of which the name is the appointed one
                if (String.Compare(tempDefinition.Name, paramName) != 0)
                {
                    continue;
                }
                InternalDefinition intDef = (InternalDefinition)iter.Key;
                listaIdExistentes.Add(intDef.Id);
                return;
            }
        }




    }
}
