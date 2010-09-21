using System;


namespace Henge.Data.Entities
{
	public class LogEntry : RelationalEntity
	{  
		public virtual long AvatarId		{ get; set; }
	    public virtual DateTime Occurred 	{ get; set; }
	    public virtual string Entry			{ get; set; }
	}
}