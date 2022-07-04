using System;

namespace ZFrame.ZException {
    public class ZFrameException : Exception {
        public ZFrameException() { }

        public ZFrameException(string message) : base(message) { }

        public ZFrameException(string message, Exception innerException) : base(message, innerException) { }

    }
}
