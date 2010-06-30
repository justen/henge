using System;
using Henge.Data.Entities;

namespace Henge.Rules.Protagonist.Take
{
	public class TakeItem : HengeRule, IProtagonist
	{
		public override bool Valid (Component subject)
		{
			//Only Actors can take stuff
			return subject is Actor;
		}
		
		protected override HengeInteraction Apply(HengeInteraction interaction)
		{
			Actor protagonist = interaction.Protagonist as Actor;
			Item subject = interaction.Subject as Item;
			if(!interaction.Finished)	
			{
				if (interaction.TraitCheck(protagonist, "Capacity") && protagonist.Traits["Capacity"].Value < protagonist.Inventory.Count )
				{
					if (interaction.ProtagonistCache.SkillCheck("Strength", interaction.SubjectCache.Weight * Constants.WeightToLiftStrength))
					{
						if (interaction.ProtagonistCache.UseEnergy(interaction.Impedance))
						{
							//delta to switch ownership of item
							interaction.Deltas.Add((success) => {
								subject.Owner.Inventory.Remove(subject);
								subject.Owner = protagonist;
								protagonist.Inventory.Add(subject);
								return true;
							});
						}
					}
				}
			}
			return interaction;
		}
	}
}

