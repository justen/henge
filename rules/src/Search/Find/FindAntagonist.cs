using System;
using System.Collections.Generic;
using System.Linq;
using Henge.Data.Entities;
using Henge.Rules.Antagonist.Search;

namespace Henge.Rules.Antagonist.Search.Find
{
	public class FindAntagonist : SearchAntagonist
	{	
		protected override IList<Component> PrepareFindList (HengeInteraction interaction, double perception)
		{
			ComponentType type = interaction.Arguments["ItemType"] as ComponentType;
			IList<Component> hiddenItems = new List<Component>();
			if (type!=null)
			{
				(interaction.Antagonist as Location).Inventory
				.Where(c => c.Type == type && c.Traits.ContainsKey("Visibility") && c.Traits["Visibility"].Value < perception).ToList()
				.ForEach(c => hiddenItems.Add(c));	
			}
			else interaction.Failure("You don't know what you're looking for", true);
			return hiddenItems;
		}
	}
}