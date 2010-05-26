using System;
using FluentNHibernate.Mapping;


namespace Henge.Data.Entities
{
	public abstract class Entity
	{
		public virtual long Id	{ get; protected set; }
	}
	
	
	public class EntityMap : ClassMap<Entity>
	{
		public EntityMap()
		{
			Id(x => x.Id);
			
		}
	}
}
