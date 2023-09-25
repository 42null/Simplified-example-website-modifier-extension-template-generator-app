namespace ExtensionGenerator;

public class PreferencesManager//TODO: Make use singleton structure
{
    private const string DELIMETER_1 = "|";

    public const string STORED_PREFRENCES_IMAGE_PATHS_HELPER_KEY = "";

    public static string[] GetStringArray(string key)
    {
        string listAsStringRaw = Preferences.Default.Get(key, "");
        return listAsStringRaw.Split(DELIMETER_1);
    }
    
    public static bool SaveStringArray(string key, string[] storeValues)
    {
        bool keyAlreadyExisted = Preferences.Default.ContainsKey(key);
        string singleStr = storeValues.Aggregate((result, next) => $"{result}{DELIMETER_1}{next}");
        Preferences.Default.Set(key, singleStr);
        Console.WriteLine("singleStr = +"+singleStr);
        return keyAlreadyExisted;
    }
}