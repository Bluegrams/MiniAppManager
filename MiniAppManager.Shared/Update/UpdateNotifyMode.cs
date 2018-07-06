using System;

#pragma warning disable 1591

namespace Bluegrams.Application
{
    /// <summary>
    /// Represents the possible options for standard update notifications.
    /// </summary>
    public enum UpdateNotifyMode
    {
        Always = 0,
        IfNewer = 1,
        Never = 3
    }
}
