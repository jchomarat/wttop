using System;
using System.Threading.Tasks;
using Terminal.Gui;
using Mono.Terminal;

namespace wttop.Widgets
{

    /// <summary>
    /// Widget that will display the text on top of the app
    /// </summary>
    public class SystemTimeWidget : WidgetFrameless
    {
        private const string headerText = "System time: ";
        private const string contentTemplate = "{0}";

        private Label _textLabel;

        protected override int RefreshTimeSeconds => 1;

        public SystemTimeWidget(IServiceProvider serviceProvider) : base(serviceProvider) { }

        protected override void DrawWidget()
        {
            var headerLabel = new Label(headerText)
            {
                X = 4,
                Y = 1
            };
            headerLabel.TextColor = Terminal.Gui.Attribute.Make(settings.LabelWidgetHeaderColor, settings.MainBackgroundColor);
            Add(headerLabel);

            _textLabel = new Label(string.Empty)
            {
                X = Pos.Right(headerLabel),
                Y = 1
            };

            Add(_textLabel);
        }

        protected override async Task Update(MainLoop MainLoop)
        {
            var time = await systemInfo.GetSystemDateTime();
            _textLabel.Text = string.Format(contentTemplate, time.ToString());
        }
    }
}