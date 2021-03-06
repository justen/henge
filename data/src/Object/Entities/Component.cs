using System;
using System.Linq;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public abstract class Component : TraitfulEntity
	{
		public virtual ComponentType Type						{ get; set; }
		public virtual DateTime LastModified					{ get; set; }
		public virtual DateTime Created							{ get; set; }
		public virtual IList<Item> Inventory			  		{ get; set; }
		// String containing the detail of this instances appearance
		public virtual string Detail 							{ get; set; }
		
		//Things that can be spawned by this component
		public virtual IList<Spawner> Spawns					{ get; set; }
		
		
		public Component()
		{
			this.Inventory		= new List<Item>();
			this.Spawns 		= new List<Spawner>();
		}
		
		
		public Component(ComponentType type)
		{
			this.Type			= type;
			this.Inventory		= new List<Item>();
			this.Spawns 		= new List<Spawner>();
			this.Created		= DateTime.Now;
			this.LastModified	= DateTime.Now;
			
			this.Traits			= new Dictionary<string, Trait>();
			this.Ticks 			= new List<Tick>();
			this.NextTick		= null;
			this.NextTickTime	= DateTime.MaxValue;
			
			if (type != null)
			{
				if (type.BaseTraits != null)
				{
					foreach (KeyValuePair<string, Trait> trait in type.BaseTraits)
					{
						this.Traits.Add(trait.Key, new Trait(trait.Value));
					}
				}
				
				if (type.BaseTick != null)
				{
					foreach (Tick tick in type.BaseTick)
					{
						//TODO: this should add new copies of the tick, not just a reference
						this.Ticks.Add(new Tick(){ Name = tick.Name, Period = tick.Period, Scheduled = tick.Scheduled } );	
					}
				}
				
				if (type.BaseSpawns != null)
				{
					foreach (Spawner spawn in type.BaseSpawns)
					{
						this.Spawns.Add(new Spawner(spawn));
					}
				}
			}
			this.UpdateNextTick();
		}
		
		
		public Appearance Appearance()
		{
			return this.Type.Appearance.FirstOrDefault();
		}
		
		
		public Appearance Inspect(Component inspector)
		{
			if (inspector is Actor) return this.Type.Appearance.LastOrDefault(a => a.Valid(inspector.Traits, (inspector as Actor).Skills));
			else return this.Type.Appearance.LastOrDefault(a => a.Valid(inspector.Traits));
		}
		
	}
}