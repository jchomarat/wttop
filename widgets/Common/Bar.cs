using Terminal.Gui;

namespace wttop.Widgets.Common
{

    // Bar chart implementation using framework native progress bar
    public class Bar : Component<float>
    {
        
        ProgressBar progressBar;
        
        public Bar(Color Foreground, Color Background)
        {
            DrawFrame(this.Bounds, 0, false);

            progressBar = new ProgressBar() {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            var colorAttributes = Terminal.Gui.Attribute.Make(Foreground, Background);
            var colorSchema = new ColorScheme();
            colorSchema.Normal = colorAttributes;
            colorSchema.HotNormal = colorAttributes;
            colorSchema.HotFocus = colorAttributes;
            colorSchema.Focus = colorAttributes;            
            progressBar.ColorScheme = colorSchema;
            
            Add(progressBar);
        }

        protected override void UpdateAction(float newValue)
        {
           // Value are between 0 and 1, hence the division
           progressBar.Fraction = newValue / 100;
        }
    }
}