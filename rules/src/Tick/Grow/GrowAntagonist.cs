using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data;
using Henge.Data.Entities;

namespace Henge.Rules.Antagonist.Tick.Grow
{
	public class GrowAntagonist : TickRule, IAntagonist
	{
		public override bool Valid (Component subject)
		{
			//Only Actors can metabolise
			return (subject is Location);
		}
		
		protected override double Visibility (HengeInteraction interaction, out Component subject)
		{
			//This does not affect visibility
			subject = null;
			return -1;
		}
		
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			Location antagonist = interaction.Antagonist as Location;
			int z = antagonist.Z;
			if (this.Validate(interaction) && !interaction.Finished)
			{
				//TODO: when we've got other conditions (weather/seasons/etc) we'll need to use them in the query here too
				List<Spawns> spawns = interaction.db.Query<Spawns>().Where(c => c.LocationType == interaction.Antagonist.Type.Id && c.MaxZ >= z && c.MinZ <= z).ToList();
				List<Item> newItems = new List<Item>();
				List<Npc> newNpcs = new List<Npc>();
				foreach (Spawns spawn in spawns)
				{
					if (spawn.SpawnRate < Constants.RandomNumber)
					{
						//spawn a new thing
						ComponentType type = interaction.db.Get<ComponentType>(x => x.Id == spawn.SpawnType);
						Component newThing = null;
						if (spawn.Class=="Item")
						{
							newThing = (Component) new Item(type);
						}
						if (spawn.Class=="NPC")
						{
							newThing = (Component) new Npc(type);
						}
						if (newThing!=null)
						{
							newThing.Traits.Add("Visibility", new Trait(1.0, 0.0,
								                                    (newThing.Traits.ContainsKey("Conspicuousness")? newThing.Traits["Conspicuousness"].Value : 1.0) * 
							                                        (antagonist.Traits.ContainsKey("Cover") ? antagonist.Traits["Cover"].Value : Constants.DefaultCover) * 
							                                        Constants.RandomNumber)
							                						);
							if (newThing is Item) newItems.Add(newThing as Item);
							if (newThing is Npc) newNpcs.Add(newThing as Npc);
						}
					}
				}	
				if (newNpcs.Count>0)
				{
					using (interaction.Lock(antagonist.Fauna))
					{
						foreach (Npc npc in newNpcs) 
						{
							npc.Location = antagonist;
							antagonist.Fauna.Add(npc);
						}
					}
				}
				if (newItems.Count>0)
				{
					using (interaction.Lock(antagonist.Inventory))
					{
						foreach (Item item in newItems)
						{
							item.Owner = antagonist;
							antagonist.Inventory.Add(item);
						}
					}
				}
			}
			return interaction;
		}
	}
}