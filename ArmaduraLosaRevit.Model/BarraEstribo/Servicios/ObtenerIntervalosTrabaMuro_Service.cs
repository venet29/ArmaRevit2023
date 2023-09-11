using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraEstribo.TipoTrabas;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraEstribo.Servicios
{
    public class ObtenerIntervalosTrabaMuro_Service
    {
        //  private readonly DatosConfinamientoDTO _configuracionInicialEstriboDTO;
        private readonly XYZ _ptobarra1;
        private readonly XYZ _ptobarra2;
        private readonly ConfiguracionBarraTrabaDTO _ConfiguracionBarraTrabaDTO;
        private double _AnchoEstriboVisible;
        private XYZ _direccionAnchoTraba;
        private double _AnchoEstriboMedia;
        private double _espesor_sinrecubriemto;

        private List<double> _espaciamietoTraba;


        private XYZ _direccionEntradoView;



        public ObtenerIntervalosTrabaMuro_Service(ConfiguracionBarraTrabaDTO _configuracionBarraTrabaDTO)
        {
            //  this._configuracionInicialEstriboDTO = _configuracionInicialEstriboDTO;
            this._ptobarra1 = _configuracionBarraTrabaDTO.Ptobarra1;
            this._ptobarra2 = _configuracionBarraTrabaDTO.Ptobarra2;
            this._espesor_sinrecubriemto = _configuracionBarraTrabaDTO.EspesroMuroOVigaFoot - Util.MmToFoot(ConstNH.CONST_RECUBRIMIENTO_BAse2cm_MM) * 2;
                // - Util.MmToFoot(_configuracionBarraTrabaDTO.DiamtroTrabaEstriboMM) * 2;
            this._direccionEntradoView = _configuracionBarraTrabaDTO.DireccionEntradoHaciaView;

            _AnchoEstriboVisible = Math.Abs(_ptobarra1.AsignarZ(0).DistanceTo(_ptobarra2.AsignarZ(0)));
            _direccionAnchoTraba = _configuracionBarraTrabaDTO.DireccionEnfierrrado;// (_ptobarra2.AsignarZ(0) - _ptobarra1.AsignarZ(0)).Normalize(); ;
            _AnchoEstriboMedia = _AnchoEstriboVisible / 2;

            _espaciamietoTraba = new List<double>();
            _ConfiguracionBarraTrabaDTO = _configuracionBarraTrabaDTO;
        }


        public List<BarraTrabaDTO> M3_ObtenerTrabaEstriboMuroDTO()
        {
            ITipoTraba tipoITipoTraba = FactoryTipoTrabas.ObtenerTipoTrabas(_ConfiguracionBarraTrabaDTO);
            List<BarraTrabaDTO> ListTipoITipoTraba = tipoITipoTraba.ObtenerListaEspacimientoTrabas();

            for (int i = 0; i < ListTipoITipoTraba.Count; i++)
            {
                XYZ _startPont_aux;
                ListTipoITipoTraba[i]._textoTraba = _ConfiguracionBarraTrabaDTO.textoTraba;
                if (ListTipoITipoTraba[i]._tipo == TipoTraba.Transversal)
                {
                    if (_ConfiguracionBarraTrabaDTO.UbicacionTraba == DireccionTraba.Izquierda)
                        _startPont_aux = _ptobarra1 + _direccionAnchoTraba * ListTipoITipoTraba[i]._espaciamiento;
                    else
                       _startPont_aux = _ptobarra2.AsignarZ(_ptobarra1.Z) + _direccionAnchoTraba * ListTipoITipoTraba[i]._espaciamiento;


                    ListTipoITipoTraba[i]._startPont_ = _startPont_aux;
                    ListTipoITipoTraba[i]._endPoint = _startPont_aux + _direccionEntradoView * _espesor_sinrecubriemto;
                }
                else
                {
                    if (_ConfiguracionBarraTrabaDTO.UbicacionTraba == DireccionTraba.Izquierda)
                        _startPont_aux = _ptobarra1 + _direccionAnchoTraba * ListTipoITipoTraba[i]._espaciamiento+ _direccionEntradoView* _espesor_sinrecubriemto/2;
                    else
                        _startPont_aux = _ptobarra2.AsignarZ(_ptobarra1.Z) + _direccionAnchoTraba * ListTipoITipoTraba[i]._espaciamiento + _direccionEntradoView * _espesor_sinrecubriemto/2;


                    ListTipoITipoTraba[i]._startPont_ = _startPont_aux;
                    ListTipoITipoTraba[i]._endPoint = _startPont_aux + _direccionAnchoTraba * ListTipoITipoTraba[i].LargoTrabaTrasnversal;
                }

            }

            return ListTipoITipoTraba;
        }




    }
}
