namespace GPSP;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}

 //   private ContentView currentContentView;
 //   private void btnClient_Clicked(object sender, EventArgs e)
	//{
 //       currentContentView = new ClientPage();
 //       Container.Content = currentContentView;
 //   }
 //   private void btnMap_Clicked(object sender, EventArgs e)
 //   {
 //       currentContentView = new MapPage();
 //       Container.Content = currentContentView;
 //   }

    private void radioClient_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
		if (radioClient.IsChecked)
            Container.Content = new ClientPage();
    }

    private void radioMap_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (radioMap.IsChecked)
            Container.Content = new MapPage();
    }
}

