using System;

namespace Code4Bugs.SimpleDataGridViewPaging.Exceptions
{
    public class QuantityRangeException : Exception
    {
        public QuantityRangeException()
        {
        }

        public QuantityRangeException(string message) : base(message)
        {
        }
    }
}