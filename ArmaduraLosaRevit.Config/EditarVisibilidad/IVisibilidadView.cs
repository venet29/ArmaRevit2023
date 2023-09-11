using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad
{
    public interface IVisibilidadView
    {
        void AsignarVisibilityBuiltInCategory(bool estado);
        void CambiarVisibilityBuiltInCategory();
        void CambiarVisibilityBuiltInCategory(bool cambiarEstado);
        bool EstadoActualHide();
        bool IsOKVisibilidad();
        void verTodasCategorias();
    }
}
