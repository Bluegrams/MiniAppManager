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
        IncludeNegativeResult = 1,
        IfNewer = 2,
        Never = 3
    }
}
