using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.UTILES
{

    public class ptoGenerarIntervaloDTO
    {

        public XYZ pto { get; set; }
        public double distaciato_L1_L2 { get; set; }
    }

    public class CrearListaPtos
    {
        public static void M2_linea(UIApplication uiapp, int contador = 100000)
        {
            Document _doc = uiapp.ActiveUIDocument.Document;
            ObjectSnapTypes snapTypes = ObjectSnapTypes.Nearest;
            //Nos permite seleccionar un punto en una posición cualquiera y nos da el dato XYZ
           XYZ  _ptoIntervalo1 = uiapp.ActiveUIDocument.Selection.PickPoint(snapTypes, "Seleccionar Punto ");

            //Nos permite seleccionar un punto en una posición cualquiera y nos da el dato XYZ
            XYZ _ptoIntervalo2 = uiapp.ActiveUIDocument.Selection.PickPoint(snapTypes, "Seleccionar Punto ");

            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("Create LineStyle-NH");

               
                    t.Commit();
                }
            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"  EX:{ex.Message}");
            }
        }

        public static List<XYZ> M2_ListaPtoSimple(UIApplication uiapp ,int contador=100000)
        {

            List<XYZ> _listaptoTramo = new List<XYZ>();
            try
            {

                XYZ _ptoIntervalo = XYZ.Zero;
                bool continuar = true;
                int cont = 1;
                while (continuar && cont<contador+1)
                {
                    try
                    {
                        continuar = false;
                        ObjectSnapTypes snapTypes = ObjectSnapTypes.Nearest;
                        //Nos permite seleccionar un punto en una posición cualquiera y nos da el dato XYZ
                        _ptoIntervalo = uiapp.ActiveUIDocument.Selection.PickPoint(snapTypes, $"Seleccionar Punto {cont}");

                        _listaptoTramo.Add(_ptoIntervalo);
                        cont += 1;
                        continuar = true;
                    }
                    catch (Exception ex)
                    {
                        Util.DebugDescripcion(ex);
                        continuar = false;
                    }
                }

            }
            catch (Exception)
            {
                _listaptoTramo.Clear();
            }
            return _listaptoTramo;
        }


        public static List<XYZ> M2_ListaPtoTramo(UIDocument _uidoc, List<XYZ> ListaPoligono4ptos)
        {
            List<XYZ> _listaptoTramo = new List<XYZ>();
            try
            {

                XYZ _ptoIntervalo = XYZ.Zero;
                bool continuar = true;
                while (continuar)
                {
                    try
                    {
                        continuar = false;
                        ObjectSnapTypes snapTypes = ObjectSnapTypes.Nearest;
                        //Nos permite seleccionar un punto en una posición cualquiera y nos da el dato XYZ
                        _ptoIntervalo = _uidoc.Selection.PickPoint(snapTypes, "Seleccionar Punto Inferior Barra");



                        if (IsDentroPoligono.IsPointInsidePolyline(_ptoIntervalo, ListaPoligono4ptos))
                        {

                            XYZ pto_inter_p1_p2 = Line.CreateBound(ListaPoligono4ptos[0], ListaPoligono4ptos[1]).ProjectSINExtendida3D(_ptoIntervalo);
                            XYZ pto_inter_p3_p4 = Line.CreateBound(ListaPoligono4ptos[2], ListaPoligono4ptos[3]).ProjectSINExtendida3D(_ptoIntervalo);

                            double distancia_1_2 = _ptoIntervalo.DistanceTo(pto_inter_p1_p2);
                            double distancia_3_4 = _ptoIntervalo.DistanceTo(pto_inter_p3_p4);

                            double distancia_LAst_ = 100000;
                            if (_listaptoTramo.Count > 0)
                            {
                                distancia_LAst_ = _ptoIntervalo.DistanceTo(_listaptoTramo.Last());
                            }

                            if (Util.CmToFoot(ConstNH.CONST_LARGO_INTERVALO_CONTINUO_LOSA_CM) > distancia_1_2 ||
                                Util.CmToFoot(ConstNH.CONST_LARGO_INTERVALO_CONTINUO_LOSA_CM) > distancia_3_4 ||
                                Util.CmToFoot(ConstNH.CONST_LARGO_INTERVALO_CONTINUO_LOSA_CM) > distancia_LAst_)
                            {
                                Util.ErrorMsg($"Intervalos generados deben ser de largo como min de {ConstNH.CONST_LARGO_INTERVALO_CONTINUO_LOSA_CM} cm");
                                continuar = true;
                                continue;
                            }

                            _listaptoTramo.Add(_ptoIntervalo);
                        }
                        else
                            Util.ErrorMsg("Pto no considerado para intervalo por estar fuerza del area de dibujo de barra");
                        continuar = true;
                    }
                    catch (Exception)
                    {
                        continuar = false;
                    }
                }


                _listaptoTramo = OrdenarPtos_sentidoP1_P2(_listaptoTramo, ListaPoligono4ptos);

            }
            catch (Exception)
            {
                _listaptoTramo.Clear();
            }
            return _listaptoTramo;
        }



        private static List<XYZ> OrdenarPtos_sentidoP1_P2(List<XYZ> listaptoTramo, List<XYZ> listaPoligono4ptos)
        {
            if (listaptoTramo.Count < 2) return listaptoTramo;

            List<XYZ> listaptoTramoOrdendado_p1_p2 = new List<XYZ>();
            List<ptoGenerarIntervaloDTO> ListaptoGenerarIntervaloDTO = new List<ptoGenerarIntervaloDTO>();

            foreach (XYZ pto in listaptoTramo)
            {
                XYZ pto_inter_p1_p2 = Line.CreateBound(listaPoligono4ptos[0], listaPoligono4ptos[1]).ProjectSINExtendida3D(pto);
                XYZ pto_inter_p3_p4 = Line.CreateBound(listaPoligono4ptos[2], listaPoligono4ptos[3]).ProjectSINExtendida3D(pto);

                double distancia_1_2 = pto.DistanceTo(pto_inter_p1_p2);
                double distancia_3_4 = pto.DistanceTo(pto_inter_p3_p4);

                double distancia_LAst_ = 100000;
                if (ListaptoGenerarIntervaloDTO.Count > 0)
                {
                    distancia_LAst_ = pto.DistanceTo(ListaptoGenerarIntervaloDTO.Last().pto);
                }

                if (Util.CmToFoot(ConstNH.CONST_LARGO_INTERVALO_CONTINUO_LOSA_CM) > distancia_1_2 ||
                    Util.CmToFoot(ConstNH.CONST_LARGO_INTERVALO_CONTINUO_LOSA_CM) > distancia_3_4 ||
                    Util.CmToFoot(ConstNH.CONST_LARGO_INTERVALO_CONTINUO_LOSA_CM) > distancia_LAst_)
                {
                    Util.ErrorMsg($"Intervalos generados deben ser de largo como min de {ConstNH.CONST_LARGO_INTERVALO_CONTINUO_LOSA_CM} cm");
                    continue;
                }



                ListaptoGenerarIntervaloDTO.Add(new ptoGenerarIntervaloDTO()
                {
                    pto = pto,
                    distaciato_L1_L2 = distancia_1_2
                });
            }

            //ordenado por distancia
            listaptoTramoOrdendado_p1_p2 = ListaptoGenerarIntervaloDTO.OrderBy(c => c.distaciato_L1_L2).Select(pp => pp.pto).ToList();


            return listaptoTramoOrdendado_p1_p2;
        }


        public static  XYZ ObtenerCentroPoligono(List<XYZ> ListaPoligono4ptos )
        {
            return new XYZ(ListaPoligono4ptos.Average(c => c.X), ListaPoligono4ptos.Average(c => c.Y), ListaPoligono4ptos.Average(c => c.Z));                           
        }

    }
}
