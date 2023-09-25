using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Storage;
using ExtensionGenerator.Accessors.RemoteConnections;
using TabbedPageSample;

namespace ExtensionGenerator;

using System;
using Microsoft.Maui.Controls;

//TODO: Add revert to default option
public partial class MainPage : ContentPage
{
    //TODO: Switch to use a timestamp instead?
    private readonly string LAST_COMMIT_SHA = "40dbc46f468cebfa74fcb70dab42e2f9b23e2388"; // Second to last one to check against version (because current version would be last pushed but would not yet have the sha) 
    
    // Launcher.OpenAsync is provided by Essentials.
    public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void OnPullFullButtonClicked(object sender, EventArgs e)
    {
    }
    
    private async void OnGenerateFullButtonClicked(object sender, EventArgs e)
    {
        
    }
    
    private async void OnCheckForUpdatesButtonClicked(object sender, EventArgs e)
    {
        // await GitHubApiService.checkForNewerVersion();
        List<Commit> commits = await GitHubApiService.GetLatestCommitsAsync("https://api.github.com/repos/42null/Simplified-example-website-modifier-extension-template-generator-app/commits");
        if (commits.Count >= 1 && commits[0].sha == LAST_COMMIT_SHA)
        {
            await Toast.Make("No new updates, last published commit is same as check sha.", ToastDuration.Long).Show();
        }else if (commits.Count >= 2 && commits[1].sha == LAST_COMMIT_SHA)
        {
            await Toast.Make("Possible update, last published commit is 1 commit from stored check sha. That version difference should be this version.", ToastDuration.Long).Show();
        }
        else
        {
            short commitsNotItCount = 0;
            foreach (Commit commit in commits)
            {
                commitsNotItCount++;
                if (commit.sha == LAST_COMMIT_SHA)
                {
                    await Toast.Make($"Newer update, last published commit is {commitsNotItCount} commits from stored check sha.", ToastDuration.Long).Show();
                    commitsNotItCount = 0;
                    break;
                }
            }

            if (commitsNotItCount != 0)
            {
                await Toast.Make($"Unable to determine if new updates are available. Was not able to find stored commit sha in repository history.", ToastDuration.Long).Show();
            }
        }

    }
}
