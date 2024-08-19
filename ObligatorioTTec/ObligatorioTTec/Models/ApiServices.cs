using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObligatorioTTec.Models;
using System.ComponentModel;


namespace ObligatorioTTec.Models
{
        public class ApiService
        {
            private readonly HttpClient _httpClient;

            public ApiService()
            {
                _httpClient = new HttpClient

                {
                    BaseAddress = new Uri("https://localhost:7085/api/"), // Replace with your API's base URL
                    MaxResponseContentBufferSize = 1024,
                    Timeout = TimeSpan.FromSeconds(30),

                    
                };
            }

            public async Task<List<Usuario>> GetItemsAsync()
            {
                var response = await _httpClient.GetAsync("Usuarios");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Usuario>>(json);
                }
                return null;
            }
        public async Task<bool> RegisterUser(Usuario usuario)
        {
            var json = JsonConvert.SerializeObject(usuario);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");
            var respone = await _httpClient.PostAsync("Usuarios/RegisterUser", contenido);
            return respone.IsSuccessStatusCode; 
        }
        
        public async Task<bool> VerifyLogin(string correo, string password)
        {
            loginData data = new loginData
            {
                correo = correo,
                password = password
            };
            var json = JsonConvert.SerializeObject(data);
            var contenido = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                Console.WriteLine("Sending request to API...");
                var response = await _httpClient.PostAsync("Usuarios/VerifyLogin", contenido);
                Console.WriteLine("Request sent.");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
            }
            catch (TaskCanceledException ex)
            {
                // This might indicate a timeout
                Console.WriteLine($"TaskCanceledException (possibly a timeout): {ex.Message}");
            }
            catch (Exception ex)
            {
                // Catch any other exceptions
                Console.WriteLine($"Exception: {ex.Message}");
            }

            //try
            //{
            //    var respone = await _httpClient.PostAsync("Usuarios/VerifyLogin", contenido);
            //    if (respone.IsSuccessStatusCode)
            //    {
            //        var jsonResponse = await respone.Content.ReadAsStringAsync();
            //        var usuario = JsonConvert.DeserializeObject<Usuario>(jsonResponse);
            //        //CurrentUser.usuario = usuario;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // Log any exceptions that occur
            //    Console.WriteLine($"Exception: {ex.Message}");
            //    Console.WriteLine($"StackTrace: {ex.StackTrace}");
            //}
            
            
            return false ;
        }
    }
    
}
