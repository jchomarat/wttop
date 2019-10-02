using System;
using Terminal.Gui;

namespace wttop.Widgets.Common
{

    /// <summary>
    /// Bar chart implementation using Labels and 'I' char
    /// </summary>
    public class Bar : Component<float>
    {      
        Label percentText;

        Label percentBar;

        char barChar = 'I';
        
        public Bar(Color BarColor, Color BackgroundColor)
        {
            DrawFrame(this.Bounds, 0, false);

            Label openingBraquet = new Label("[")
            {
                X = 0,
                Y = 0
            };

            Add(openingBraquet);

            Label closingBraquet = new Label("]")
            {
                X = Pos.AnchorEnd(5),
                Y = 0
            };

            Add(closingBraquet);

            percentBar = new Label(string.Empty)
            {
                X = Pos.Right(openingBraquet),
                Y = 0,
                Width = Dim.Fill(5)
            };

            percentBar.TextColor = Terminal.Gui.Attribute.Make(BarColor, BackgroundColor);

            Add(percentBar);

            percentText = new Label(string.Empty)
            {
                X = Pos.Right(closingBraquet),
                Y = 0
            };

            Add(percentText);
        }

        /// <summary>
        /// The idea here is to add as many barChar to "mock" a bar.
        /// The percentage is calcaultaed from the bar width (width being char)
        /// </summary>
        /// <param name="newValue"></param>
        protected override void UpdateAction(float newValue)
        {
           percentText.Text = $" {newValue} %   ";
           // Get the label that acts as progress bar width, to calculate how many | to write
           var lblWidth = percentBar.Bounds.Width;
           int charCount = (int)Math.Floor((newValue*lblWidth)/100);
           percentBar.Text = new string(barChar, charCount);
        }
    }
}