using System;
using System.Linq;

using Henge.Data.Entities;

namespace Henge.Rules.Antagonist.Take
{
	public class TakeItem : HengeRule, IAntagonist
	{
		public override bool Valid(Component subject)
		{
			//can only take Items
			return subject is Item;
		}
		
		
		protected override double Visibility(HengeInteraction interaction)
		{
			//Don't modify visibility
			return -1;
		}
		
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			Location venue	= ((Actor)interaction.Protagonist).Location;
			Item antagonist = interaction.Antagonist as Item;
			
			//check no one is guarding the item, if they are then add them to the interferers list
			if (venue.Inventory.Contains(antagonist))
			{
				venue.Fauna
					.Where(c => c.Traits.ContainsKey("Guard") && c.Traits["Guard"].Subject == antagonist).ToList()
					.ForEach(c => interaction.Interferers.Add(c));
				venue.Inhabitants
					.Where(c => c.Traits.ContainsKey("Guard") && c.Traits["Guard"].Subject == antagonist).ToList()
					.ForEach(c => interaction.Interferers.Add(c));

				if (interaction.Interferers.Any()) Constants.Randomise(interaction.Interferers);
			}
			else interaction.Failure("The item is no longer here", false);
			
			return interaction;
		}
	}
}