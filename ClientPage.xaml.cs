using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Threading.Tasks;


namespace GPSP;

public partial class ClientPage : ContentView
{
	public ClientPage()
	{
		InitializeComponent();
	}

    private async void btnSubmit_Clicked(object sender, EventArgs e)
    {
        string fname = txtFirstName.Text;
        string lname = txtLastName.Text;
        string phone = txtPhone.Text;
        string address = txtAddress.Text;
        if (fname is null || lname is null || phone is null || address is null)
        {
            await DisplayToast("Empty fields, check again!");
            return;
        }
        //await Navigation.PushAsync(new InfoPage($"{fname} {lname}"));
        await DisplayToast("Client Created!");
    }

    private async Task DisplayToast(string txt)
    {
        var cancel = new CancellationTokenSource();
        var duration = ToastDuration.Short;
        double fontsize = 14;
        var toast = Toast.Make(txt, duration, fontsize);
        await toast.Show(cancel.Token);
    }

    private async void btnExit_Clicked(object sender, EventArgs e)
    {
        var platform = DeviceInfo.Platform;
        var device = (platform == DevicePlatform.WinUI) ? "Windows" : (platform == DevicePlatform.Android) ? "Android" : "";

        var result = await Application.Current.MainPage.DisplayAlert("Exit", $"Are you sure you want to quit {device}?", "Yes", "No");
        if (result)
            App.Current.Quit();
    }
}