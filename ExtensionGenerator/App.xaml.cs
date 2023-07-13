using TabbedPageSample;

namespace ExtensionGenerator;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppTabbedPage();
	}
}

