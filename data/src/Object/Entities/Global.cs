using System;

namespace Henge.Data.Entities
{
	public class Global : ObjectEntity
	{
		public virtual long LastAvatarId { get; set; }
		
		
		public long NewAvatarId()
		{
			return ++LastAvatarId;
		}
	}
}

