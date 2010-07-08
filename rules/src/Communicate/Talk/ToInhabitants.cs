using System;
using System.Collections.Generic;

using Henge.Data.Entities;

namespace Henge.Rules.Antagonist.Communicate.Talk
{
	public class ToInhabitants : HengeRule, IAntagonist
	{
		public override bool Valid (Component subject)
		{
			//Talking to a Location
			return (subject is Location);
		}
		
		protected override double Visibility (HengeInteraction interaction)
		{
			//visibility doesn't change
			return -1;
		}
		
		#region implemented abstract members of Henge.Rules.HengeRule
		protected override IInteraction Apply (HengeInteraction interaction)
		{
			interaction.Arguments.Add("Recipients", (interaction.Antagonist as Location).Inhabitants);
			return interaction;
		}
		#endregion
	}
}