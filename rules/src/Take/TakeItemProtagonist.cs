using System;
using Henge.Data.Entities;

namespace Henge.Rules.Protagonist.Take
{
	public class TakeItem : HengeRule, IProtagonist
	{
		public override bool Valid(Component subject)
		{
			//Only Actors can take stuff
			return subject is Actor;
		}
		
		
		protected override double Visibility(HengeInteraction interaction)
		{
			//Set our visibility back to default * conspicuousness
			return (Constants.StandardVisibility * interaction.ProtagonistCache.Conspicuousness);
		}
		
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			Actor protagonist	= interaction.Protagonist as Actor;
			Item antagonist		= interaction.Antagonist as Item;
			
			if(!interaction.Finished)	
			{
				if (antagonist != null)
				{
					if (interaction.ProtagonistCache.Capacity < protagonist.Inventory.Count )
					{
						double load		= 0;
						double weight	= interaction.AntagonistCache.Weight;
						
						foreach (Item item in protagonist.Inventory) load += item.Traits["Weight"].Value;
						
						if (interaction.ProtagonistCache.Strength >= (load + weight) * Constants.WeightToLiftStrength)
						{	
							interaction.Impedance += weight * Constants.WeightToLiftStrength;
							if (interaction.ProtagonistCache.UseEnergy(interaction.Impedance))
							{
								//delta to switch ownership of item
								interaction.Deltas.Add((success) => {
									antagonist.Owner.Inventory.Remove(antagonist);
									antagonist.Owner = protagonist;
									protagonist.Inventory.Add(antagonist);
									protagonist.Traits["Weight"].Value += weight;
									return true;
								});
								interaction.Success(string.Format("{0} taken", antagonist.Inspect(protagonist).ShortDescription));
							}
							else interaction.Failure(string.Format("You're too tired to pick up the {0}", antagonist.Inspect(protagonist).ShortDescription), false);
						}
						else interaction.Failure(string.Format("You're carrying too much weight to manage the {0} as well", antagonist.Inspect(protagonist).ShortDescription), false);
					}
					else interaction.Failure("Your hands are full", false);
				}
				else interaction.Failure("You are unable to pick that up", true);
			}
			return interaction;
		}
	}
}

