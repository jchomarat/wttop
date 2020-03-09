using System;
using Terminal.Gui;

namespace wttop.Core
{
    //https://github.com/migueldeicaza/gui.cs/issues/322
    public class WttopWindow: Window
    {
        public Action F1_Pressed;

        public WttopWindow (string Title) : base (Title)
        {
        }

        public override bool ProcessKey (KeyEvent keyEvent)
        {
            // Maybe to use later ...
            return base.ProcessKey (keyEvent);
        }
    }
}