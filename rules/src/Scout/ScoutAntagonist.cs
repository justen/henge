using System;

using Henge.Data.Entities;

namespace Henge.Rules.Antagonist.Scout
{
	public class ScoutAntagonist : HengeRule, IAntagonist
	{
		public override bool Valid (Component subject)
		{
			//you can only scout a Location
			return (subject is Location);
		}
		
		protected override double Visibility (HengeInteraction interaction)
		{
			//It's a location. You can't change visibility.
			return -1;
		}
		
		#region implemented abstract members of Henge.Rules.HengeRule
		protected override HengeInteraction Apply (HengeInteraction interaction)
		{
			//Nothing to do
			return interaction;
		}
		#endregion
	}
}

