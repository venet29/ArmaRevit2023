using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Bim.AcotarPAsada.Modelo
{


    //ver obs1
    public class ParBordeParalelo
    {
        private Curve _curve_Analizada;


        private  View _view;
        private  List<PlanarFace> listaPLanos_Geometria;

        public bool Isok { get; set; }
        public List<PlanarFace> ListaPLAnar_paraReferencias { get; }
        public EnumTipoPAsada TipoPasada { get; }
        public XYZ Pt1 { get; set; }
        public XYZ Pt2 { get; set; }

        public XYZ Pt_leader { get; set; }

        public PlanarFace face_analizada { get; set; }
        public PlanarFace face_referencia_IzqInf { get; set; }

        public PlanarFace face_referencia_DereSup { get; set; }
        public XYZ Pt_origin { get; set; }
        public XYZ Pt_texto { get; set; }


        public ParBordeParalelo()
        {
            Isok = false;
        }

        public ParBordeParalelo(View _view, List<PlanarFace> listaPLAnar_paraReferencias, List<PlanarFace> listaPLanos_Geometria, EnumTipoPAsada rectangular)
        {
            Isok = true;
            this._view = _view;
            ListaPLAnar_paraReferencias = listaPLAnar_paraReferencias;
            this.listaPLanos_Geometria = listaPLanos_Geometria;
            this.TipoPasada = rectangular;
        }

        public bool M1_ObtenerHaciaIzq()
        {
            try
            {
                if (!Isok) return false;
                //primero apuntando a la izq
                var list_IZq = listaPLanos_Geometria.OrderBy(c => c.FaceNormal.X).ToList();
                if (list_IZq.Count == 0) return false;
                face_analizada = list_IZq[0];

                if (!MetodosparaNormales_X()) return false;
              
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener coordenadas a la Izquierda. ex:{ex.Message}");
                return false;
            }
            return true;
        }


        //**

        public bool M2_ObtenerHaciaDere()
        {
            try
            {
                if (!Isok) return false;
                //primero apuntando a la izq
                var list_IZq = listaPLanos_Geometria.OrderByDescending(c => c.FaceNormal.X).ToList();
                if (list_IZq.Count == 0) return false;
                face_analizada = list_IZq[0];

                if (!MetodosparaNormales_X()) return false;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener coordenadas a la Izquierda. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool M3_ObtenerHaciaInferior()
        {
            try
            {
                if (!Isok) return false;
                //primero apuntando a la izq
                var list_Inferior = listaPLanos_Geometria.OrderBy(c => c.FaceNormal.Y).ToList();
                if (list_Inferior.Count == 0) return false;
                face_analizada = list_Inferior[0];

                if (!MetodosparaNormales_Y()) return false;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener coordenadas a la Izquierda. ex:{ex.Message}");
                return false;
            }
            return true;
        }


        //

        public bool M4_ObtenerHaciaSup()
        {
            try
            {
                if (!Isok) return false;
                //primero apuntando a la izq
                var list_Superior = listaPLanos_Geometria.OrderByDescending(c => c.FaceNormal.Y).ToList();
                if (list_Superior.Count == 0) return false;
                face_analizada = list_Superior[0];

                if (!MetodosparaNormales_Y()) return false;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener coordenadas a la Izquierda. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public bool M5_ObtenerRadial()
        {
            try
            {
                if (!Isok) return false;
                //primero apuntando a la izq
                var list_Superior = listaPLanos_Geometria.OrderByDescending(c => c.FaceNormal.Z).ToList();
                if (list_Superior.Count == 0) return false;
                face_analizada = list_Superior[0];

                if (face_analizada==null) return false;

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al obtener coordenadas a la Izquierda. ex:{ex.Message}");
                return false;
            }
            return true;
        }



        private bool MetodosparaNormales_X()
        {
            try
            {
                // ref1 apunta bajo
                var list_bajo = ListaPLAnar_paraReferencias.OrderBy(c => c.FaceNormal.Y).ToList();
                if (list_bajo.Count == 0) return false;
                face_referencia_IzqInf = list_bajo[0];

                // ref1 apunta arriba
                var list_arriba = ListaPLAnar_paraReferencias.OrderByDescending(c => c.FaceNormal.Y).ToList();
                if (list_arriba.Count == 0) return false;
                face_referencia_DereSup = list_arriba[0];

                if (!calculosGEnerales()) return false;

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private bool MetodosparaNormales_Y()
        {
            try
            {
                // ref1 apunta bajo
                var list_izq = ListaPLAnar_paraReferencias.OrderBy(c => c.FaceNormal.X).ToList();
                if (list_izq.Count == 0) return false;
                face_referencia_IzqInf = list_izq[0];

                // ref1 apunta arriba
                var list_dere = ListaPLAnar_paraReferencias.OrderByDescending(c => c.FaceNormal.X).ToList();
                if (list_dere.Count == 0) return false;
                face_referencia_DereSup = list_dere[0];

                if(! calculosGEnerales()) return false;

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private bool calculosGEnerales()
        {
            /// anailzando face principal
            var listaCUrvas = ((Face)face_analizada).ObtenerListaCurvas();
            if (listaCUrvas.Count == 0) return false;

            _curve_Analizada = listaCUrvas.OrderBy(c => Math.Abs(((Line)c).Direction.Z)).FirstOrDefault();

            if (_curve_Analizada == null) return false;

            Pt1 = _curve_Analizada.GetEndPoint(0);
            Pt2 = _curve_Analizada.GetEndPoint(1);

            Pt1 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano(_view.ViewDirection, _view.Origin, Pt1);
            Pt2 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano(_view.ViewDirection, _view.Origin, Pt2);

            // var ptoMedio = (Pt1 + Pt2) / 2;
            //Pt_leader = (0.73 + 2) * face_analizada.FaceNormal + ptoMedio;
            // Pt_origin = (1.44 + 2) * face_analizada.FaceNormal + ptoMedio;
            // Pt_texto = (1.01 + 2) * face_analizada.FaceNormal + ptoMedio;

            return true;
        }


    }
}
