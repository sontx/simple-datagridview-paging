using System;

namespace Code4Bugs.SimpleDataGridViewPaging
{
    /// <inheritdoc />
    /// <summary>
    /// Manages querying data from the database.
    /// </summary>
    public interface IDbRequestHandler : IDisposable
    {
        int NumberOfRecords { get; }

        object DataSource(int maxRecords, int pageOffset);
    }
}