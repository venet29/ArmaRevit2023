using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Cubicacion.Ayuda;
using ArmaduraLosaRevit.Model.Cubicacion.model;

namespace ArmaduraLosaRevit.Model.Cubicacion.Repetir
{
    class EntreElev
    {
       
        private List<LevelDTO> _lista_Level_habilitados;
        private List<LevelDTO> _lista_Level;
        private readonly List<ElevCubDto> _Lista_Elev;

        public List<CubBarra> ListaCubBarra_ConRepetidas { get; internal set; }
        public List<CubBarra> ListaCubBarra_SinElevExluidas { get; internal set; }

        public List<CubBarra> listaCubBarra { get; set; }

        public List<object[]> ListaObjetos { get;  set; }


        public EntreElev(List<CubBarra> listaCubBarra, ManejadorCubDTO manejadorCubDTO)
        {
            this._Lista_Elev = new List<ElevCubDto>();
            if (manejadorCubDTO.lista_Elev != null)
                this._Lista_Elev = manejadorCubDTO.lista_Elev;

            this._lista_Level = new List<LevelDTO>();
            if (manejadorCubDTO.lista_view != null)
                this._lista_Level = manejadorCubDTO.lista_view;

            this.listaCubBarra = new List<CubBarra>();
            this.listaCubBarra.AddRange( listaCubBarra);

            this.ListaObjetos = new List<object[]>();

            _lista_Level_habilitados = _lista_Level.Where(c => c.IsSelected).ToList();
            ListaCubBarra_ConRepetidas = new List<CubBarra>();
            ListaCubBarra_SinElevExluidas = new List<CubBarra>();
        }



        internal bool CopiarEntreaElev()
        {
            if (_Lista_Elev.Count == 0) return false;

            try
            {
                var listareperido = _Lista_Elev.Where(c => c.IsSelected && c.DequienSeCopiaStr != "").ToList();

                if (listareperido.Count == 0) return false;

                foreach (var item in listareperido)
                {

                    string nombreViewaCopiar = item.Nombre;

                    nombreViewaCopiar = AyudaObtenerNombreEjeElev.ObtenerSufijo(nombreViewaCopiar);

                    string tituloBase = AyudaObtenerNombreEjeElev.ObtenerPrefijo(item.Nombre);

                    var listaDeEjesParaCopiar = nombreViewaCopiar.Split('=').Select(c => tituloBase + " " + c).ToList();

                    var lst = (from rp in listaDeEjesParaCopiar
                               from lb in listaCubBarra
                               where (lb.Eje == item.Nombre)
                               select lb.CopiarDesde(rp+"*")).ToList();

                    ListaCubBarra_ConRepetidas.AddRange(lst);
                }

      
                //lista 1
                BorrarBarrasAntiguasDeLista(listareperido);
            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'CopiarEntreaElev' ex:{ex.Message}");
                return false; ;
            }
            return true;
        }

        private bool BorrarBarrasAntiguasDeLista(List<ElevCubDto> listareperido)
        {
            if (ListaCubBarra_ConRepetidas.Count == 0) return false;

            var listaCOnNombre = listareperido.Select(c => c.Nombre).ToList();
            //borrar las barras basae
        var lst_barrasBase = (
                                from lb in listaCubBarra
                                where (!listaCOnNombre.Exists(c=> c==lb.Eje))
                                select lb).ToList();

            if (lst_barrasBase.Count == 0) return false;

            listaCubBarra = lst_barrasBase;

            // lista2 
            ListaObjetos.AddRange(listaCubBarra.Select(x => x.ObtenerDato_array()));
            return true;
        }

        internal bool VerificarSiExcluyenElev()
        {
            if (_Lista_Elev.Count == 0) return false;

            try
            {
                var listaElev_Excluidas = _Lista_Elev.Where(c => !c.IsSelected).ToList();
                if (listaElev_Excluidas.Count == 0) return false;

                var listaElev_Analizdos = _Lista_Elev.Where(c => c.IsSelected).Select(x => x.Nombre).ToList();

                ListaCubBarra_SinElevExluidas = listaCubBarra.Where(c => listaElev_Analizdos.Contains(c.Eje)).ToList();

            }
            catch (Exception)
            {
                return false; ;
            }
            return true;
        }
    }
}
