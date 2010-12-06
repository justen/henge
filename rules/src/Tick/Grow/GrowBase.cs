using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data;
using Henge.Data.Entities;

namespace Henge.Rules.Antagonist.Tick.Grow
{
	public abstract class GrowBase : TickRule, IAntagonist
	{		
		protected override double Visibility (HengeInteraction interaction, out Component subject)
		{
			//This does not affect visibility
			subject = null;
			return -1;
		}
		
		protected IInteraction Grow(Location location, HengeInteraction interaction)
		{
			int z = location.Z;
			
			//TODO: when we've got other conditions (weather/seasons/etc) we'll need to use them in the query here too
			IList<Spawner> spawns = location.Spawns;
			List<Item> newItems = new List<Item>();
			List<Npc> newNpcs = new List<Npc>();
			Component newThing;
			foreach (Spawner spawn in spawns)
			{
				if ((spawn.MinZ < z) && (spawn.MaxZ > z))
				{
					if (spawn.SpawnRate > Constants.RandomNumber)
					{
						//spawn a new thing
						System.Type type = Type.GetType(string.Format("Henge.Data.Entities.{0},Henge.Data", spawn.Class));
						newThing = (Component) Activator.CreateInstance( type, spawn.ComponentType);
						if (newThing!=null)
						{
							newThing.Traits.Add("Visibility", new Trait(1.0, 0.0,
								                                    (newThing.Traits.ContainsKey("Conspicuousness")? newThing.Traits["Conspicuousness"].Value : 1.0) * 
							                                        (location.Traits.ContainsKey("Cover") ? location.Traits["Cover"].Value : Constants.DefaultCover) * 
							                                        Constants.RandomNumber)
							                						);
							if (newThing is Item) newItems.Add(newThing as Item);
							if (newThing is Npc) newNpcs.Add(newThing as Npc);
						}
					}
				}			
			}	
			if ((newNpcs.Count>0)/*this next condition is a hack until a better limit is coded up*/ &&(location.Fauna.Count<5))
			{
				using (interaction.Lock(location.Fauna))
				{
					foreach (Npc npc in newNpcs) 
					{
						npc.Location = location;
						location.Fauna.Add(npc);
					}
				}
			}
			if ((newItems.Count>0)/*this next condition is a hack until a better limit is coded up*/ &&(location.Inventory.Count<20))
			{
				using (interaction.Lock(location.Inventory))
				{
					foreach (Item item in newItems)
					{
						item.Owner = location;
						location.Inventory.Add(item);
					}
				}
			}
			return interaction;
		}
	}
}