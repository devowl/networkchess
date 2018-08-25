using System.ComponentModel;

namespace NC.Client.Interfaces
{
    /// <summary>
    /// Main window interaction.
    /// </summary>
    public interface IMainWindow
    {
        /// <summary>
        /// Main window closing.
        /// </summary>
        event CancelEventHandler Closing;
    }
}