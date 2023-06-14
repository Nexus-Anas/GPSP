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
            map.MapClicked += OnMapClicked;
            mapContainer.Content = map;
        }
    }




    private void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        var map = (MyMap)sender;

        // Create a new pin at the clicked location
        var pin = new Pin
        {
            Label = "Custom Pin",
            Type = PinType.Generic,
            Location = e.Location
        };

        // Add the pin to the map
        map.Pins.Add(pin);

        // Check if the new pin is within proximity of other pins
        var pinsWithinProximity = map.Pins.Where(p => p != pin && CalculateDistance(p.Location, pin.Location) <= 100);

        if (pinsWithinProximity.Count() >= 2)
        {
            // Calculate the center point of the pins within proximity
            var centerLocation = GetCenterLocation(pinsWithinProximity.Select(p => p.Location));

            // Check if the new pin falls within the existing circle
            var existingCircle = map.MapElements.OfType<Circle>().FirstOrDefault();
            if (existingCircle != null && CalculateDistance(centerLocation, pin.Location) <= existingCircle.Radius.Meters)
            {
                // The new pin falls within the existing circle, no need to create another circle
                return;
            }

            // Remove any existing circles from the map
            map.MapElements.Remove(existingCircle);

            // Create a circle overlay with a radius of 100 meters
            var circle = new Circle
            {
                Center = centerLocation,
                Radius = new Distance(100),
                StrokeColor = Colors.Red,
                StrokeWidth = 2f
            };

            // Add the circle overlay to the map
            map.MapElements.Add(circle);
        }
    }














    private double CalculateDistance(Location location1, Location location2)
    {
        // Calculate the distance between the two locations using the Haversine formula
        const double earthRadius = 6371000; // Earth's radius in meters
        double lat1 = location1.Latitude;
        double lon1 = location1.Longitude;
        double lat2 = location2.Latitude;
        double lon2 = location2.Longitude;

        double dLat = DegreesToRadians(lat2 - lat1);
        double dLon = DegreesToRadians(lon2 - lon1);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        double distance = earthRadius * c;
        return distance;
    }

    private double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }

    private Location GetCenterLocation(IEnumerable<Location> locations)
    {
        double sumLatitude = 0;
        double sumLongitude = 0;
        int count = 0;

        foreach (var location in locations)
        {
            sumLatitude += location.Latitude;
            sumLongitude += location.Longitude;
            count++;
        }

        double averageLatitude = sumLatitude / count;
        double averageLongitude = sumLongitude / count;

        return new Location(averageLatitude, averageLongitude);
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