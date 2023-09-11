using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Prueba;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.DTO;
using ArmaduraLosaRevit.Model.REFUERZOLOSAS.TipoTag;
using ArmaduraLosaRevit.Model.Seleccionar;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.REFUERZOLOSAS.Calculos
{
    public class ACalculosRefuerzoV2
    {
        private readonly UIApplication _uiapp;
        protected SeleccinarMuroRefuerzo _lsm1;
        protected SeleccinarMuroRefuerzo _lsm2;
        protected XYZ pInicialMouse;
        protected XYZ pFinalMouse;
        protected XYZ pmedio;
        protected XYZ p1;
        protected XYZ p2;
        protected XYZ p3;
        protected XYZ p4;

        protected Transform trans1;
        protected Transform trans2_rotacion;

        protected Transform Invertrans1;
        protected Transform InverTrans2_rotacion;

        protected double _anguloSeleccion;
      
        
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
        public ACalculosRefuerzoV2(UIApplication _uiapp, SeleccinarMuroRefuerzo lsm1, SeleccinarMuroRefuerzo lsm2)
        {
            ListaGraficar = new List<XYZ>();
            listaPtos = new List<XYZ>();
    
            this.p1 = lsm1.ListaPtosBordeMuroIntersectado[0];
            this.p2 = lsm1.ListaPtosBordeMuroIntersectado[1];
            this.p3 = lsm2.ListaPtosBordeMuroIntersectado[0];
            this.p4 = lsm2.ListaPtosBordeMuroIntersectado[1];

            this.pInicialMouse = lsm1.PtoInterseccionSobreBorde;
       
          

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
            _anguloSeleccion = Util.GradosToRadianes(_lsm1.AnguloDireccionenfierrado);
          
            Line lineBorde = Line.CreateBound(p3.AsignarZ(0), p4.AsignarZ(0));
            pFinalMouse = lineBorde.ProjectExtendidaXY0(pInicialMouse.AsignarZ(0)).AsignarZ((p3.Z+ p4.Z)/2);
            pmedio = (pInicialMouse + pFinalMouse) / 2;
        }

        protected void OBtenerTrasformadas()
        {
            XYZ ptcentral = new XYZ(listaPtos.Average(pt => pt.X), listaPtos.Average(pt => pt.Y), listaPtos.Average(pt => pt.Z));

            trans1 = Transform.CreateTranslation(-pmedio);
            trans2_rotacion = Transform.CreateRotationAtPoint(XYZ.BasisZ, -_anguloSeleccion, XYZ.Zero);

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

            bool ptoInter = Util.IsIntersection2(p1.AsignarZ(0), p4.AsignarZ(0), p2.AsignarZ(0), p3.AsignarZ(0));
            if (ptoInter)
            {
                XYZ auxp4 = p4;
                p4 = p3;
                p3 = auxp4;
            }


            _p1Tras = trans2_rotacion.OfPoint(trans1.OfPoint(p1));
            _p2Tras = trans2_rotacion.OfPoint(trans1.OfPoint(p2));
            _p3Tras = trans2_rotacion.OfPoint(trans1.OfPoint(p3));
            _p4Tras = trans2_rotacion.OfPoint(trans1.OfPoint(p4));

            // Y
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

            //x
            if (_p4Tras.X < _p1Tras.X)
            {
                XYZ auxp4 = _p4Tras;
                _p4Tras = _p1Tras;
                _p1Tras = auxp4;
            }

            if (_p3Tras.X < _p2Tras.X)
            {
                XYZ auxp2 = _p3Tras;
                _p3Tras = _p2Tras;
                _p2Tras = auxp2;
            }


            //formar rectangulo

            if (_p1Tras.Y < _p4Tras.Y)
                _p4Tras = new XYZ(_p4Tras.X, _p1Tras.Y,_p4Tras.Z );
            else
                _p1Tras = new XYZ(_p1Tras.X, _p4Tras.Y,_p1Tras.Z);


            if (_p2Tras.Y > _p3Tras.Y)
                _p3Tras = new XYZ(_p3Tras.X, _p2Tras.Y, _p3Tras.Z);
            else
                _p2Tras = new XYZ(_p2Tras.X, _p3Tras.Y, _p2Tras.Z);


            ListaGraficar.Add(_p1Tras);
            ListaGraficar.Add(_p2Tras);
            ListaGraficar.Add(_p3Tras);
            ListaGraficar.Add(_p4Tras);

        }
        public IGeometriaTag GenerarTagEstribo(string tipoPosicionRef)
        {
            IGeometriaTag _newGeometriaTag;
            CalculoBarraRefuerzo Estribo1a = null;
            if (tipoPosicionRef == "Superior") {
                Estribo1a = ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersSup[numbersSup.Length-1]}Sup").FirstOrDefault();
            }
            else { 
             Estribo1a = ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersInf[numbersInf.Length - 1]}Inf").FirstOrDefault(); 
            }

            if (Estribo1a == null)
            {
                Util.ErrorMsg("Error al seleccionar ubicacion de estribo");
                _newGeometriaTag = new GeomeTagNull();
            }
            else
            {
                XYZ factorDesplazamiento = XYZ.Zero;
                if (tipoPosicionRef == "Superior"){
                    factorDesplazamiento=(ListaBArrasSuperior[1].pa_Orig - ListaBArrasSuperior[0].pa_Orig).Normalize()*Util.CmToFoot(32);
                }
                else if(tipoPosicionRef == "Inferior"){
                    factorDesplazamiento = (ListaBArrasInferior[1].pa_Orig - ListaBArrasInferior[0].pa_Orig).Normalize() * Util.CmToFoot(12);
                }
                else
                {
                    factorDesplazamiento = XYZ.Zero; //(ListaBArrasSuperior[1].pa_Orig - ListaBArrasSuperior[0].pa_Orig).Normalize() * Util.CmToFoot(0);
                }

                _newGeometriaTag = FactoryGeomTagRefuerzo.CrearGeometriaTagBarraRefuerzo(_uiapp,
                                                                                        TipoBarraRefuerzo.EstriboRef,
                                                                                          Estribo1a.pa_Orig+ factorDesplazamiento,
                                                                                          Estribo1a.pd_Orig+ factorDesplazamiento,
                                                                                        (Estribo1a.pa_Orig + Estribo1a.pd_Orig) / 2+ factorDesplazamiento);
            }
            return _newGeometriaTag;
        }
        public IGeometriaTag GenerarTagRefuerzo(string tipoPosicionRef)
        {
            //CalculoBarraRefuerzo Estribo1a = ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersSup[numbersSup.Length - 1]}Sup").FirstOrDefault();
            CalculoBarraRefuerzo Estribo1a = null;
            if (tipoPosicionRef != "Inferior")
            {
                Estribo1a = ListaBArrasSuperior.Where(pp => pp.Name == $"b{numbersSup[numbersSup.Length - 1]}Sup").FirstOrDefault();
            }
            else if (tipoPosicionRef == "Inferior")
            {
                Estribo1a = ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersInf[numbersInf.Length-1]}Inf").FirstOrDefault();
            }
            else
            {
                Estribo1a = ListaBArrasInferior.Where(pp => pp.Name == $"b{numbersInf[0]}Inf").FirstOrDefault();
            }


            XYZ factorDesplazamiento = XYZ.Zero;
     
            if (tipoPosicionRef == "Inferior")
            {
                factorDesplazamiento = (ListaBArrasInferior[1].pa_Orig - ListaBArrasInferior[0].pa_Orig).Normalize() * Util.CmToFoot(19);
            }


            IGeometriaTag _newGeometriaTag = FactoryGeomTagRefuerzo.CrearGeometriaTagBarraRefuerzo(_uiapp,
                                                                                        _tipoBarra,
                                                                                          Estribo1a.pa_Orig+ factorDesplazamiento,
                                                                                          Estribo1a.pd_Orig+ factorDesplazamiento,
                                                                                         (Estribo1a.pa_Orig + Estribo1a.pd_Orig) / 2+ factorDesplazamiento);
            return _newGeometriaTag;
        }
    }
}
