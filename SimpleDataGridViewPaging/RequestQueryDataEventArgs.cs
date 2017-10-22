using System;

namespace Code4Bugs.SimpleDataGridViewPaging
{
    /// <summary>
    ///     Represents the method that will handle an event that request a query data.
    /// </summary>
    /// <param name="sender">Who did fire this event?</param>
    /// <param name="e">Contains value for this event</param>
    public delegate void RequestQueryDataEventHandler(object sender, RequestQueryDataEventArgs e);

    /// <inheritdoc />
    /// <summary>
    ///     Contains value for events that request a query data.
    /// </summary>
    public class RequestQueryDataEventArgs : EventArgs
    {
        internal RequestQueryDataEventArgs(int maxRecords, int pageOffset)
        {
            MaxRecords = maxRecords;
            PageOffset = pageOffset;
        }

        public int MaxRecords { get; }
        public int PageOffset { get; }
    }
}