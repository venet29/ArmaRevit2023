using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag.Tipos
{
    public class GeomeTagF11A : GeomeTagBase, IGeometriaTag
    {

        public GeomeTagF11A(Document doc, XYZ ptoMOuse, List<XYZ> listaPtosPerimetroBarras, SolicitudBarraDTO _solicitudBarraDTO) :
            base(doc, ptoMOuse, listaPtosPerimetroBarras, _solicitudBarraDTO)
        { }

        public GeomeTagF11A() { }

        public bool Ejecutar(GeomeTagArgs args)
        {
            try
            {
                AnguloRadian = args.angulorad;
                if (!M1_ObtnerPtosInicialYFinalDeBarra(AnguloRadian)) return false;
                M2_CAlcularPtosDeTAg();
                M3_DefinirRebarShape();
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error ejecutar TagF11A  ex:${ex.Message}");
                return false;
            }
            return true;

        }

        public void M3_DefinirRebarShape()
        {


            AsignarPArametros(this);


            if (_ubicacionEnlosa == UbicacionLosa.Inferior && _anguloBArraGrado > 91)
            {
                 AgregarTagPathreinLitsta("A", -12, -10, _p1);
            }
            else if ( _ubicacionEnlosa == UbicacionLosa.Superior && _anguloBArraGrado > 91)
            {
               AgregarTagPathreinLitsta("A", 12, -10, _p2);
            }

        }


        public bool M4_IsFAmiliaValida() => true;
        public void M5_DefinirRebarShapeAhorro(Action<GeomeTagF11A> rutina)
        {
            rutina(this);
        }

        public void AsignarPArametros(GeomeTagBase _geomeTagBase)
        {


            _geomeTagBase.TagP0_A.IsOk = false;
            _geomeTagBase.TagP0_E.IsOk = false;

            if (_geomeTagBase._ubicacionEnlosa == UbicacionLosa.Inferior || _geomeTagBase._ubicacionEnlosa == UbicacionLosa.Izquierda)
            {
                // _geomeTagBase.TagP0_D.CAmbiar(_geomeTagBase.TagP0_C);
                _geomeTagBase.TagP0_D.IsOk = false;
                _geomeTagBase.TagP0_C.CAmbiar(_geomeTagBase.TagP0_B);
                _geomeTagBase.TagP0_B.CAmbiar(_geomeTagBase.TagP0_A);
            }
            else
            {
                _geomeTagBase.TagP0_D.CAmbiar(_geomeTagBase.TagP0_A);
                _geomeTagBase.TagP0_C.CAmbiar(_geomeTagBase.TagP0_B);
                //_geomeTagBase.TagP0_B.CAmbiar(_geomeTagBase.TagP0_A);
                _geomeTagBase.TagP0_B.IsOk = false;

            }


        }
    }


}
