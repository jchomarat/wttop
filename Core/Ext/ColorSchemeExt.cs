using Terminal.Gui;

namespace wttop.Core.ext
{
    /// <summary>
    /// Color scheme extension
    /// </summary>
    public static class ColorSchemeExt
    {
        /// <summary>
        /// Set the color for all states
        /// </summary>
        /// <param name="ColorScheme">The color scheme</param>
        /// <param name="ForegroundColor">The foreground color</param>
        /// <param name="BackgroundColor">The background color</param>
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