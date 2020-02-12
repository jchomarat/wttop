using System;
using System.Threading.Tasks;
using Terminal.Gui;
using wttop.Widgets.Common;
using Mono.Terminal;

namespace wttop.Widgets {

    /// <summary>
    /// Widget that will display the memory (RAM) graph
    /// </summary>
    public class MemoryGraph : WidgetFrame
    { 
        Label details;
        
        Bar bar;

        protected override int RefreshTimeSeconds
        {
            get
            {
                return 20;
            }
        }

        public MemoryGraph(IServiceProvider serviceProvider) : base(serviceProvider) {}

        protected override void DrawWidget()
        {  
            this.Title = settings.MemoryWidgetTitle;
            
            Label title = new Label("Memory usage: ")
            {
                X = 1,
                Y = 1
            };
            
            bar = new Bar(settings.MemoryBarColor, settings.MainBackgroundColor)
            {
                X = Pos.Right(title),
                Y = 1,
                Width = Dim.Percent(30),
                Height= Dim.Sized(1)
            };

            details = new Label(string.Empty)
            {
                X = Pos.Left(bar) + 1,
                Y = Pos.Bottom(bar)
            };
           
            Add(title);
            Add(bar);
            Add(details);
        }
        
        protected override async Task Update(MainLoop MainLoop)
        {
            var memoryUsage = await systemInfo.GetMemoryUsage();
            bar.Update(MainLoop, memoryUsage.PercentageUsed);
            details.Text = $"{memoryUsage.AvailableGB} GB available";
        }
    }
}