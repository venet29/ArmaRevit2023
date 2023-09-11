using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Traslapo.Contenedores;
using ArmaduraLosaRevit.Model.Traslapo.DTO;
using ArmaduraLosaRevit.Model.Traslapo.Help;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Traslapo.Calculos
{
    public class ControladorContenedorDatosTraslapoV2
    {

        List<ContenedorDatosTraslapoV2> _cont;
        public ControladorContenedorDatosTraslapoV2()
        {
            //public enum TipoBarra { f1, f3, f4, f7, f9, f9a, f10b, f11, f16, f17, f18, f19, f20, f21, NONE }
            _cont = new List<ContenedorDatosTraslapoV2>();

            //_cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f17),
            //                        new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f17B_Tras), new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f16_Dere),//izquierda
            //                        new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f16_Izq), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f17A_Tras))); // derecha



            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f1),
                                                    new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f1), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f3), //izquierda
                                                    new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f3), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f1))); //derecha

            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f3),
                                                    new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f3), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f3),//izquierda
                                                    new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f3), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f3))); // derecha

            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f4),
                                                  new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f1), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f1),//izquierda
                                                  new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f1), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f1))); // derecha

            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f7),
                                                  new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f1), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f11),//izquierda
                                                  new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f11), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f1))); // derecha

            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f9),
                                                  new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f9a, TipoCaraObjeto.Superior), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f9a, TipoCaraObjeto.Superior),//izquierda
                                                  new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f9a, TipoCaraObjeto.Superior), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f9a, TipoCaraObjeto.Superior))); // derecha

            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f9a),
                                                 new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f9a, TipoCaraObjeto.Superior), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f3, TipoCaraObjeto.Superior),//izquierda
                                                 new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f3, TipoCaraObjeto.Superior), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f9a, TipoCaraObjeto.Superior))); // derecha

            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f10),
                                               new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f10a, TipoCaraObjeto.Superior), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f10a, TipoCaraObjeto.Superior),//izquierda
                                               new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f10a, TipoCaraObjeto.Superior), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f10a, TipoCaraObjeto.Superior))); // derecha

            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f10a),
                                                new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f10a, TipoCaraObjeto.Superior), new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f3, TipoCaraObjeto.Superior),//izquierda
                                                new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f3, TipoCaraObjeto.Superior), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f10a, TipoCaraObjeto.Superior))); // derecha

            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f11),
                                              new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f11a), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f11a),//izquierda
                                              new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f11a), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f11a))); // derecha
            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f11a),
                                          new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f11a), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f3),//izquierda
                                          new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f3), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f11a))); // derecha

            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f12),
                                              new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f12), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f12),//izquierda
                                              new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f12), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f12))); // derecha
            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f16),
                                              new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f16_Izq), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f16_Dere),//izquierda
                                              new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f16_Izq), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f16_Dere))); // derecha

            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f16a),
                                   new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f16a), new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f16a),//izquierda
                                   new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f16a), new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f16a))); // derecha

            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f16b),
                                   new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f16b), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f16b),//izquierda
                                   new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f16b), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f16b))); // derecha

            //_cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f17),
            //                                 new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f16_Izq), new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f17A_Tras),//izquierda
            //                                 new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f17B_Tras), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f16_Dere))); // derecha

            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f17),
                                             new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f17B_Tras), new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f16_Dere),//izquierda
                                             new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f16_Izq), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f17A_Tras))); // derecha

            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f18),
                                new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f17A_Tras), new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f17B_Tras),//izquierda
                                new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f17A_Tras), new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f17B_Tras))); // derecha

            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f19),
                             new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f19_Izq), new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f19_Dere),//izquierda
                             new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f19_Izq), new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f19_Dere))); // derecha


            //_contenedorDatosTraslapo.Add(new ContenedorDatosTraslapo(TipoBarra.f20, TipoBarra.f20, TipoBarra.f16));
            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f20),
                   new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f20A_Izq_Tras), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f20A_Dere_Tras),//izquier
                   new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f20B_Izq_Tras), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f20B_Dere_Tras))); // derecha

            //************************* f20
            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f20a),
                 new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f20a), new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f16a),//izquier
                 new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f20a), new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f16a))); // derecha

            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f20b),
                      new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f16b), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f20b),
                    new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f16b), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f20b)));//izquier

            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f20aInv),
             new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f20aInv), new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f16b),//izquier
             new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f20aInv), new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f16b))); // derecha

            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f20bInv),
                      new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f16a), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f20bInv),
                    new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f16a), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f20bInv)));//izquier
            //f21 *********************************
            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f21),
                     new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f21A_Izq_Tras), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f21A_Dere_Tras),//izquierda
                     new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f21B_Izq_Tras), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f21B_Dere_Tras))); // derecha

            //f22 *************************************
            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f22a),
                        new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f22a), new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f22a),//izquierda
                        new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f22a), new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f22a))); // derecha

            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f22b),
             new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f22b), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f22b),//izquierda
             new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f22b), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f22b))); // derecha

            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f22aInv),
                  new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f22aInv), new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f22aInv),//izquierda
                  new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f22aInv), new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.f22aInv))); // derecha

            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f22bInv),
             new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f22bInv), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f22bInv),//izquierda
             new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f22bInv), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.f22bInv))); // derecha

            //****************************************************************************************************************************************************************
            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.s1),
                     new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.s1), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.s1),//izquierda
                     new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.s1), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.s1))); // derecha
            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.s2),
                   new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.s2), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.s2),//izquierdann
                   new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.s2), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.s2))); // derecha
            _cont.Add(new ContenedorDatosTraslapoV2(new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.s4),
           new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.s4), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.s4),//izquierda
           new TipoPathReinfDTO(UbicacionLosa.Izquierda, TipoBarra.s4), new TipoPathReinfDTO(UbicacionLosa.Derecha, TipoBarra.s4))); // derecha

        }

        public ContenedorDatosTraslapoV2 ObtenerBarraIzqBajoTraslapo(TipoBarra _tipoBarra, UbicacionLosa ubicacionlosa)
        {
            return _cont.Where(c => c._tipoBarra.Tipobarra == _tipoBarra).Select(ci => ci.M1_AsignaSentidoCorrespondiente(ubicacionlosa)).FirstOrDefault();
        }


    }
}
