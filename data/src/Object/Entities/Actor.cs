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
			foreach (string skill in type.BaseSkills.Keys)
			{
				this.Skills.Add(skill, new Skill() { Value = type.BaseSkills[skill].Value, Children = new List<string>(type.BaseSkills[skill].Children) });	
			}				
		}  
		
		
		public Actor()
		{
			this.Skills	= new Dictionary<string, Skill>();
		}
	}
}