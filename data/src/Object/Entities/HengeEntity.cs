using System;
using System.Linq;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public abstract class HengeEntity : ObjectEntity
	{
		public String Name { get; set; }

	    public Dictionary<string, object> Attributes {get; set;}
		public EntityType Type {get; set;}
		//Extra bits and bobs of appearance that make this differ from its type appearance
		public Appearance		IndividualAppearance			{get; set;}
		public List<Appearance> ConditionalIndividualAppearance	{get; set;}	
		
		public HengeEntity()
		{
			this.Attributes = new Dictionary<string, object>();
			this.ConditionalIndividualAppearance = new List<Appearance>();
		}
		
	}
}