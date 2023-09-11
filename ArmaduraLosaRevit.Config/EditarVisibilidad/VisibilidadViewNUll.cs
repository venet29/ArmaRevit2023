using System;

namespace ArmaduraLosaRevit.ConfigurarParametro.EditarVisibilidad
{
    public class VisibilidadViewNUll : IVisibilidadView
    {
       // private bool _isOKVisibilidad;
        public VisibilidadViewNUll()
        {
        }

        public void AsignarVisibilityBuiltInCategory(bool estado) { }

        public void CambiarVisibilityBuiltInCategory() { }
        public bool IsOKVisibilidad() => false;

       public bool EstadoActualHide() => false;

        public void CambiarVisibilityBuiltInCategory(bool cambiarEstado)
        {
          
        }

        public void verTodasCategorias() {}
    }
}