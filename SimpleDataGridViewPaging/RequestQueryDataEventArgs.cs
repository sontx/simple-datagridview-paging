using System;

namespace SimpleDataGridViewPaging
{
    public delegate void RequestQueryDataEventHandler(object sender, RequestQueryDataEventArgs e);

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
