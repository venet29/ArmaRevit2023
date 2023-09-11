using ArmaduraLosaRevit.Model.BarraV.Automatico.model.Data.Modelo;
using ArmaduraLosaRevit.Model.BarraV.Ayuda;
using ArmaduraLosaRevit.Model.Enumeraciones;
using Autodesk.Revit.DB;
using System;

using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.ModelViga
{
    public class BarraFlexionTramosDTO
    {
        // List<string> listaVIgasIDem = new List<string>();
        // ayuda12353
        //List<string> listaVIgasIDem = new List<string>() { "1368805", "1370915", "1370046", "1613691",  "1372051", "1605204", "1605674", "1606144", "1606614", "1373090", "1607569", "1608048", "1608527", "1609006", "1609485", "1609964", "1610443","1610922",
        //                                                     "1368811", "1370052", "1370921", "1613697", "1614172", "1372057", "1605210", "1605680", "1606150", "1606620", "1373096", "1607575", "1608054", "1608533", "1609012", "1609491", "1609970", "1610449", "1610928",
        //                                                     "1370927","1373102","1605216","1605686","1606156","1606626","1607581","1608060","1608539","1609018","1609497","1609976","1610455","1610934","1613703","1614178",
        //                                                     "1370929","1373104","1605218","1605688","1606158","1606628","1607583","1608062","1608541","1609020","1609499","1609978","1610457","1610936","1613705","1614180",
        //                                                     "1373223","1373223","1373223","1606208","1606678","1607633","1608112","1608591","1609070","1609549","1610028","1610507","1610986","1370981","1613755","1614230",
        //                                                     "1370983","1372119","1373233","1605270","1605740","1606210","1606680","1607635","1608114","1608593","1609072","1609551","1610030","1610509","1610988","1613757","1614232",
        // "1368195","1368197","1368199","1368201","1368203","1368205","1369446","1369448","1369450","1369452","1369454","1369456"};
        private Tabla01_Info_Barras_Flexion obj;
        public BarraFlexionTramosDTO(Tabla01_Info_Barras_Flexion obj)
        {
            this.obj = obj;
            IsOk = true;
            ListVigas = new List<VigasPosicion>();
        }

        public bool IsOk { get; internal set; }
        public string Tipo_Elemento { get; set; }
        public int Unique_Name_ETABS { get; set; }
        public int ID_Name_REVIT_Inicio { get; set; }
        public int ID_Name_REVIT_Final { get; set; }
        public string Eje_ETABS { get; private set; }
        public string Eje_REVIT { get; private set; }
        public string Label_Beam { get; private set; }
        public TipoCaraObjeto Ubicacion_Armado { get; private set; }
        
        public int Seccion_Inicio { get;  set; }
      //  public int SeccionInicial { get; set; }
        public int Seccion_Final { get; set; }

        public int Fila_Barra { get; private set; }
        public string Identificador_Barras { get; private set; }
        public int n_Barras { get; private set; }
        public int diametro_Barras__mm { get; private set; }
        public XYZnh p1_mm { get; set; }
        public XYZnh p2_mm { get; set; }
        public XYZ P1_Revit_foot { get; internal set; }
        public XYZ P2_Revit_foot { get; internal set; }
        public string IdentiFIcadorParaTraslapo { get; set; }



        public UbicacionTraslapoNh TipoTraslapo_ { get; set; }
      
        public CasosTraslapoDTO CasosTraslapoDTO_InicioTramo { get; set; }
        public CasosTraslapoDTO CasosTraslapoDTO_FinTramo { get; set; }
        
        public bool IsConLaeader { get; internal set; }
        public TipoBarraRefuerzoViga TipoBarraRefuerzoViga { get; internal set; }
        public bool IsBuscarContinudadEntreVigas { get; set; }// para buscar continudad de barra entra vigas    
        public VigaGeometriaDTO VigaContenedor { get; set; }
        public List<VigasPosicion> ListVigas { get; set; }


        public TipoPataBarra inicial_tipoBarraH { get; internal set; }
        public TipoPataBarra TipoPataIzqInf { get; set; }
        public TipoPataBarra TipoPataDereSup { get; set; }
        public string VigaIdem { get; set; }


        public string BArrasIdem { get; set; }


        internal bool Crear()
        {
            try
            {
                IsBuscarContinudadEntreVigas = false;
                IsConLaeader = false;
                Tipo_Elemento = obj.Tipo_Elemento;
                Unique_Name_ETABS = Convert.ToInt32(obj.Unique_Name_ETABS);//  Convert.ToInt32(row["Unique_Name_ETABS"]);
                ID_Name_REVIT_Inicio = Convert.ToInt32(obj.ID_Name_REVIT);
                ID_Name_REVIT_Final = ID_Name_REVIT_Inicio;
                //   if (!(ID_Name_REVIT==1370058 || ID_Name_REVIT == 1370060)) return false;
                Label_Beam = obj.Label_Beam;// row["Label_Beam"].ToString();

                Eje_ETABS = obj.Eje_ETABS;// row["Eje_ETABS"].ToString();
                Eje_REVIT = obj.Eje_REVIT;//  row["Eje_REVIT"].ToString();

                Ubicacion_Armado = (obj.Ubicacion_Armado == "MIN" ? TipoCaraObjeto.Superior : TipoCaraObjeto.Inferior);// row["Ubicacion_Armado"].ToString();

                Seccion_Inicio = Convert.ToInt32(obj.Seccion);// Convert.ToInt32(row["Seccion"]);                
                Seccion_Final = Seccion_Inicio;

                Fila_Barra = Convert.ToInt32(obj.Fila_Barra);
                Identificador_Barras = obj.Identificador_Barras;//  row["Identificador_Barras"].ToString();

                n_Barras = Convert.ToInt32(obj.n_Barras);
                diametro_Barras__mm = Convert.ToInt32(obj.diametro_Barras__mm);
                if (diametro_Barras__mm == 0) return false;
                p1_mm = new XYZnh(Convert.ToDouble(obj.Pos_HOM_H_1__mm), 0, Convert.ToDouble(obj.Pos_HOM_V_1__mm));
                p2_mm = new XYZnh(Convert.ToDouble(obj.Pos_HOM_H_2__mm), 0, Convert.ToDouble(obj.Pos_HOM_V_2__mm));
                TipoBarraRefuerzoViga = TipoBarraRefuerzoViga.NONE;

                if (Ubicacion_Armado == TipoCaraObjeto.Superior)
                    IdentiFIcadorParaTraslapo = "SUP_" + Fila_Barra;
                else
                    IdentiFIcadorParaTraslapo = "INF_" + Fila_Barra;


                // ayuda12353
                //if (listaVIgasIDem.Contains(obj.ID_Name_REVIT))
                //{
                //    VigaIdem = obj.Unique_Name_ETABS;
                //    BArrasIdem = obj.Unique_Name_ETABS;
                //}
                //else
                //{
                VigaIdem = obj.IDEM_a_Viga;
                BArrasIdem = obj.Barra_IDEM_a;

                //}


                BArrasIdem = obj.Barra_IDEM_a;
                Debug.WriteLine($"IDvigaRevit:{ID_Name_REVIT_Inicio} VigaIdem ={ obj.IDEM_a_Viga}  BArrasIdem={obj.Barra_IDEM_a}  ");

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
