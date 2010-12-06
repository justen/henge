using System;
using System.Linq;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public abstract class TraitfulEntity : ObjectEntity
	{
		public virtual string Name								{ get; set; }
		public virtual IList<Tick> Ticks						{ get; set; }
		public virtual Tick NextTick							{ get; set; }
		public virtual DateTime NextTickTime					{ get; set; }
		public virtual IDictionary<string, Trait> Traits 		{ get; set; }
		
		public TraitfulEntity()
		{
			this.Ticks			= new List<Tick>();
			this.Traits 		= new Dictionary<string, Trait>();	
		}
		
		
		public void UpdateNextTick()
		{
			DateTime nextTime 	= DateTime.MaxValue;
			Tick nextTick		= null;
			
			foreach (Tick t in this.Ticks)
			{
				if (t.Scheduled < nextTime)
				{
					nextTime = t.Scheduled;
					nextTick = t;
				}
			}
			
			this.NextTickTime 	= nextTime;
			this.NextTick		= nextTick;
		}
	}
}

