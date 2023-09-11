using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Enumeraciones;
using ArmaduraLosaRevit.Model.Armadura;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.ElementoBarraRoom.Familias
{
    public interface ITipoHook
    {
        void DefinirHook();
        void voidDefinirHookWall();
    }
    public class TipoHook: ITipoHook
    {
        private UbicacionLosa _ubicacionEnlosa;
        private string _tipoBarra;


        public RebarHookType tipodeHookEndPrincipal { get; set; }
        public RebarHookType tipodeHookStartPrincipal { get; set; }

        public RebarHookType tipodeHookStarAlternativa { get; set; }
        public RebarHookType tipodeHookEndAlternativa { get; set; }

        private Document _doc;
        private TipoRefuerzo _tiporefuerzo;
        private int _angle_pelotaLosa1;
        private XYZ DireccionMayor;

        public TipoHook(SolicitudBarraDTO solicitudDTO)
        {
            _ubicacionEnlosa = solicitudDTO.UbicacionEnlosa;
            _tipoBarra = solicitudDTO.TipoBarra;
            _doc = solicitudDTO.UIdoc.Document;

            //valores aseignar
            _tiporefuerzo = TipoRefuerzo.PathRefuerza;
            _angle_pelotaLosa1 = 0;
        }
        public TipoHook()
        {

        }
        /// <summary>
        /// NO SE UTILIZA
        ///  obtiene el tipo de gancho -
        /// </summary>
        /// <param name="ubicacionEnlosa"></param>
        public void DefinirHook()
        {
            //solo para 
            //Curve curve = null;

            switch (_ubicacionEnlosa)
            {
                case UbicacionLosa.Izquierda:
                    //    TipoOrientacion = TipoOrientacionBarra.Horizontal;
                    switch (_tipoBarra)
                    {
                        case "f1":
                            //HOOK barra Principal

                            if (_tiporefuerzo == TipoRefuerzo.PathRefuerza)
                            {
                                tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                            }
                            else if (_tiporefuerzo == TipoRefuerzo.AreaRefuerzo)
                            {
                                tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                            }
                 
                            break;
                        case "f3":
                    
                            break;
                        case "f4":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                      
                            break;
                        case "s3":
                        case "f7":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 degNH", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                       
                            break;
                        case "s1":
                        case "f9":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                            break;
                        case "f9a":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            break;
                        case "f10a":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            //tipodeHookEndPrincipal = TiposRebar_Shape_Hook.ObtenerHook("Standard - 90 deg.", doc);
                            break;
                        case "f10":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                     
                            break;
                        case "f11":
                            //tipodeHookStartPrincipal = TiposRebar_Shape_Hook.ObtenerHook("Standard - 90 deg.", doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
        
                            break;
                        case "19":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);

              
                            break;
                        default:
                            break;
                    }

                    break;
                case UbicacionLosa.Derecha:
                    //  TipoOrientacion = TipoOrientacionBarra.Horizontal;
                    switch (_tipoBarra)
                    {
                        case "f1":
                            //HOOK barra Principal                           
                            if (_tiporefuerzo == TipoRefuerzo.PathRefuerza) //caso pathReinf
                            {
                                tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                            }
                            else if (_tiporefuerzo == TipoRefuerzo.AreaRefuerzo) //caso areaReinf
                            {
                                tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                            }
                         
                            break;
                        case "f3":
                        
                            break;
                        case "f4":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                            
                            break;
                        case "s3":
                        case "f7":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Rebar Hook 90", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                            
                            break;
                        case "s1":
                        case "f9":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                            break;
                        case "f9a":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            break;
                        case "f10a":
                            //tipodeHookStartPrincipal = TiposRebar_Shape_Hook.ObtenerHook("Standard - 90 deg.", doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            break;
                        case "f10":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            
                            break;
                        case "f11":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            //tipodeHookEndPrincipal = TiposRebar_Shape_Hook.ObtenerHook("Standard - 90 deg.", doc);
                         
                            break;
                        case "19":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);


                       

                            break;
                        default:
                            break;
                    }

                    break;
                case UbicacionLosa.Superior:
                    //  TipoOrientacion = TipoOrientacionBarra.Vertical;

                    switch (_tipoBarra)
                    {

                        case "f1":
                            //HOOK barra Principal


                            //HOOK barra Principal                           
                            if (_tiporefuerzo == TipoRefuerzo.PathRefuerza) //caso pathReinf
                            {
                                tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);

                            }
                            else if (_tiporefuerzo == TipoRefuerzo.AreaRefuerzo) //caso areaReinf
                            {
                                tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                                DireccionMayor = new XYZ(1 * Math.Cos(Util.GradosToRadianes(_angle_pelotaLosa1 + 90)), 1 * Math.Sin(Util.GradosToRadianes(_angle_pelotaLosa1 + 90)), 0);
                            }


                    
                            break;
                        case "f3":
                
                            break;
                        case "f4":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                       
                            break;
                        case "s3":
                        case "f7":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Rebar Hook 90", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
          
                            break;
                        case "s1":
                        case "f9":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                            break;
                        case "f9a":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            break;
                        case "f10a":
                            //tipodeHookStartPrincipal = TiposRebar_Shape_Hook.ObtenerHook("Standard - 90 deg.", doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            break;
                        case "f10":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                   
                            break;
                        case "f11":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            //tipodeHookEndPrincipal = TiposRebar_Shape_Hook.ObtenerHook("Standard - 90 deg.", doc);
                    
                            break;
                        case "19":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);

                
                            break;
                        default:
                            break;
                    }
                    break;
                case UbicacionLosa.Inferior:
                    //  TipoOrientacion = TipoOrientacionBarra.Vertical;
                    switch (_tipoBarra)
                    {

                        case "f1":
                            //HOOK barra Principal


                            //HOOK barra Principal                           
                            if (_tiporefuerzo == TipoRefuerzo.PathRefuerza) //caso pathReinf
                            {
                                tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);

                            }
                            else if (_tiporefuerzo == TipoRefuerzo.AreaRefuerzo) //caso areaReinf
                            {
                                tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                                DireccionMayor = new XYZ(1 * Math.Cos(Util.GradosToRadianes(_angle_pelotaLosa1 + 90)), 1 * Math.Sin(Util.GradosToRadianes(_angle_pelotaLosa1 + 90)), 0);
                            }


                    
                            break;
                        case "f3":
                          
                            break;
                        case "f4":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                  
                            break;
                        case "s3":
                        case "f7":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Rebar Hook 90", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                    
                            break;
                        case "s1":
                        case "f9":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);
                            break;
                        case "f9a":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            break;
                        case "f10a":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            //tipodeHookEndPrincipal = TiposRebar_Shape_Hook.ObtenerHook("Standard - 90 deg.", doc);
                            break;
                        case "f10":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                        
                            break;
                        case "f11":
                            //tipodeHookStartPrincipal = TiposRebar_Shape_Hook.ObtenerHook("Standard - 90 deg.", doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                    
                            break;
                        case "19":
                            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
                            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 180 deg.", _doc);

                            break;
                        default:
                            break;
                    }

                    break;
                default:
                    break;
            }
        }


        public void voidDefinirHookWall()
        {

            tipodeHookStartPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
            tipodeHookEndPrincipal = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);

            tipodeHookEndAlternativa = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
            tipodeHookStarAlternativa = TipoRebarHookType.ObtenerHook("Standard - 90 deg.", _doc);
        }
    }
}
