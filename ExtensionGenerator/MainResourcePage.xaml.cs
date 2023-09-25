using System.Net.Mime;
using System.Runtime.InteropServices.ComTypes;

namespace ExtensionGenerator;


using Microsoft.Maui.Controls;
using System.IO;

// TODO: Getting drag and drop into to work on different os's is taking longer than expected. Right now a file picker is being used instead  
public partial class MainResourcePage : ContentPage
{
    private readonly int ICON_WIDTH = 15;
    private readonly int SECTOR_HEADER_FONT_SIZE = 22;
    
    private DropGestureRecognizer DropGestureRecognizer { get; set; }
    
    private List<string> imageFilePaths = new List<string>();
    
    
    // Color Themes
    // private ColorInterpreter colorController = new ("Primary", "Secondary", "Tertiary");
    
    public MainResourcePage()
    {
        imageFilePaths = PreferencesManager.GetStringArray(PreferencesManager.STORED_PREFRENCES_IMAGE_PATHS_HELPER_KEY).ToList();
        foreach (var imageFilePath in imageFilePaths)
        {
            Console.WriteLine("!!!"+imageFilePath);
        }
        InitializeComponent();
        InitializeAsync();
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