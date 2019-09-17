using Terminal.Gui;

namespace wttop.Core.ext
{
    public static class ColorSchemeExt
    {
        public static void SetColorsForAllStates(this ColorScheme ColorScheme, Color ForegroundColor, Color BackgroundColor)
        {
            var attributes = Terminal.Gui.Attribute.Make(ForegroundColor, BackgroundColor);
            ColorScheme.Focus = attributes;
            ColorScheme.HotFocus = attributes;
            ColorScheme.HotNormal = attributes;
            ColorScheme.Normal = attributes;
        }
    }
}