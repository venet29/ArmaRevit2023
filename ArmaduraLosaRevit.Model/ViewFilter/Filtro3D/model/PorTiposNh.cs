using ArmaduraLosaRevit.Model.Visibilidad.Entidades;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ArmaduraLosaRevit.Model.modeloNH
{
    public class PorTiposNh : INotifyPropertyChanged
    {

        private string _nombre;
        public string Nombre
        {
            get { return _nombre; }
            set
            {
                _nombre = value;
                NotifyPropertyChanged("Nombre");
            }
        }


        private string _nombreFiltro;
        public string NombreFiltro
        {
            get { return _nombreFiltro; }
            set
            {
                _nombreFiltro = value;
                NotifyPropertyChanged("NombreFiltro");
            }
        }


        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {

                _isVisible = value;
                NotifyPropertyChanged("IsVisible");
            }
        }


        public PorTiposNh(string tipo, string tipofiltro = "")
        {
            Nombre = tipo;
            if (tipofiltro == "")
                NombreFiltro = tipo;
            else
                NombreFiltro = tipofiltro;
            IsVisible = true;
        }


        //por tipo

        internal static List<PorTiposNh> ObtenerTiposELev()
        {
            System.Collections.Generic.List<PorTiposNh> ListaObs = new System.Collections.Generic.List<PorTiposNh>();
            ListaObs.Add(new PorTiposNh("Barra Horizontal", "ELEV_BA_H")); 
            ListaObs.Add(new PorTiposNh("Barra Vertical", "ELEV_BA_V"));

            ListaObs.Add(new PorTiposNh("M_Malla Horizontal", "ELEV_MA_H"));
            ListaObs.Add(new PorTiposNh("M_Malla Vertical", "ELEV_MA_V"));
            ListaObs.Add(new PorTiposNh("M_Traba Malla", "ELEV_MA_T"));
            ListaObs.Add(new PorTiposNh("C_Estribo Confinamiento", "ELEV_CO"));
            ListaObs.Add(new PorTiposNh("C_Traba Confinamiento", "ELEV_CO_T"));
            ListaObs.Add(new PorTiposNh("E_Estribo Muro", "ELEV_ES"));
            ListaObs.Add(new PorTiposNh("E_Lateral Estribo Muro", "ELEV_ES_L"));
            ListaObs.Add(new PorTiposNh("E_Traba Estribo Muro", "ELEV_ES_T"));
            ListaObs.Add(new PorTiposNh("V_Estribo Viga", "ELEV_ES_V")); 
            ListaObs.Add(new PorTiposNh("V_Lateral Estribo Viga", "ELEV_ES_VL"));
            ListaObs.Add(new PorTiposNh("V_Traba Estribo Viga", "ELEV_ES_VT"));

      

            ListaObs.Add(new PorTiposNh("Refuerzo Muro", "ELEV_REFUERZO_MURO")); //   ListaObs.Add(new PorTiposNh("ELEV_HORQ"));
            ListaObs.Add(new PorTiposNh("Barra Horq.Horizontal", "ELEV_BA_CABEZA_HORQ"));
            ListaObs.Add(new PorTiposNh("Barra Horq.Vertical", "ELEV_BA_HORQ"));
            ListaObs.Add(new PorTiposNh("Barra Escalera", "ELEV_ESC"));
            return ListaObs;
        }

        internal static List<PorTiposNh> ObtenerTiposLosa()
        {

            System.Collections.Generic.List<PorTiposNh> ListaObs = new System.Collections.Generic.List<PorTiposNh>();
            ListaObs.Add(new PorTiposNh("Barra Losa Inferior", "LOSA_INF")); 
            ListaObs.Add(new PorTiposNh("BArra Losa Superior", "LOSA_SUP"));
            ListaObs.Add(new PorTiposNh("Barra Sup.S1","LOSA_SUP_S1"));
            ListaObs.Add(new PorTiposNh("Barra Sup.S2","LOSA_SUP_S2"));
            ListaObs.Add(new PorTiposNh("Barra Sup.S3","LOSA_SUP_S3"));
            ListaObs.Add(new PorTiposNh("Barra Sup.S4","LOSA_SUP_S4"));

            ListaObs.Add(new PorTiposNh("1_Refuerzo Cabeza Muro", "REFUERZO_BA_CAB_MURO"));
            ListaObs.Add(new PorTiposNh("1_Suple Cabeza Muro", "REFUERZO_SUPLE_CAB_MU"));
           
            ListaObs.Add(new PorTiposNh("2_Barra Refuerzo 2mro", "REFUERZO_BA_REF_LO"));
            ListaObs.Add(new PorTiposNh("2_Estribo Refuerzo 2muros", "REFUERZO_EST_REF_LO"));

            ListaObs.Add(new PorTiposNh("3_Barra Ref.borde", "REFUERZO_BA_BORDE"));
            ListaObs.Add(new PorTiposNh("3_Estribo Ref.borde", "REFUERZO_EST_BORDE"));

            ListaObs.Add(new PorTiposNh("Barra Losa.Esc F1A","LOSA_ESC_F1_45"));
            ListaObs.Add(new PorTiposNh("Barra Losa.Esc F1B", "LOSA_ESC_F1_45_CONPATA"));
            ListaObs.Add(new PorTiposNh("Barra Losa.Esc F1C" ,"LOSA_ESC_F1_135_SINPATA"));

            ListaObs.Add(new PorTiposNh("Barra Losa.Esc F3BA", "LOSA_ESC_F3_BA"));
            ListaObs.Add(new PorTiposNh("Barra Losa.Esc F3AB", "LOSA_ESC_F3_AB"));
            ListaObs.Add(new PorTiposNh("Barra Losa.Esc F30A", "LOSA_ESC_F3_0A"));
            ListaObs.Add(new PorTiposNh("Barra Losa.Esc F3A0", "LOSA_ESC_F3_A0"));
            ListaObs.Add(new PorTiposNh("Barra Losa.Esc F30B", "LOSA_ESC_F3_0B"));
            ListaObs.Add(new PorTiposNh("Barra Losa.Esc F3B0", "LOSA_ESC_F3_B0"));


            ListaObs.Add(new PorTiposNh("Barra Losa.Esc F3_45","LOSA_ESC_F3_45"));
            ListaObs.Add(new PorTiposNh("Barra Losa.Esc F3_135", "LOSA_ESC_F3_135"));
            ListaObs.Add(new PorTiposNh("Barra Losa.Esc F3b_45", "LOSA_ESC_F3B_45"));
            ListaObs.Add(new PorTiposNh("Barra Losa.Esc F3B_135", "LOSA_ESC_F3B_135"));


            ListaObs.Add(new PorTiposNh("BArra f1 Incli.","LOSA_INCLI_F1"));
            ListaObs.Add(new PorTiposNh("BArra f3 Incli.","LOSA_INCLI_F3"));
            ListaObs.Add(new PorTiposNh("BArra f4 Incli.","LOSA_INCLI_F4"));
            
            ListaObs.Add(new PorTiposNh("Barra Reparticion","LOSA_SUP_BR"));
            ListaObs.Add(new PorTiposNh("Pata de Barra","LOSA_SUP_BPT"));

            ListaObs.Add(new PorTiposNh("Estribo refuerzo","REFUERZO_ES"));
            ListaObs.Add(new PorTiposNh("Barra Refuerzo","REFUERZO_BA"));
            

            

            return ListaObs;
        }

        internal static List<PorTiposNh> ObtenerTiposFund()
        {

            System.Collections.Generic.List<PorTiposNh> ListaObs = new System.Collections.Generic.List<PorTiposNh>();
            ListaObs.Add(new PorTiposNh("Barra Inf Fundacion", "FUND_BA_INF"));//     ListaObs.Add(new PorTiposNh("Barra Inf Fundacion", "FUND_BA"));
            ListaObs.Add(new PorTiposNh("Barra Sup Fundacion", "FUND_BA_SUP"));

            ListaObs.Add(new PorTiposNh("Estribo Fundacion", "FUND_ES")); 
            ListaObs.Add(new PorTiposNh("Traba Fundacion", "FUND_ES_T"));
                    
            ListaObs.Add(new PorTiposNh("Pata de barra","FUND_SUP_BPT"));//  ListaObs.Add(new PorTiposNh("FUND_BA_BPT"));  borrado
            return ListaObs;
        }




        //**********************************

        internal static List<PorTiposNh> ObtenerTiposViewELev()
        {

            System.Collections.Generic.List<PorTiposNh> ListaObs = new System.Collections.Generic.List<PorTiposNh>();
            ListaObs.Add(new PorTiposNh("ELE_BA_H")); ListaObs.Add(new PorTiposNh("ELEV_BA_V")); ListaObs.Add(new PorTiposNh("ELEV_BA_CABEZA_HORQ")); ListaObs.Add(new PorTiposNh("ELEV_BA_HORQ"));



            return ListaObs;
        }

        internal static List<PorTiposNh> ObtenerTipoViewLosa()
        {

            System.Collections.Generic.List<PorTiposNh> ListaObs = new System.Collections.Generic.List<PorTiposNh>();
            ListaObs.Add(new PorTiposNh("LOSA_INF")); ListaObs.Add(new PorTiposNh("LOSA_SUP")); ListaObs.Add(new PorTiposNh("LOSA_SUP_S1")); ListaObs.Add(new PorTiposNh("LOSA_SUP_S2"));

            return ListaObs;
        }

        internal static List<PorTiposNh> ObtenerTiposViewFund()
        {

            System.Collections.Generic.List<PorTiposNh> ListaObs = new System.Collections.Generic.List<PorTiposNh>();
            ListaObs.Add(new PorTiposNh("FUND_BA"));

            return ListaObs;
        }

        //***********************************
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }


}
