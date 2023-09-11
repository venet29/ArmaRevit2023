using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Extension
{
    public class ExtensionViewDTO {
        public bool Isok { get; set; }
        public double valorz { get; set; }

        public ExtensionViewDTO(bool Isok,double valorz)
        {
            this.Isok = Isok;
            this.valorz = valorz;
        }
        public ExtensionViewDTO()
        {

        }
    }
    public static class ExtensionView
    {

        public static string ObtenerNombreIsDependencia(this View _view)
        {
            string nombreActua = _view.Name;
            Parameter _paraDependency = _view.GetParameter2("Dependency");

            if (_paraDependency == null) return nombreActua;

            string IsDepend = _paraDependency.AsString();
            if (IsDepend == null) return nombreActua;
            if (IsDepend == "Primary" || IsDepend == "Independent" || (!IsDepend.Contains("Dependent on"))) return nombreActua;

            nombreActua = _paraDependency.AsString().Replace("Dependent on ", "");

            return nombreActua;
        }
        public static bool IsDependencia(this View _view)
        {
            string nombreActua = _view.Name;
            Parameter _paraDependency = _view.GetParameter2("Dependency");

            if (_paraDependency == null) return false;

            string IsDepend = _paraDependency.AsString();
            if (IsDepend == null) return false;
            if (IsDepend == "Primary" || IsDepend == "Independent" || (!IsDepend.Contains("Dependent on"))) return false;

            return true;

        }

        public static XYZ ViewDirection6(this View _view) => _view.ViewDirection.Redondear8();
        public static XYZ RightDirection6(this View _view) => _view.RightDirection.Redondear8();

        public static string ObtenerNombre_ViewNombre(this View _ViewMantenerBArras)
        {
            string NombreAntiguoVista = "";
            try
            {
                Parameter nombrePara = ParameterUtil.FindParaByName(_ViewMantenerBArras, "ViewNombre");
                if (nombrePara != null)
                {
                    NombreAntiguoVista = (nombrePara.AsString() == null ? "" : nombrePara.AsString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"'ObtenerNombreAntiguoVista' ex:{ex.Message}");

            }
            return NombreAntiguoVista;
        }


        public static string ObtenerNombreTipoView(this View _ViewMantenerBArras,string tipoParametro)
        {
            string NombreAntiguoVista = "";
            try
            {
                Parameter nombrePara = ParameterUtil.FindParaByName(_ViewMantenerBArras, tipoParametro);
                if (nombrePara != null)
                {
                    NombreAntiguoVista = (nombrePara.AsString() == null ? "" : nombrePara.AsString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"'ObtenerNombreAntiguoVista' ex:{ex.Message}");

            }
            return NombreAntiguoVista;
        }



        public static int ObtenerNombre_EscalaConfiguracion(this View _ViewMantenerBArras)
        {
            int NombreAntiguoVista = 50;
            try
            {
                Parameter nombrePara = ParameterUtil.FindParaByName(_ViewMantenerBArras, "EscalaConfiguracion");

                int scalaVIEW = _ViewMantenerBArras.Scale;
                if (nombrePara != null)
                {
                    string EscalaParametro = nombrePara.AsString();
                    // string auxNOmbre2 = nombrePara.AsValueString();

                    if (EscalaParametro == null && scalaVIEW != 100)
                        return 50;
                    else if (EscalaParametro == null && scalaVIEW == 100)
                        return 100;

                    EscalaParametro = EscalaParametro.Trim();
                    if (EscalaParametro == "100")
                        return 100;
                    else
                        return 50;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"'Obtener EscalaConfiguracion' ex:{ex.Message}");
            }
            return NombreAntiguoVista;
        }


        public static int ObtenerEscalaPAraPAthSimbol(this View _ViewMantenerBArras)
        {
            try
            {
                //return ObtenerNombre_EscalaConfiguracion(_ViewMantenerBArras);
                return _ViewMantenerBArras.Scale;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"'Obtener ObtenerEscalaPAthSimbol' ex:{ex.Message}");

            }
            return 50;
        }


        public static string ObtenerNombre_TipoEstructura(this View _ViewMantenerBArras)
        {
            string TipoEstructura = "";
            try
            {
                Parameter nombrePara = ParameterUtil.FindParaByName(_ViewMantenerBArras, "TIPO DE ESTRUCTURA (VISTA)");
                if (nombrePara != null)
                {
                    TipoEstructura = (nombrePara.AsString() == null ? "" : nombrePara.AsString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"'ObtenerNombreAntiguoVista' ex:{ex.Message}");

            }
            return TipoEstructura;
        }



        public static XYZ NH_ObtenerPtoSObreVIew(this View _view, XYZ ptoInicia)
        {
            XYZ resul = XYZ.Zero;
            try
            {
                resul = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano_conRedondeo8(_view.ViewDirection, _view.Origin, ptoInicia);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'obtenerPtoSObreVIew' ex: {ex.Message}");

            }
            return resul;
        }


        public static bool NH_IsEndireccionRightDirection(this View _view, XYZ ptoInicia)
        {
            double resul = 0;
            try
            {
                if (ptoInicia.X == 0 && ptoInicia.Y == 0) return true;

                resul = Util.GetProductoEscalar(_view.RightDirection, ptoInicia.GetXY0());
                if (Math.Abs(resul) > 0.9)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener 'obtenerPtoSObreVIew' ex: {ex.Message}");

            }
            return false;
        }

        public static void DesactivarViewTemplate_ConTrans(this View _view)
        {
            try
            {
                Document _doc = _view.Document;

                Parameter par = _view.GetParameters("View Template").FirstOrDefault();

                int valor = par.Id.IntegerValue;
                if (valor == -1) return;
                using (Transaction tr = new Transaction(_doc, "NoneView Template"))
                {
                    tr.Start();

                    par.Set(new ElementId(-1));
                    tr.Commit();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex.Message} ");
            }
        }

        public static void DesactivarViewTemplate_SinTrans(this View _view)
        {
            try
            {
                Document _doc = _view.Document;

                Parameter par = _view.GetParameters("View Template").FirstOrDefault();

                int valor = par.Id.IntegerValue;
                if (valor == -1) return;

                par.Set(new ElementId(-1));

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex.Message} ");
            }
        }
        public static void ActivarViewTemplate_SinTrans(this View _view, ElementId _idtemplate )
        {
            try
            {
                if (_idtemplate == null) return;

                Document _doc = _view.Document;
                Parameter par = _view.GetParameters("View Template").FirstOrDefault();

                int valor = par.Id.IntegerValue;
                if (par == null) return;

                par.Set(_idtemplate);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex.Message} ");
            }
        }

        public static ElementId ObtenerTemplate(this View _view)
        {
            try
            {
                return _view.ViewTemplateId;
                //el codigo innferior entrega un valor deid deistint0

                Document _doc = _view.Document;
                Parameter par = _view.GetParameters("View Template").FirstOrDefault();

                int valor = par.Id.IntegerValue;


                return par.Id;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex.Message} ");
            }
            return null;
        }

        public static ExtensionViewDTO Obtener_Z_SoloPLantas(this View _view, bool IsCOnMEnsaje=true)
        {
            try
            {
                if (_view == null)
                {
                    Util.ErrorMsg($" vista {_view.Name} es null");
                    return new ExtensionViewDTO() { Isok = false, valorz = 0 };
                }

                if (_view.GenLevel != null)
                {
                    return new ExtensionViewDTO() { Isok = true, valorz = _view.GenLevel.ProjectElevation };
                }
                else
                {
                    var ptoref = _view.Origin;

                     if (!Util.IsSimilarValor(_view.RightDirection.Z, 0, 0.00000001) && IsCOnMEnsaje)
                    {
                        Util.ErrorMsg($" vista {_view.Name} no esta completamente horizonta");
                    }
                    return new ExtensionViewDTO() { Isok = true, valorz = ptoref.Z };
                }

                
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex.Message} ");
            }

            Util.ErrorMsg($" vista {_view.Name} es null");
            return new ExtensionViewDTO() { Isok = false, valorz = 0 };
        }



        public static bool MOdificarCropRegion_ConTras(this View _view, bool estado)
        {
            try
            {
                Document _doc = _view.Document;
                using (Transaction tr = new Transaction(_doc, "Modificar Estado CropRegion"))
                {
                    tr.Start();
                    _view.CropBoxVisible = estado;
                    tr.Commit();
                }

            }
            catch (Exception)
            {
                Util.ErrorMsg($"Error al cambiar estado Crop Region viewId:{_view.Name}");
                return false;
            }

            return true;
        }

    }
}
