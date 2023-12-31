using System.Net.Mime;
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
    
    public static async Task<FileResult> userPickFile(ContentType fileTypeExtension)//TODO: Get working with filtering & w/message, make passable when
    {
        try
        {
            // PickOptions pickOptions = new PickOptions
            // {
            //     FileTypes = new FilePickerFileType(
            //         new Dictionary<Microsoft.Maui.Devices.DevicePlatform, IEnumerable<string>>
            //         {
            //             { Microsoft.Maui.Devices.DevicePlatform.macOS, new[] { "application/json" } }
            //         }
            //     )
            // };
            
            var result = await FilePicker.Default.PickAsync();
            if (result != null)
            {
                string withoutApplicationPartStr = fileTypeExtension.ToString().Substring("application/".Length);
                if (result.ContentType == withoutApplicationPartStr){// Temp until getting select filters working
                    
                    await Toast.Make($"Proper file type selected, processing...").Show();
                }else if(fileTypeExtension.Equals(new ContentType("application/image"))){//TODO: Restructure to use recursively or better option
                    if (withoutApplicationPartStr is "ico" or "svg" or "png" or "jpg" or "jpeg")
                    {
                        await Toast.Make($"Proper file type selected (image), processing...").Show();
                    }
                }else{
                    await Toast.Make($"File selected was of wrong type, must be of type \""+fileTypeExtension.ToString()+"\"").Show();
                    return null;
                }
            }
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception occurred when picking file. Message: "+ex.Message);
        }
        return null;
    }
    
    
    
    
    
    
    
    
    
    
    public static async Task<FileResult> GetFileContents(PickOptions options, CancellationToken cancellationToken)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                    result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                {
                    using var stream = await result.OpenReadAsync();
                    var image = ImageSource.FromStream(() => stream);
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