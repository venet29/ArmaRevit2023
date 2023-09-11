using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo
{
    public enum TipoCara
    {
        Derecha,Izquierdo, Arriba, Bajo
    }
    public class EnvoltoriosPlanos
    {
        private UIApplication uiapp;
        private View _view;
        private double LargoMinimo_foot = ConstNH.CONST_LARGOMIN_DIMENSION_PASADA;
        public XYZ p1 { get; set; }  //simpre izq o inferior
        public XYZ p2 { get; set; }  //simpre dere o sup
        
        public XYZ Direccion_ { get; set; }
        public XYZ Normal { get; set; }
        public XYZ Normal_Positivo { get; set; }
        public PlanarFace planarFace { get; }
        public bool IsOk { get; set; }
        public List<EnvoltorioElementoIntersectar> ListaElemento2 { get; private set; }
        public List<EnvoltorioElementoIntersectar> ListaElemento1 { get; private set; }

        public EnvoltorioElementoIntersectar refrenciaSeleccionado_Arriba_Dere { get; private set; }
        public EnvoltorioElementoIntersectar refrenciaSeleccionado_Izq_Inf { get; private set; }
        public EnvoltorioElementoIntersectar refrenciaSeleccionado { get; private set; }
        public TipoCara TipoCara_ { get; private set; }
        public List<XYZ> ListaPuntosPlanarFAce { get; private set; }

        public EnvoltoriosPlanos(UIApplication uiapp, PlanarFace c)
        {
            this.uiapp = uiapp;
            this._view = uiapp.ActiveUIDocument.ActiveView;
            this.planarFace = c;
            this.IsOk = false;
        }
        internal void ObtenerPtoInicialFinal()
        {
            try
            {
                ListaPuntosPlanarFAce = planarFace.ObtenerListaPuntos().OrderByDescending(c => c.Z).ToList();
                if (ListaPuntosPlanarFAce.Count != 4) return;
                p1 = ListaPuntosPlanarFAce[0].AsignarZ(_view.Origin.Z);
                p2 = ListaPuntosPlanarFAce[1].AsignarZ(_view.Origin.Z);
                Normal = planarFace.FaceNormal;
                Normal_Positivo = Normal.ObtenerCopia(1);


                ListaElemento2 = new List<EnvoltorioElementoIntersectar>();
                ListaElemento1 = new List<EnvoltorioElementoIntersectar>();
                IsOk = true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool M0_GenerarDIreccion_DerechaSuperior()
        {
            try
            {
                (XYZ p1_izqInf, XYZ p2_dereDup) = Util.Ordena2PtosV2(p1.ObtenerCopia(), p2.ObtenerCopia());

                p1 = p1_izqInf.ObtenerCopia();
                p2 = p2_dereDup.ObtenerCopia();

                Direccion_ = (p2.GetXY0() - p1.GetXY0()).Normalize();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'M0_GenerarDIreccion_DerechaSuperior'. ex:{ex.Message}");
                return false;
            }
            return true;
        }


        public bool M1_BuscarInterseccionesConElemento_ambosSentidos(List<EnvoltorioElementoIntersectar> lista)
        {
            try
            {
                M0_GenerarDIreccion_DerechaSuperior();
                M1_BuscarPosicionCara();
                M2_BuscarMismoSentido(lista);

                M3_BUscarSentidoContrario(lista);

                M4_generarReferenciasSelecionadaMO();

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al 'BuscarInterseccionesConElemento'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool M1_BuscarPosicionCara()
        {
            try
            {
                if (Normal.X > 0 && Normal.X > Normal.Y && Math.Abs(Normal.X) > Math.Abs(Normal.Y))
                    TipoCara_ = TipoCara.Derecha;
                else if (Normal.X < 0 && Normal.X < Normal.Y && Math.Abs(Normal.X) > Math.Abs(Normal.Y))
                    TipoCara_ = TipoCara.Izquierdo;
                else if (Normal.Y > 0 && Normal.Y > Normal.X && Math.Abs(Normal.X) < Math.Abs(Normal.Y))
                    TipoCara_ = TipoCara.Arriba;
                else if (Normal.Y < 0 && Normal.Y < Normal.X && Math.Abs(Normal.X) < Math.Abs(Normal.Y))
                    TipoCara_ = TipoCara.Bajo;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al 'BuscarInterseccionesConElemento'. ex:{ex.Message}");
                return false;
            }
            return true;
        }


        //haci de derecha o superior
        public bool M2_BuscarMismoSentido(List<EnvoltorioElementoIntersectar> lista)
        {
            try
            {
                bool ISValido = false; XYZ ptoInterseccion = default; double largo_foot = 0;

                //falta asignar direccion de distancia


                for (int i = 0; i < lista.Count; i++)
                {

                    (ISValido, ptoInterseccion, largo_foot) = IsInterseccionMismaDireccion(lista[i], p2, Direccion_);

                    if (ISValido)
                    {
                        ListaElemento1.Add(new EnvoltorioElementoIntersectar(lista[i]._EnvoltorioGrid, ptoInterseccion, largo_foot));
                    }
                }

                ListaElemento1 = ListaElemento1.Where(c => c.IsOk).OrderBy(c => c.distanciaInterseccion_foot).ToList();
                refrenciaSeleccionado_Arriba_Dere = ListaElemento1[0];
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        // hacia la izq-inferior
        public bool M3_BUscarSentidoContrario(List<EnvoltorioElementoIntersectar> lista)
        {
            try
            {
                bool ISValido = false; XYZ ptoInterseccion = default; double largo = 0;

                for (int i = 0; i < lista.Count; i++)
                {
                    (ISValido, ptoInterseccion, largo) = IsInterseccionMismaDireccion(lista[i], p1, -Direccion_);

                    if (ISValido)
                    {
                        ListaElemento2.Add(new EnvoltorioElementoIntersectar(lista[i]._EnvoltorioGrid, ptoInterseccion, largo));
                    }
                }

                ListaElemento2 = ListaElemento2.Where(c => c.IsOk).OrderBy(c => c.distanciaInterseccion_foot).ToList();
                refrenciaSeleccionado_Izq_Inf = ListaElemento2[0];
            }
            catch (Exception)
            {
                ListaElemento2 = new List<EnvoltorioElementoIntersectar>();
                return false;
            }
            return true;
        }

        public bool M4_generarReferenciasSelecionadaMO()
        {
            try
            {
                if (ListaElemento1.Count > 0 && ListaElemento2.Count > 0)
                {
                    refrenciaSeleccionado = (ListaElemento1[0].distanciaInterseccion_foot > ListaElemento2[0].distanciaInterseccion_foot ? ListaElemento2[0] : ListaElemento1[0]);
                }
                else if (ListaElemento1.Count > 0)
                {
                    refrenciaSeleccionado = ListaElemento1[0];
                }
                else if (ListaElemento2.Count > 0)
                {
                    refrenciaSeleccionado = ListaElemento2[0];
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al 'BuscarInterseccionesConElemento'. ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public (bool, XYZ, double) IsInterseccionMismaDireccion(EnvoltorioElementoIntersectar env, XYZ PtoBusqueda, XYZ direccionRayoBusqueda)
        {
            try
            {
                XYZ Pto = default;
                bool IsInters = false;

                (IsInters, Pto) = env.LineRuta.IsProject_enplanoXY0(PtoBusqueda);

                if (IsInters)
                {
                    XYZ Direc = (Pto - PtoBusqueda.GetXY0()).Normalize();

                    double resul = Util.GetProductoEscalar(direccionRayoBusqueda, Direc);
                    if (resul > 0.99999)
                    {
                        double espaciamiento_foot = Pto.DistanceTo(PtoBusqueda.GetXY0());

                        if (espaciamiento_foot < LargoMinimo_foot)
                            return (false, null, 0);

                        return (true, Pto.AsignarZ(PtoBusqueda.Z), espaciamiento_foot);
                    }
                }


            }
            catch (Exception ex)
            {

                Util.ErrorMsg($"Error en 'Is Interseccion Misma Direccion'. \n\n ex:{ex.Message}");
            }
            return (false, null, 0);
        }
    }
}
