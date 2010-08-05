using System;
using System.Collections.Generic;

using Henge.Data.Entities;

namespace Henge.Rules.Antagonist.Communicate.Talk
{
	public class ToOccupants : HengeRule, IAntagonist
	{
		public override bool Valid (Component subject)
		{
			//Talking to a building
			return (subject is Edifice);
		}
		
		protected override double Visibility (HengeInteraction interaction, out Component subject)
		{
			//visibility doesn't change
			subject = null;
			return -1;
		}
		
		#region implemented abstract members of Henge.Rules.HengeRule
		protected override IInteraction Apply (HengeInteraction interaction)
		{
			if (this.Validate(interaction))	interaction.Arguments.Add("Recipients", (interaction.Antagonist as Edifice).Inhabitants);
			return interaction;
		}
		#endregion
	}
}