using System;

namespace In.Sontx.SimpleDataGridViewPaging.Exceptions
{
    public abstract class RuntimeException : Exception
    {
        public RuntimeException() { }
        public RuntimeException(string message) : base(message) { }
    }
}
