using System;
using Terminal.Gui;

namespace wttop.Core
{
    /// <summary>
    /// Create the main Wttop Windows based on the terminal Window
    /// See https://github.com/migueldeicaza/gui.cs/issues/322
    /// </summary>
    public class WttopWindow: Window
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="Title"></param>
        public WttopWindow (string Title) : base (Title)
        { }

        /// <summary>
        /// Process any key pressed, so far this function is not processing keys
        /// They are all passed to the base class
        /// </summary>
        /// <param name="keyEvent">A key event</param>
        /// <returns>True is this has been processed</returns>
        public override bool ProcessKey (KeyEvent keyEvent)
        {
            // Maybe to use later ...
            return base.ProcessKey (keyEvent);
        }
    }
}