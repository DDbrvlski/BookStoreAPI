﻿namespace BookStoreAPI.Infrastructure.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message)
        {

        }
    }
}
