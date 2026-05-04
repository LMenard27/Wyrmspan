using System;

public class IllegalMoveException : System.Exception
{
    /*
    Constructor for IllegalMoveException
    */
    public IllegalMoveException() {
    }

    /*
    Constructor for IllegalMoveException with a message
    */
    public IllegalMoveException(string message)
        : base(message) {
    }

    /*
    Constructor for IllegalMoveException with a message and inner exception
    */
    public IllegalMoveException(string message, Exception inner)
        : base(message, inner) {
    }
}