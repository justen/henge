using System;
using Henge.Data.Entities;

namespace Henge.Rules
{
	public abstract class HengeRule : Rule
	{
		public override IInteraction Apply (IInteraction interaction)
		{
			interaction =  this.Apply(interaction as HengeInteraction);
			
			// if the rule has modified the visibility of the subject, apply the visibility change
			// Setting Visibility to <0 in the derived Rule signals that the rule shouldn't modify visibility
			double visibility = this.Visibility(interaction as HengeInteraction);
			if (visibility>=0)  
			{
				Component subject = interaction.Subject;
				interaction.Deltas.Add((success) => {
						Trait trait = subject.Traits["Visibility"];
						trait.Value = visibility;
						return true;
				});
			}
			return interaction;
		}
	
		
		protected abstract HengeInteraction Apply(HengeInteraction interaction);
		protected abstract double Visibility(HengeInteraction interaction);
	}
}
