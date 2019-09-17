using Terminal.Gui;
using Mono.Terminal;

namespace wttop.Widgets {

    // Base class for all widget with a border
    // It implements FrameWiew, built in Terminal.Gui to be able to draw on the terminal. FrameView draws a border around the widget
    public abstract class Widget : FrameView {

        public Widget(string text) : base(text){}

        public virtual void Init() {}

        // Abstract method that each widget will have to implement in order to refresh their content
        public abstract bool Update(MainLoop MainLoop);
    }

    // Base class for all widget without a border
    // It implements View, built in Terminal.Gui to be able to draw on the terminal. View does not draw borders
    public abstract class WidgetFrameless : View {

        public WidgetFrameless() : base(){}

        public abstract bool Update(MainLoop MainLoop);
    }

}