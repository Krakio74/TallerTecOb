using Newtonsoft.Json;

/* Unmerged change from project 'ObligatorioTTec (net8.0-windows10.0.19041.0)'
Before:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObligatorioTTec.Models;
using System.ComponentModel;
using Plugin.Maui.Biometric;
After:
using ObligatorioTTec.Models;
using Plugin.Collections.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
*/

/* Unmerged change from project 'ObligatorioTTec (net8.0-android)'
Before:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObligatorioTTec.Models;
using System.ComponentModel;
using Plugin.Maui.Biometric;
After:
using ObligatorioTTec.Models;
using Plugin.Collections.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
*/

/* Unmerged change from project 'ObligatorioTTec (net8.0-maccatalyst)'
Before:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObligatorioTTec.Models;
using System.ComponentModel;
using Plugin.Maui.Biometric;
After:
using ObligatorioTTec.Models;
using Plugin.Collections.Generic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
*/
using Plugin.Maui.Biometric;
using System.Text;


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
            await tomarFoto(usuario.Correo);
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
                var respone = await _httpClient.PostAsync("Usuarios/VerifyLogin", contenido);
                if (respone.IsSuccessStatusCode)
                {
                    if (DeviceInfo.Current.Platform == DevicePlatform.Android || DeviceInfo.Current.Platform == DevicePlatform.iOS)
                    {
                        var result = await BiometricAuthenticationService.Default.AuthenticateAsync(new AuthenticationRequest()
                        {
                            Title = "Por favor autentifique",
                            NegativeText = "Cancelar"
                        }, CancellationToken.None);
                        if (result.Status == BiometricResponseStatus.Success)
                        {
                            var jsonResponse = await respone.Content.ReadAsStringAsync();
                            var usuario = JsonConvert.DeserializeObject<Usuario>(jsonResponse);
                            CurrentUser.usuario = usuario;
                            App.CineDB.SetUsuario(usuario);

                            return true;
                        }
                        return false;
                    }
                    else
                    {
                        var jsonResponse = await respone.Content.ReadAsStringAsync();
                        var usuario = JsonConvert.DeserializeObject<Usuario>(jsonResponse);
                        CurrentUser.usuario = usuario;
                        App.CineDB.SetUsuario(usuario);

                        return true;
                    }

                }
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }


            return false;
        }
        public async Task<string> tomarFoto(string correo)
        {
            var photo = await MediaPicker.CapturePhotoAsync();

            if (photo == null)
                return null;

            // Define the path to the app's data directory
            var appDataDirectory = FileSystem.AppDataDirectory;
            var imagesDirectory = Path.Combine(appDataDirectory, "Images");

            // Ensure the directory exists
            Directory.CreateDirectory(imagesDirectory);

            // Original file path
            var originalFilePath = Path.Combine(imagesDirectory, photo.FileName);

            // Save the photo to the app's data directory
            using (var stream = await photo.OpenReadAsync())
            using (var fileStream = File.OpenWrite(originalFilePath))
            {
                await stream.CopyToAsync(fileStream);
            }

            // Get the file extension
            var extension = Path.GetExtension(originalFilePath);

            // Define new file path with the provided name
            var newFilePath = Path.Combine(imagesDirectory, $"{correo}{extension}");

            // Rename the file
            if (File.Exists(originalFilePath))
            {
                File.Move(originalFilePath, newFilePath);
            }

            return newFilePath;
        }
    }


}
