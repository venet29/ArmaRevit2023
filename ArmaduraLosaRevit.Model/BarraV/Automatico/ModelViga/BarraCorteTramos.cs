using ArmaduraLosaRevit.Model.BarraEstribo.DTO;
using ArmaduraLosaRevit.Model.BarraV.Automatico.model.Data.Modelo;
using ArmaduraLosaRevit.Model.BarraV.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using System;

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga
{
    public class BarraCorteTramos
    {

        private Tabla02_Info_Barras_Corte obj;
        public BarraCorteTramos(Tabla02_Info_Barras_Corte obj)
        {
            this.obj = obj;
            IsOk = true;

        }

        public bool IsOk { get; internal set; }
        public string Tipo_Elemento { get; set; }
        public int Unique_Name_ETABS { get; set; }
        public int ID_Name_REVIT { get; set; }
        public string Eje_ETABS { get; private set; }
        public string Eje_REVIT { get; private set; }
        public DatosConfinamientoAutoDTO configuracionInicialEstriboDTO { get; private set; }
        public string Label_Beam { get; private set; }
        public TipoCaraObjeto Ubicacion_Armado { get; private set; }
        public int Seccion { get; private set; }

        public XYZnh p1_mm { get; set; }
        public XYZnh p2_mm { get; set; }
        public XYZ P1_Revit_foot { get; internal set; }
        public XYZ P2_Revit_foot { get; internal set; }

  
        public bool IsBuscarContinudadEntreVigas { get; set; }// para buscar continudad de barra entra vigas    


        public string VigaIdem { get; set; }
        public string BArrasIdem { get; set; }


        public LargoEstriboEnbarra LargoEstriboEnbarra { get; internal set; }

        public double Posi_Inicial_Porcentaje { get; set; }
        public double Posi_Final_Porcentaje { get; set; }

        // datos creados 
        public Rebar RebarEstribo { get; internal set; }
        public List<ElementId> ListaLateralesRebar { get;  set; }
        public List<ElementId> ListaTrabasRebar { get; set; }

        //Estribo  idem - estribo ylaterales
        public BarraCorteTramos EstriboIdem { get; set; }

        internal bool Crear()
        {
            try
            {
                ListaLateralesRebar = new List<ElementId>();
                ListaTrabasRebar = new List<ElementId>();
                LargoEstriboEnbarra = LargoEstriboEnbarra.Completa;
                Posi_Inicial_Porcentaje = 0;
                Posi_Final_Porcentaje = 0;
                IsBuscarContinudadEntreVigas = false;
  

                Tipo_Elemento = obj.Tipo_Elemento;
                Unique_Name_ETABS = Convert.ToInt32(obj.Unique_Name_ETABS);//  Convert.ToInt32(row["Unique_Name_ETABS"]);
                ID_Name_REVIT = Convert.ToInt32(obj.ID_Name_REVIT);
                //   if (!(ID_Name_REVIT==1370058 || ID_Name_REVIT == 1370060)) return false;
                Label_Beam = obj.Label_Beam;// row["Label_Beam"].ToString();

                Eje_ETABS = obj.Eje_ETABS;// row["Eje_ETABS"].ToString();
                Eje_REVIT = obj.Eje_REVIT;//  row["Eje_REVIT"].ToString();
                Seccion = Convert.ToInt32(obj.Seccion);// Convert.ToInt32(row["Seccion"]);

                int NumEroEstribo = Convert.ToInt32(obj.n_Estribos);// Convert.ToInt32(row["Seccion"]);
                int NumEroLAterales = Convert.ToInt32(obj.n_Laterales);// Convert.ToInt32(row["Seccion"]);

                int DiametroEstribo = Convert.ToInt32(obj.diametro_Estribos_mm);// Convert.ToInt32(row["Seccion"]);
                int DiametroLAterales = Convert.ToInt32(obj.diametro_Laterales_mm);// Convert.ToInt32(row["Seccion"]);

                // ayuda12353
                if (DiametroEstribo == 0)
                    Util.ErrorMsg("DiamtroCero");

                double EspaciemEstribo = Convert.ToInt32(obj.espaciamiento_Estribos__cm);// Convert.ToInt32(row["Seccion"]);

                if (NumEroEstribo == 0 || NumEroEstribo == 0 || NumEroEstribo == 0 || NumEroEstribo == 0)
                { IsOk = false;
                    return false;
                }

                configuracionInicialEstriboDTO =
                                            new DatosConfinamientoAutoDTO()
                                            {
                                                IsEstribo = true,
                                                DiamtroEstriboMM = DiametroEstribo,
                                                espaciamientoEstriboCM = EspaciemEstribo,
                                                cantidadEstribo = (NumEroEstribo==1? "E.": (NumEroEstribo == 2 ? "E.D." : (NumEroEstribo == 3 ? "E.T." : "E."))),
                                                tipoConfiguracionEstribo = TipoConfiguracionEstribo.EstriboViga,
                                                tipoEstriboGenera = TipoEstriboGenera.Eviga,

                                                IsLateral = true,
                                                DiamtroLateralEstriboMM = DiametroLAterales,
                                                cantidadLaterales = NumEroLAterales,
                                                IsExtenderLatInicio = false,
                                                IsExtenderLatFin = false,

                                                IsTraba = false,
                                                cantidadTraba = 1,
                                                DiamtroTrabaEstriboMM = 8,
                                                espaciamientoTrabaCM = 20,
                                                TipoDiseñoEstriboViga = TipoDisenoEstriboVIga.AsignarViga
                                                
                                            };


                VigaIdem = obj.IDEM_a_Viga;
                BArrasIdem = obj.Barra_IDEM_a;
                
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear barra 'Crear'. ex:{ex.Message}");
                return false;
            }
            return true;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
