using System.Windows.Input;
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
    private readonly string LAST_COMMIT_SHA = "57036c577ad75c31ed02a2ca19ec3f27a2d3ed22"; // Second to last one to check against version (because current version would be last pushed but would not yet have the sha) 
    
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
            Console.WriteLine("No new updates, last published commit is same as check sha.");
        }else if (commits.Count >= 2 && commits[1].sha == LAST_COMMIT_SHA)
        {
            Console.WriteLine("Possible update, last published commit is 1 commit from stored check sha, that version difference should be this version.");
        }
        else
        {
            short commitsNotItCount = 0;
            foreach (Commit commit in commits)
            {
                commitsNotItCount++;
                if (commit.sha == LAST_COMMIT_SHA)
                {
                    Console.WriteLine($"Newer update, last published commit is {commitsNotItCount} commits from stored check sha.");
                    commitsNotItCount = 0;
                    break;
                }
            }

            if (commitsNotItCount != 0)
            {
                Console.WriteLine($"Unable to determine if new updates are available, was not able to find stored commit sha in repository history.");
            }
        }

    }
}
