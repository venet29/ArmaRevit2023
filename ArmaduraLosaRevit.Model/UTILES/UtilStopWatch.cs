using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.UTILES
{

    //UtilStopWatch _utilStopWatch = new UtilStopWatch();
    //_utilStopWatch.IniciarMedicion();

    //            _utilStopWatch.Terminar($"cont: ----------> a) OBtenerRefrenciaMuroOViga_paraAuto", false)
    //            _utilStopWatch.StopYContinuar($"cont: ----------> a) OBtenerRefrenciaMuroOViga_paraAuto", false);
    //            _utilStopWatch.TerminarYGuardarTxTTiempoTotal("dentro M2_1_ObtenerDatosEstribo");
    public class UtilStopWatch
    {
        private  Stopwatch _stopwatchTiempoIntervalo;
        private Stopwatch _stopwatchTiempoTotal;

        public List<string> _listIntervalos  { get; set; }
        TimeSpan timeSpanAnterior;
        TimeSpan DeltaTimeSpan;
        public UtilStopWatch()
        {


            _stopwatchTiempoIntervalo = new Stopwatch();
            _stopwatchTiempoTotal = new Stopwatch();
            _listIntervalos = new List<string>();
            timeSpanAnterior = new TimeSpan();
        }
        public void IniciarMedicion(string mje="")
        {
            Debug.WriteLine(mje);
           _stopwatchTiempoIntervalo.Start();
            _stopwatchTiempoTotal.Start();
        }

        public  void Terminar(string mejse="",bool IsCOnformato=true)
        {
            if (_stopwatchTiempoIntervalo == null) return;
            _stopwatchTiempoIntervalo.Stop();


      

            if (timeSpanAnterior == null)
            {
                timeSpanAnterior = _stopwatchTiempoIntervalo.Elapsed;
                DeltaTimeSpan = _stopwatchTiempoIntervalo.Elapsed;
            }
            else
            {
                DeltaTimeSpan = _stopwatchTiempoIntervalo.Elapsed - timeSpanAnterior;
                timeSpanAnterior = _stopwatchTiempoIntervalo.Elapsed;
            }


            if (IsCOnformato)
            {
                Debug.WriteLine("{0} : {1:hh\\:mm\\:ss}   -- Delta:{2:hh\\:mm\\:ss}", mejse, _stopwatchTiempoIntervalo.Elapsed, DeltaTimeSpan);
                _listIntervalos.Add(String.Format("{0} : {1:hh\\:mm\\:ss} -- Delta:{2:hh\\:mm\\:ss}", mejse, _stopwatchTiempoIntervalo.Elapsed,DeltaTimeSpan));
            }
            else
            {
                Debug.WriteLine("{0} : {1}   -- Delta:{2}", mejse, _stopwatchTiempoIntervalo.Elapsed, DeltaTimeSpan);
                _listIntervalos.Add(String.Format("{0} : {1}   -- Delta:{2}", mejse, _stopwatchTiempoIntervalo.Elapsed, DeltaTimeSpan));
            }
        }
        public  void StopYContinuar(string txt = "", bool IsCOnformato = true)
        {
            Terminar(txt, IsCOnformato);

            _stopwatchTiempoIntervalo.Start();
        }


        public void TerminarYGuardarTxTTiempoTotal(string nombreArchivo,string ruta="")
        {
            _stopwatchTiempoTotal.Stop();
            _listIntervalos.Add(String.Format("{0} : {1:hh\\:mm\\:ss}", "Tiempo Final Cierre", _stopwatchTiempoTotal.Elapsed));
            LogNH.Limpiar_sbuilder();
            foreach (var item in _listIntervalos)
            {
                LogNH.Agregar_registro(item);
            }
            if(ruta=="")
                LogNH.Guardar_registro(nombreArchivo+"_"+ DateTime.Now.ToString("MM_dd_yyyy Hmmss"));
            else
                LogNH.Guardar_registro(nombreArchivo + "_" + DateTime.Now.ToString("MM_dd_yyyy Hmmss"), ruta);
        }

        public  void TerminarMiliseg(string txt = "")
        {
            if (_stopwatchTiempoIntervalo == null) return;
            _stopwatchTiempoIntervalo.Stop();
            _listIntervalos.Add(String.Format("{0} : {1:hh\\:mm\\:ss}", "Tiempo Final Cierre", _stopwatchTiempoTotal.Elapsed));
            Debug.WriteLine("{0} : {1}", txt, _stopwatchTiempoIntervalo.Elapsed);
        }

        public string Terminar_formatoMje()
        {
            TimeSpan ts = _stopwatchTiempoTotal.Elapsed;
            _stopwatchTiempoTotal.Stop();
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            return elapsedTime;
        }

    }
}
