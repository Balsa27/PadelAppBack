﻿namespace PadelApp.Application.Exceptions;

public class PriceNotFoundException : Exception
{
    public PriceNotFoundException(string message) : base(message) { }
}