using System;

namespace SimpleDataGridViewPaging.Exceptions
{
    public abstract class RuntimeException : Exception
    {
        public RuntimeException() { }
        public RuntimeException(string message) : base(message) { }
    }
}
