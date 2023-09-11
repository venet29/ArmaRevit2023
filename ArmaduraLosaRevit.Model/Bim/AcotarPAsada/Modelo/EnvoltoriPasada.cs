using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Pasadas.Model;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo
{
    public class EnvoltoriPasada
    {
        private readonly UIApplication _uiapp;
        private object uIapp;
        private EnumTipoPAsada rectangular;

        public Element _pasada { get; private set; }

        public ParBordeParalelo parBordeParalelo1 { get; private set; }
        public PlanarFace FAceCaraNormalSup { get; private set; }
        public List<XYZ> ListaPtosFaceCaraNormalSup { get; private set; }
        public EnumPasadasConGrilla DimesionVerticalDireccion  { get; set; }

        public EnumPasadasConGrilla DimesionHorizonalDireccion { get; set; }
        

        public bool _IsOk { get; set; }
        public List<PlanarFace> listaPLanos_Geometria { get; set; }
        internal List<EnvoltoriosPlanos> ListaPLanosPAsadas { get; private set; }
        public XYZ PtoCentralCaraSuperior { get; private set; }

        #region 3 propiedad para buscar automaticamente la mejor posisicion de las dimensiones, de modo que no se traslapen

        public XYZ PtoCentralCaraSuperior_trasformada { get; set; }
        public double Distancia_trasformada_foot { get; set; } // distancia desde la pasada analizada a la pazada actaul
        public double AnguloRespectoPasadaAnalizada_grado { get; set; } // angulo desde la pasada analizada a la pazada actaul
        public XYZ DireccionFAceNormalCAraDerecha { get; private set; }
        public XYZ DireccionFAceNormalInferior { get; private set; }


        #endregion
        public EnvoltoriPasada(UIApplication uiapp, Element pasada)
        {
            this._uiapp = uiapp;
            this._pasada = pasada;
            this._IsOk = false;
            ListaPLanosPAsadas = new List<EnvoltoriosPlanos>();
            listaPLanos_Geometria = new List<PlanarFace>();
        }

        public EnvoltoriPasada(object uIapp)
        {
            this.uIapp = uIapp;
            ListaPLanosPAsadas = new List<EnvoltoriosPlanos>();
            listaPLanos_Geometria = new List<PlanarFace>();
        }

        public bool ObtenerInfo_PLanos()
        {
            try
            {
                if (_pasada.IsValidObject)
                {
                    List<PlanarFace> listaPLanos_paraReferencia = new List<PlanarFace>();
                    var listaTotal = _pasada.ListaFace_Conferencias(true)[0];
                    rectangular = EnumTipoPAsada.NONE;

                    if (listaTotal.Count == 2) //circular
                    {
                        listaPLanos_paraReferencia = listaTotal.Where(c => Math.Abs(c.FaceNormal.Z) > 0.1).ToList();

                        if (listaPLanos_paraReferencia == null) return false;

                        if (listaPLanos_paraReferencia.Count != 2)
                        {
                            Util.ErrorMsg($"Pasadas con id:{_pasada.Id} contiene caras verticales distintas de 4. Numero caras verticales:{listaPLanos_paraReferencia.Count}");
                            return false;
                        }

                        listaPLanos_Geometria = _pasada.ListaFace(true)[0].Where(c => Math.Abs(c.FaceNormal.Z) > 0.1).ToList();

                        if (listaPLanos_Geometria == null) return false;

                        if (listaPLanos_Geometria.Count != 2)
                        {
                            Util.ErrorMsg($"Pasadas con id:{_pasada.Id} contiene caras verticales distintas de 4. Numero caras verticales:{listaPLanos_Geometria.Count}");
                            return false;
                        }

                        rectangular = EnumTipoPAsada.Circular;

                        return false;
                    }
                    else if (listaTotal.Count == 6)//rectagular
                    {
                        listaPLanos_paraReferencia = listaTotal.Where(c => Math.Abs(c.FaceNormal.Z) < 0.1).ToList();

                        if (listaPLanos_paraReferencia == null) return false;

                        if (listaPLanos_paraReferencia.Count != 4)
                        {
                            Util.ErrorMsg($"Pasadas con id:{_pasada.Id} contiene caras verticales distintas de 4. Numero caras verticales:{listaPLanos_paraReferencia.Count}");
                            return false;
                        }

                        if (!ObtenerPuntoCentralCaraSUperio()) return false;

                        if(! Obtener4CarasLaterales()) return false;

                        if (!ObtenerDireccionCaraDere()) return false;

                        if (!ObtenerDireccionCaraInferior()) return false;                       
                    }
                    else
                    {
                        ListaPLanosPAsadas = new List<EnvoltoriosPlanos> ();
                        return false;
                    }

                    parBordeParalelo1 = new ParBordeParalelo(_uiapp.ActiveUIDocument.ActiveView, listaPLanos_paraReferencia, listaPLanos_Geometria, rectangular);

                    var listaPLanosCOnGEom= _pasada.ListaFace()[0];
                    FAceCaraNormalSup = listaPLanosCOnGEom.Where(c => c.FaceNormal.Z > 0.1).OrderByDescending(c=>c.FaceNormal.Z>0).FirstOrDefault();
                    if (FAceCaraNormalSup == null)
                        Util.ErrorMsg("Error al obtener cara superior de pasada. Valor null");
                    else
                        ListaPtosFaceCaraNormalSup= FAceCaraNormalSup.ObtenerListaPuntos();

                    
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obener datos pasada:{_pasada.Id}\n\n ex:{ex.Message}");
                return false;
            }
            return true;
        }


        private bool ObtenerPuntoCentralCaraSUperio()
        {
            var caraSuperior = _pasada.ListaFace(true)[0].Where(c => Math.Abs(c.FaceNormal.Z) > 0.3).FirstOrDefault();
            if (caraSuperior != null)
            {
                PtoCentralCaraSuperior = caraSuperior.ObtenerCenterDeCara();
            }
            else
            {
                Util.ErrorMsg($"No se puede encontrar cara superior ");
                return false;
            }
            return true;
        }

        private bool Obtener4CarasLaterales()
        {
            listaPLanos_Geometria = _pasada.ListaFace(true)[0].Where(c => Math.Abs(c.FaceNormal.Z) < 0.1).ToList();

            if (listaPLanos_Geometria == null) return false;

            if (listaPLanos_Geometria.Count != 4)
            {
                Util.ErrorMsg($"Pasadas con id:{_pasada.Id} contiene caras verticales distintas de 4. Numero caras verticales:{listaPLanos_Geometria.Count}");
                return false;
            }

            ListaPLanosPAsadas = listaPLanos_Geometria.Select(c => new EnvoltoriosPlanos(_uiapp, c)).ToList();
            ListaPLanosPAsadas.ForEach(c => c.ObtenerPtoInicialFinal());

            rectangular = EnumTipoPAsada.Rectangular;
            _IsOk = true;
            return true;
        }
        private bool ObtenerDireccionCaraDere()
        {
            var caraMasDerecha = listaPLanos_Geometria.OrderByDescending(c=> c.FaceNormal.X).FirstOrDefault()  ;

            if (caraMasDerecha == null) return false;

            DireccionFAceNormalCAraDerecha = caraMasDerecha.FaceNormal.GetXY0();
            return true;

        }

        private bool ObtenerDireccionCaraInferior()
        {
            var caraMasInferior = listaPLanos_Geometria.OrderBy(c => c.FaceNormal.Y).FirstOrDefault();

            if (caraMasInferior== null) return false;

            DireccionFAceNormalInferior = caraMasInferior.FaceNormal.GetXY0();
            return true;

        }
    }
}
