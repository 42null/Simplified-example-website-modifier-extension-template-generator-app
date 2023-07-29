using System.Net.Mime;
using AppKit;
using CloudKit;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Storage;
using GameController;
using TabbedPageSample;

namespace ExtensionGenerator;


using System;
using System.Text.Json;
using Microsoft.Maui;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Accessibility;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using CoreVideo;
using Microsoft.Maui.HotReload;
using System.Drawing;

public enum SpecialType
{
    Color,
    Image
}
public class Sector
{
    [JsonPropertyName("header")]
    public String Header { get; set; } //TODO: Rename to Name?

    [JsonPropertyName("settingKey")]
    public String SettingKey { get; set; }

    [JsonPropertyName("settings")]
    public List<Setting> Settings { get; set; }

    [JsonPropertyName("childSectors")]
    public List<Sector> ChildSectors { get; set; }

    public Sector()
    {
        // Ensures lists are initialized
        Settings = new List<Setting>();
        ChildSectors = new List<Sector>();
    }
}

public class Setting
{
    [JsonPropertyName("name")]
    public String Name { get; set; }

    [JsonPropertyName("value")]
    public String Value { get; set; }
    
    [JsonPropertyName("specialType")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SpecialType SpecialType { get; set; }
    
    [JsonPropertyName("description")]
    public String Description { get; set; }

    [JsonPropertyName("placeHolder")]
    public String PlaceHolder { get; set; }

    [JsonPropertyName("settingKey")]
    public String SettingKey { get; set; }

    public Setting()
    {
        if (SpecialType == SpecialType.Color)
        {
            Description = "Use a 3 or 6 digit hexcode color value";
            PlaceHolder = "#ffffff";
        }
        else
        {
            SpecialType = SpecialType.Image;
        }
    }
}


//TODO: Add revert to default option
public partial class JsonFileEditor : ContentPage
{
    private readonly int ICON_WIDTH = 15;
    private readonly int SETTINGS_WIDTH = 400; //TODO: Set off of screen
    private readonly int SETTINGS_WIDTH_LABEL;
    private readonly int SETTINGS_WIDTH_ENTRY;
    private readonly Thickness SETTINGS_SPACEING = new (0, 10, 10, 0);
    private readonly int SECTOR_HEADER_FONT_SIZE = 22;
    
    // Color Themes
    private ColorInterpreter colorController = new ("Primary", "Secondary", "Tertiary");
    
    private JsonController jsonController;
    private Layout jsonLayout;
    private readonly string JsonDefaultFilename = "error_defaults.json";
    private readonly string JsonSaveFilename = "error_save.json";
    
    int count = 0;
    VerticalStackLayout mainLayout;
    Layout topInfoArea;

    public JsonFileEditor() : this("error_defaults.json", "error_save.json")
    {
    }

    public JsonFileEditor(string fromFileFilename, string saveFilename)
    {
        JsonDefaultFilename = fromFileFilename;
        JsonSaveFilename = saveFilename;
        InitializeComponent();
                
        
        SETTINGS_WIDTH_LABEL = SETTINGS_WIDTH / 2; // Set here so math is not repeated
        SETTINGS_WIDTH_ENTRY = SETTINGS_WIDTH / 2; // Set here so math is not repeated
        
        // Set theme colors
        
        //Get the sections that already exist in MainPage.xaml.
        topInfoArea = (VerticalStackLayout)FindByName("top_info_area");

        InitializeAsync();
    }


    private async void InitializeAsync()
    {
        jsonController = new JsonController(JsonDefaultFilename, colorController);
        jsonLayout = await jsonController.GetStackFromFile();
        topInfoArea.Children.Add(jsonLayout);
        
        
        // jsonController.GenerateJsonFromSettings();
    }



    private void OnGenerateButtonClicked(object sender, EventArgs e)
    {
        Console.WriteLine("Attempting to save file");
    }
    
    private void OnExportButtonClicked(object sender, EventArgs e)
    {
        // SemanticScreenReader.Announce(CounterBtn.Text);

        jsonController.GenerateJsonFromSettings();
        try
        {
            FileManager.SaveFile(JsonSaveFilename, jsonController.getSettingsJsonString(), new CancellationToken());
        }
        catch (FileSaveException)
        {
            //TODO: Do something better with reporting the error
            Console.WriteLine("File location not accessible");
        }        
    }
    
    private async void OnImportButtonClicked(object sender, EventArgs e)
    {
        Console.WriteLine(":::: Import Clicked");

        var fileResult = await FileManager.userPickFile(/*new ContentType("json")*/);//Code below does not run if option was canceled, even if null
        if (fileResult == null) { return; }//Here just for just in case
        Console.WriteLine("fileResult  = "+fileResult.FullPath);
        Console.WriteLine("ContentType = "+fileResult.ContentType);
        // if (fileResult.FileName == )
        // {
        //     
        // }
        
        
        
        await using var stream = await FileSystem.OpenAppPackageFileAsync(fileResult.FullPath);
        using var reader = new StreamReader(stream);
        
        var jsonAsStringContents = reader.ReadToEnd();
        // Console.WriteLine("jsonAsStringContents = "+jsonAsStringContents);

        JsonControllerGeneric jsonControllerGeneric = new JsonControllerGeneric(jsonAsStringContents);
        GenericJsonObject fullGenericJsonObject = jsonControllerGeneric.GetGenericObject();

        Layout layout = await jsonController.GetStackFromFile();
        jsonController.TraverseLayoutReplace(fullGenericJsonObject, layout);
        
        
        
        // attemptToFillCurrentEditior(test);
    }

    private async void attemptToFillCurrentEditior(JsonControllerGeneric newObject)//TODO: Make save backup of old values
    {


    }











    public async Task<FileResult> PickJsonFile()
    {
        try
        {
            var options = new PickOptions
            {
                PickerTitle = "Please select a JSON file",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.iOS, new[] { "public.json" } },
                    { DevicePlatform.Android, new[] { "application/json" } },
                    { DevicePlatform.WinUI, new[] { ".json" } },
                    { DevicePlatform.Tizen, new[] { "application/json" } },
                    { DevicePlatform.macOS, new[] { "json" } }
                })
            };

            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                if (result.FileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    using var stream = await result.OpenReadAsync();
                    // Process the selected JSON file here
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            // The user canceled or something went wrong
        }

        return null;
    }

    
}