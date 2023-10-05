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
    
    //TODO: Check if key holds values of same generic
    public static bool SaveStringArray(string key, string[] storeValues)
    {
        bool keyAlreadyExisted = Preferences.Default.ContainsKey(key);
        string singleStr = storeValues.Aggregate((result, next) => $"{result}{DELIMETER_1}{next}");
        if (singleStr.Substring(0, 1) == $"{DELIMETER_1}")
        {
            singleStr = singleStr.Substring(1);
        }
        Preferences.Default.Set(key, singleStr);
        return keyAlreadyExisted;
    }
    
    // public static T[] CheckStringArray<T,U>(string key, U[] checkValues) where U : Boolean,Double,Int32,Single,Int64,String,DateTime
    public static T[] CheckPreferencesArray<T>(string key, T[] checkValues) //Forced to use string right now
    {
        List<T> foundValues = new List<T>();
        Type[] allowedTypes = new Type[]
        {
            typeof(bool), typeof(double), typeof(int), typeof(float), typeof(long), typeof(string), typeof(DateTime)
        };

        if (!allowedTypes.Contains(typeof(T)))
        {
            throw new ArgumentException($"Type {typeof(T)} is not usable for checkValues.");
        }

        if (Preferences.ContainsKey(key))
        {
            var fromPreferences = GetStringArray(key);// .Default.Get(key, checkValues); //TODO: Use empty T value instead of provided list, should not matter as should be found in the check if empty
            // foreach (T value in fromPreferences)
            foreach (T value in checkValues)
            {
                if (fromPreferences.Contains(value.ToString()))
                {
                    foundValues.Add(value);
                }
            }
        }

        return foundValues.ToArray();
    }
    
    // public static bool RemoveFromStringArray(string key, string storedValues)
    // {
    //     bool keyAlreadyExisted = Preferences.Default.ContainsKey(key);
    //     string singleStr = storeValues.Aggregate((result, next) => $"{result}{DELIMETER_1}{next}");
    //     Preferences.Default.Set(key, singleStr);
    //     Console.WriteLine("singleStr = +"+singleStr);
    //     return keyAlreadyExisted;
    // }
}