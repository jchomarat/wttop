using Terminal.Gui;

namespace wttop.Widgets.Common
{

    public class Indicator : Component<string>
    {
        
        Label lbl;
        bool isOn = true;
        
        Terminal.Gui.Attribute colorAttributesOn;
        Terminal.Gui.Attribute colorAttributesOff;

        public Indicator(Color ForegroundOn, Color BackgroundOn, Color ForegroundOff, Color BackgroundOff)
        {
            DrawFrame(this.Bounds, 0, false);

            lbl = new Label("0") {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            colorAttributesOn = Terminal.Gui.Attribute.Make(ForegroundOn, BackgroundOn);
            colorAttributesOff = Terminal.Gui.Attribute.Make(ForegroundOff, BackgroundOff);
            lbl.TextColor = colorAttributesOn;
            Add(lbl);
        }

        protected override void UpdateAction(string newValue)
        {
           lbl.TextColor = (isOn ? colorAttributesOff : colorAttributesOn);
           isOn = !isOn;
        }
    }
}