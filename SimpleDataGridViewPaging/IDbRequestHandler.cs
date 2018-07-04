using System;

namespace Code4Bugs.SimpleDataGridViewPaging
{
    public interface IDbRequestHandler : IDisposable
    {
        int NumberOfRecords { get; }

        object DataSource(int maxRecords, int pageOffset);
    }
}