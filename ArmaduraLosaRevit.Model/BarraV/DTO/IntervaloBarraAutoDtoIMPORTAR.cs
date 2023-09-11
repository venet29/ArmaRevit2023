

using ArmaduraLosaRevit.Model.BarraAreaPath.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraEstriboP.DTO;
using ArmaduraLosaRevit.Model.BarraV.Automatico;
using ArmaduraLosaRevit.Model.BarraV.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;


namespace ArmaduraLosaRevit.Model.BarraV.DTO
{
    public class IntervalosBarraAutoDtoIMPORTAR
    {
        public XYZnh PtoBordeMuro { get; set; }
        public XYZnh PtoCentralSobreMuro { get; set; }
        public string Pier { get; set; }
        public string Story { get; set; }
        public int Linea { get; set; }

        public int Inicial_diametroMM { get; set; }
        public int Inicial_Cantidadbarra { get; set; }

        public List<CoordenadasBarraIMPORTAR> ListaCoordenadasBarra { get; set; }

        public TipoElementoElevancion Tipo { get; set; }


        public DatosBarraAutoDTO _datosBarraAutoDTO { get; set; }
        public DatosMallasAutoDTO _datosMallasDTO { get; set; }
        public DatosConfinamientoAutoDTO _datosConfinamientoDTO { get; set; }
        public DatosEstriboAutoDTO _datosEstriboDTO { get; set; }


        public double EspaciamientoREspectoBordeFoot { get; set; }
        public Orientacion orientacion { get; set; }
        public Orientacion OrientacionTagGrupoBarras { get; set; }
        public UbicacionEnPier ubicacionEnPier { get; set; }


        public IntervalosBarraAutoDtoIMPORTAR()
        {
 
        }

        public IntervalosBarraAutoDto M1_ObtenerIntervalosBarraAutoDto(ArmaduraTrasformada _armaduraTrasformada, Autodesk.Revit.DB.Document _doc)
        {
            var projectBasePointPositiov2n = BasePoint.GetProjectBasePoint(_doc);
            var surveyPointPositionv2 = BasePoint.GetSurveyPoint(_doc);
            XYZ SurveyPoint = XYZ.Zero;//  projectBasePointPositiov2n.Position;

            //llenar List<CoordenadasBarra>
            List<CoordenadasBarra> listaCoordenadasBarra = new List<CoordenadasBarra>();
            foreach (CoordenadasBarraIMPORTAR item in ListaCoordenadasBarra)
            {
                listaCoordenadasBarra.Add(item.ObtenerCoordenadasBarra(_armaduraTrasformada, Util.FootToCm(SurveyPoint.Z)));
            }

            //crear  IntervalosBarraAutoDto
            return new IntervalosBarraAutoDto()
            {
                PtoBordeMuro = _armaduraTrasformada.Ejecutar(PtoBordeMuro.GetXYZ_cmTofoot()- XYZ.BasisZ* SurveyPoint.Z),
                PtoCentralSobreMuro = _armaduraTrasformada.Ejecutar(PtoCentralSobreMuro.GetXYZ_cmTofoot() -XYZ.BasisZ * SurveyPoint.Z),
                Pier =Pier,
                Story= Story,
                orientacion = orientacion,
                OrientacionTagGrupoBarras = OrientacionTagGrupoBarras,
                ubicacionEnPier = ubicacionEnPier,
                Inicial_diametroMM = Inicial_diametroMM,
                Inicial_Cantidadbarra = Inicial_Cantidadbarra,
                ListaCoordenadasBarra = listaCoordenadasBarra,
                Linea= Linea,
                x_referencia_ordenar= Math.Round(ListaCoordenadasBarra[0].ptoIni.X, 3),
                z_referencia_ordenar = Math.Round(ListaCoordenadasBarra[0].ptoIni.Z, 3)
            };
        }
        public IntervalosMallaDTOAuto M2_ObtenerIntervalosMallaAutoDto(ArmaduraTrasformada _armaduraTrasformada, Autodesk.Revit.DB.Document _doc)
        {
            var projectBasePointPositiov2n = BasePoint.GetProjectBasePoint(_doc);
            var surveyPointPositionv2 = BasePoint.GetSurveyPoint(_doc);
            XYZ SurveyPoint = XYZ.Zero;//  projectBasePointPositiov2n.Position;

            CoordenadasBarra _coordenadasBarra = ListaCoordenadasBarra[0].ObtenerCoordenadasBarra(_armaduraTrasformada, Util.FootToCm(SurveyPoint.Z));
           
            List<XYZ> _ListaPtos = new List<XYZ>();
            _ListaPtos.Add(_coordenadasBarra.ptoIni_foot);
            _ListaPtos.Add(_coordenadasBarra.ptoFin_foot);

            List<XYZ> _ListaPtos_mallaVertical = new List<XYZ>();
            _ListaPtos_mallaVertical.Add(_coordenadasBarra.ptoIni_MallaVertical);
            _ListaPtos_mallaVertical.Add(_coordenadasBarra.ptoFin_MallaVertical);


            _datosMallasDTO.IsBuscarCororonacion = _coordenadasBarra.IsBuscarCororonacion;
            return new IntervalosMallaDTOAuto()
            {
                Pier = Pier,
                Story = Story,               
                _datosMallasDTO = _datosMallasDTO,
                ListaPtos = _ListaPtos,
                ListaPtos_mallaVertical = _ListaPtos_mallaVertical,
                //IsBuscarCororonacion = _coordenadasBarra.IsBuscarCororonacion,
                IsOk = true

            };
        }
        public IntervalosConfinaDTOAuto M3_ObtenerIntervalosEstriboAutoDto(ArmaduraTrasformada _armaduraTrasformada, Autodesk.Revit.DB.Document _doc)
        {
            //llenar List<CoordenadasBarra>

            return M4_ObtenerIntervalosConfinaminetoAutoDto(_armaduraTrasformada, _doc,TipoEstriboGenera.EMuro);
        }
        public IntervalosConfinaDTOAuto M4_ObtenerIntervalosConfinaminetoAutoDto(ArmaduraTrasformada _armaduraTrasformada, Autodesk.Revit.DB.Document _doc, TipoEstriboGenera tipoEstriboGenera =TipoEstriboGenera.EConfinamiento)
        {
            var projectBasePointPositiov2n = BasePoint.GetProjectBasePoint(_doc);
            var surveyPointPositionv2 = BasePoint.GetSurveyPoint(_doc);
            XYZ SurveyPoint = XYZ.Zero;//  projectBasePointPositiov2n.Position;

            CoordenadasBarra _coordenadasBarra = ListaCoordenadasBarra[0].ObtenerCoordenadasBarra(_armaduraTrasformada,Util.FootToCm(SurveyPoint.Z));

            List<XYZ> _ListaPtos = new List<XYZ>();
            _ListaPtos.Add(_coordenadasBarra.ptoIni_foot);
            _ListaPtos.Add(_coordenadasBarra.ptoFin_foot);
            XYZ aux_pto = _armaduraTrasformada.Ejecutar(_datosConfinamientoDTO.centroPier.GetXYZ_cmTofoot() - XYZ.BasisZ * SurveyPoint.Z);
            _datosConfinamientoDTO.centroPier = new XYZnh(aux_pto.X, aux_pto.Y, aux_pto.Z);
            _datosConfinamientoDTO.tipoConfiguracionEstribo =(tipoEstriboGenera==TipoEstriboGenera.EMuro? TipoConfiguracionEstribo.EstriboMuro: TipoConfiguracionEstribo.Estribo);
            _datosConfinamientoDTO.tipoEstriboGenera  = tipoEstriboGenera;
            if (_datosConfinamientoDTO.IsLateral !=true)  _datosConfinamientoDTO.IsLateral = false;
            if (_datosConfinamientoDTO.IsTraba != true) _datosConfinamientoDTO.IsTraba = false;

            return new IntervalosConfinaDTOAuto()
            {
                Pier = Pier,
                Story = Story,
                _datosConfinaDTO = _datosConfinamientoDTO,
                ListaPtos = _ListaPtos,
                

            };
        }
    }
}
