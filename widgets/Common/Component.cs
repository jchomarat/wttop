using Terminal.Gui;
using Mono.Terminal;

namespace wttop.Widgets.Common {

    /// <summary>
    /// Base class for each custom components
    /// It takes the type of value that will be sent to the Update method
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Component<T> : View
    {
        
        protected abstract void UpdateAction(T newValue);

        public bool Update(MainLoop MainLoop, T newValue)
        {
            MainLoop.Invoke(() => {
                UpdateAction(newValue);
            });
            return true;
        }
    }

}