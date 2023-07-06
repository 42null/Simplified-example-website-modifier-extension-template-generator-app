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
using Microsoft.Maui.HotReload;

namespace ExtensionGenerator;

public class Sector
{
    [JsonPropertyName("header")]
    public String Header { get; set; } // Rename to Name, if desired

    [JsonPropertyName("settingKey")]
    public String SettingKey { get; set; }

    [JsonPropertyName("settings")]
    public List<Setting> Settings { get; set; }

    [JsonPropertyName("childSectors")]
    public List<Sector> ChildSectors { get; set; }
    
    public Sector()
    {
        // Ensure lists are initialized
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

    [JsonPropertyName("description")]
    public String Description { get; set; }

    [JsonPropertyName("placeHolder")]
    public String PlaceHolder { get; set; }

    [JsonPropertyName("settingKey")]
    public String SettingKey { get; set; }
}

//TODO: Add revert to default option
public partial class MainPage : ContentPage
{
    private readonly int ICON_WIDTH = 15;
    private readonly int SETTINGS_WIDTH = 400; //TODO: Set off of screen
    private readonly int SETTINGS_WIDTH_LABEL;
    private readonly int SETTINGS_WIDTH_ENTRY;
    private readonly Thickness SETTINGS_SPACEING = new Thickness(0, 10, 10, 0);
    private readonly int SECTOR_HEADER_FONT_SIZE = 22;
    
    // Color Themes
    private ColorInterpreter colorController = new ColorInterpreter("Primary", "Secondary", "Tertiary");
    
    int count = 0;
    VerticalStackLayout mainLayout;
    Layout topInfoArea;

    public MainPage()
    {
        InitializeComponent();
        
        SETTINGS_WIDTH_LABEL = SETTINGS_WIDTH / 2; // Set here so math is not repeated
        SETTINGS_WIDTH_ENTRY = SETTINGS_WIDTH / 2; // Set here so math is not repeated
        
        // Set theme colors
        

        //Get the sections that already exist in MainPage.xaml.
        topInfoArea = (VerticalStackLayout)FindByName("top_info_area");
        

        //Genenerate label-entry pairs from file
        LoadMauiAssetJSONAsync("defaults.json");

    }

    async Task LoadMauiAssetJSONAsync(String fileName)
    {
        try
        {
            await using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
            using var reader = new StreamReader(stream);
            

            var jsonAsStringContents = reader.ReadToEnd();
            // Console.WriteLine(jsonAsStringContents);
            Sector sector = JsonSerializer.Deserialize<Sector>(jsonAsStringContents);

            StackLayout sectorElement = await CreateSectorElementAsync(sector);

            sectorElement.Margin = new Thickness(0, 20, 210, 20); //TODO: Make programmatic once icon size is working
            
            topInfoArea.Children.Add(sectorElement);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error attempting to use settings from file \"{fileName}\" with error {ex.Message}.");
            return;
        }
    }

    async Task<StackLayout> CreateSectorElementAsync(Sector sector)
    {
        StackLayout sectorElement = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
            HorizontalOptions = LayoutOptions.Center,
            // Margin = new Thickness(0,20,110,20) //TODO: Make programmatic once icon size is working
            Margin = new Thickness(110,20,0,20) //TODO: Make programmatic once icon size is working
        };

        Label header = new Label
        {
            Text = sector.Header,
            VerticalOptions = LayoutOptions.Center,
            HorizontalTextAlignment = TextAlignment.End,
            WidthRequest = SETTINGS_WIDTH_LABEL,
            FontSize = SECTOR_HEADER_FONT_SIZE
        };

        sectorElement.Children.Add(header);

        if (sector.Settings is { Count: > 0 })
        {
            foreach (Setting setting in sector.Settings)
            {
                StackLayout settingElement = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                };
                
                Label label = new Label
                {
                    Text = setting.Name + ": ",
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.End,
                    WidthRequest = SETTINGS_WIDTH_LABEL,
                    Padding = SETTINGS_SPACEING
                };
                Entry entry = new Entry
                {
                    Text = setting.Value,
                    Placeholder = setting.PlaceHolder,
                    VerticalOptions = LayoutOptions.Center,
                    WidthRequest = SETTINGS_WIDTH_ENTRY,
                    BackgroundColor = colorController.GetPrimary(),
                    Margin = SETTINGS_SPACEING
                };
                //TODO: Because same, build icon once and clone?
                ImageButton infoIcon = new ImageButton
                {
                    Source = "info.png",
                    HeightRequest = ICON_WIDTH,
                    WidthRequest = ICON_WIDTH,
                };

                // ToolTipProperties.SetText(infoIcon, "Click to Save your data");

                settingElement.Children.Add(label);
                settingElement.Children.Add(entry);
                settingElement.Children.Add(infoIcon);

                //Adds period ending if one does not already exist.
                ToolTipProperties.SetText(infoIcon,
                    setting.Description + (setting.Description.EndsWith(".") ? "" : "."));
                
                sectorElement.Children.Add(settingElement);
            }
        
        }

        // Recursive call to process child sectors
        if (sector.ChildSectors is { Count: > 0 })
        {
            Console.WriteLine("ChildSectors true with "+sector.ChildSectors.Count + " for '"+sector.Header+"'");
            foreach (var childSector in sector.ChildSectors)
            {
                // Console.WriteLine("Child sector: " + childSector.Header);
                StackLayout childSectorElement = await CreateSectorElementAsync(childSector);
                sectorElement.Children.Add(childSectorElement);
            }
        }
        
        return sectorElement;
    }

    // private void OnCounterClicked(object sender, EventArgs e)
    // {
    //     count++;
    //
    //     if (count == 1)
    //         CounterBtn.Text = $"Clicked {count} time";
    //     else
    //         CounterBtn.Text = $"Clicked {count} times";
    //
    //     SemanticScreenReader.Announce(CounterBtn.Text);
    // }
}
