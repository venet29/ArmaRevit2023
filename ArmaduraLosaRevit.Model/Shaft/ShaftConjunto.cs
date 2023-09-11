using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.Shaft.Entidades;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Shaft
{
   public class ShaftConjunto
    {
        private Document _doc;
        private  Opening _openigSelecionado;
        private  Level LevelShaft;
        private XYZ _ptoMouse;
        private PlanarFace _PLanarFaseInferior;
        private double _coordInf;
        private PlanarFace _PlanarFaseSuperior;
        private double _coordSup;
        private List<ShaftIndividual> ListaCaraSUperior;
        public ShaftIndividual shaftUnicoSeleccoinado { get; set; }

        public ShaftConjunto(Document _doc,SeleccionarOpeningConMouse seleccionarOpeningConMouse,Level LevelShaft)
        {
            this._doc = _doc;
            this._openigSelecionado = seleccionarOpeningConMouse._OpeningSelecciondo;
            this._ptoMouse = seleccionarOpeningConMouse.PtoMOuse;
            ListaCaraSUperior = new List<ShaftIndividual>();
            this.LevelShaft = LevelShaft;
        }
        public ShaftConjunto(Document _doc, Opening _openidn,XYZ ptoSleccon, Level LevelShaft)
        {
            this._doc = _doc;
            this._openigSelecionado = _openidn;
            this._ptoMouse = ptoSleccon;
            ListaCaraSUperior = new List<ShaftIndividual>();
            this.LevelShaft = LevelShaft;
        }
        public void Ejecutar()
        {
            M1_ObtenerCaraInferiorOpening();
            M2_OBtenerBordeOpeningSeleccionado();
            M3_ObtenerShaftIndividual();
        }



        public void M1_ObtenerCaraInferiorOpening()
        {
			#region PASO 3 OPCIONES DE GEOMETRIA
			StringBuilder sbuilder = new StringBuilder();

			Options opt = new Options();
			opt.ComputeReferences = false; //TRUE SI ES QUE QUIERO ACOTAR CON LAS CARAS O BORDES O COLOCAR FAMILIAS FACEBASED
			opt.DetailLevel = ViewDetailLevel.Coarse;
			opt.IncludeNonVisibleObjects = true;//TRUE OBTENIENE LOS OBJETOS INVISIBLES Y/O VACIOS
			#endregion

			GeometryElement geo = _openigSelecionado.get_Geometry(opt);// ESTO ES UNA LISTA DE GeometryObjects
            //ConstantesGenerales.sbLog.Clear();
            //ConstantesGenerales.sbLog.AppendLine(DateTime.Now.ToString("MM_dd_yyyy Hmm"));
            foreach (GeometryObject obj in geo) // 2013
            {
                if (obj is Solid)
                {
                    Solid solid = obj as Solid;

                    foreach (Face face in solid.Faces)
                    {
                        
                        PlanarFace pf = face as PlanarFace;
                        if (pf == null) continue;
  

                        if (SeleccionarPtoDentroPlanarFace.EStaPuntoALInteriroDeCaraDeUnaLosa(_ptoMouse, pf) &&
                            SeleccionarPtoDentroPlanarFace.IsCaraNormalEnEjeZPositivo(pf.FaceNormal))
                        {
                            Debug.Print("Planar face encontraa" + face.GetBoundingBox().ToString());
                            _PlanarFaseSuperior = pf;
                            _coordSup = pf.Origin.Z;

                        }
                        if (SeleccionarPtoDentroPlanarFace.EStaPuntoALInteriroDeCaraDeUnaLosa(_ptoMouse, pf) &&
                           SeleccionarPtoDentroPlanarFace.IsCaraNormalEnEjeZNegativo(pf.FaceNormal))
                        {
                            Debug.Print("Planar face encontraa" + face.GetBoundingBox().ToString());
                            _PLanarFaseInferior = pf;
                            _coordInf = pf.Origin.Z;
                            // ListaCaraSUperior.Add(pf);

                        }
                    }
                }
             

            }

          //  LogNH.guardar_registro_StringBuilder(ConstantesGenerales.sbLog, ConstantesGenerales.rutaLogNh,"PLanarshaft");
        }

        public bool M2_OBtenerBordeOpeningSeleccionado()
        {
            if ((_PlanarFaseSuperior == null) || (_PlanarFaseSuperior == null))
            { Util.ErrorMsg("Una de las caras superio o inferior del Opening es NULL"); return false; }

            ListaCaraSUperior = _PlanarFaseSuperior.GetEdgesAsCurveLoops().Select(c => new ShaftIndividual(_doc,c, LevelShaft)).ToList();
            return true;
        }

        public bool M3_ObtenerShaftIndividual()
        {
            shaftUnicoSeleccoinado = ListaCaraSUperior.Where(shi => shi.IsPtoDentroShaf(_ptoMouse)).DefaultIfEmpty(new ShaftIndividual()).FirstOrDefault();
            return true;
        }

    }
}
