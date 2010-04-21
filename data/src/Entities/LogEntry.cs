using System;


namespace Henge.Data.Entities
{
	public class LogEntry : Entity
	{  
	    public virtual DateTime Occurred 	{get; set;}
	    public virtual string Entry			{get; set;}
	}
}