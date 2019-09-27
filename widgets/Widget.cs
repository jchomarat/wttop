using Terminal.Gui;
using Mono.Terminal;

namespace wttop.Widgets
{
    // Base class for all widget with a border
    // It implements FrameWiew, built in Terminal.Gui to be able to draw on the terminal. FrameView draws a border around the widget
    public abstract class WidgetFrame : FrameView 
    {
        protected virtual int RefreshTimeSeconds
        {
            get
            {
                return 1;
            }
        }

        public WidgetFrame() : base(string.Empty){}

        public void RefreshIfNeeded(MainLoop MainLoop, int tick)
        {
            // tick == 0 first occurence of the refresh
            if ((tick%RefreshTimeSeconds) == 0 || tick == 0)
                Update(MainLoop);
        }

        // Abstract method that each widget will have to implement in order to refresh their content
        protected abstract void Update(MainLoop MainLoop);
    }

    // Base class for all widget without a border
    // It implements View, built in Terminal.Gui to be able to draw on the terminal. View does not draw borders
    public abstract class WidgetFrameless : View
    {
        protected virtual int RefreshTimeSeconds
        {
            get
            {
                return 1;
            }
        }

        public WidgetFrameless() : base(){}

        public void RefreshIfNeeded(MainLoop MainLoop, int tick)
        {
            // tick == 0 first occurence of the refresh
            if ((tick%RefreshTimeSeconds) == 0 || tick == 0)
                Update(MainLoop);
        }

        protected abstract void Update(MainLoop MainLoop);
    }

}