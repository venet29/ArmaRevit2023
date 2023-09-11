using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.ExtStore.Factory;
using ArmaduraLosaRevit.Model.ExtStore.model;
using ArmaduraLosaRevit.Model.ExtStore.Tipos;
using ArmaduraLosaRevit.Model.Pasadas.Model;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace ArmaduraLosaRevit.Model.Pasadas.Servicio
{
    
    class ServicioCambiarPasada
    {
        private UIApplication _uiapp;
        private Document _doc;
        private UIDocument _uidoc;
        private ColorTipoPasada _estadoComparar;
        private string _estadoShaft_string;
        private string _colorEstadoShaft_string;
        private string _comentario_string;
        private EstadoPasada _estadoPAsada;
#pragma warning disable CS0169 // The field 'ServicioCambiarPasada._olorEstadoShaft_string' is never used
        private string _olorEstadoShaft_string;
#pragma warning restore CS0169 // The field 'ServicioCambiarPasada._olorEstadoShaft_string' is never used
        private readonly List<EnvoltorioBase> listaEnvoltorioMEP;

        public ServicioCambiarPasada(UIApplication uiapp, List<EnvoltorioBase> listaEnvoltorioMEP)
        {
            _uiapp = uiapp;
            _doc = uiapp.ActiveUIDocument.Document;
            _uidoc = _uiapp.ActiveUIDocument;
            this.listaEnvoltorioMEP = listaEnvoltorioMEP;
        }


        public bool RevisarPAsada(string C0mentario = "Revisar")
        {
            _estadoComparar = ColorTipoPasada.PASADA_AZUL;
            _estadoShaft_string = "Revision";
            _colorEstadoShaft_string = "Blue";
            _comentario_string = C0mentario;
            _estadoPAsada = EstadoPasada.Revision;
            return CambiarEstadoPAsada();
        }

        public bool RechazarPAsada(string C0mentario= "Rechazar")
        {
            _estadoComparar = ColorTipoPasada.PASADA_GRIS;
            _estadoShaft_string = "Rechazar";
            _colorEstadoShaft_string = "Gray";
            _comentario_string = C0mentario;
            _estadoPAsada = EstadoPasada.Rechazar;
            return CambiarEstadoPAsada();
        }


        private bool CambiarEstadoPAsada( )
        {
            try
            {
               
                var list = _uidoc.Selection.GetElementIds().ToList();

                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CambiarNH");

                    for (int i = 0; i < list.Count; i++)
                    {
                        ElementId eleId = list[i];
                        var elel = _doc.GetElement(list[i]);

                        if (!elel.Name.Contains("PASADA_")) continue;

                        var lis = listaEnvoltorioMEP.Where(c => c.ListaPasadas.Count > 0).ToList();


                        EnvoltorioBase _envoltorioElementoDucto = lis.Where(c => c.ListaPasadas.Exists(r => r.Pasada?.Id.IntegerValue == list[i].IntegerValue)).FirstOrDefault();

                        if (_envoltorioElementoDucto == null) continue;

                        EnvoltorioShaft Pasada = _envoltorioElementoDucto.ListaPasadas.Find(c => c.Pasada.Id.IntegerValue == eleId.IntegerValue);
                        if (Pasada == null) continue;


                        PAsadasCuandradaDTO _pasadasCuandradaDTO = Pasada.DatosPasadas;

                        if (_pasadasCuandradaDTO.NombrePasada == ColorTipoPasada.PASADA_VERDE )
                            _pasadasCuandradaDTO.NombrePasada = _estadoComparar;
                        else if ((_pasadasCuandradaDTO.NombrePasada == _estadoComparar || _pasadasCuandradaDTO.NombrePasada == ColorTipoPasada.PASADA_NARANJO))
                            _pasadasCuandradaDTO.NombrePasada = ColorTipoPasada.PASADA_VERDE;


                        ServicioCrearPAsadas _ServicioCrearPAsadas = new ServicioCrearPAsadas(_uiapp);
                        if (_ServicioCrearPAsadas.Crear_sintrasn(_pasadasCuandradaDTO))
                        {
                            //_envoltorioElementoDucto.ListaPasadas.Remove(Pasada);
                            _doc.Delete(Pasada.Pasada.Id);

                            Pasada.Pasada = _ServicioCrearPAsadas.familyInstance;
                            if (_pasadasCuandradaDTO.NombrePasada == ColorTipoPasada.PASADA_VERDE)
                            {
                                Pasada.Estado = EstadoPasada.Validado;
                                _envoltorioElementoDucto.EstadoShaft = "ConPasada";
                                _envoltorioElementoDucto.ColorEstadoShaft = "Green";
                                _comentario_string = "Pasada validada";
                            }
                            else if (_pasadasCuandradaDTO.NombrePasada == _estadoComparar)
                            {
                                Pasada.Estado = _estadoPAsada;
                                _envoltorioElementoDucto.Comentario = _comentario_string;
                                _envoltorioElementoDucto.EstadoShaft = _estadoShaft_string;
                                _envoltorioElementoDucto.ColorEstadoShaft = _colorEstadoShaft_string;
                            }
                        }
                        else
                        {
                            Pasada.Estado = EstadoPasada.ConError;
                            _comentario_string = "Error en pasada";
                        }

                        //** para guardar datos internos
                        DatosExtStoreDTO _CreadorExtStoreDTO = FactoryExtStore.ObtnerCreacionOpening();
                        CreadorExtStoreComplejo _CreadorExtStore = new CreadorExtStoreComplejo(_uiapp, _CreadorExtStoreDTO);
                        double area = Pasada.DatosPasadas.ancho_foot * Pasada.DatosPasadas.largo_foot;
                        _CreadorExtStore.M1_SET_DataInElement_XYZ_SInTrans(Pasada.Pasada, Pasada._ObjetosEncontradosDTO.ptoInterseccion, Pasada.Estado, area, _comentario_string);
                    }
                    t.Commit();
                }


            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                Util.ErrorMsg($"Error en ObtenerLista");

                return false;
            }
            return true;

        }

        internal bool CambiarTodasPAsadaAROJO()
        {
            try
            {          
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CambiarNH");

                    for (int i = 0; i < listaEnvoltorioMEP.Count; i++)
                    {
                        EnvoltorioBase _envoltorioElementoDucto = listaEnvoltorioMEP[i];

                        for (int j = 0; j < _envoltorioElementoDucto.ListaPasadas.Count; j++)
                        {
                            EnvoltorioShaft Pasada = _envoltorioElementoDucto.ListaPasadas[j];
                            PAsadasCuandradaDTO _pasadasCuandradaDTO = Pasada.DatosPasadas;
                            if (_pasadasCuandradaDTO == null) continue;


                            // al inicio para obtener  familia roja
                            _pasadasCuandradaDTO.NombrePasada = ColorTipoPasada.PASADA_ROJA;
                            ServicioCrearPAsadas _ServicioCrearPAsadas = new ServicioCrearPAsadas(_uiapp);
                            if (_ServicioCrearPAsadas.Crear_sintrasn(_pasadasCuandradaDTO))
                            {
                                _doc.Delete(Pasada.Pasada.Id);

                                Pasada.Pasada = _ServicioCrearPAsadas.familyInstance;
                                                                
                                _envoltorioElementoDucto.PasadaId = Pasada.Pasada.Id.IntegerValue;
                                // Pasada.Estado = EstadoPasada.NoValidado; linea borrara pq no permite crear chat se cambi estado en metood 'CambiarEstado_TodasPAsadaAROJO'
                                _envoltorioElementoDucto.EstadoShaft = "ShaftCreado";
                                _envoltorioElementoDucto.ColorEstadoShaft = "Red";
                               // _envoltorioElementoDucto.ColorEstadoShaft_letra = "Black";
                                _comentario_string = "Aceptada";
                            }
                            else
                            {
                                Pasada.Estado = EstadoPasada.ConError;
                                _comentario_string = "Error en pasada";
                            }

                            //** para guardar datos internos
                            DatosExtStoreDTO _CreadorExtStoreDTO = FactoryExtStore.ObtnerCreacionOpening();
                            CreadorExtStoreComplejo _CreadorExtStore = new CreadorExtStoreComplejo(_uiapp, _CreadorExtStoreDTO);
                            double area = Pasada.DatosPasadas.ancho_foot * Pasada.DatosPasadas.largo_foot;
                            _CreadorExtStore.M1_SET_DataInElement_XYZ_SInTrans(Pasada.Pasada, Pasada._ObjetosEncontradosDTO.ptoInterseccion, Pasada.Estado, area, _comentario_string);
                        }

                    }

                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en ObtenerLista\n\nex:{ex.Message}");

                return false;
            }
            return true;
        }

        // funcion para cambiar estado, se hace esto pq se dibuja o cambio primero la pasada roja por la verde creada previamente
        // anrtes de dibujar el shft, pq al dibujar el char 'parece' que la face pierde la referencia y no se dibuja o cuasa error
        internal bool CambiarEstado_TodasPAsadaAROJO()
        {
            try
            {
                using (Transaction t = new Transaction(_doc))
                {
                    t.Start("CambiarNH");

                    for (int i = 0; i < listaEnvoltorioMEP.Count; i++)
                    {
                        EnvoltorioBase _envoltorioElementoDucto = listaEnvoltorioMEP[i];

                        for (int j = 0; j < _envoltorioElementoDucto.ListaPasadas.Count; j++)
                        {
                            EnvoltorioShaft Pasada = _envoltorioElementoDucto.ListaPasadas[j];

                            if(Pasada.Estado == EstadoPasada.Validado)
                                Pasada.Estado = EstadoPasada.NoValidado;
                        }
                    }
                    t.Commit();
                }
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'CambiarEstado_TodasPAsadaAROJO'. ex:{ex.Message}");

                return false;
            }
            return true;
        }
    }
}
