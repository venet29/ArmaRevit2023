using WinForms = System.Windows.Forms;
using System.Diagnostics;

namespace ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad.Ayuda
{
    public class UtilVisibilidad
    {
        const string _caption = "Mensajeria";
        public static void ErrorMsg(string msg)
        {
            Debug.WriteLine(msg);
            WinForms.MessageBox.Show(msg,
              _caption,
              WinForms.MessageBoxButtons.OK,
              WinForms.MessageBoxIcon.Error);
        }

        public static void InfoMsg(string msg)
        {
            Debug.WriteLine(msg);
            WinForms.MessageBox.Show(msg,
              _caption,
              WinForms.MessageBoxButtons.OK,
              WinForms.MessageBoxIcon.Information);
        }
    }
}
