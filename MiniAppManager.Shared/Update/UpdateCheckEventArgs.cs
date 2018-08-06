using System;

namespace Bluegrams.Application
{
    /// <summary>
    /// Contains the event data for an update check.
    /// </summary>
    public class UpdateCheckEventArgs : EventArgs
    {
        /// <summary>
        /// True if check for updates was successful, false if no Update object could be retrieved.
        /// </summary>
        public bool Successful { get; private set; }

        /// <summary>
        /// The retrieved update information.
        /// </summary>
        public AppUpdate Update { get; private set; }

        /// <summary>
        /// Holds the exception that caused the failure if the update check was not successful.
        /// </summary>
        public Exception UpdateCheckException { get; private set; }

        /// <summary>
        /// Creates a new instance of type UpdateCheckEventArgs.
        /// </summary>
        public UpdateCheckEventArgs(bool success, AppUpdate update, Exception ex = null)
        {
            Successful = success;
            Update = update;
            UpdateCheckException = ex;
        }
    }
}
