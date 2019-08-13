using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui;
using wttop.Widgets.Common;
using Mono.Terminal;
using wttop.Helpers;

namespace wttop.Widgets {
    
    public class MemoryGraph : Widget {
        
        Label details;
        Bar bar;
        ISystemInfo systemInfo;

        public MemoryGraph(string text, IServiceProvider serviceProvider) : base(text){
            systemInfo = serviceProvider.GetService<ISystemInfo>();
            DrawWidget();
        }

        void DrawWidget(){
            
            Label lbl = new Label("Memory usage")
            {
                X = 1,
                Y = 1
            };
            
            bar = new Bar(){
                X = 14,
                Y = 1,
                Width = Dim.Sized(20),
                Height= Dim.Sized(1)
            };

            details = new Label(string.Empty)
            {
                X = Pos.Left(bar) + 1,
                Y = Pos.Bottom(bar)
            };
           
            Add(lbl);
            Add(bar);
            Add(details);
        }
        public override bool Update(MainLoop MainLoop)
        {
            var memoryUsage = systemInfo.GetMemoryUsage();
            bar.Update(MainLoop, memoryUsage.PercentageUsed);
            details.Text = $"{memoryUsage.UsedGB} Gb / {memoryUsage.TotalGB} Gb used";
            return true;
        }
    }
}