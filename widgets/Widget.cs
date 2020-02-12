using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui;
using Mono.Terminal;
using wttop.Core;

namespace wttop.Widgets
{
    /// <summary>
    /// Base class for all widget with a border.
    /// It implements FrameWiew, built in Terminal.Gui to be able to draw on the terminal.
    /// FrameView draws a border around the widget
    /// </summary>
    public abstract class WidgetFrame : FrameView 
    {
        protected ISystemInfo systemInfo;

        protected Settings settings;

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

        public WidgetFrame(IServiceProvider serviceProvider, bool DrawAsynchronously) : base(string.Empty)
        {
            systemInfo = serviceProvider.GetService<ISystemInfo>();
            settings = serviceProvider.GetService<Settings>();

            if (!DrawAsynchronously)
                Draw();
        }

        protected abstract void DrawWidget();

        private void Draw()
        {
            DrawWidget();

            // Update content widget as it is just loaded
            Task.Run(async () => await Update(Application.MainLoop));
        }

        /// <summary>
        /// Method called by the main thread to check whether the widget needs to be refreshed
        /// </summary>
        /// <param name="MainLoop">Equivalent to UI thread</param>
        /// <param name="tick">Current tick of the loop - each tick is one second (reset after one hour) </param>
        public async Task RefreshIfNeeded(MainLoop MainLoop, int tick)
        {
            // tick == 0 first occurence of the refresh
            if ((tick%RefreshTimeSeconds) == 0 || tick == 0)
                await Update(MainLoop);
        }

        /// <summary>
        /// Abstract method that each widget will have to implement in order to refresh their content
        /// </summary>
        /// <param name="MainLoop">Equivalent to UI thread</param>
        protected abstract Task Update(MainLoop MainLoop);
    }

    /// <summary>
    /// Base class for all widget without a border.
    /// It implements View, built in Terminal.Gui to be able to draw on the terminal.
    /// View does not draw borders
    /// </summary>
    public abstract class WidgetFrameless : View
    {
        protected ISystemInfo systemInfo;

        protected Settings settings;
        
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

        public WidgetFrameless(IServiceProvider serviceProvider) : base()
        {
            systemInfo = serviceProvider.GetService<ISystemInfo>();
            settings = serviceProvider.GetService<Settings>();
            Draw();
        }

        protected abstract void DrawWidget();

        private void Draw()
        {
            DrawWidget();

            // Update content widget as it is just loaded
            Task.Run(async () => await Update(Application.MainLoop));
        }

        /// <summary>
        /// Method called by the main thread to check whether the widget needs to be refreshed
        /// </summary>
        /// <param name="MainLoop">Equivalent to UI thread</param>
        /// <param name="tick">Current tick of the loop - each tick is one second (reset after one hour) </param>
        public async Task RefreshIfNeeded(MainLoop MainLoop, int tick)
        {
            // tick == 0 first occurence of the refresh
            if ((tick%RefreshTimeSeconds) == 0 || tick == 0)
                await Update(MainLoop);
        }

        /// <summary>
        /// Abstract method that each widget will have to implement in order to refresh their content
        /// </summary>
        /// <param name="MainLoop">Equivalent to UI thread</param>
        protected abstract Task Update(MainLoop MainLoop);
    }

}