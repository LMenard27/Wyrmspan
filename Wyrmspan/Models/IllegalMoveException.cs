using System;

public class IllegalMoveException : System.Exception
{
    public IllegalMoveException() {
    }

    public IllegalMoveException(string message)
        : base(message) {
    }

    public IllegalMoveException(string message, Exception inner)
        : base(message, inner) {
    }
}