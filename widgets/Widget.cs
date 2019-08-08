using System.Collections.Generic;
using Terminal.Gui;
using Mono.Terminal;

namespace wttop.Widgets {

    public abstract class Widget : FrameView {

        public Widget(string text) : base(text){}

        public abstract bool Update(MainLoop MainLoop);
    }

}