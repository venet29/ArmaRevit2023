using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES.ParaBarras
{
    public class ObtenerDatosRebarInsystem
    {
        
        private List<ElementId> ListElemId;
        
        public  string RebarSaphe1 { get; private set; }
        public  string RebarSaphe2 { get; private set; }

        public  double LargoAlternative_foot { get; private set; }
        public  double LargoPrimaria_Foot { get; private set; }
        public  bool IsAlternative { get; private set; }
        public PathReinforcement _PathReinforcement { get; }

        private Document _doc;

        public int numeroBarrasPrimaria { get; private set; }
        public int numeroBarrasSecundario { get; private set; }
        public bool IsBarraPrimaria { get; private set; }
        public bool IsBarraSecuandaria { get; private set; }
        public bool Isok { get; set; }

        public ObtenerDatosRebarInsystem(PathReinforcement _pathReinforcement)
        {
            this._PathReinforcement = _pathReinforcement;
            this._doc = _pathReinforcement.Document;
            Isok = true;
        }

        public  bool Obtener()
        {
            Isok = false;
            try
            {
                Parameter IsBArraAlternativa = ParameterUtil.FindParaByName(_PathReinforcement.Parameters, "Alternating Bars");
                IsAlternative = false;

                LargoAlternative_foot = LargoPrimaria_Foot = 0;

                Parameter largoPrimaria = ParameterUtil.FindParaByName(_PathReinforcement.Parameters, "Primary Bar - Length");
                LargoPrimaria_Foot = largoPrimaria.AsDouble();
              
           
                if (IsBArraAlternativa.AsValueString() == "Yes")
                {
                    IsAlternative = true;
                    Parameter largoAlternativa = ParameterUtil.FindParaByName(_PathReinforcement.Parameters, "Alternating Bar - Length");
                    LargoAlternative_foot = largoAlternativa.AsDouble();
                }

            }
            catch (Exception ex)
            { 
               Util.ErrorMsg($"Error al obtener largos de 'LargoPathRinforment'  ex:{ex.Message}");
                return false;
            }
            
            return Isok = true; ;
        }

        public bool ObtenerCantiadad()
        {
            try
            {

                Parameter IsBArraAlternativa = ParameterUtil.FindParaByName(_PathReinforcement.Parameters, "Alternating Bars");
                IsAlternative = false;
                numeroBarrasPrimaria = numeroBarrasSecundario = 0;
                if (!Validar()) return false;

                RebarInSystem rebarInSystem1 = (RebarInSystem)_doc.GetElement(ListElemId[0]);

                RebarSaphe1 = ParameterUtil.FindParaByBuiltInParameter(rebarInSystem1, BuiltInParameter.REBAR_SHAPE, _doc);
                numeroBarrasPrimaria = ParameterUtil.FindParaByName(rebarInSystem1, "Quantity").AsInteger();


                if (IsBArraAlternativa.AsValueString() == "Yes" && ListElemId.Count==2)
                {
                    IsAlternative = true;
                    RebarInSystem rebarInSystem2 = (RebarInSystem)_doc.GetElement(ListElemId[1]);
                    RebarSaphe2 = ParameterUtil.FindParaByBuiltInParameter(rebarInSystem2, BuiltInParameter.REBAR_SHAPE, _doc);
                    numeroBarrasSecundario = ParameterUtil.FindParaByName(rebarInSystem2, "Quantity").AsInteger();
                }

            }
            catch (Exception)
            {
                Isok = false;
                MsjeError(_PathReinforcement.Id.IntegerValue.ToString());

            }
            return true;
        }
        public bool ObtenerCantiadadCasoF17()
        {
            try
            {


                Parameter IsBArraAlternativa = ParameterUtil.FindParaByName(_PathReinforcement.Parameters, "Alternating Bars");
                IsAlternative = false;

                if (!ValidarF17()) return false;

                RebarInSystem rebarInSystem1 = (RebarInSystem)_doc.GetElement(ListElemId[0]);

                RebarSaphe1 = ParameterUtil.FindParaByBuiltInParameter(rebarInSystem1, BuiltInParameter.REBAR_SHAPE, _doc);
                int RebarSaphe1Numero = ParameterUtil.FindParaByName(rebarInSystem1, "Quantity").AsInteger();

                RebarInSystem rebarInSystem2 = (RebarInSystem)_doc.GetElement(ListElemId[1]);
                RebarSaphe2 = ParameterUtil.FindParaByBuiltInParameter(rebarInSystem2, BuiltInParameter.REBAR_SHAPE, _doc);
                int RebarSaphe2Numero = ParameterUtil.FindParaByName(rebarInSystem2, "Quantity").AsInteger();


                if (RebarSaphe1 == "M_00")
                {
                    numeroBarrasPrimaria = RebarSaphe2Numero;
                    numeroBarrasSecundario = RebarSaphe1Numero;

                }
                else
                {
                    numeroBarrasPrimaria = RebarSaphe1Numero;
                    numeroBarrasSecundario = RebarSaphe2Numero;
                }


            }
            catch (Exception)
            {
                Isok = false;
                MsjeError(_PathReinforcement.Id.IntegerValue.ToString());

            }
            return true;
        }

        private bool ValidarF17()
        {
            Isok = false;
            try
            {
                var Ilist_RebarInSystem = _PathReinforcement.GetRebarInSystemIds();

                if (Ilist_RebarInSystem == null)
                {
                    MsjeError(_PathReinforcement.Id.IntegerValue.ToString());
                    return false;
                }
                ListElemId = Ilist_RebarInSystem.ToList();
                if (ListElemId.Count != 2)
                {
                    MsjeError(_PathReinforcement.Id.IntegerValue.ToString());
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            Isok = true;
            return true;
        }

        private bool Validar()
        {
            Isok = false;
            try
            {
                var Ilist_RebarInSystem = _PathReinforcement.GetRebarInSystemIds();

                if (Ilist_RebarInSystem == null)
                {
                    MsjeError(_PathReinforcement.Id.IntegerValue.ToString());
                    return false;
                }
                ListElemId = Ilist_RebarInSystem.ToList();

            }
            catch (Exception)
            {
                return false;
            }
            Isok = true;
            return true;
        }


        private void MsjeError(string id_PathReinforcement)
        {
            IsBarraPrimaria = false;
            IsBarraSecuandaria = false;
            Util.ErrorMsg("Erro al calcular numero de barras Caso F17     id Pathreinf:{id_PathReinforcement}");
        }
    }
}
