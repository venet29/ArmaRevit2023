using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Extension;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ArmaduraLosaRevit.Model.BarraV.Seleccion
{
   public class DireccionRecorrido
    {
        private  View view;
        private  DireccionRecorrido_ direccionRecorrido;
        //largo donde se distribuyen una cantidad de barras , el espaciento se de que calcular con este valor
        // si barra tiene direccion de recorrido entrando en vista (perpendicular cara muro) largoRecorrido=0-> para considerar espesor muro
        public double LargoRecorridoCm { get; set; }
        public bool IsLargoRecorridoCm { get; set; } //si es true, calculo toda con largorecorrido-> false : toma espesor muro
        public XYZ DireccionEntradoView6 { get; set; }
        public XYZ DireccionNormalFace { get;  set; }

#pragma warning disable CS0649 // Field 'DireccionRecorrido.direManualRecorrido' is never assigned to, and will always have its default value null
        private  XYZ direManualRecorrido;
#pragma warning restore CS0649 // Field 'DireccionRecorrido.direManualRecorrido' is never assigned to, and will always have its default value null
#pragma warning disable CS0649 // Field 'DireccionRecorrido.direManualPataEnFierrado' is never assigned to, and will always have its default value null
        private XYZ direManualPataEnFierrado;
#pragma warning restore CS0649 // Field 'DireccionRecorrido.direManualPataEnFierrado' is never assigned to, and will always have its default value null

        public DireccionRecorrido(View  view, DireccionRecorrido_ direccionRecorrido, double largoRecorrido=0)
        {
            this.view = view;
            this.direccionRecorrido = direccionRecorrido;
            this.LargoRecorridoCm = largoRecorrido;
            this.IsLargoRecorridoCm=(largoRecorrido == 0 ? false : true);
            this.DireccionEntradoView6 = -view.ViewDirection.Redondear8();
        }


        public XYZ ObtenerDireccionRecorridoBarra6(XYZ _Direction)
        {
            switch (direccionRecorrido)
            {
                case DireccionRecorrido_.DireccionZ:
                    return new XYZ(0, 0, 1);

                case DireccionRecorrido_.ParaleloDerechaVista:
                    return view.RightDirection.Redondear8().Normalize();

                case DireccionRecorrido_.PerpendicularEntradoVista:
                    return _Direction;// DireccionEntradoView6.Redondear8();

                case DireccionRecorrido_.Manual:
                    return direManualRecorrido;

                default:
                    return DireccionEntradoView6;
            }

        }



        public XYZ ObtenerDireccionLineaBarra(XYZ direccionEnfierrrar_aux)
        {
            switch (direccionRecorrido)
            {
                case DireccionRecorrido_.DireccionZ:
                    return new XYZ(0, 0, 1);

                case DireccionRecorrido_.ParaleloDerechaVista:
                    return DireccionEntradoView6;

                case DireccionRecorrido_.PerpendicularEntradoVista:
                    return direccionEnfierrrar_aux;

                case DireccionRecorrido_.Manual:
                    return direccionEnfierrrar_aux;

                default:
                    return DireccionEntradoView6;
            }

        }


        public XYZ ObtenerDireccionPataEnFierrado(XYZ direccionEnfierrrar_aux)
        {
            switch (direccionRecorrido)
            {
                case DireccionRecorrido_.DireccionZ:
                    return DireccionEntradoView6;

                case DireccionRecorrido_.ParaleloDerechaVista:
                    return DireccionEntradoView6;

                case DireccionRecorrido_.PerpendicularEntradoVista:
                    return direccionEnfierrrar_aux;

                case DireccionRecorrido_.Manual:
                    return direManualPataEnFierrado;
                default:
                    return direccionEnfierrrar_aux;
            }
        }
    }
}
