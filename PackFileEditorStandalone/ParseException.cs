using System;

namespace PackFileEditorStandalone
{
    public class ParseException : Exception
    {
        public long OccurredAt { get; private set; }
        
        public ParseException(string message, long position) : base(message) {
            OccurredAt = position;
        }

        public ParseException (string message, long position, Exception x)
        : base(message, x) {
            OccurredAt = position;
        }
    }
}

