using ArmaduraLosaRevit.Model.Armadura;
using ArmaduraLosaRevit.Model.EditarTipoPath.WPF.Ayuda;
using ArmaduraLosaRevit.Model.Seleccionar.Barras;
using ArmaduraLosaRevit.Model.UTILES.ParaBarras;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.EditarPath.CAmbiosPAth
{
    public class CambiarSoloDiametroEspaciamieto
    {
        private readonly UIApplication _uiapp;
        private Document _doc;
        private readonly EstadoCambioTIpoBarras estadoCambioTIpoBarras_;
        private readonly SeleccionarPathReinfomentConPto _SeleccionarPathReinfomentConPto;

        public PathReinforcement _pathReinforcement { get; set; }

        public CambiarSoloDiametroEspaciamieto(UIApplication uiapp, EstadoCambioTIpoBarras estadoCambioTIpoBarras_, SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto)
        {
            this._uiapp = uiapp;
            this._doc = _uiapp.ActiveUIDocument.Document;
            this.estadoCambioTIpoBarras_ = estadoCambioTIpoBarras_;
            _SeleccionarPathReinfomentConPto = _seleccionarPathReinfomentConPto;
            _pathReinforcement = _SeleccionarPathReinfomentConPto.PathReinforcement;
        }

        public bool Ejecutar()
        {
            try
            {
                using (TransactionGroup t = new TransactionGroup(_doc))
                {
                    t.Start("CambiarDiametroOEspaciemiento-NH");

                    using (Transaction m = new Transaction(_doc))
                    {
                        m.Start("CambiarSOloDiametroOEspaciemiento-NH");

                        if (estadoCambioTIpoBarras_.IsCambioDiametro)
                        {
                            int diametro_ = Util.ConvertirStringInInteger(estadoCambioTIpoBarras_.diametroNuevo);
                            var rebarBarType = TiposRebarBarType.getRebarBarType("Ø" + diametro_, _doc, true);
                            if (rebarBarType == null)
                            {
                                Util.ErrorMsg($"Error, no se encontro el tipo barra Ø{diametro_}");
                                return false;
                            }

                            ParameterUtil.SetParaElementId(_pathReinforcement, BuiltInParameter.PATH_REIN_TYPE_1, rebarBarType.Id);
                            ParameterUtil.SetParaElementId(_pathReinforcement, BuiltInParameter.PATH_REIN_TYPE_2, rebarBarType.Id);
                        }

                        if (estadoCambioTIpoBarras_.IsCambioEspacimeiento)
                        {
                            double espa_foot = Util.CmToFoot(Util.ConvertirStringInDouble(estadoCambioTIpoBarras_.EspaciamientoNuevo));
                            ParameterUtil.SetParaDouble(_pathReinforcement, BuiltInParameter.PATH_REIN_SPACING, espa_foot);
                        }

                        m.Commit();
                    }


                    if (estadoCambioTIpoBarras_.IsCambioEspacimeiento)
                    {
                        DatosPathRinforment _LargoPathRinforment = new DatosPathRinforment(_pathReinforcement);
                        //_LargoPathRinforment.Obtener();
                        _LargoPathRinforment.M2_ObtenerCantiadad_REbarshape_cantidad_();


                        using (Transaction c = new Transaction(_doc))
                        {
                            c.Start("CambiarCAntidadBarras-NH");

                            ParameterUtil.SetParaStringNH(_pathReinforcement, "NumeroPrimario", _LargoPathRinforment.numeroBarrasPrimaria.ToString());
                            ParameterUtil.SetParaStringNH(_pathReinforcement, "NumeroSecundario", _LargoPathRinforment.numeroBarrasSecundario.ToString());

                            c.Commit();
                        }
                    }


                    t.Assimilate();
                }



            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al cambiar diametro o espciamiento de path   \n   ex:{ex.Message}");
                return false; ;
            }
            return true;
        }
    }
}
