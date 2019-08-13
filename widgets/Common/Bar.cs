using System;
using Terminal.Gui;

namespace wttop.Widgets.Common {

    public class Bar : Component {

        Label barFilling;
        Char barIndicator = 'I';
        Terminal.Gui.Attribute textColor = Terminal.Gui.Attribute.Make(Color.Black, Color.Gray);

        public Bar() {
            DrawFrame(this.Bounds, 0, true);

            barFilling = new Label(string.Empty) {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            barFilling.TextColor = textColor;
            Add(barFilling);
        }

        protected override void UpdateAction(object newValue){
            var nbDot = (int)Math.Floor((decimal)((int)newValue / 5)); // Because it's 20 the widht, dirty, need to be changed
            barFilling.Text = String.Empty.PadLeft(nbDot, barIndicator);
        }
    }
}