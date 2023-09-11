using System.Data;

namespace ArmaduraLosaRevit.Model.BarraV.Automatico.model.Data

{
    public interface IRepositoryDataAccess
    {
        //1) crear tabla
        int CrearTabla(string Tabla, string RutaArchivo, string queryString);
        //2) Borra tabla
        int BorrarTabla(string Tabla, string RutaArchivo);
        //3) select tabla
        DataTable SeleccionTablaADatatble(string RutaArchivo, string queryString);

        //***************************************************************
        //4) agregar
        int AgregarDato(string RutaArchivo, string Tabla, string Campos, string Condicion);
        //5) borrar
        int BorrarDato(string RutaArchivo, string Tabla, string Condicion);
        //6) actualizar
        int UpdateDato(string RutaArchivo, string Tabla, string sentencia, string Condicion);

        //***************************************************************
        //general 'EjecutarSentencia' se opcupa en los puntos 1,2,3,4,5
        int EjecutarSentencia(string queryString, string proveedor_RutaArchivo);
     
        bool Existetabla(string tabla, string RutaArchivo);
    }
}