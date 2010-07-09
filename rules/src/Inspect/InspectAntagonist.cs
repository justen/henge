using System;

using Henge.Data.Entities;

namespace Henge.Rules.Antagonist.Inspect
{
	public class InspectAntagonist : HengeRule, IAntagonist
	{
		public override bool Valid (Component subject)
		{
			//you can inspect anything
			return true;
		}
		
		protected override double Visibility (HengeInteraction interaction)
		{
			//You're not actually doing anything to the target, so leave its visibility alone
			return -1;
		}
		
		#region implemented abstract members of Henge.Rules.HengeRule
		protected override IInteraction Apply (HengeInteraction interaction)
		{
			//Nothing to do
			return interaction;
		}
		#endregion
	}
}

