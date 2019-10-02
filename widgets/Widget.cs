using Terminal.Gui;
using Mono.Terminal;

namespace wttop.Widgets
{
    /// <summary>
    /// Base class for all widget with a border.
    /// It implements FrameWiew, built in Terminal.Gui to be able to draw on the terminal.
    /// FrameView draws a border around the widget
    /// </summary>
    public abstract class WidgetFrame : FrameView 
    {
        /// <summary>
        /// Override this method to change the refresth rate.
        /// Default is eeach seconds
        /// </summary>
        protected virtual int RefreshTimeSeconds
        {
            get
            {
                return 1;
            }
        }

        public WidgetFrame() : base(string.Empty){}

        /// <summary>
        /// Method called by the main thread to check whether the widget needs to be refreshed
        /// </summary>
        /// <param name="MainLoop">Equivalent to UI thread</param>
        /// <param name="tick">Current tick of the loop - each tick is one second (reset after one hour) </param>
        public void RefreshIfNeeded(MainLoop MainLoop, int tick)
        {
            // tick == 0 first occurence of the refresh
            if ((tick%RefreshTimeSeconds) == 0 || tick == 0)
                Update(MainLoop);
        }

        /// <summary>
        /// Abstract method that each widget will have to implement in order to refresh their content
        /// </summary>
        /// <param name="MainLoop">Equivalent to UI thread</param>
        protected abstract void Update(MainLoop MainLoop);
    }

    /// <summary>
    /// Base class for all widget without a border.
    /// It implements View, built in Terminal.Gui to be able to draw on the terminal.
    /// View does not draw borders
    /// </summary>
    public abstract class WidgetFrameless : View
    {
        /// <summary>
        /// Override this method to change the refresth rate.
        /// Default is eeach seconds
        /// </summary>
        protected virtual int RefreshTimeSeconds
        {
            get
            {
                return 1;
            }
        }

        public WidgetFrameless() : base(){}

        /// <summary>
        /// Method called by the main thread to check whether the widget needs to be refreshed
        /// </summary>
        /// <param name="MainLoop">Equivalent to UI thread</param>
        /// <param name="tick">Current tick of the loop - each tick is one second (reset after one hour) </param>
        public void RefreshIfNeeded(MainLoop MainLoop, int tick)
        {
            // tick == 0 first occurence of the refresh
            if ((tick%RefreshTimeSeconds) == 0 || tick == 0)
                Update(MainLoop);
        }

        /// <summary>
        /// Abstract method that each widget will have to implement in order to refresh their content
        /// </summary>
        /// <param name="MainLoop">Equivalent to UI thread</param>
        protected abstract void Update(MainLoop MainLoop);
    }

}