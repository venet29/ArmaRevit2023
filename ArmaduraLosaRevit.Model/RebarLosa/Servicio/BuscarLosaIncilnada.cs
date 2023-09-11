using ArmaduraLosaRevit.Model.Elemento_Losa.Ayuda;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.ObtenerZLosa;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using ArmaduraLosaRevit.Model.RebarLosa.DTO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Vector3D = System.Windows.Media.Media3D;

namespace ArmaduraLosaRevit.Model.RebarLosa.Barras.Servicio
{
    public class BuscarLosaIncilnada : AObtenerZPara4ptosPath
    {


        public XYZ normal { get; set; }
        public double angleRAd { get; set; }
        public double angleGRADO { get; set; }
        public bool NormalEncontrada { get; set; }
        public XYZ VectorDireccionLosaExternaInclinada { get; private set; }
        public VectorDireccionLosaExternaInclinadaDTO _vectorDireccionLosaExternaInclinadaDTO { get; set; }
    

        public BuscarLosaIncilnada(UIApplication uiapp, List<XYZ> listaPtos,Floor _floor,  double rangoBusqedaCm) : base(uiapp, listaPtos, _floor, rangoBusqedaCm)
        {
            losaActual = _floor;
        }

        //1(0)   4(3)
        //2(1)   3(2)
        //busca del punto 2 hacia la dere
        public bool obtenerPendienteLosaContiguaFinal(XYZ VectroBUsqueda) => obtenerPendienteLosaContigua(VectroBUsqueda, 2);
        public bool obtenerPendienteLosaContiguaFinal(XYZ VectroBUsqueda, XYZ ptoSleccionMOuse) => obtenerPendienteLosaContigua(VectroBUsqueda, 2, ptoSleccionMOuse);
        //busca del punto 1 hacia la izq
        public bool obtenerPendienteLosaContiguaInicia(XYZ VectroBUsqueda) => obtenerPendienteLosaContigua(VectroBUsqueda, 1);
        public bool obtenerPendienteLosaContiguaInicia(XYZ VectroBUsqueda, XYZ ptoSleccionMOuse) => obtenerPendienteLosaContigua(VectroBUsqueda, 1, ptoSleccionMOuse);
        public bool obtenerPendienteLosaContigua(XYZ VectroBUsqueda, int PtoReferencia , XYZ ptoSleccionMOuse=null)
        {
            NormalEncontrada = false;

            if (!OBtener3D()) return false;

            XYZ ptoMovidoi1cmExtLosa = default;

            ConstNH.corte();// dejar solo :       ptoMovidoi1cmExtLosa = ObtenerPtoAnalizado1cmExteriroRoom(PtoReferencia);
            if (ptoSleccionMOuse==null)
                ptoMovidoi1cmExtLosa = ObtenerPtoAnalizado1cmExteriroRoom(PtoReferencia);
            else
                ptoMovidoi1cmExtLosa = ptoSleccionMOuse;


            Floor floorIntersectadaExternaRoom = OBtenerRefrenciaLosa(_elem3d, ptoMovidoi1cmExtLosa + new XYZ(0, 0, Util.CmToFoot(2)), VectroBUsqueda, false);
            if (floorIntersectadaExternaRoom == null)
            {
                //Util.ErrorMsg("No se encontro losa inclinada");
                Debug.WriteLine("No se encontro losa inclinada");
                return false;
            }
            //if (!BuscaNormalPlanarFaceNOhorizontal(floorIntersectadaExternaRoom)) return false;
            if (!BuscaNormalPlanarFaceTop(floorIntersectadaExternaRoom)) return false;


            ObtenerVectorDIreccionLosaYAngulo((PtoReferencia == 2
                                                            ? _listaPtos[3] - _listaPtos[2]
                                                            : _listaPtos[1] - _listaPtos[0]));

            _vectorDireccionLosaExternaInclinadaDTO = new VectorDireccionLosaExternaInclinadaDTO()
            {
                direccionLosa = VectorDireccionLosaExternaInclinada,
                Losa = floorIntersectadaExternaRoom,
                PosicionDeBusquedaEnu = (PtoReferencia == 2 ? PosicionDeBusqueda.Fin : PosicionDeBusqueda.Inicio),
                IsLosaEncontrada = true
            };


            return NormalEncontrada;
        }



        internal VectorDireccionLosaExternaInclinadaDTO ObtenerVectorDireccionLosaExternaInclinadaDTO(Element floor)
        {
            if (_vectorDireccionLosaExternaInclinadaDTO.Losa.Id.IntegerValue == floor.Id.IntegerValue)
                return new VectorDireccionLosaExternaInclinadaDTO() { IsLosaEncontrada = false };
            else if(IsLosasConIgualDireccion(_vectorDireccionLosaExternaInclinadaDTO.Losa, (Floor)floor))
                return new VectorDireccionLosaExternaInclinadaDTO() { IsLosaEncontrada = false };
            else
                return _vectorDireccionLosaExternaInclinadaDTO;
        }

        private bool IsLosasConIgualDireccion(Floor losa_Encontrada, Floor losa_seleccionado)
        {
            try
            {
                //a)
                XYZ face_losa_Encontrada_Nomal = losa_Encontrada.ObtenerNormal();
                //b)
                XYZ face_losa_seleccionado_Nomal = losa_seleccionado.ObtenerNormal();

                if (Util.IsParallel(face_losa_Encontrada_Nomal, face_losa_seleccionado_Nomal))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex:{ex.Message}");
                return false;
            }
   
        }

        // obtiene la cara planer face contor en normal z positivo
        private bool BuscaNormalPlanarFaceTop(Floor floorIntersectada)
        {
            if (floorIntersectada != null)
            {
               // PlanarFace face_ =  floorIntersectada.ObtenerCaraSuperiorElem();
                XYZ face_Nomal = floorIntersectada.ObtenerNormal();

                if (Util.PointsUpwards(face_Nomal))
                {
                    normal = face_Nomal;
                    NormalEncontrada = true;
                    return true;
                }

            }
            return false;
        }
        private void ObtenerVectorDIreccionLosaYAngulo(XYZ vect)
        {
            // XYZ vect = _listaPtos[3] - _listaPtos[2];
            VectorDireccionLosaExternaInclinada = Util.CrossProduct(vect.Normalize(), normal).Normalize();
            angleRAd = VectorDireccionLosaExternaInclinada.GetAngleEnZ_respectoPlanoXY(false);
            angleGRADO = VectorDireccionLosaExternaInclinada.GetAngleEnZ_respectoPlanoXY(true);

        }



        private bool OBtener3D()
        {

            _elem3d = TiposFamilia3D.Get3DBuscar(_doc);

            if (_elem3d == null)
            {
                Util.ErrorMsg("Error: no se encontro 3D_NoEditar. Vuelva a cargar parametros inicales");

                return true;
            }
            return true;
        }
    }
}
