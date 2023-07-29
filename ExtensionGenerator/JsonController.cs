using System.Linq;
using System.Reflection;
using System.Text;
using ExtensionGenerator;
using static System.IO.File;

namespace TabbedPageSample;

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
using System.Text.Json.Nodes;
using Microsoft.Maui.HotReload;


public class JsonController
{
    //TODO: MOVE!!!
    private readonly int ICON_WIDTH = 15;
    private readonly static int SETTINGS_WIDTH = 400;
    private readonly static int SETTINGS_WIDTH_LABEL = SETTINGS_WIDTH / 2; // Set here so math is not repeated
    private readonly static int SETTINGS_WIDTH_ENTRY = SETTINGS_WIDTH / 2; // Set here so math is not repeated
    private readonly Thickness SETTINGS_SPACEING = new (0, 10, 10, 0);
    private readonly int SECTOR_HEADER_FONT_SIZE = 22;
    
    
    private String jsonFilename;
    private ColorInterpreter colorController;
    
    //Stored generateable
    public Layout StackLayout { get; private set; }

    // private JsonObject JsonObject;
    private JsonObject lastTransversedBuiltSettings;
    
        
    public JsonController(String filename, ColorInterpreter colorController)
    {
        this.jsonFilename = filename;
        this.colorController = colorController;
    }

    public async Task<Layout> GetStackFromFile()
    {
        if (StackLayout == null)
        {
            StackLayout = await LoadMauiAssetJSONAsync(jsonFilename);
        }
        return StackLayout;
    }

    public async Task<JsonObject> GenerateJsonFromSettings()
    {
        JsonObject primaryObject = new JsonObject();

        // stackLayout = await GetStackFromFile();
        Console.WriteLine(StackLayout.Children.ToString());
        
        Func<Layout, JsonNode?> traverse = traverseLayout;
        
        lastTransversedBuiltSettings = (JsonObject) traverse(StackLayout);
        Console.WriteLine("result = "+lastTransversedBuiltSettings.ToJsonString());

        // string json = result.ToString();
        // FileManager.SaveFile(fileName, json, CancellationToken.None);
        
        return primaryObject;
    }
    
    private JsonNode traverseLayout(Layout layout)
    {
        JsonObject jsonObject = new JsonObject();
            
        foreach (Element element in layout.Children)
        {
            // Check if the child is a layout
            if (element is Layout childLayout)
            {
                if (String.IsNullOrEmpty(childLayout.AutomationId))
                {
                    JsonNode? jsonNode = traverseLayout(childLayout);
                    JsonObject node = (JsonObject)jsonNode;

                    var firstProperty = node.FirstOrDefault();
                    if (!string.IsNullOrEmpty(firstProperty.Key) && !string.IsNullOrEmpty(firstProperty.Value.ToString()))
                    {
                        // Console.WriteLine("firstProperty.Value = "+firstProperty.Value);
                        jsonObject.Add(firstProperty.Key, firstProperty.Value.ToString());
                    }
                    // jsonObject.Add(childLayout.AutomationId, traverse(childLayout));
                }
                else
                {
                    jsonObject.Add(childLayout.AutomationId, traverseLayout(childLayout));
                }

            }else if (element is Entry entry)
            {
                // Console.WriteLine("Entry Text = "+entry.Text);
                if (!string.IsNullOrEmpty(entry.Text)){
                    jsonObject.Add(entry.AutomationId, entry.Text);
                }
            }
        }

        return jsonObject;
    }
    
    // private JsonNode traverseLayoutReplace(Layout layout, JsonObject replacement)
    public JsonNode TraverseLayoutReplace(GenericJsonObject replacement, Layout layout)
    {
        JsonObject jsonObject = new JsonObject();
                
        foreach (Element element in layout.Children)
        {
            // Check if the child is a layout
            if (element is Layout childLayout)
            {
                if (String.IsNullOrEmpty(childLayout.AutomationId))
                {
                    JsonNode? jsonNode = TraverseLayoutReplace(replacement, childLayout);
                    JsonObject node = (JsonObject)jsonNode;

                    var firstProperty = node.FirstOrDefault();
                    
                    if (!string.IsNullOrEmpty(firstProperty.Key) && !string.IsNullOrEmpty(firstProperty.Value.ToString()))
                    {
                        // Console.WriteLine(firstProperty.Key+"--"+firstProperty.Value.ToString()+"__"+replacement.GetString(firstProperty.Key));
                        // Console.WriteLine("A: "+replacement.GetString(firstProperty.Key)+"\n_______");
                        // Type layoutType = childLayout.GetType();
                        // PropertyInfo[] properties = layoutType.GetProperties();
                        // Console.WriteLine("childLayout Information:");
                        // Console.WriteLine("Type: "+layoutType);
                        // Console.WriteLine("id: "+childLayout.Id);
                        // foreach (PropertyInfo property in properties)
                        // {
                        //     // var value = property.GetValue(childLayout);
                        //     Console.WriteLine($"{property.Name}: ");
                        //     // Console.WriteLine($"{property.Name}: {value}");
                        // }
                        foreach (Element sectorElement in childLayout.Children)
                        {
                            // Update entry value
                            if (sectorElement is Entry entry)
                            {
                                // readonly int lengthDivider = entry.Text.Length;
                                Console.Write($"Updated: {entry.Text, -25} ->");//TODO: Find a way to make it more dynamic without causing the error `A constant value is expected` 
                                entry.Text = replacement.GetString(firstProperty.Key);
                                Console.Write($" {entry.Text}\n");
                            }
                        }
                        
                    }
                }
                else
                {
                    jsonObject.Add(childLayout.AutomationId, TraverseLayoutReplace(replacement.GetGenericObject(childLayout.AutomationId), childLayout));
                }

            }else if (element is Entry entry)
            {
                // Console.WriteLine("Entry Text = "+entry.Text);
                if (!string.IsNullOrEmpty(entry.Text)){
                    jsonObject.Add(entry.AutomationId, entry.Text);
                }
            }
        }

        return jsonObject;
    }

    public string getSettingsJsonString()
    {
        if (lastTransversedBuiltSettings == null)
        {
            GenerateJsonFromSettings();
        }

        return lastTransversedBuiltSettings.ToString();
    }


    private async Task<Layout> LoadMauiAssetJSONAsync(String fileName)
    {
        try
        {
            await using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
            using var reader = new StreamReader(stream);
            

            var jsonAsStringContents = reader.ReadToEnd();
            // Console.WriteLine(jsonAsStringContents);
            Sector sector = JsonSerializer.Deserialize<Sector>(jsonAsStringContents);

            StackLayout sectorStack = await CreateSectorElementAsync(sector, colorController);

            sectorStack.Margin = new Thickness(0, 20, 210, 20); //TODO: Make programmatic once icon size is working
            
            // topInfoArea.Children.Add(sectorStack);

            // JsonObject generatedExportJson = GenerateJsonFromSettings();
            
            
            return sectorStack;
        }
        catch (Exception ex)
        {
            // Console.WriteLine($"Error attempting to use settings from file \"{fileName}\" with error {ex.Message}.");
            throw new ArgumentException($"Error attempting to use settings from file \"{fileName}\" with error {ex.Message}.");
        }
    }


    async Task<StackLayout> CreateSectorElementAsync(Sector sector, ColorInterpreter colorController)
    {
        StackLayout sectorElement = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
            HorizontalOptions = LayoutOptions.Center,
            // Margin = new Thickness(0,20,110,20) //TODO: Make programmatic once icon size is working
            Margin = new Thickness(110,20,0,20), //TODO: Make programmatic once icon size is working
            AutomationId = sector.SettingKey
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
                    Margin = SETTINGS_SPACEING,
                    AutomationId = setting.SettingKey
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
            // Console.WriteLine("ChildSectors true with "+sector.ChildSectors.Count + " for '"+sector.Header+"'");
            foreach (var childSector in sector.ChildSectors)
            {
                // Console.WriteLine("Child sector: " + childSector.Header);
                StackLayout childSectorElement = await CreateSectorElementAsync(childSector, colorController);
                
                sectorElement.Children.Add(childSectorElement);
            }
        }
        
        return sectorElement;
    }
}