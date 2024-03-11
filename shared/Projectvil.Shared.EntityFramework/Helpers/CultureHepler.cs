namespace Projectvil.Shared.EntityFramework.Helpers;

public static class CultureHepler
{
    public static void Use(string language)
    {
        Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(language);
    }
}