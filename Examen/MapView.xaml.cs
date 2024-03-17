using Microsoft.Maui.Maps;
using System.Net.NetworkInformation;
using static Microsoft.Maui.ApplicationModel.Permissions;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Controls.PlatformConfiguration;
using static Java.Util.Jar.Attributes;
using Kotlin.Jvm.Internal;
namespace Examen;

public partial class MapView : ContentPage
{
    public Double lat;
    public Double longi; 
    public String desc;
    public MapView(Double latitud, Double longitud,String Desc)
	{
        InitializeComponent();
        lat=latitud;
        longi = longitud;
        desc = Desc;
        Location position = new Location(lat, longi);
        MapSpan mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromMiles(1));

        // Usar el control Map de Microsoft.Maui.Controls.Maps
        var map = new Microsoft.Maui.Controls.Maps.Map(mapSpan);

        Pin pin = new Pin
        {
            Location = position,
            Label = $"{ desc }",
            Address = $"Latitud: {lat}, Longitud: {longi}"

        };

        // Agregar el pin al mapa
        map.Pins.Add(pin);

        // Mover el mapa para que la región se ajuste al pin
        map.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(1)));

        // Agregar el mapa al contenido de la página
        Content = map;
    }


}