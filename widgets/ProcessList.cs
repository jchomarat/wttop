using System;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui;
using wttop.Widgets.Common;
using Mono.Terminal;
using wttop.Helpers;

namespace wttop.Widgets {
    
    public class ProcessList : Widget
    { 
        Label upl;
        Label dl;
        
        ISystemInfo systemInfo;

        int valueUpl = 0;
        
        int valueDl = 0;

        public ProcessList(string text, IServiceProvider serviceProvider) : base(text)
        {
            systemInfo = serviceProvider.GetService<ISystemInfo>();
            DrawWidget();
        }

        void DrawWidget()
        {            
        }

        public override bool Update(MainLoop MainLoop)
        {
            return true;
        }
    }
}