using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data;
using Henge.Data.Entities;

namespace Henge.Rules.Antagonist.Tick.Grow
{
	public class GrowAntagonist : GrowBase, IAntagonist
	{
		public override bool Valid (Component subject)
		{
			//This is the spawn rule for MapComponents
			return (subject is MapComponent);
		}
		
		protected override double Visibility (HengeInteraction interaction, out Component subject)
		{
			//This does not affect visibility
			subject = null;
			return -1;
		}
		
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			Location location = (interaction.Antagonist as MapComponent).Location;
			if (this.Validate(interaction) && location!=null)
			{
				this.Grow(location, interaction);
			}
			return interaction;
		}
	}
}