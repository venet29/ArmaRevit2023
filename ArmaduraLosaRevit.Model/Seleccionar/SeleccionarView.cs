using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Seleccionar
{
    public class SeleccionarView
    {



        public static View ObtenerViewPOrNombre(UIDocument uidoc, string nombre)
        {

            //buscar primer nivel
            FilteredElementCollector Colectornivel = new FilteredElementCollector(uidoc.Document);
            View Lv = Colectornivel
                     .OfClass(typeof(View))
                     .OfCategory(BuiltInCategory.OST_Views)
                     .Cast<View>()
                     .Where(X => X.Name == nombre).FirstOrDefault();

            return Lv;
        }


        public static List<View> ObtenerView_losa_elev_fund(UIDocument uidoc)
        {
            List<View> ListLv = new List<View>();
            try
            {


                //buscar primer nivel
                FilteredElementCollector Colectornivel = new FilteredElementCollector(uidoc.Document);
                ListLv = Colectornivel
                          .OfClass(typeof(View))
                          .OfCategory(BuiltInCategory.OST_Views)
                          .Cast<View>()
                          .Where(X => X.IsTemplate==false && ( X.ViewType == ViewType.FloorPlan || X.ViewType == ViewType.Section)).ToList();


            }
            catch (Exception)
            {
                ListLv.Clear();
                Util.ErrorMsg("Erro al seleccionar elevaciones");
            }
            return ListLv;
        }

        public static List<View> ObtenerView_losa_elev_fund_estructura(UIDocument uidoc, bool IStamplate=false)
        {
            List<View> ListLv = new List<View>();
            try
            {
                //buscar primer nivel
                ListLv.Clear();
                FilteredElementCollector Colectornivel = new FilteredElementCollector(uidoc.Document);
                ListLv = Colectornivel
                          .OfClass(typeof(View))
                          .OfCategory(BuiltInCategory.OST_Views)
                          .Cast<View>()
                          .Where(X => X.IsTemplate == IStamplate && (X.ViewType == ViewType.FloorPlan || X.ViewType == ViewType.Section ||
                                                                    X.ViewType == ViewType.CeilingPlan || X.ViewType== ViewType.EngineeringPlan)).ToList(); //EngineeringPlan :para estructura de fundacione
            }
            catch (Exception)
            {
                ListLv.Clear();
                Util.ErrorMsg("Erro al seleccionar elevaciones");
            }
            return ListLv;
        }

        public static List<View> ObtenerView_losa_elev_fund_estructura_sheet(UIDocument uidoc)
        {
            List<View> ListLv = new List<View>();
            try
            {


                //buscar primer nivel
                FilteredElementCollector Colectornivel = new FilteredElementCollector(uidoc.Document);
                ListLv = Colectornivel
                          .OfClass(typeof(View))
                          .OfCategory(BuiltInCategory.OST_Views)
                          .Cast<View>()
                          .Where(X => X.IsTemplate == false && (X.ViewType == ViewType.FloorPlan || X.ViewType == ViewType.Section || 
                                       X.ViewType == ViewType.CeilingPlan )).ToList();


            }
            catch (Exception)
            {
                ListLv.Clear();
                Util.ErrorMsg("Erro al seleccionar elevaciones");
            }
            return ListLv;
        }

        public static List<View> ObtenerView_Todos(UIDocument uidoc)
        {
            List<View> ListLv = new List<View>();
            try
            {


                //buscar primer nivel
                FilteredElementCollector Colectornivel = new FilteredElementCollector(uidoc.Document);
                ListLv = Colectornivel
                          .OfClass(typeof(View))
                          .OfCategory(BuiltInCategory.OST_Views)
                          .Cast<View>()
                          .ToList();


            }
            catch (Exception)
            {
                ListLv.Clear();
                Util.ErrorMsg("Erro al seleccionar elevaciones");
            }
            return ListLv;
        }

        internal static List<ViewSheet> Getsheet(UIDocument uidoc)
        {
            List<ViewSheet> ListLv = new List<ViewSheet>();
            try
            {


                //buscar primer nivel
                FilteredElementCollector Colectornivel = new FilteredElementCollector(uidoc.Document);
                ListLv = Colectornivel
                          .OfClass(typeof(ViewSheet))
                          .OfCategory(BuiltInCategory.OST_Sheets)
                          .Cast<ViewSheet>()
                          .Where(X => X.IsTemplate == false && (X.ViewType == ViewType.DrawingSheet)).ToList();


            }
            catch (Exception)
            {
                ListLv.Clear();
                Util.ErrorMsg("Erro al seleccionar elevaciones");
            }
            return ListLv;
        }

        public List<ViewSection> ObtenerTodosViewSection(Document document)
        {

            List<ViewSection> ListaResul = new List<ViewSection>();
            FilteredElementCollector collector = new FilteredElementCollector(document).OfClass(typeof(ViewSection));

            foreach (ViewSection current in collector)
            {
                ViewFamilyType vft = document.GetElement(current.GetTypeId()) as ViewFamilyType;

                //dejo la opcion vacio  para agregar un filltro por nombre
                if (null != vft && vft.Name != "vacio" && vft.FamilyName.ToLower() == "section")
                {
                    ListaResul.Add(current);
                }
            }
            return ListaResul;

        }
    }



}
