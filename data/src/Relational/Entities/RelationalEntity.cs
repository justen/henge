using System;


namespace Henge.Data.Entities
{
	public abstract class RelationalEntity : Entity
	{
		public virtual long Id { get; protected set; }
	}
}
