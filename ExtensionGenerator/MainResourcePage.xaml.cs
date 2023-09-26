using System.Net.Mime;
using System.Runtime.InteropServices.ComTypes;

namespace ExtensionGenerator;


using Microsoft.Maui.Controls;
using System.IO;

// TODO: Getting drag and drop into to work on different os's is taking longer than expected. Right now a file picker is being used instead  
public partial class MainResourcePage : ContentPage
{
    private readonly int ICON_WIDTH = 15;
    private readonly int RESOURCE_FILENAME_FONT_SIZE = 32;
    private readonly int RESOURCE_FULL_PATH_FONT_SIZE = 24;
    
    Layout topInfoArea;
    Layout savedResourcesDisplayArea;
    

    private DropGestureRecognizer DropGestureRecognizer { get; set; }
    
    private List<string> imageFilePaths = new List<string>();
    
    
    // Color Themes
    private ColorInterpreter colorController = new ("Primary", "Secondary", "Tertiary");
    
    public MainResourcePage()
    {
        InitializeComponent();
        InitializeAsync();
        
        display_saved_resources_area = (VerticalStackLayout)FindByName("display_saved_resources_area");

        
        imageFilePaths = PreferencesManager.GetStringArray(PreferencesManager.STORED_PREFRENCES_IMAGE_PATHS_HELPER_KEY).ToList();
        
        foreach (var imageFilePath in imageFilePaths)
        {
            if (imageFilePath != "")
            {
                Console.WriteLine(imageFilePath);
                

                StackLayout resourceDisplay = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.Center,
                    Background = Colors.Aqua,//TODO: MAKE USE PROGRAMMATIC VALUES!
                    
                    // Margin = new Thickness(0,20,110,20) //TODO: Make programmatic once icon size is working
                    Margin = new Thickness(110,20,0,20), //TODO: Make programmatic once icon size is working
                    AutomationId = imageFilePath.GetHashCode().ToString()
                };
                Image resourceImage = new Image
                {
                    Source = imageFilePath
                };
                //TODO: !!!!!!!!!!! Make multi-platform character switch. HIGH PRIORITY
                string fileName = imageFilePath.Substring(imageFilePath.LastIndexOf("/")+1);
                Label fileNameHeader = new Label
                {
                    Text = fileName,
                    TextColor = Colors.Black,
                    VerticalOptions = LayoutOptions.Start,
                    FontSize = RESOURCE_FILENAME_FONT_SIZE,

                    HorizontalTextAlignment = TextAlignment.Center,
                    
                    // WidthRequest = SETTINGS_WIDTH_LABEL,
                };
                Label fileNameFullPath = new Label
                {
                    Text = imageFilePath,
                    TextColor = Colors.Gray,
                    VerticalOptions = LayoutOptions.Start,
                    FontSize = RESOURCE_FULL_PATH_FONT_SIZE,

                    HorizontalTextAlignment = TextAlignment.Center,
                    
                    // WidthRequest = SETTINGS_WIDTH_LABEL,
                };
                resourceDisplay.Children.Add(resourceImage);
                resourceDisplay.Children.Add(fileNameHeader);
                resourceDisplay.Children.Add(fileNameFullPath);
                
                
                display_saved_resources_area.Children.Add(resourceDisplay);


                // savedResourcesDisplayArea.Children.Add(resourceDisplay);
            }
        }
    }
    

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // if (GeneratorButton.Parent is HorizontalStackLayout footer)
        // {
        //     footer.BackgroundColor = Colors.Transparent;//Does not work
        // }
    }

    private async void InitializeAsync()
    {
        
    }
    
    private async void OnAddImageFilePathToMainResourcesPicker(object sender, EventArgs e){
        var fileResult = await FileManager.userPickFile(new ContentType("application/image")); //Code below does not run if option was canceled, even if null
        if (fileResult == null)
        {
            return;
        } //Here just for just in case

        Console.WriteLine("fileResult = " + fileResult.FullPath);
        imageFilePaths.Add(fileResult.FullPath);
        PreferencesManager.SaveStringArray(PreferencesManager.STORED_PREFRENCES_IMAGE_PATHS_HELPER_KEY, imageFilePaths.ToArray());
    }
    
    
}