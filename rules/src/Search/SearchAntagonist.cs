using System;
using System.Collections.Generic;
using System.Linq;

using Henge.Data.Entities;


namespace Henge.Rules.Antagonist.Search
{
	public class SearchAntagonist : HengeRule, IAntagonist
	{
		public override bool Valid(Component subject)
		{
			//you can only try to search a Location
			return (subject is Location);
		}
		
		
		protected override double Visibility(HengeInteraction interaction, out Component subject)
		{
			//Don't change visibility
			subject = null;
			return -1;
		}
		

		protected override IInteraction Apply(HengeInteraction interaction)
		{
			if (this.Validate(interaction))
			{
				Actor protagonist = interaction.Protagonist as Actor;
				if (protagonist.Location == interaction.Antagonist)
				{
					//potentially want to check for interferers here
					IList<Component> hiddenItems = new List<Component>();
					double perception = 1 - (protagonist.Skills.ContainsKey("Perception")? protagonist.Skills["Perception"].Value : Constants.DefaultSkill);
					this.PrepareFindList(interaction, perception);
					Constants.Randomise(hiddenItems);
					interaction.Arguments.Add("Items", hiddenItems);
				}
				else interaction.Failure("You're unable to search a location you aren't in", true);
			}
			return interaction;
		}

		
		protected virtual IList<Component> PrepareFindList(HengeInteraction interaction, double perception)
		{
			var query = from c in (interaction.Antagonist as Location).Inventory
						where c.Traits.ContainsKey("Visibility") && c.Traits["Visibility"].Value < perception
						select c;

			return query.Cast<Component>().ToList();
		}
	}
}

