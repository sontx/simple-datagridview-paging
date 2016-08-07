using System;

namespace In.Sontx.SimpleDataGridViewPaging
{
    /// <summary>
    /// Represents the method that will handle an event that request a query data.
    /// </summary>
    /// <param name="sender">Who did fire this event?</param>
    /// <param name="e">Contains value for this event</param>
    public delegate void RequestQueryDataEventHandler(object sender, RequestQueryDataEventArgs e);

    /// <summary>
    /// Contains value for events that request a query data.
    /// </summary>
    public class RequestQueryDataEventArgs : EventArgs
    {
        public int MaxRecords { get; private set; }
        public int PageOffset { get; private set; }

        internal RequestQueryDataEventArgs(int maxRecords, int pageOffset)
        {
            this.MaxRecords = maxRecords;
            this.PageOffset = pageOffset;
        }
    }
}
