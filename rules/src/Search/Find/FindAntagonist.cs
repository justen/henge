using System;
using System.Collections.Generic;
using System.Linq;
using Henge.Data.Entities;

namespace Henge.Rules.Antagonist.Search.Find
{
	public class FindAntagonist : HengeRule, IAntagonist
	{
		public override bool Valid (Component subject)
		{
			//you can only try to search a Location
			return (subject is Location);
		}
		
		protected override double Visibility (HengeInteraction interaction)
		{
			//Don't change visibility
			return -1;
		}
		
		#region implemented abstract members of Henge.Rules.HengeRule
		protected override HengeInteraction Apply (HengeInteraction interaction)
		{
			Actor protagonist = interaction.Protagonist as Actor;
			if (protagonist.Location == interaction.Antagonist)
			{
				//potentially want to check for interferers here
				IList<Component> hiddenItems = new List<Component>();
				ComponentType type = interaction.Arguments["ItemType"] as ComponentType;
				if (type!=null)
				{
					double perception = 1 - (protagonist.Skills.ContainsKey("Perception")? protagonist.Skills["Perception"].Value : Constants.DefaultSkill);
					(interaction.Antagonist as Location).Inventory
						.Where(c => c.Type == type && c.Traits.ContainsKey("Visibility") && c.Traits["Visibility"].Value < perception).ToList()
						.ForEach(c => hiddenItems.Add(c));
					Constants.Randomise(hiddenItems);
					interaction.Arguments.Add("Items", hiddenItems);
				}
				else interaction.Failure("You don't know what you're looking for", true);
			}
			else interaction.Failure("You're unable to search a location you aren't in", true);
			return interaction;
		}
		#endregion
	}
}