using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.Seleccionar;
using ArmaduraLosaRevit.Model.UTILES;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Shaft.Buscar
{
    class BuscarBordeMAScercanaViewDTO
    {
        public XYZ PtoiNTER { get; set; }
        public Line Linea { get; set; }
        public double Distancia { get; internal set; }
    }
    class BuscarBordeMAScercanaView
    {
        private readonly UIApplication _uiapp;
        private readonly Opening _openigSelecionado;
        private readonly Document _doc;
        private View _view;
        private List<PlanarFace> _listaPlanarFace;

        public BuscarBordeMAScercanaView(UIApplication _uiapp, Opening _openigSelecionado)
        {
            this._uiapp = _uiapp;
            this._openigSelecionado = _openigSelecionado;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this._view = _uiapp.ActiveUIDocument.Document.ActiveView;
        }

        public XYZ Direccion { get; private set; }
        public Curve CurvaBorde { get; private set; }

        public void M1_ObtenerCaraInferiorOpening(XYZ _ptoMouse)
        {


            _listaPlanarFace = new List<PlanarFace>();

            #region PASO 3 OPCIONES DE GEOMETRIA
            StringBuilder sbuilder = new StringBuilder();

            Options opt = new Options();
            opt.ComputeReferences = false; //TRUE SI ES QUE QUIERO ACOTAR CON LAS CARAS O BORDES O COLOCAR FAMILIAS FACEBASED
            opt.DetailLevel = ViewDetailLevel.Coarse;
            opt.IncludeNonVisibleObjects = true;//TRUE OBTENIENE LOS OBJETOS INVISIBLES Y/O VACIOS
            #endregion

            GeometryElement geo = _openigSelecionado.get_Geometry(opt);// ESTO ES UNA LISTA DE GeometryObjects


            XYZ vectorBusqueda = -_view.ViewDirection;
            foreach (GeometryObject obj in geo) // 2013
            {
                if (obj is Solid)
                {
                    Solid solid = obj as Solid;
                    foreach (Face face in solid.Faces)
                    {

                        PlanarFace pf = face as PlanarFace;
                        if (pf == null) continue;

                        if (SeleccionarPtoDentroPlanarFace.EStaPuntoALInteriroDeCaraDeUnaLosa(vectorBusqueda, pf))
                        {
                            Debug.Print("Planar face encontraa" + face.GetBoundingBox().ToString());
                            _listaPlanarFace.Add(pf);

                        }
                    }
                }
            }
            //  LogNH.guardar_registro_StringBuilder(ConstantesGenerales.sbLog, ConstantesGenerales.rutaLogNh,"PLanarshaft");
        }


        public bool M2_ObtenerBordeConcurvas(XYZ _ptoMouse)
        {
            var ListaLineas = new List<BuscarBordeMAScercanaViewDTO>();
            CurveArray listaCUrvas = new CurveArray();
            if (_openigSelecionado.Category.Name == "Rectangular Straight Wall Opening")
            {
                var muro = _openigSelecionado.Host;
                (bool reult, PlanarFace caravisible) = muro.ObtenerCaraVerticalVIsible(_view);
                if (!reult)
                {
                    Util.ErrorMsg($"Erro al obtener 'M2_ObtenerBordeConcurvas'");
                    return false;
                }

                var ptoCentralCara = caravisible.ObtenerCenterDeCara();
                // p4   p3
                // p1   p2
                var ListaPto = _openigSelecionado.BoundaryRect.ToList();

                var pt1 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano(_view.ViewDirection, ptoCentralCara, ListaPto[0]);
                var pt3 = ProyectadoEnPlano.ObtenerPtoProyectadoEnPlano(_view.ViewDirection, ptoCentralCara, ListaPto[1]);

                var pt2 = new XYZ(pt3.X, pt3.Y, pt1.Z);
                var pt4 = new XYZ(pt1.X, pt1.Y, pt3.Z);

                listaCUrvas.Append(Line.CreateBound(pt1, pt2));
                listaCUrvas.Append(Line.CreateBound(pt2, pt3));
                listaCUrvas.Append(Line.CreateBound(pt3, pt4));
                listaCUrvas.Append(Line.CreateBound(pt4, pt1));
            }
            else
                listaCUrvas = _openigSelecionado.BoundaryCurves;

            foreach (Curve item in listaCUrvas)
            {
                if (item.GetType().Name == "Arc") continue;
                XYZ ptoExtension = ((Line)item).ProjectSINExtendida3D(_ptoMouse);
                ListaLineas.Add(new BuscarBordeMAScercanaViewDTO() { Linea = (Line)item, PtoiNTER = ptoExtension, Distancia = _ptoMouse.DistanceTo(ptoExtension) });
            }
            var resul = ListaLineas.OrderBy(c => c.Distancia).FirstOrDefault();

            if (resul == null) return false;

            Direccion = resul.Linea.Direction;
            CurvaBorde = resul.Linea;
            return true;

        }




    }
}
