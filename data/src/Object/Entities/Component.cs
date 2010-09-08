using System;
using System.Linq;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public abstract class Component : ObjectEntity
	{
		public virtual String Name 								{ get; set; }
		public virtual ComponentType Type						{ get; set; }
	    public virtual IDictionary<string, Trait> Traits 		{ get; set; }
		public virtual DateTime LastModified					{ get; set; }
		public virtual DateTime Created							{ get; set; }
		public virtual IList<Item> Inventory			  		{ get; set; }
		// String containing the detail of this instances appearance
		public virtual string Detail 							{ get; set; }
		
		public virtual IList<Tick> Ticks						{ get; set; }
		public virtual Tick NextTick							{ get; set; }		
		
		public Component()
		{
			this.Inventory		= new List<Item>();
			this.Traits 		= new Dictionary<string, Trait>();
		}
		
		
		public Component(ComponentType type)
		{
			this.Ticks = new List<Tick>();
			this.Type			= type;
			this.Inventory		= new List<Item>();
			this.Created		= DateTime.Now;
			this.LastModified	= DateTime.Now;
			this.Traits			= new Dictionary<string, Trait>();
			
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
						this.Ticks.Add(tick);	
					}
				}
			}
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
		
		public void UpdateNextTick(bool stashCurrent = false)
		{
			Tick nextTick = null;
			if (stashCurrent)
			{
				this.Ticks.Add(this.NextTick);	
			}
			if (this.Ticks.Count > 0)	
			{
				nextTick = this.Ticks[0];
				foreach(Tick tick in this.Ticks)
				{
					if (tick.Scheduled < nextTick.Scheduled)
					{
						nextTick = tick;	
					}
				}
				this.Ticks.Remove(nextTick);
			}
			this.NextTick = nextTick;
		}
	}
}