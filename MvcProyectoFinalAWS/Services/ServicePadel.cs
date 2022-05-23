using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NugetPadelAWS_DSC;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace MvcProyectoFinalAWS.Services
{
    public class ServicePadel
    {
        private string UrlApi;
        private MediaTypeWithQualityHeaderValue Header;

        public ServicePadel()
        {
            this.UrlApi = "https://u9w863fhoh.execute-api.us-east-1.amazonaws.com/Prod";
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }
        //Get token
        public async Task<string> GetTokenAsync(string mail, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Usuario usuario = new Usuario
                {
                    Mail = mail,
                    Password = password
                };
                string json = JsonConvert.SerializeObject(usuario);
                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                string request = "/Prod/api/Auth/Login";
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    JObject jObject = JObject.Parse(data);
                    string token = jObject.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }

        //Get Generico
        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data =
                        await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }

        }

        //Get Generico con Token
        private async Task<T> CallApiAsync<T>(string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }

            }
        }



        //Todos los campos
        public async Task<List<CampoPadel>> GetTodosCampos()
        {
            string request = "/Prod/api/Campos/GetTodosCampos";
            List<CampoPadel> campos =
                await this.CallApiAsync<List<CampoPadel>>(request);
            return campos;
        }

        //FindCampo
        public async Task<CampoPadel> FindCampo(int idcampo)
        {
            string request = "/Prod/api/Campos/GetCampo/" + idcampo;
            CampoPadel campo =
                await this.CallApiAsync<CampoPadel>(request);
            return campo;
        }




        //Get partidos de usuario [Authorize]

        public async Task<List<Partido>> GetPartidosUsuario(int idusuario, string token)
        {
            string request = "/Prod/api/Partidos/GetPartidosUsuario/" + idusuario;
            List<Partido> partidos =
                 await this.CallApiAsync<List<Partido>>(request, token);
            return partidos;

        }


        //Find Partido [Authorize]
        public async Task<List<Partido>> FindPartido(string token, int idpartido)
        {
            string request = "/Prod/api/Partidos/FindPartido/" + idpartido;
            List<Partido> partidos =
                 await this.CallApiAsync<List<Partido>>(request, token);
            return partidos;

        }



        //Get Usuario registrado (obtiene info del token)
        public async Task<Usuario> GetUsuarioToken(string token)
        {
          
            string request = "/Prod/api/Auth/GetUsuarioToken";
            Usuario usuario = await this.CallApiAsync<Usuario>(request, token);
            return usuario;
        }







        //Crear partido
        public async Task CrearPartido(int idcampo, int idpista, DateTime fecha, string horainicio, string horafinal, double precio, int idjugador, string nombrecampo)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/Prod/api/Partidos/CrearPartido";

                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Partido partido = new Partido();
                partido.IdCampo = idcampo;
                partido.IdPista = idpista;
                partido.Fecha = fecha;
                partido.HoraInicio = horainicio;
                partido.HoraFinal = horafinal;
                partido.Precio = precio;
                partido.Jugador1 = idjugador;
                partido.NombrePista = nombrecampo;

                string json = JsonConvert.SerializeObject(partido);
                StringContent content = new StringContent
                    (json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
            await this.EnviarMail();
        }


        

        public async Task CrearUsuario(string username, string mail, string password, string nombre, string apellidos, string pregunta)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/Prod/api/Usuarios/RegistrarUsuario";

                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                Usuario usuario = new Usuario
                {
                    UserName = username,
                    Mail = mail,
                    Password = password,
                    Nombre = nombre,
                    Apellidos = apellidos,
                    PreguntaSeguridad = pregunta
                };
                

                string json = JsonConvert.SerializeObject(usuario);
                StringContent content = new StringContent
                    (json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
           
        }

        public async Task BorrarPartido(int idpartido)
            {

                using (HttpClient client = new HttpClient())
                {
                    string request = "/Prod/api/Partidos/BorrarPartido/" + idpartido;
                    client.BaseAddress = new Uri(this.UrlApi);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(this.Header);
                    HttpResponseMessage response =
                    await client.DeleteAsync(request);
                }
            }

        public async Task BorrarUsuario(int idusuario)
        {

            using (HttpClient client = new HttpClient())
            {
                string request = "/Prod/api/Usuarios/BorrarUsuario/" + idusuario;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                await client.DeleteAsync(request);
            }
        }



        //enviar Mail Partido
        public async Task<string> EnviarMail()
        {
        //    string urlCorreo = "https://prod-230.westeurope.logic.azure.com:443/workflows/b827d7fe674744c2b15c1723b2b575b3/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=_YIMjqzJUIEOUszShltr0enSIvfSoyQVFGOxojTqcA8";
        //    string correo = "diego.sanchezcanamero@tajamar365.com";
        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(this.Header);
        //        Mail mail = new Mail
        //        {
        //            Email = correo,
        //            Body = "Partido Creado! Diviertete y gana",
        //            Subject = "A jugar!"
        //        };
        //        var json = JsonConvert.SerializeObject(mail);
        //        StringContent content =
        //            new StringContent(json, Encoding.UTF8, "application/json");
        //        HttpResponseMessage response =
        //            await client.PostAsync(urlCorreo, content);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            string data =
        //                await response.Content.ReadAsStringAsync();
        //            return "Partido Creado";
        //        }
        //        else
        //        {
                    return null;
        //        }
        //    }
        }

    }
}
  
