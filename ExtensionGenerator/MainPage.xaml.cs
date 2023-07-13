using CommunityToolkit.Maui.Storage;
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

//TODO: Add revert to default option
public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
        
    }
}
