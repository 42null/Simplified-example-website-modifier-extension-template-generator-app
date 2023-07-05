namespace ExtensionGenerator;
public class ColorInterpreter //TODO: Rename to ColorController?
{
    private Color primary, secondary, tertiary;

    // Make take different sizes and construct based on what isn't provided. Right now set only to 3 values  
    public ColorInterpreter(String primaryName, String secondaryName, String tertiaryName)
    {
        primary   = getStylesColor(primaryName);
        secondary = getStylesColor(secondaryName);
        tertiary  = getStylesColor(tertiaryName);
    }

    private Color getStylesColor(String colorName)
    {
        if (App.Current.Resources.TryGetValue(colorName, out var foundColor))
        {
            return (Color)foundColor;
        }
        else
        {
            return Colors.Transparent;

            // throw new ArgumentException($"Failed to find color \"{colorName}\" in Colors.xaml");
        }
        // TODO: Create alternate path if color fails to be found
        return Colors.Aqua;
    }

    public Color GetPrimary()
    {
        return primary;
    }
    public Color GetSecondary()
    {
        return secondary;
    }
    public Color GetTertiary()
    {
        return tertiary;
    }
}