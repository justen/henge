using System;

using Henge.Data.Entities;

namespace Henge.Rules.Antagonist.Metabolise
{
	public class MetaboliseAntagonist : HengeRule, IAntagonist
	{
		public override bool Valid (Component subject)
		{
			//this is an empty rule
			return true;
		}
		
		protected override double Visibility (HengeInteraction interaction, out Component subject)
		{
			//This is an empty rule
			subject = null;
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