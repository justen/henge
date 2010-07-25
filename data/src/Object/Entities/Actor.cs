using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public abstract class Actor : MapComponent
	{
		public virtual IDictionary<string, Skill> Skills	{get; set;}
		
		
		public Actor(ComponentType type) : base(type)
		{
			this.Skills	= new Dictionary<string, Skill>();
		}  
		
		
		public Actor()
		{
			this.Skills	= new Dictionary<string, Skill>();
		}
	}
}