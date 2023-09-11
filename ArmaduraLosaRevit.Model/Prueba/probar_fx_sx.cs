using ArmaduraLosaRevit.Model.ElementoBarraRoom;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.Ayudas;
using ArmaduraLosaRevit.Model.ElementoBarraRoom.PathReinforment.DTO;
using ArmaduraLosaRevit.Model.Enumeraciones;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArmaduraLosaRevit.Model.Prueba
{
    public partial class probar_fx_sx : Form
    {
        public bool IsOK { get; set; }
        public string tipobarra { get; set; }
        public UbicacionLosa direccion { get; set; }
        public bool IsAjusteBarra_Recorrido { get; set; }
        public bool IscambiarLargos { get; private set; }
        public PathSymbol_REbarshape_FxxDTO PathSymbol_REbarshape_FxxDTO_ { get; set; }

        public probar_fx_sx()
        {
            InitializeComponent();
            IsOK = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            tipobarra = comboBox_barra.Text;
            direccion = EnumeracionBuscador.ObtenerEnumGenerico(UbicacionLosa.Derecha, comboBox_Direccion.Text);

            tipobarra=ObtenerCasoAoBDeAhorro.Obtenercaso_16_20_21_22_AoB(tipobarra, direccion);

            IsOK = true;
            IsAjusteBarra_Recorrido = checkBox1_ajustabarra.Checked;

            if (checkBox_cambiarLargos.Checked)
            {
                IscambiarLargos = true;

                PathSymbol_REbarshape_FxxDTO_ = new PathSymbol_REbarshape_FxxDTO()
                {
                    IsOK = true,
                    DesDereInf_foot = ObtenerNUmero(textBox_DesDereInf.Text),
                    DesDereSup_foot = ObtenerNUmero(textBox_DesDereSup.Text),

                    DesIzqInf_foot = ObtenerNUmero(textBox_DesIzqInf.Text),
                    DesIzqSup_foot = ObtenerNUmero(textBox_DesIzqSup.Text),

                    pataIzq_foot = ObtenerNUmero(textBox_pataIzq.Text),
                    pataDere_foot = ObtenerNUmero(textBox_pataDere.Text),

                };

            }
            else IscambiarLargos = false;

            Close();
        }



        private double ObtenerNUmero(string textBox_DesDereInf)
        {
            if (!Util.IsNumeric(textBox_DesDereInf) || textBox_DesDereInf == "0")
            {
                return 0;
            }

            double dist = Util.ConvertirStringInDouble(textBox_DesDereInf);
            return Util.CmToFoot(dist);
        }

        private void comboBox_barra_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        internal CargarBarraRoomDTO ObtenerCargarBarraRoomDTO()
        {
            try
            {


                CargarBarraRoomDTO _CargarBarraRoomDTO = new CargarBarraRoomDTO(tipobarra, direccion);

                if (IscambiarLargos)
                {
                    _CargarBarraRoomDTO.PathSymbol_REbarshape_FxxDTO_ = PathSymbol_REbarshape_FxxDTO_;
                }
                else
                {
                    PathSymbol_REbarshape_FxxDTO _PathSymbol_REbarshape_FxxDTO = new PathSymbol_REbarshape_FxxDTO() { IsOK = false, CopiarFamiliasDiferentesPatas = false };
                    _CargarBarraRoomDTO.PathSymbol_REbarshape_FxxDTO_ = _PathSymbol_REbarshape_FxxDTO;
                }

                return _CargarBarraRoomDTO;
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error al crear datos de diseño  \n  ex:{ex.Message}");
                return null;
            }
        }
    }
}
