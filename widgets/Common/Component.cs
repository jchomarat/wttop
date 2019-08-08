using System;
using Terminal.Gui;
using Mono.Terminal;

namespace wttop.Widgets.Common {

    public abstract class Component : View {
        
        protected abstract void UpdateAction(object newValue);

        public virtual bool Update(MainLoop MainLoop, object newValue) {
            MainLoop.Invoke(() => {
                UpdateAction(newValue);
            });
            return true;
        }
    }

}