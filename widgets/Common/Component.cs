using Terminal.Gui;
using Mono.Terminal;

namespace wttop.Widgets.Common {

    public abstract class Component<T> : View
    {
        
        protected abstract void UpdateAction(T newValue);

        public virtual bool Update(MainLoop MainLoop, T newValue)
        {
            MainLoop.Invoke(() => {
                UpdateAction(newValue);
            });
            return true;
        }
    }

}