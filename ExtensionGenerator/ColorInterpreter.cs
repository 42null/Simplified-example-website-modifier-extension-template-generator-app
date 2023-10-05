namespace ExtensionGenerator;
public class ColorInterpreter //TODO: Rename to ColorController?
{
    private Color primary, secondary, tertiary, positive, negative;

    // Make take different sizes and construct based on what isn't provided. Right now set only to all values  
    public ColorInterpreter(string primaryName, string secondaryName, string tertiaryName, string positiveName, string negativeName)
    {
        primary   = getStylesColor(primaryName);
        secondary = getStylesColor(secondaryName);
        tertiary  = getStylesColor(tertiaryName);
        
        positive  = getStylesColor(positiveName);
        negative  = getStylesColor(negativeName);
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

    public Color GetPositive()
    {
        return positive;
    }
    public Color GetNegative()
    {
        return negative;
    }
}