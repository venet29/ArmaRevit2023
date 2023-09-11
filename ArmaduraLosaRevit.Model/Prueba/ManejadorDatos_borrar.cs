using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.Prueba
{
    //C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7\System.Management.dll

    public class ManejadorDatos
    {
      //  private static string url;
      //  private static HttpWebRequest request;
        public ResultDTO resultnh { get; set; }
        private HttpClient _httpClient;
        private Conterrors _errors;
        private object Debun;

        public ManejadorDatos()
        {
            //url = $"https://devnh.site/api/tasks";

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://apirestmembresia-nh.herokuapp.com/");
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
            _httpClient.DefaultRequestHeaders.Clear();
        }

        //a)usuario 
        public bool PostBitacora(string _comando)
        {
            try
            {
                //caso ejemrjencia
                resultnh = new ResultDTO() { Isok = true, msg = "carga forzada" };
                return true;

                Task.Run(async () => await PostBitacoraAsync(_comando)).Wait();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error inscripc ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public async Task PostBitacoraAsync(string _comando)
        {
            try
            {

                InfoSystema _InfoSystema = new InfoSystema();
                _InfoSystema.EjecutarInfoSistem();
                _InfoSystema.M3_getMacAddress3();

                BitacoraDTO _usuariosDTO = new BitacoraDTO()
                {
                    idMAC = _InfoSystema.mac,
                    iPUsuario = "cargado",
                    usuario = _InfoSystema.usuario,
                    comando = _comando
                };

                var jsonResul = JsonConvert.SerializeObject(_usuariosDTO);
                HttpContent httpContent = new StringContent(jsonResul, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("api/bitacora", httpContent);


                if (response.IsSuccessStatusCode)
                {
                    var result = response.EnsureSuccessStatusCode();
                    var json = await response.Content.ReadAsStringAsync();
                    resultnh = JsonConvert.DeserializeObject<ResultDTO>(json);
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    _errors = JsonConvert.DeserializeObject<Conterrors>(json);
                    resultnh = new ResultDTO()
                    {
                        Isok = false,
                        msg = _errors.errors[0].msg
                    };
                }

            }
            catch (WebException ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                resultnh = new ResultDTO() {
                    msg = $" error catch ex:{ex.Message}",
                    Isok = false
                };
            }

        }


        public bool GetBitacora()
        {
            try
            {
                Task.Run(async () => await GetBitacoraAsync()).Wait();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error inscripc ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public async Task GetBitacoraAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/bitacora");
                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                    var json = await response.Content.ReadAsStringAsync();

                    var _usuariosDTO = new ListaBitacoraDTO();
                    if (response.Content.Headers.ContentType.MediaType == "application/json")
                    {
                        _usuariosDTO = JsonConvert.DeserializeObject<ListaBitacoraDTO>(json);
                    }
                    else if (response.Content.Headers.ContentType.MediaType == "application/xml")
                    {
                    }
                }
                else
                { }
            }
            catch (WebException ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
            }
        }

        //b) inscripcion

        public bool PostInscripcion(string empresa)
        {
            try
            {
                //caso ejemrjencia
                //resultnh = new ResultDTO() {Isok=true,msg="carga forzada" };
                //return true;
                Task.Run(async () => await PostInscripcionASync(empresa)).Wait();
            }
            catch (Exception ex )
            {
                Debug.WriteLine($"error inscripc ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public async Task PostInscripcionASync(string empresa)
        {
            try
            {

                InfoSystema _InfoSystema = new InfoSystema();
                _InfoSystema.EjecutarInfoSistem();

                InscripcionDTO _inscripcionDTO = new InscripcionDTO()
                {
                    idMAC = _InfoSystema.mac,
                    iPUsuario = _InfoSystema.IpPublic,
                    usuario = _InfoSystema.usuario,
                    EmpresaEjecutable = empresa
                };

                var jsonResul = JsonConvert.SerializeObject(_inscripcionDTO);
                HttpContent httpContent = new StringContent(jsonResul, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("api/inscripcion", httpContent);


                if (response.IsSuccessStatusCode)
                {
                    var result = response.EnsureSuccessStatusCode();
                    var json = await response.Content.ReadAsStringAsync();
                    resultnh = JsonConvert.DeserializeObject<ResultDTO>(json);
                }
                else
                {
                    var json = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode ==HttpStatusCode.OK) 
                    {
                        resultnh = JsonConvert.DeserializeObject<ResultDTO>(json);
                    }
                    else
                    {
                        _errors = JsonConvert.DeserializeObject<Conterrors>(json);
                        resultnh = new ResultDTO()
                        {
                            Isok = false,
                            msg = _errors.errors[0].msg
                        };
                    }
                    
                }

            }
            catch (WebException ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
                resultnh = new ResultDTO()
                {
                    msg = $" error catch ex:{ex.Message}",
                    Isok = false
                };
            }

        }

        public bool GetInscripcion()
        {
            try
            {
                Task.Run(async () => await GetInscripcionASync()).Wait();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"error inscripc ex:{ex.Message}");
                return false;
            }
            return true;
        }
        public async Task GetInscripcionASync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/inscripcion");
                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                    var json = await response.Content.ReadAsStringAsync();

                    var _ListaInscripcionDTO = new ListaInscripcionDTO();
                    if (response.Content.Headers.ContentType.MediaType == "application/json")
                    {
                        _ListaInscripcionDTO = JsonConvert.DeserializeObject<ListaInscripcionDTO>(json);
                    }
                    else if (response.Content.Headers.ContentType.MediaType == "application/xml")
                    {
                    }
                }
                else
                { }
            }
            catch (WebException ex)
            {
                Debug.WriteLine($" ex:{ex.Message}");
            }
        }
    }

    //moel
    #region model

    public class Conterrors
    {
        public errors[] errors { get; set; }

    }

    public class errors
    {
        public long value { get; set; }
        public string msg { get; set; }
        public string param { get; set; }
        public string location { get; set; }
    }

    public class ResultDTO
    {
        public string msg { get; set; }
        public bool Isok { get; set; }
        public ResultDTO()
        {
            msg = "conexion fallida";
            Isok = false;
        }
    }

    public class ListaBitacoraDTO
    {
        public string msg { get; set; }
        public List<BitacoraDTO> bitacoras { get; set; }

    }

    public class BitacoraDTO
    {
        public string idMAC { get; set; }
        public string iPUsuario { get; set; }
        public string usuario { get; set; }
        public string comando { get; set; }
        public bool Isok { get; set; }
        public int __v { get; set; }
        public string _id { get; set; }

    }

    public class ListaInscripcionDTO
    {
        public string msg { get; set; }
        public List<InscripcionDTO> Inscripcions { get; set; }

    }

    public class InscripcionDTO
    {
        public string idMAC { get; set; }
        public string iPUsuario { get; set; }
        public string usuario { get; set; }
        public string EmpresaEjecutable { get; set; }
        public bool    IsPermitido { get; set; }
        public int __v { get; set; }
        public string _id { get; set; }

    }

    public class InfoSystema
    {
        #region 0)propieades
        public string disco { get; set; }
        public string mac { get; set; } //realmente es la direccion o numero de la placa madre
        public string IpPublic { get; set; }
        public string usuario { get; set; }
        public string ruta { get; set; }
        public string caso { get; set; }
       

        #endregion
        #region 1)Contructor
        public InfoSystema(string ruta, string caso)
        {
            this.ruta = ruta;
            this.caso = caso;
            EjecutarInfoSistem();
        }
        public InfoSystema() { }

        #endregion

        #region Metodos Sitema

        public bool EjecutarInfoSistem()
        {

            try
            {
                // OBTENER INFO DISCO DURO
                string consultaSQLArquitectura = "SELECT * FROM Win32_Processor";
                ManagementObjectSearcher objArquitectura = new ManagementObjectSearcher(consultaSQLArquitectura);
                ManagementObject serialDD = new ManagementObject(@"Win32_PhysicalMedia='\\.\PHYSICALDRIVE0'");

                foreach (PropertyData prop in serialDD.Properties)
                {
                    Console.WriteLine("{0}: {1}", prop.Name, prop.Value);
                }

                disco = serialDD.GetPropertyValue("SerialNumber").ToString();
                usuario = Environment.UserName;

                M3_getMacAddress3();
            }
            catch (Exception)
            {

                return false;
            }
            return true;

        }

        public bool M3_getMacAddress3()
        {
            try
            {
                ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select * From Win32_BaseBoard");
                foreach (ManagementObject getserial in MOS.Get())
                {
                    mac = getserial["SerialNumber"].ToString();  // realmente placa madre
                }
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
        #endregion

    }

    #endregion
}
