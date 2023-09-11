using ArmaduraLosaRevit.Model.BarraV.Buscar;
using ArmaduraLosaRevit.Model.ConfiguracionInicial.model;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.ParametrosShare;
using ArmaduraLosaRevit.Model.ParametrosShare.Buscar;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using ArmaduraLosaRevit.Model.Visibilidad;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using MySqlX.XDevAPI;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.ConfiguracionInicial.Elemento
{
    public class AnalisisMuros
    {
        private UIApplication _uiapp;
        private Document _doc;
        private View _view;
        private List<Element> ListaWall_ConCOronacion;
        private List<Element> ListaWall_ANtesCOronacion; // wall que sobre ellos tiene un muro con coronacion
        private List<Element> ListaWall_enView;
        private View3D _view3D;
        List<EnvoltorioMuro> listEnvoltorioMuro;
        private List<dynamic> listaDeMurosConCoroncacionSobre;
        private static bool IsEncriptar = true;
        private List<Element> ListadeMurosIntermedios;

        public AnalisisMuros(UIApplication uiapp)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _doc.ActiveView;
            ListaWall_ConCOronacion = new List<Element>();
            listEnvoltorioMuro = new List<EnvoltorioMuro>();
            ListadeMurosIntermedios = new List<Element>();

#if DEBUG
            IsEncriptar = false;
# endif
        }



        public bool Inicio()
        {
            try
            {
                ElementId result = AyudaBuscaParametrerShared.ObtenerParameterShare(_doc, CONST_PARAMETER.CONT_INFORVT);
                if (result == null)
                {
                    ManejadorCrearParametrosShare _ManejadorCrearParametrosShare = new ManejadorCrearParametrosShare(_uiapp, RutaArchivoCompartido: "ParametrosNH");
                    _ManejadorCrearParametrosShare.EjecutarElementos();
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'AnalisisMurosInicio'. ex:{ex.Message}");
                return false;
            }
            return true;
        }


        public bool BuscarMuros()
        {
            try
            {
                _view3D = TiposFamilia3D.Get3DBuscar(_doc);
                if (_view3D == null)
                {
                    Util.ErrorMsg("Error, favor cargar configuracion inicial: 3D Buscar");
                    return false;
                }

                SeleccionarWalls.SeleccionarTodasElementos(_doc, _view);
                if (SeleccionarWalls.listAWall.Count > 0)
                    ListaWall_enView = SeleccionarWalls.listAWall;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'BuscarMurosMuro'. ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public bool AsignarCoronacion()
        {
            try
            {
                // buscar wall coronacion
                if (!M1_BUscarMurosCoronocacion()) return false;

                // buscar elenetos antes coronocion
                if (!M2_AnalizarMurosBajoCoronacion()) return false;

                ///
                M3_CopiarDatosInternos();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'AsignarCoronacionAsignarCoronacion'. ex:{ex.Message}");
                return false;
            }
            return true;
        }



        public bool M1_BUscarMurosCoronocacion()
        {
            try
            {
                for (int i = 0; i < ListaWall_enView.Count; i++)
                {
                    var _wall = ListaWall_enView[i];

                    var planarFaceSUperio = _wall.ObtenerCaraSuperior();

                    XYZ ptoBUsqueda = planarFaceSUperio.ObtenerCenterDeCara() - XYZ.BasisZ * Util.CmToFoot(2);

                    var _buscarElementosArriba = new BuscarElementosArriba(_uiapp, Util.CmToFoot(60), _view3D);

                    if (_buscarElementosArriba.BuscarParaCoronacion(ptoBUsqueda, new XYZ(0, 0, 1)))
                        ListaWall_ConCOronacion.Add(_wall);
                    else
                    {
                        EnvoltorioMuro _newEnvoltorioMuro = new EnvoltorioMuro() { Muros = _wall, Busqueda = _buscarElementosArriba };
                        listEnvoltorioMuro.Add(_newEnvoltorioMuro);
                    }

                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M1_BUscarMurosCoronocacion'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool M2_AnalizarMurosBajoCoronacion()
        {
            try
            {

                var array = ListaWall_ConCOronacion.Select(c => c.Id).ToArray();

                listaDeMurosConCoroncacionSobre = new List<dynamic>();

                for (int i = 0; i < listEnvoltorioMuro.Count; i++)
                {
                    var _muroActual = listEnvoltorioMuro[i];
                    var _ListwallSobre = _muroActual.Busqueda.ListaObjEncontrados.Where(c => c.nombreTipo == TipoElementoBArraV.muro).ToArray();
                    bool IsEncontrado = false;
                    for (int j = 0; j < _ListwallSobre.Length; j++)
                    {
                        var wallSObre = _ListwallSobre[j];
                        var muroCoronacion = ListaWall_ConCOronacion.Where(c => c.Id == wallSObre.elemt.Id).FirstOrDefault();
                        if (muroCoronacion != null)
                        {
                            XYZ centroCAraZuperior = muroCoronacion.ObtenerCaraSuperior().ObtenerCenterDeCara();
                            var anoNH = new { wallnh = _muroActual.Muros, wallCoronacion = muroCoronacion.Id, Zcoronacion = centroCAraZuperior.Z };
                            //  anoNH.wallCoronacion;
                            listaDeMurosConCoroncacionSobre.Add(anoNH);
                            IsEncontrado = true;
                        }
                    }

                    if (!IsEncontrado)
                        ListadeMurosIntermedios.Add(_muroActual.Muros);
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'AnalizarMurosBajoCoronacion'. ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public bool M3_CopiarDatosInternos()
        {
            try
            {
                var clave = EmbeberNH.getKey(ConstNH.CONST_EMBEBER);
                using (Transaction tr = new Transaction(_doc, "Regenerate_nh"))
                {
                    tr.Start();
                    //con coronacion
                    for (int i = 0; i < ListaWall_ConCOronacion.Count; i++)
                    {
                        var _wall = ListaWall_ConCOronacion[i];
                        var obj = new { tipoAnononimo = ConstNH.CONST_TIPO_ANONIMO_ESTRIBOCORONACION, IsCoronacion = "si" };
                        string json = JsonConvert.SerializeObject(obj);
                        // var claveAleatori = EmbeberNH.getKey(ConstNH.CONST_EMBEBER);
                        if (IsEncriptar)
                            json = EmbeberNH.Ejecutar(json, ConstNH.CONST_EMBEBER);
                        ParameterUtil.SetParaStringNH(_wall, CONST_PARAMETER.CONT_INFORVT, json);
                    }
                    //con coroncion sobre ellos
                    for (int i = 0; i < listaDeMurosConCoroncacionSobre.Count; i++)
                    {
                        var _walAntesCoronacion = listaDeMurosConCoroncacionSobre[i];
                        var obj = new { tipoAnononimo = ConstNH.CONST_TIPO_ANONIMO_ESTRIBOCORONACION, IsCoronacion = "no", IsSobreCoronacion = "si", AlturaMaxima = _walAntesCoronacion.Zcoronacion };
                        string json = JsonConvert.SerializeObject(obj);
                        if (IsEncriptar)
                            json = EmbeberNH.Ejecutar(json, ConstNH.CONST_EMBEBER);
                        ParameterUtil.SetParaStringNH(_walAntesCoronacion.wallnh, CONST_PARAMETER.CONT_INFORVT, json);
                    }

                    //muros intermedios
                    for (int i = 0; i < ListadeMurosIntermedios.Count; i++)
                    {
                        var _walAntesCoronacion = ListadeMurosIntermedios[i];
                        var obj = new { tipoAnononimo = ConstNH.CONST_TIPO_ANONIMO_ESTRIBOCORONACION, IsCoronacion = "no", IsSobreCoronacion = "no" };
                        string json = JsonConvert.SerializeObject(obj);
                        if (IsEncriptar)
                            json = EmbeberNH.Ejecutar(json, ConstNH.CONST_EMBEBER);
                        ParameterUtil.SetParaStringNH(_walAntesCoronacion, CONST_PARAMETER.CONT_INFORVT, json);
                    }

                    tr.Commit();
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M3_CopiarDatosInternos'. ex:{ex.Message}");
                return false;
            }
            return true;
        }


        public bool DesAsignar()
        {
            try
            {
                for (int i = 0; i < ListaWall_ConCOronacion.Count; i++)
                {
                    var _wall = ListaWall_ConCOronacion[i];
                    var nombrepara = ParameterUtil.FindParaByName(_wall.Parameters, CONST_PARAMETER.CONT_INFORVT);

                    if (nombrepara != null)
                    {
                        string jsonDesencriptado = EmbeberNH.DesEjecutar(nombrepara.AsString(), ConstNH.CONST_EMBEBER);
                        // Convertir el JSON desencriptado a objeto
                        var objeto = JsonConvert.DeserializeObject(jsonDesencriptado);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'DesAsignar'. ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public static (bool, dynamic) DesAsignar(Element _elem)
        {
#if DEBUG
            IsEncriptar = false;
# endif
            if (_elem == null) return (false, null);
            dynamic objeto = default;
            try
            {

                var nombrepara = ParameterUtil.FindParaByName(_elem.Parameters, CONST_PARAMETER.CONT_INFORVT);

                if (nombrepara != null)
                {
                    var stringttt= nombrepara.AsString();
                    if (stringttt == "") return (false, null);

                    if (IsEncriptar)
                        stringttt = EmbeberNH.DesEjecutar(nombrepara.AsString(), ConstNH.CONST_EMBEBER);
                    // Convertir el JSON desencriptado a objeto
                    objeto = JsonConvert.DeserializeObject(stringttt);
                    if (objeto == null) return (false, null);
          ;
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'DesAsignar'. ex:{ex.Message}");
                return (false, null);
            }
            return (true, objeto);
        }
    }
}