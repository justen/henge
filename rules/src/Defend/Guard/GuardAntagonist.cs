using System;
using Henge.Data.Entities;

namespace Henge.Rules.Antagonist.Defend.Guard
{
	public class Guard : HengeRule, IAntagonist
	{
		public override bool Valid (Component subject)
		{
			//Currently only allow Locations to be guarded
			return subject is Location;
		}
		
		
		protected override HengeInteraction Apply(HengeInteraction interaction)
		{
			//nothing to do
			return interaction;
		}
		
	}
}

