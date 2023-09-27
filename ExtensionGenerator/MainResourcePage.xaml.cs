using System.Net.Mime;
using System.Runtime.InteropServices.ComTypes;

namespace ExtensionGenerator;


using Microsoft.Maui.Controls;
using System.IO;

// TODO: Getting drag and drop into to work on different os's is taking longer than expected. Right now a file picker is being used instead  
public partial class MainResourcePage : ContentPage
{
    private static readonly int ICON_WIDTH = 15;
    private static readonly int RESOURCE_ELEMENT_DISPLAY_HEIGHT = 72;
    private static readonly int RESOURCE_FILENAME_FONT_SIZE  = (int)(RESOURCE_ELEMENT_DISPLAY_HEIGHT * 0.4);
    private static readonly int RESOURCE_FULL_PATH_FONT_SIZE = (int)(RESOURCE_ELEMENT_DISPLAY_HEIGHT * 0.2);
    
    Layout topInfoArea;
    Layout savedResourcesDisplayArea;
    

    private DropGestureRecognizer DropGestureRecognizer { get; set; }
    
    private List<string> imageFilePaths;
    
    
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
                    HorizontalOptions = LayoutOptions.Center,
                    Background = Colors.Aqua,//TODO: MAKE USE PROGRAMMATIC VALUES!
                    Margin = new Thickness(5,5,5,5),

                    // Margin = new Thickness(20,20,20,20), //TODO: Make programmatic once icon size is working
                    HeightRequest = RESOURCE_ELEMENT_DISPLAY_HEIGHT,
                    WidthRequest = (int)(DeviceDisplay.MainDisplayInfo.Width * 0.35),
                    
                    AutomationId = imageFilePath.GetHashCode().ToString()
                };
                StackLayout verticalResourceElementsSegment = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    WidthRequest = (int)(resourceDisplay.WidthRequest * 0.80),
                    // AutomationId = 
                };
                
                Image resourceImage = new Image
                {
                    Source = imageFilePath,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(5,5,5,5),
                    HeightRequest = RESOURCE_ELEMENT_DISPLAY_HEIGHT,
                    WidthRequest = (int)(resourceDisplay.WidthRequest - verticalResourceElementsSegment.WidthRequest),
                };
                //TODO: !!!!!!!!!!! Make multi-platform character switch. HIGH PRIORITY
                string fileName = imageFilePath.Substring(imageFilePath.LastIndexOf("/")+1);
                Label fileNameHeader = new Label
                {
                    Text = fileName,
                    TextColor = Colors.Black,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    FontSize = RESOURCE_FILENAME_FONT_SIZE,

                    HorizontalTextAlignment = TextAlignment.Center,
                    
                    // WidthRequest = SETTINGS_WIDTH_LABEL,
                };
                Label fileNameFullPath = new Label
                {
                    Text = imageFilePath,
                    TextColor = Colors.Gray,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start,
                    FontSize = RESOURCE_FULL_PATH_FONT_SIZE,
                    // BackgroundColor = Colors.Beige,
                    HorizontalTextAlignment = TextAlignment.Center,
                    
                    // WidthRequest = SETTINGS_WIDTH_LABEL,
                };
                resourceDisplay.Children.Add(resourceImage);
                
                verticalResourceElementsSegment.Children.Add(fileNameHeader);
                verticalResourceElementsSegment.Children.Add(fileNameFullPath);
                
                resourceDisplay.Children.Add(verticalResourceElementsSegment);

                Frame resourceDisplayFrame = new Frame // Add to frame so a corner radius can be achieved
                {
                    IsClippedToBounds = true, 
                    CornerRadius = 10,
                    Padding = new Thickness(2,-2,-2,-2),
                    Content = resourceDisplay,
                    WidthRequest = resourceDisplay.WidthRequest
                };
                
                display_saved_resources_area.Children.Add(resourceDisplayFrame);
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