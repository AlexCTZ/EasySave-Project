public class LanguageManager
{
    public static string CurrentLanguage { get; set; } = "fr"; // par défaut, le français

    public static void ToggleLanguage()
    {
        CurrentLanguage = (CurrentLanguage == "fr") ? "an" : "fr";
    }
}