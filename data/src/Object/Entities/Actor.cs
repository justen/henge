using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public abstract class Actor : MapComponent
	{
		public IDictionary<string, Skill> Skills	{get; set;}
		
		
		public Actor()
		{
			this.Skills		= new Dictionary<string, Skill>();
		}  
	}
}