using System.Text;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;

namespace ExtensionGenerator;

public class FileManager
{
    public static async Task SaveFile(string fileName, string saveData, CancellationToken cancellationToken)
    {
        using var stream = new MemoryStream(Encoding.Default.GetBytes(saveData));
        var fileSaverResult = await FileSaver.Default.SaveAsync(fileName, stream, cancellationToken);
        if (fileSaverResult.IsSuccessful)
        {
            await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}").Show(cancellationToken);
        }
        else
        {
            await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}").Show(cancellationToken);
        }
    }
}