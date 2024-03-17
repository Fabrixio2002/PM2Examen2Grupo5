using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace Examen;

public partial class ListaSitios : ContentPage
{
    private HttpClient _httpClient;
    string id;
    public ListaSitios()
    {
        InitializeComponent();
        _httpClient = new HttpClient();
        LoadSitios();
    }



    private async void LoadSitios()
    {
        try
        {
            var sitios = await GetPosts();
            sitiosCollectionView.ItemsSource = sitios;
        }
        catch (Exception ex)
        {
            // Manejar errores aquí, por ejemplo, mostrar un mensaje al usuario
            Console.WriteLine($"Error al cargar los sitios: {ex.Message}");
        }
    }


    public async static Task<List<Sitios>> GetPosts()
    {
        List<Sitios> posts = new List<Sitios>();

        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage responseMessage = await client.GetAsync("https://w1h73shc-7163.use.devtunnels.ms/api/Sitios");

                if (responseMessage.IsSuccessStatusCode)
                {
                    string result = await responseMessage.Content.ReadAsStringAsync();
                    posts = JsonConvert.DeserializeObject<List<Sitios>>(result);
                }
                else
                {
                    // Si la solicitud no fue exitosa, lanzar una excepción o manejar el error de otra manera
                    throw new HttpRequestException($"Error al realizar la solicitud GET: {responseMessage.StatusCode}");
                }
            }

            return posts;
        }
        catch (Exception ex)
        {
            // Manejar excepciones
            Console.WriteLine($"Error al obtener los sitios: {ex.Message}");
            throw; // Lanza la excepción para que sea manejada por el código que llama a este método
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Actualizar());//Para cambiar de Pantalla

    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        if (sitiosCollectionView.SelectedItem is Sitios selectedSitio)
        {
            int sitioId = selectedSitio.Id;
            bool answer = await DisplayAlert("Confirmar eliminación", "¿Estás seguro de que deseas eliminar este sitio?", "Sí", "No");

            if (answer)
            {
                bool result = await DeleteSitio(sitioId);

                if (result)
                {
                     LoadSitios();
                    await DisplayAlert("Éxito", "El sitio se ha eliminado correctamente.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Ha ocurrido un error al eliminar el sitio.", "OK");
                }
            }
        }
    }

    private void Button_Clicked_2(object sender, EventArgs e)
    {

    }

    private async void sitiosCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        btnEliminar.IsEnabled = e.CurrentSelection.Count > 0;

    }

    private async Task<bool> DeleteSitio(int sitioId)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.DeleteAsync($"https://w1h73shc-7163.use.devtunnels.ms/api/Sitios/{sitioId}");

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                // Manejar excepciones
                Console.WriteLine($"Error al eliminar el sitio: {ex.Message}");
                return false;
            }
        }
    }
