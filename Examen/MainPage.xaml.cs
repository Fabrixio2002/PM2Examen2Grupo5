using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace Examen
{
    public partial class MainPage : ContentPage
    {
        private HttpClient _httpClient;
        FileResult photo;
        public MainPage()
        {
            InitializeComponent();
            _httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(300) // Establecer el tiempo de espera a 30 segundos
            };
        }


        public string GetImagen()
        {
            if (photo != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Stream stream = photo.OpenReadAsync().Result;
                    stream.CopyTo(ms);
                    byte[] data = ms.ToArray();

                    String Base64 = Convert.ToBase64String(data);

                    return Base64;
                }
            }
            return null;
        }


        public byte[] GetImagenArray()
        {
            if (photo != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Stream stream = photo.OpenReadAsync().Result;
                    stream.CopyTo(ms);
                    byte[] data = ms.ToArray();



                    return data;
                }
            }
            return null;
        }






        //METODO POST PARA ENVIAR DATOS A NUESTRA BASE DE DATOS
        private async void Agregar_Clicked(object sender, EventArgs e)
        {
            try
            {
                // URL de la API
                var url = "https://w1h73shc-7163.use.devtunnels.ms/api/Sitios";

                // Obtener la ubicación actual
                var request = new GeolocationRequest(GeolocationAccuracy.Default);
                var location = await Geolocation.GetLocationAsync(request);
                var latitude = location?.Latitude ?? 0;
                var longitude = location?.Longitude ?? 0;
                txtla.Text = latitude.ToString();
                txtlo.Text = longitude.ToString();

                double LatidDou = Convert.ToDouble(txtla.Text);
                double longiDou = Convert.ToDouble(txtlo.Text);

                // Crear un objeto de ejemplo de Sitios
                var sitio = new Sitios
                {
                    Id = 0,
                    Descripcion = txtarea.Text,
                    Latitud = LatidDou,
                    Longitud = longiDou,
                    Audio = "ruta/audio",
                    Foto =GetImagen()
                };

                // Serializar el objeto a JSON
                var jsonData = JsonSerializer.Serialize(sitio);

                // Configurar el contenido de la solicitud
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Realizar la solicitud HTTP POST
                var response = await _httpClient.PostAsync(url, content);

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer y procesar el contenido de la respuesta
                   // var responseContent = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Datos Enviados","DATOS GUARADOS EN API", "Aceptar");
                }
                else
                {
                    // Mostrar un mensaje de alerta si la solicitud no fue exitosa
                    await DisplayAlert("Error", "Hubo un problema al enviar los datos", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ocurrió una excepción", ex.Message, "Aceptar");
            }
        }



        private void btnlist_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ListaSitios());//Para cambiar de Pantalla

        }

        private async void fotico_Clicked(object sender, EventArgs e)
        {
            photo = await MediaPicker.CapturePhotoAsync();
            if (photo != null)
            {
                try
                {
                    // Obtener la ruta del directorio de caché para guardar la imagen
                    string filePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    // Reducir el tamaño de la imagen (opcional)
                    // var resizedPhoto = await ResizeImage(photo);

                    // Crear un nuevo FileStream para guardar la imagen de forma asíncrona
                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
                    {
                        // Abrir el stream de la foto
                        using (Stream sourcephoto = await photo.OpenReadAsync())
                        {
                            // Copiar el contenido del stream de la foto al FileStream de forma asíncrona
                            await sourcephoto.CopyToAsync(fileStream);
                        }
                    }

                    // Mostrar la imagen en el control Image después de que se haya guardado completamente
                    foto.Source = ImageSource.FromFile(filePath);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Error al guardar la imagen: {ex.Message}", "Aceptar");
                }
            }
        }

      


    }
}
