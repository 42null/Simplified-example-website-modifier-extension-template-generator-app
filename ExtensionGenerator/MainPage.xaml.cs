﻿using System;
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
using Microsoft.Maui.HotReload;

namespace ExtensionGenerator;

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
public partial class MainPage : ContentPage
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
        // jsonController = new JsonController("defaults.json", colorController);

        // Task<Layout> jsonLayout = jsonController.GetStackFromFile();
        // topInfoArea.Children.Add(await jsonLayout);
        
        // LoadMauiAssetJSONAsync("defaults.json");

        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        jsonController = new JsonController("defaults.json", colorController);
        Layout jsonLayout = await jsonController.GetStackFromFile();
        topInfoArea.Children.Add(jsonLayout);
        
        
        // jsonController.GenerateJsonFromSettings();
    }



    private void OnCounterClicked(object sender, EventArgs e)
    {
        // count++;
        //
        // if (count == 1)
        //     CounterBtn.Text = $"Clicked {count} time";
        // else
        //     CounterBtn.Text = $"Clicked {count} times";
    
        // SemanticScreenReader.Announce(CounterBtn.Text);

        jsonController.GenerateJsonFromSettings();
        string fileName = "projectConfiguration.json";
        FileManager.SaveFile(fileName, jsonController.getSettingsJsonString(), new CancellationToken());

    }
}
