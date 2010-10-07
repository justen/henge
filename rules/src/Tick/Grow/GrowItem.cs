using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data;
using Henge.Data.Entities;

namespace Henge.Rules.Antagonist.Tick.Grow
{
	public class GrowItem : GrowBase, IAntagonist
	{
		public override bool Valid (Component subject)
		{
			//This is the spawn rule for Items
			return (subject is Item);
		}
		
		protected override double Visibility (HengeInteraction interaction, out Component subject)
		{
			//This does not affect visibility
			subject = null;
			return -1;
		}
		
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			Item item = interaction.Antagonist as Item;
			Location location = null;
			Component currentLevel = item;
			while ((location == null) && (currentLevel != null))
			{
				if (currentLevel is Item)
				{
					if (item.Owner is Location)
					{
						location = item.Owner as Location;
					}
				}
				else
				{
					
				}
			}
			if (this.Validate(interaction) && location!=null)
			{
				this.Grow(location, interaction);
			}
			return interaction;
		}
	}
}