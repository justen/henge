using System;


namespace Henge.Data.Entities
{
	public abstract class Entity
	{
		public virtual long Id { get; protected set; }
	}
}
