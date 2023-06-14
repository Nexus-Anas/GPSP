using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Text.Json;
using MyMap = Microsoft.Maui.Controls.Maps.Map;
namespace GPSP;

public partial class MapPage : ContentView
{
	public MapPage()
	{
		InitializeComponent();

        //var location = new Location(34.010183, -6.847194);
        //var mapSpan = new MapSpan(location, 0.01, 0.01);
        //var map = new MyMap(mapSpan) { IsShowingUser = true};

        //var pin = new Pin
        //{
        //    Label = "Rabat",
        //    Address = "Morocco's Capital",
        //    Type = PinType.Place,
        //    Location = new Location(34.010183, -6.847194)
        //};
        //map.Pins.Add(pin);
        //mapContainer.Content = map;
        LoadMapWithLocation();
    }

    private async void LoadMapWithLocation()
    {
        var location = await Geolocation.GetLastKnownLocationAsync();
        if (location != null)
        {
            var mapSpan = new MapSpan(new Location(location.Latitude, location.Longitude), 0.005, 0.005);
            var map = new MyMap(mapSpan) { IsShowingUser = true };

            var pin = new Pin
            {
                Label = "My Location",
                Type = PinType.Place,
                Location = new Location(location.Latitude, location.Longitude)
            };

            map.Pins.Add(pin);
            mapContainer.Content = map;
        }
    }


    private void btnSearch_Clicked(object sender, EventArgs e) => SearchLocation();

    private void SearchLocation()
    {
        string searchQuery = txtSearch.Text;
        string apiKey = "AIzaSyAvhOjGXJpnyKOHH8Lxu53My2gr2f2pTDw"; // Replace with your actual geocoding API key

        // Create an HttpClient to make the API request
        using (HttpClient client = new HttpClient())
        {
            try
            {
                // Construct the request URL with the search query and API key
                string requestUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={searchQuery}&key={apiKey}";

                // Send the request and get the response
                string response = client.GetStringAsync(requestUrl).Result;

                // Parse the JSON response
                var result = JsonSerializer.Deserialize<GeocodingResponse>(response);

                // Process the results
                if (result.status == "OK" && result.results.Length > 0)
                {
                    var location = result.results[0].geometry.location;
                    double latitude = location.lat;
                    double longitude = location.lng;

                    // Do something with the latitude and longitude, such as updating the map or displaying the result
                    // Example: Display an alert with the coordinates
                    Application.Current.MainPage.DisplayAlert("Search Result", $"Latitude: {latitude}, Longitude: {longitude}", "OK");
                }
                else
                {
                    // Handle the case when no results are found
                    Application.Current.MainPage.DisplayAlert("Search Result", "No results found", "OK");
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur during the search
                Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }

}