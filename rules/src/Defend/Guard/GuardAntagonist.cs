using System;

using Henge.Data.Entities;


namespace Henge.Rules.Antagonist.Defend.Guard
{
	public class Guard : HengeRule, IAntagonist
	{
		public override bool Valid (Component subject)
		{
			//Guard whatever you like...
			return true;
		}
		
		
		protected override double Visibility (HengeInteraction interaction, out Component subject)
		{
			//Don't change visibility
			subject = null;
			return -1;
		}
		
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			if (this.Validate(interaction))
			{
				//put the appropriate guarding trait into the interaction
				if (interaction.Antagonist is Location || interaction.Antagonist is Edifice)
				{
					//for Locations and Edifices, we want to impede entry
					interaction.Arguments.Add("Impede", null);
				}
				if (!(interaction.Antagonist is Location))
				{
					//for everything *except* Locations we want to stop people from stealing or attacking
					interaction.Arguments.Add("Guard", null);
				}
			}
			return interaction;
		}
		
	}
}

