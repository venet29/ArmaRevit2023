using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmaduraLosaRevit.Model.Cubicacion.Ayuda;
using ArmaduraLosaRevit.Model.Cubicacion.model;

namespace ArmaduraLosaRevit.Model.Cubicacion.Repetir
{
    class EntreLosas
    {
        private List<CubBarra> listaCubBarra;
        private List<LevelDTO> _lista_Level_habilitados;
        private List<LevelDTO> _lista_Level;
        private readonly List<LosaCubDto> _Lista_Losa;

        public List<CubBarra> ListaCubBarra_ConRepetidas { get; internal set; }
        public List<CubBarra> ListaCubBarra_SinLosaExluidas { get; internal set; }

        public EntreLosas(List<CubBarra> listaCubBarra,  ManejadorCubDTO manejadorCubDTO)
        {
            this._Lista_Losa = new List<LosaCubDto>();
            if (manejadorCubDTO.lista_Losa!= null)
                this._Lista_Losa.AddRange( manejadorCubDTO.lista_Losa);

            this._lista_Level = new List<LevelDTO>();
            if (manejadorCubDTO.lista_view != null)
                this._lista_Level.AddRange(manejadorCubDTO.lista_view);

            this.listaCubBarra = listaCubBarra;
            _lista_Level_habilitados = _lista_Level.Where(c => c.IsSelected).ToList();
            ListaCubBarra_ConRepetidas = new List<CubBarra>();
            ListaCubBarra_SinLosaExluidas = new List<CubBarra>();
        }



        internal bool CopiarEntrealosas()
        {
            if (_Lista_Losa.Count == 0) return false;

            try
            {
                var listareperido = _Lista_Losa.Where(c => c.IsSelected && c.DequienSeCopia.Name != "ninguno" && c.DequienSeCopia.Name != "").ToList();

                foreach (var item in listareperido)
                {
                    string niveloRIGINAL = item.ViewLosa.GenLevel.Name;
                    string nivel = item.ViewLosa.GenLevel.Name;
                    double OrdeElevacion = item.ViewLosa.GenLevel.ProjectElevation;

                    if (NombreDefinidoUsuario.ObtenerNombreDefinidoUsuario(niveloRIGINAL, _lista_Level_habilitados))
                        nivel = NombreDefinidoUsuario.nivelModificado;

                    if (NombreDefinidoUsuario.ObtenerOrdenPorElevacion(niveloRIGINAL, _lista_Level_habilitados))
                        OrdeElevacion = NombreDefinidoUsuario.OrdeElevacion;


                    var listrepetda=listaCubBarra.Where(c => c.Eje == item.DequienSeCopia.Name).Select(x => x.CopiarDesde(item.Nombre+"*", nivel, OrdeElevacion)).ToList();
                    ListaCubBarra_ConRepetidas.AddRange(listrepetda);
                }

            }
            catch (Exception ex)
            {
                Util.ErrorMsg($"Error en 'CopiarEntrealosas' ex:{ex.Message}");
                return false; ;
            }
            return true;
        }

        internal bool VerificarSiExcluyenLosa()
        {
            if (_Lista_Losa.Count == 0) return false;

            try
            {
                var listaLosasExcluidas = _Lista_Losa.Where(c => !c.IsSelected).ToList();
                if (listaLosasExcluidas.Count == 0) return false;


                var listaLosaAnalizdos = _Lista_Losa.Where(c => c.IsSelected).Select(x=> x.Nombre).ToList();

                ListaCubBarra_SinLosaExluidas= listaCubBarra.Where(c => listaLosaAnalizdos.Contains(c.Eje)).ToList();

  
            }
            catch (Exception)
            {

                return false; ;
            }
            return true;
        }
    }
}
