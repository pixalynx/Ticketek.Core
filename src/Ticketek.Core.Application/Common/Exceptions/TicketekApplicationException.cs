namespace Ticketek.Core.Application.Common.Exceptions;

public class TicketekApplicationException : Exception
{
    public TicketekApplicationException(string message) : base(message) 
    {
        
    }
}