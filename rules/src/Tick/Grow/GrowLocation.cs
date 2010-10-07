using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data;
using Henge.Data.Entities;

namespace Henge.Rules.Antagonist.Tick.Grow
{
	public class GrowLocation : GrowBase, IAntagonist
	{
		public override bool Valid (Component subject)
		{
			//This is the spawn rule for Locations only
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
			if (this.Validate(interaction))
			{
				this.Grow(interaction.Antagonist as Location, interaction);
			}
			return interaction;
		}
	}
}