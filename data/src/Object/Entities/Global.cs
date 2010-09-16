using System;

namespace Henge.Data.Entities
{
	public class Global : ObjectEntity
	{
		public virtual long LastAvatarID { get; set; }
		
		public long NewAvatarID()
		{
			return ++LastAvatarID;
		}
	}
}

