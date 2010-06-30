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
		
		
		protected override HengeInteraction Apply(HengeInteraction interaction)
		{
			//put the appropriate guarding trait into the interaction
			if ((interaction.Antagonist is Location)||(interaction.Antagonist is Edifice))
			{
				//for Locations and Edifices, we want to impede entry
				interaction.Arguments.Add("Impede", null);
			}
			if (!(interaction.Antagonist is Location))
			{
				//for everything *except* Locations we want to stop people from stealing or attacking
				interaction.Arguments.Add("Guard", null);
			}
			
			return interaction;
		}
		
	}
}

