using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.Calculos.Ayuda;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.TipoTag;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.Calculos
{
    public class ACalculosRefuerzo
    {
        private readonly UIApplication _uiapp;
        protected SeleccinarMuroRefuerzo _lsm1;
        protected SeleccinarMuroRefuerzo _lsm2;
        protected XYZ pInicialMouse;
        protected XYZ pFinalMouse;
        protected XYZ p1;
        protected XYZ p2;
        protected XYZ p3;
        protected XYZ p4;

        protected Transform trans1;
        protected Transform trans2_rotacion;

        protected Transform Invertrans1;
        protected Transform InverTrans2_rotacion;

        protected double _anguloSeleccion_Grado;


        protected XYZ _p1Tras;
        protected XYZ _p2Tras;
        protected XYZ _p3Tras;
        protected XYZ _p4Tras;

        protected int[] numbersSup;
        protected int[] numbersInf;
        public List<XYZ> listaPtos { get; set; }
        protected List<XYZ> ListaGraficar;
        public TipoBarraRefuerzo _tipoBarra { get; set; }

        public List<CalculoBarraRefuerzo> ListaBArrasSuperior { get; set; }
        public List<CalculoBarraRefuerzo> ListaBArrasInferior { get; set; }
        public List<EstriboRefuerzoDTO> ListaEstriboRefuerzoDTO { get; set; }
        /*
p1 y p2 tiene que ser ptos a la izqAbajo
p3 yp4 tiene que ser tos al dereArriba

*/
        public ACalculosRefuerzo(UIApplication _uiapp, SeleccinarMuroRefuerzo lsm1, SeleccinarMuroRefuerzo lsm2)
        {
            ListaGraficar = new List<XYZ>();
            listaPtos = new List<XYZ>();

            this.p1 = lsm1.ListaPtosBordeMuroIntersectado[0];
            this.p2 = lsm1.ListaPtosBordeMuroIntersectado[1];
            this.p3 = lsm2.ListaPtosBordeMuroIntersectado[0];
            this.p4 = lsm2.ListaPtosBordeMuroIntersectado[1];

            this.pInicialMouse = (p1 + p2) / 2;
            this.pFinalMouse = (p3 + p4) / 2;

            listaPtos.Add(p1);
            listaPtos.Add(p2);
            listaPtos.Add(p3);
            listaPtos.Add(p4);

            trans1 = null;
            Invertrans1 = null;
            trans2_rotacion = null;
            InverTrans2_rotacion = null;
            this._uiapp = _uiapp;
            this._lsm1 = lsm1;
            this._lsm2 = lsm2;
        }

        protected void ObtenerAngulodeSeleccion()
        {
            _anguloSeleccion_Grado = Util.AnguloEntre2PtosGrado90(pInicialMouse.GetXY0(), pFinalMouse.GetXY0(), true);
        }

        protected void OBtenerTrasformadas()
        {
            XYZ ptcentral = new XYZ(listaPtos.Average(pt => pt.X), listaPtos.Average(pt => pt.Y), listaPtos.Average(pt => pt.Z));

            trans1 = Transform.CreateTranslation(-ptcentral);
            trans2_rotacion = Transform.CreateRotationAtPoint(XYZ.BasisZ, -Util.GradosToRadianes(_anguloSeleccion_Grado), XYZ.Zero);

            Invertrans1 = trans1.Inverse;
            InverTrans2_rotacion = trans2_rotacion.Inverse;

        }
        /*
                   p1    p4
         p1mouse   |     |   p2mouse
                   p2    p3             
             */
        protected void Ordenar4PtosInicales()
        {

            _p1Tras = trans2_rotacion.OfPoint(trans1.OfPoint(p1));
            _p2Tras = trans2_rotacion.OfPoint(trans1.OfPoint(p2));
            _p3Tras = trans2_rotacion.OfPoint(trans1.OfPoint(p3));
            _p4Tras = trans2_rotacion.OfPoint(trans1.OfPoint(p4));

            if (_p1Tras.Y < _p2Tras.Y)
            {
                XYZ auxp1 = _p2Tras;
                _p2Tras = _p1Tras;
                _p1Tras = auxp1;
            }

            if (_p4Tras.Y < _p3Tras.Y)
            {
                XYZ auxp1 = _p4Tras;
                _p4Tras = _p3Tras;
                _p3Tras = auxp1;
            }


            if (_p4Tras.X < _p1Tras.X)
            {
                XYZ auxp4 = _p1Tras;
                _p1Tras = _p4Tras;
                _p4Tras = auxp4;
            }

            if (_p3Tras.X < _p2Tras.X)
            {
                XYZ auxp3 = _p2Tras;
                _p2Tras = _p3Tras;
                _p3Tras = auxp3;
            }
        }
        public IGeometriaTag GenerarTagEstribo(bool IsEstribo)
        {
            IGeometriaTag _newGeometriaTag;
            CalculoBarraRefuerzo Estribo1a = ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersInf[numbersInf.Length - 1]}Inf").FirstOrDefault();

            if (!IsEstribo)
            {
                _newGeometriaTag = new GeomeTagNull();
            }
            else if (Estribo1a == null)
            {
                Util.ErrorMsg("Error al seleccionar ubicacion de estribo");
                _newGeometriaTag = new GeomeTagNull();
            }
            else
            {
                _newGeometriaTag = FactoryGeomTagRefuerzo.CrearGeometriaTagBarraRefuerzo(_uiapp,
                                                                                        TipoBarraRefuerzo.EstriboRef,
                                                                                          Estribo1a.pa_Orig,
                                                                                          Estribo1a.pd_Orig,
                                                                                        (Estribo1a.pa_Orig + Estribo1a.pd_Orig) / 2);
            }
            return _newGeometriaTag;
        }
        public IGeometriaTag GenerarTagRefuerzo(double factorDesplamiento)
        {
            XYZ direcionDesplazamiento = XYZ.Zero;
#pragma warning disable CS0219 // The variable 'Estribo1a' is assigned but its value is never used
            CalculoBarraRefuerzo Estribo1a = null;
#pragma warning restore CS0219 // The variable 'Estribo1a' is assigned but its value is never used

            XYZ valaroInicial_SupParaTexto = XYZ.Zero;
            XYZ ValorFinal_SUpParaTexto = XYZ.Zero;
            //Buscar pto superior par atexto

            if (ListaBArrasSuperior.Count > 0)
            {
                var angle = Util.AnguloEntre2PtosGrado90(ListaBArrasSuperior.First().pa_Orig, ListaBArrasSuperior.First().pd_Orig, true);

                (ValorFinal_SUpParaTexto, valaroInicial_SupParaTexto, direcionDesplazamiento) = ValoresPara_superior(angle);
            }
            else if (ListaBArrasInferior.Count > 0)
            {

                var angle = Util.AnguloEntre2PtosGrado90(ListaBArrasInferior.First().pa_Orig, ListaBArrasInferior.First().pd_Orig, true);

                (ValorFinal_SUpParaTexto, valaroInicial_SupParaTexto, direcionDesplazamiento) = VAloresPAra_Inferior(angle);
            }
            else
                return null;



            direcionDesplazamiento = direcionDesplazamiento * factorDesplamiento;

            IGeometriaTag _newGeometriaTag = FactoryGeomTagRefuerzo.CrearGeometriaTagBarraRefuerzo(_uiapp,
                                                                                       _tipoBarra,
                                                                                         valaroInicial_SupParaTexto + direcionDesplazamiento,
                                                                                         ValorFinal_SUpParaTexto + direcionDesplazamiento,
                                                                                        (valaroInicial_SupParaTexto + ValorFinal_SUpParaTexto) / 2 + direcionDesplazamiento);
            return _newGeometriaTag;
        }

        private (XYZ ValorFinal_SUpParaTexto, XYZ valaroInicial_SupParaTexto, XYZ direcionDesplazamiento) ValoresPara_superior(double angle)
        {
            XYZ Aux_ = XYZ.Zero;
            XYZ valaroInicial_SupParaTexto = XYZ.Zero;
            XYZ ValorFinal_SUpParaTexto = XYZ.Zero;
            XYZ direcionDesplazamiento = XYZ.Zero;

            (XYZ pa_Orig_aux, XYZ pd_Orig_aux) = AyudaCalculo.Ordena2Ptos(ListaBArrasSuperior.First().pa_Orig, ListaBArrasSuperior.Last().pa_Orig);

            var direccionBArras = (pd_Orig_aux - pa_Orig_aux).AsignarZ(0).Normalize();
            direcionDesplazamiento = Util.CrossProduct(new XYZ(0, 0, 1), direccionBArras);

            CalculoBarraRefuerzo Sup_Estribo1a = null;
            CalculoBarraRefuerzo Inf_Estribo1a = null;

            if (ListaBArrasInferior.Count > 0)
            {
                Sup_Estribo1a = ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersSup[numbersSup.Length - 1]}Sup").FirstOrDefault();
                Inf_Estribo1a = ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersInf[numbersInf.Length - 1]}Inf").FirstOrDefault();
            }
            else
            {
                Sup_Estribo1a = ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersSup[numbersSup.Length - 1]}Sup").FirstOrDefault();
                Inf_Estribo1a = ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersSup[0]}Sup").FirstOrDefault();
            }

            //*****
            if (angle >= 0)
            {
                (valaroInicial_SupParaTexto, Aux_) = AyudaCalculo.Ordena2Ptos(Sup_Estribo1a.pa_Orig, Inf_Estribo1a.pa_Orig);
                (ValorFinal_SUpParaTexto, Aux_) = AyudaCalculo.Ordena2Ptos(Sup_Estribo1a.pd_Orig, Inf_Estribo1a.pd_Orig);
            }
            else
            {
                (Aux_, valaroInicial_SupParaTexto) = AyudaCalculo.Ordena2Ptos(Sup_Estribo1a.pa_Orig, Inf_Estribo1a.pa_Orig);
                (Aux_, ValorFinal_SUpParaTexto) = AyudaCalculo.Ordena2Ptos(Sup_Estribo1a.pd_Orig, Inf_Estribo1a.pd_Orig);
            }

            return (valaroInicial_SupParaTexto, ValorFinal_SUpParaTexto, direcionDesplazamiento);
        }

        private (XYZ ValorFinal_SUpParaTexto, XYZ valaroInicial_SupParaTexto, XYZ direcionDesplazamiento) VAloresPAra_Inferior(double angle)
        {
            XYZ Aux_ = XYZ.Zero;
            XYZ direcionDesplazamiento = XYZ.Zero;
            XYZ valaroInicial_SupParaTexto = XYZ.Zero;
            XYZ ValorFinal_SUpParaTexto = XYZ.Zero;

            (XYZ pa_Orig_aux, XYZ pd_Orig_aux) = AyudaCalculo.Ordena2Ptos(ListaBArrasInferior.First().pa_Orig, ListaBArrasInferior.Last().pd_Orig);

            var direccionBArras = (pd_Orig_aux - pa_Orig_aux).AsignarZ(0).Normalize();
            direcionDesplazamiento = Util.CrossProduct(new XYZ(0, 0, 1), direccionBArras);

            CalculoBarraRefuerzo Sup_Estribo1a = null;
            CalculoBarraRefuerzo Inf_Estribo1a = null;

            if (ListaBArrasSuperior.Count > 0)
            {
                Sup_Estribo1a = ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersSup[numbersSup.Length - 1]}Sup").FirstOrDefault();
                Inf_Estribo1a = ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersInf[numbersInf.Length - 1]}Inf").FirstOrDefault();
            }
            else
            {
                Sup_Estribo1a = ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersInf[0]}Inf").FirstOrDefault();
                Inf_Estribo1a = ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersInf[numbersInf.Length - 1]}Inf").FirstOrDefault();
                //valaroInicial_SupParaTexto = Estribo1a.pa_Orig;
                //ValorFinal_SUpParaTexto = Estribo1a.pd_Orig;
            }

            //*****
            if (angle > 0)
            {
                (valaroInicial_SupParaTexto, Aux_) = AyudaCalculo.Ordena2Ptos(Sup_Estribo1a.pa_Orig, Inf_Estribo1a.pa_Orig);
                (ValorFinal_SUpParaTexto, Aux_) = AyudaCalculo.Ordena2Ptos(Sup_Estribo1a.pd_Orig, Inf_Estribo1a.pd_Orig);
            }
            else
            {
                (Aux_, valaroInicial_SupParaTexto) = AyudaCalculo.Ordena2Ptos(Sup_Estribo1a.pa_Orig, Inf_Estribo1a.pa_Orig);
                (Aux_, ValorFinal_SUpParaTexto) = AyudaCalculo.Ordena2Ptos(Sup_Estribo1a.pd_Orig, Inf_Estribo1a.pd_Orig);
            }

            return (valaroInicial_SupParaTexto, ValorFinal_SUpParaTexto, direcionDesplazamiento); ;
        }



    }
}
