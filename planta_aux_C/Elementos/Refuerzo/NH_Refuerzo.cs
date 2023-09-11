
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VARIOS;
using Microsoft.VisualBasic;
using Autodesk;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Windows;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Colors;
using Autodesk.Windows;
using planta_aux_C;

using AcApp = Autodesk.AutoCAD.ApplicationServices.Application;
using planta_aux_C.Utiles;
using planta_aux_C.Rutinas;
using VARIOS_C;
using ESTRIBOS_MUROS;
using planta_aux_C.enumera;
using VARIOS_C.Datos;


namespace planta_aux_C.Elementos.Refuerzo
{
    public class NH_Refuerzo
    {

        #region 0)Propiedades
        public int espaciamiento { get; set; }
        public int diametro { get; set; }
        public int largoTraslapoInicial { get; set; }
        public int largoTraslapoFinal { get; set; }
        public int cantidadBarras { get; set; }
        public double angulo { get; set; }
        public bool capa_inferior { get; set; }
        public string cuantia { get; set; }
        public Tiporefuerzo tiporefuerzo { get; set; }
        public List<NH_PolilineaRef> ListabarrasRefuerzos { get; set; }

        public NH_PolilineaRef line_Sup { get; set; }
        public NH_PolilineaRef line_Inf { get; set; }

        public NH_PolilineaRef lineaReferenciaSuperior_ultima { get; set; }
        public NH_PolilineaRef lineaReferenciaInferior_ultima { get; set; }

        #endregion

        #region 1)Constructores

        public NH_Refuerzo(NH_PolilineaRef lineaReferenciaInferior, NH_PolilineaRef lineaReferenciaSuperior, int cantidadBarras, Tiporefuerzo tiporefuerzo)
        {
            this.line_Inf = lineaReferenciaInferior;
            this.line_Sup = lineaReferenciaSuperior;
            this.cantidadBarras = cantidadBarras;
            this.tiporefuerzo = tiporefuerzo;
            ListabarrasRefuerzos = new List<NH_PolilineaRef>();
        }

        public NH_Refuerzo(List<NH_PolilineaRef> lista, int cantidadBarras, int espaciamiento, int diametro, bool capa_inferior, Tiporefuerzo tiporefuerzo)
        {
            this.capa_inferior = capa_inferior;
            this.diametro = diametro;
            line_Sup = new NH_PolilineaRef();
            line_Inf = new NH_PolilineaRef();
            lineaReferenciaSuperior_ultima = new NH_PolilineaRef();
            lineaReferenciaInferior_ultima = new NH_PolilineaRef();
            ListabarrasRefuerzos = new List<NH_PolilineaRef>();
            this.tiporefuerzo = tiporefuerzo;

            if (this.tiporefuerzo == Tiporefuerzo.ConEstribos)
            {
                largoTraslapoInicial = ComunesLosa.largo_traslapo(diametro);
                largoTraslapoFinal = ComunesLosa.largo_traslapo(diametro);
            }
            else if (this.tiporefuerzo == Tiporefuerzo.CabezaMuro && diametro <= 12)
            {
                if (Math.Abs(lista[0].angle) < 0.1) // vertical
                {
                    if (lista[0].PuntoSelecionFinalMouse.Y > lista[0].PuntoSelecionInicialMouse.Y)
                    {
                        largoTraslapoInicial = ComunesLosa.largo_traslapo(diametro);
                        largoTraslapoFinal = 100;
                    }
                    else
                    {
                        largoTraslapoInicial = 100;
                        largoTraslapoFinal = ComunesLosa.largo_traslapo(diametro);
                    }

                }
                else
                {
                    if (lista[0].PuntoSelecionFinalMouse.X > lista[0].PuntoSelecionInicialMouse.X)
                    {
                        largoTraslapoInicial = ComunesLosa.largo_traslapo(diametro);
                        largoTraslapoFinal = 100;
                    }
                    else
                    {
                        largoTraslapoInicial = 100;
                        largoTraslapoFinal = ComunesLosa.largo_traslapo(diametro);
                    }
                }


            }
            else if (this.tiporefuerzo == Tiporefuerzo.CabezaMuro && diametro > 12)
            {
                if (Math.Abs(lista[0].angle) < 0.1) // vertical
                {
                    if (lista[0].PuntoSelecionFinalMouse.Y > lista[0].PuntoSelecionInicialMouse.Y)
                    {
                        largoTraslapoInicial = ComunesLosa.largo_traslapo(diametro);
                        largoTraslapoFinal = 100;
                    }
                    else
                    {
                        largoTraslapoInicial = 100;
                        largoTraslapoFinal = ComunesLosa.largo_traslapo(diametro);
                    }

                }
                else
                {
                    if (lista[0].PuntoSelecionFinalMouse.X > lista[0].PuntoSelecionInicialMouse.X)
                    {
                        largoTraslapoInicial = ComunesLosa.largo_traslapo(diametro);
                        largoTraslapoFinal = 100;
                    }
                    else
                    {
                        largoTraslapoInicial = 100;
                        largoTraslapoFinal = ComunesLosa.largo_traslapo(diametro);
                    }
                }

            }

            this.espaciamiento = espaciamiento;
            this.cantidadBarras = cantidadBarras;

            if (capa_inferior)
            { cuantia = "F=F'=" + cantidadBarras + "%%c" + diametro; }
            else
            { cuantia = "F'=" + cantidadBarras + "%%c" + diametro; }

            Load_CargandoPtoBarraSegunOrientacion(lista);

            if (lista.Count == 2)
            {
                line_Inf.CalcularAngulo();
                line_Sup.CalcularAngulo();
            }
            else
            {
                line_Inf.CalcularAngulo(lista[0].angle);
                line_Sup.CalcularAngulo(lista[0].angle);
            }
            CalcularBarrasrRefuerzo();
        }






        #endregion

        #region 2)metodos

        private void Load_CargandoPtoBarraSegunOrientacion(List<NH_PolilineaRef> lista)
        {
            NH_PolilineaRef poli1;
            NH_PolilineaRef poli2;
            poli1 = lista.ElementAt(0);
            var aux_1 = comunes.coordenada_modificar_Y_manda(poli1.ptIni, poli1.ptFin);

            if (Math.Abs(poli1.angle) < 0.01)// solo si  poli1 compeltamente horizontal
            {
                poli1.ptIni = aux_1[1];
                poli1.ptFin = aux_1[0];
            }
            else
            {
                poli1.ptIni = aux_1[0];
                poli1.ptFin = aux_1[1];
            }

            poli2 = lista.ElementAt(lista.Count - 1);
            var aux_2 = comunes.coordenada_modificar_Y_manda(poli2.ptIni, poli2.ptFin);

            if (Math.Abs(poli2.angle) < 0.01)// solo si  poli1 compeltamente horizontal
            {
                poli2.ptIni = aux_2[1];
                poli2.ptFin = aux_2[0];
            }
            else
            {
                poli2.ptIni = aux_2[0];
                poli2.ptFin = aux_2[1];
            }


            if (!(Intersecciones.InterSection4Point(new Point3d(poli1.ptIni.X, poli1.ptIni.Y, 0),
                                                  new Point3d(poli2.ptIni.X, poli2.ptIni.Y, 0),
                                                  new Point3d(poli1.ptFin.X, poli1.ptFin.Y, 0),
                                                  new Point3d(poli2.ptFin.X, poli2.ptFin.Y, 0), Intersect.OnBothOperands)))
            {


                var aux = comunes.coordenada_modificar(poli1.ptIni, poli2.ptIni);
                line_Inf.ptIni = new Point2d(aux[0].X, aux[0].Y);
                line_Inf.ptFin = new Point2d(aux[1].X, aux[1].Y);

                var aux1 = comunes.coordenada_modificar(poli1.ptFin, poli2.ptFin);
                line_Sup.ptIni = new Point2d(aux1[0].X, aux1[0].Y);
                line_Sup.ptFin = new Point2d(aux1[1].X, aux1[1].Y);
            }
            else
            {
                var aux = comunes.coordenada_modificar(poli1.ptIni, poli2.ptFin);
                line_Inf.ptIni = new Point2d(aux[0].X, aux[0].Y);
                line_Inf.ptFin = new Point2d(aux[1].X, aux[1].Y);

                var aux1 = comunes.coordenada_modificar(poli1.ptFin, poli2.ptIni);
                line_Sup.ptIni = new Point2d(aux1[0].X, aux1[0].Y);
                line_Sup.ptFin = new Point2d(aux1[1].X, aux1[1].Y);
            }
        }
        // calcula las barras superiores he inferiores de refuerzo
        //line_Sup: se utiliza para generar las barras superiores
        //line_Inf: se utiliza para generar las barras Inferiore
        public void CalcularBarrasrRefuerzo()
        {
            int menos1Barras = 0;
            Point2d ptIni_aux, ptFin_aux;
            if ((cantidadBarras % 2) == 0)
            {
                Console.WriteLine("Es Par");
                menos1Barras = 0;
            }
            else
            {
                menos1Barras = 1;
                Console.WriteLine("Es Impar");

            }


            //linea central solo si es cabelza de muro y si no son dos barras

            if (tiporefuerzo == Tiporefuerzo.CabezaMuro && cantidadBarras != 2)
            {
                if (line_Sup.angle >= 0)
                {
                    ptIni_aux = new Point2d((line_Sup.ptIni.X + line_Inf.ptIni.X) / 2 - Math.Sin(line_Sup.angle) - Math.Cos(line_Sup.angle) * largoTraslapoInicial, (line_Sup.ptIni.Y + line_Inf.ptIni.Y) / 2 + Math.Cos(line_Sup.angle) - Math.Sin(line_Sup.angle) * largoTraslapoInicial);
                    ptFin_aux = new Point2d((line_Sup.ptFin.X + line_Inf.ptFin.X) / 2 - Math.Sin(line_Sup.angle) + Math.Cos(line_Sup.angle) * largoTraslapoFinal, (line_Sup.ptFin.Y + line_Inf.ptFin.Y) / 2 + Math.Cos(line_Sup.angle) + Math.Sin(line_Sup.angle) * largoTraslapoFinal);
                }
                else
                {
                    line_Sup.angle = -line_Sup.angle;
                    ptIni_aux = new Point2d((line_Sup.ptIni.X + line_Inf.ptIni.X) / 2 + Math.Sin(Math.Abs(line_Sup.angle)) - Math.Cos(Math.Abs(line_Sup.angle)) * largoTraslapoInicial, (line_Sup.ptIni.Y + line_Inf.ptIni.Y) / 2 + Math.Cos(Math.Abs(line_Sup.angle)) + Math.Sin(Math.Abs(line_Sup.angle)) * largoTraslapoInicial);
                    ptFin_aux = new Point2d((line_Sup.ptFin.X + line_Inf.ptFin.X) / 2 + Math.Sin(Math.Abs(line_Sup.angle)) + Math.Cos(Math.Abs(line_Sup.angle)) * largoTraslapoFinal, (line_Sup.ptFin.Y + line_Inf.ptFin.Y) / 2 + Math.Cos(Math.Abs(line_Sup.angle)) - Math.Sin(Math.Abs(line_Sup.angle)) * largoTraslapoFinal);
                    line_Sup.angle = -line_Sup.angle;
                }


                var barra_ = new NH_PolilineaRef() { ptIni = ptIni_aux, ptFin = ptFin_aux };
                ListabarrasRefuerzos.Add(barra_);

                if ((cantidadBarras % 2) == 0)
                {
                    Console.WriteLine("Es Par");
                    menos1Barras = -1;
                }
                else
                {
                    menos1Barras = 0;
                    Console.WriteLine("Es Impar");

                }

            }



            //line_superior
            for (int i = 0; i < cantidadBarras / 2 + menos1Barras; i++)
            {

                if (line_Sup.angle >= 0)
                {
                    ptIni_aux = new Point2d(line_Sup.ptIni.X - Math.Sin(line_Sup.angle) * (espaciamiento * i + 5) - Math.Cos(line_Sup.angle) * largoTraslapoInicial, line_Sup.ptIni.Y + Math.Cos(line_Sup.angle) * (espaciamiento * i + 5) - Math.Sin(line_Sup.angle) * largoTraslapoInicial);
                    ptFin_aux = new Point2d(line_Sup.ptFin.X - Math.Sin(line_Sup.angle) * (espaciamiento * i + 5) + Math.Cos(line_Sup.angle) * largoTraslapoFinal, line_Sup.ptFin.Y + Math.Cos(line_Sup.angle) * (espaciamiento * i + 5) + Math.Sin(line_Sup.angle) * largoTraslapoFinal);
                }
                else
                {
                    line_Sup.angle = -line_Sup.angle;
                    ptIni_aux = new Point2d(line_Sup.ptIni.X + Math.Sin(Math.Abs(line_Sup.angle)) * (espaciamiento * i + 5) - Math.Cos(Math.Abs(line_Sup.angle)) * largoTraslapoInicial, line_Sup.ptIni.Y + Math.Cos(Math.Abs(line_Sup.angle)) * (espaciamiento * i + 5) + Math.Sin(Math.Abs(line_Sup.angle)) * largoTraslapoInicial);
                    ptFin_aux = new Point2d(line_Sup.ptFin.X + Math.Sin(Math.Abs(line_Sup.angle)) * (espaciamiento * i + 5) + Math.Cos(Math.Abs(line_Sup.angle)) * largoTraslapoFinal, line_Sup.ptFin.Y + Math.Cos(Math.Abs(line_Sup.angle)) * (espaciamiento * i + 5) - Math.Sin(Math.Abs(line_Sup.angle)) * largoTraslapoFinal);
                    line_Sup.angle = -line_Sup.angle;
                }


                var barra_ = new NH_PolilineaRef() { ptIni = ptIni_aux, ptFin = ptFin_aux };
                ListabarrasRefuerzos.Add(barra_);

            }

            // referencia linea final
            if (line_Sup.angle >= 0)
            {
                lineaReferenciaSuperior_ultima.ptIni = new Point2d(line_Sup.ptIni.X - Math.Sin(line_Sup.angle) * (espaciamiento * ((long)(cantidadBarras / 2 + menos1Barras) - 1) + 5), line_Sup.ptIni.Y + Math.Cos(line_Sup.angle) * (espaciamiento * ((long)(cantidadBarras / 2 + menos1Barras) - 1) + 5));
                lineaReferenciaSuperior_ultima.ptFin = new Point2d(line_Sup.ptFin.X - Math.Sin(line_Sup.angle) * (espaciamiento * ((long)(cantidadBarras / 2 + menos1Barras) - 1) + 5), line_Sup.ptFin.Y + Math.Cos(line_Sup.angle) * (espaciamiento * ((long)(cantidadBarras / 2 + menos1Barras) - 1) + 5));
            }
            else
            {
                line_Sup.angle = -line_Sup.angle;
                lineaReferenciaSuperior_ultima.ptIni = new Point2d(line_Sup.ptIni.X + Math.Sin(line_Sup.angle) * (espaciamiento * ((long)(cantidadBarras / 2 + menos1Barras) - 1) + 5), line_Sup.ptIni.Y + Math.Cos(line_Sup.angle) * (espaciamiento * ((long)(cantidadBarras / 2 + menos1Barras) - 1) + 5));
                lineaReferenciaSuperior_ultima.ptFin = new Point2d(line_Sup.ptFin.X + Math.Sin(line_Sup.angle) * (espaciamiento * ((long)(cantidadBarras / 2 + menos1Barras) - 1) + 5), line_Sup.ptFin.Y + Math.Cos(line_Sup.angle) * (espaciamiento * ((long)(cantidadBarras / 2 + menos1Barras) - 1) + 5));
                line_Sup.angle = -line_Sup.angle;
            }

            //line_inferior
            for (int i = 0; i < cantidadBarras / 2; i++)
            {
                if (line_Sup.angle >= 0)
                {
                    ptIni_aux = new Point2d(line_Inf.ptIni.X + Math.Sin(line_Inf.angle) * (espaciamiento * i + 5) - Math.Cos(line_Sup.angle) * largoTraslapoInicial, line_Inf.ptIni.Y - Math.Cos(line_Inf.angle) * (espaciamiento * i + 5) - Math.Sin(line_Sup.angle) * largoTraslapoInicial);
                    ptFin_aux = new Point2d(line_Inf.ptFin.X + Math.Sin(line_Inf.angle) * (espaciamiento * i + 5) + Math.Cos(line_Sup.angle) * largoTraslapoFinal, line_Inf.ptFin.Y - Math.Cos(line_Inf.angle) * (espaciamiento * i + 5) + Math.Sin(line_Sup.angle) * largoTraslapoFinal);
                }
                else
                {
                    line_Inf.angle = -line_Inf.angle;
                    ptIni_aux = new Point2d(line_Inf.ptIni.X - Math.Sin(Math.Abs(line_Inf.angle)) * (espaciamiento * i + 5) - Math.Cos(Math.Abs(line_Sup.angle)) * largoTraslapoInicial, line_Inf.ptIni.Y - Math.Cos(Math.Abs(line_Inf.angle)) * (espaciamiento * i + 5) + Math.Sin(Math.Abs(line_Sup.angle)) * largoTraslapoInicial);
                    ptFin_aux = new Point2d(line_Inf.ptFin.X - Math.Sin(Math.Abs(line_Inf.angle)) * (espaciamiento * i + 5) + Math.Cos(Math.Abs(line_Sup.angle)) * largoTraslapoFinal, line_Inf.ptFin.Y - Math.Cos(Math.Abs(line_Inf.angle)) * (espaciamiento * i + 5) - Math.Sin(Math.Abs(line_Sup.angle)) * largoTraslapoFinal);
                    line_Inf.angle = -line_Inf.angle;
                }
                ListabarrasRefuerzos.Add(new NH_PolilineaRef() { ptIni = ptIni_aux, ptFin = ptFin_aux });
            }

            // referencia linea final
            if (line_Sup.angle >= 0)
            {
                lineaReferenciaInferior_ultima.ptIni = new Point2d(line_Inf.ptIni.X + Math.Sin(line_Inf.angle) * (espaciamiento * ((long)(cantidadBarras / 2) - 1) + 5), line_Inf.ptIni.Y - Math.Cos(line_Inf.angle) * (espaciamiento * (cantidadBarras / 2 - 1) + 5));
                lineaReferenciaInferior_ultima.ptFin = new Point2d(line_Inf.ptFin.X + Math.Sin(line_Inf.angle) * (espaciamiento * ((long)(cantidadBarras / 2) - 1) + 5), line_Inf.ptFin.Y - Math.Cos(line_Inf.angle) * (espaciamiento * (cantidadBarras / 2 - 1) + 5));
            }
            else
            {
                line_Inf.angle = -line_Inf.angle;
                lineaReferenciaInferior_ultima.ptIni = new Point2d(line_Inf.ptIni.X - Math.Sin(Math.Abs(line_Inf.angle)) * (espaciamiento * ((long)(cantidadBarras / 2) - 1) + 5), line_Inf.ptIni.Y - Math.Cos(Math.Abs(line_Inf.angle)) * (espaciamiento * (cantidadBarras / 2 - 1) + 5));
                lineaReferenciaInferior_ultima.ptFin = new Point2d(line_Inf.ptFin.X - Math.Sin(Math.Abs(line_Inf.angle)) * (espaciamiento * ((long)(cantidadBarras / 2) - 1) + 5), line_Inf.ptFin.Y - Math.Cos(Math.Abs(line_Inf.angle)) * (espaciamiento * (cantidadBarras / 2 - 1) + 5));
                line_Inf.angle = -line_Inf.angle;
            }


        }



        #endregion



    }



}
