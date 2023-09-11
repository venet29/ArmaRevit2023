
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System.Diagnostics;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Tag
{
    [TransactionAttribute(TransactionMode.Manual)]

    public class Cmd_GeometriaTagTest : IExternalCommand

    {

        public Result Execute(ExternalCommandData commandData,ref string messages, ElementSet elements)
        {



            UIApplication app = commandData.Application;
            Document _doc = app.ActiveUIDocument.Document;
            Level _Level = _doc.ActiveView.GenLevel;

            XYZ ptosMOuse = new XYZ(-46.405655737, 196.561067979, 26.541994751);
            List<XYZ> listaPtosBorde = new List<XYZ>();
            listaPtosBorde.Add(new XYZ(- 54.166666667, 201.464566929, 26.541994751));
            listaPtosBorde.Add(new XYZ(-54.166666667, 192.015748032, 26.541994751));
            listaPtosBorde.Add(new XYZ(-37.483595801, 192.015748032, 26.541994751));
            listaPtosBorde.Add(new XYZ(-37.483595801, 201.464566929, 26.541994751));

            SolicitudBarraDTO _solicitudBarraDTO = new SolicitudBarraDTO(commandData.Application, "F4", UbicacionLosa.Derecha, TipoConfiguracionBarra.refuerzoInferior, false);


            IGeometriaTag _geometriaTag = FactoryGeomTag.CrearGeometriaTag(_doc, ptosMOuse, _solicitudBarraDTO, listaPtosBorde);
          
            Debug.Assert(_geometriaTag != null);
            _geometriaTag.M1_ObtnerPtosInicialYFinalDeBarra(0);
            _geometriaTag.M2_CAlcularPtosDeTAg();


            Debug.Assert(_geometriaTag.listaTag.Count == 7);


            Debug.Assert(_geometriaTag.listaTag[0].nombreFamilia == "M_Path Reinforcement Tag(ID_cuantia_largo)_A_");
            Debug.Assert(_geometriaTag.listaTag[0].posicion.DistanceTo(new XYZ(-52.034120735, 197.151619160, 26.541994751))<0.1);

            Debug.Assert(_geometriaTag.listaTag[1].nombreFamilia == "M_Path Reinforcement Tag(ID_cuantia_largo)_B_");
            Debug.Assert(_geometriaTag.listaTag[1].posicion.DistanceTo(new XYZ(-54.166666667, 196.561067979, 26.541994751)) < 0.1);
            
            Debug.Assert(_geometriaTag.listaTag[2].nombreFamilia == "M_Path Reinforcement Tag(ID_cuantia_largo)_C_");
            Debug.Assert(_geometriaTag.listaTag[2].posicion.DistanceTo(new XYZ(-47.882033690, 196.265792388, 26.541994751)) < 0.1);

            Debug.Assert(_geometriaTag.listaTag[3].nombreFamilia == "M_Path Reinforcement Tag(ID_cuantia_largo)_F_");
            Debug.Assert(_geometriaTag.listaTag[3].posicion.DistanceTo(new XYZ(-48.046075685, 196.790726772, 26.541994751)) < 0.1);

            Debug.Assert(_geometriaTag.listaTag[4].nombreFamilia == "M_Path Reinforcement Tag(ID_cuantia_largo)_L_");
            Debug.Assert(_geometriaTag.listaTag[4].posicion.DistanceTo(new XYZ(-44.765235789, 196.823535171, 26.541994751)) < 0.1);

            Debug.Assert(_geometriaTag.listaTag[5].nombreFamilia == "M_Path Reinforcement Tag(ID_cuantia_largo)_D_");
            Debug.Assert(_geometriaTag.listaTag[5].posicion.DistanceTo(new XYZ(-36.663385827, 196.561067979, 26.541994751)) < 0.1);

            Debug.Assert(_geometriaTag.listaTag[6].nombreFamilia == "M_Path Reinforcement Tag(ID_cuantia_largo)_E_");
            Debug.Assert(_geometriaTag.listaTag[6].posicion.DistanceTo(new XYZ(-38.631889764, 197.151619160, 26.541994751)) < 0.1);
            //Iguañ a 16  == listaDeRoomEnLosa.count

            return Result.Succeeded;

        }

    }


}
