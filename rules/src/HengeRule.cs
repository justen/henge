using System;
using Henge.Data.Entities;

namespace Henge.Rules
{
	public abstract class HengeRule : Rule
	{
		public override IInteraction Apply (IInteraction interaction)
		{
			interaction = this.Apply(interaction as HengeInteraction);
			
			// if the rule has modified the visibility of the subject, apply the visibility change
			// Setting Visibility to <0 in the derived Rule signals that the rule shouldn't modify visibility
			Component subject = null;
			double visibility = this.Visibility(interaction as HengeInteraction, out subject);
		
			if (visibility >= 0 && subject != null)
			{
				if (!subject.Traits.ContainsKey("Visibility"))
				{
					using (interaction.Lock(subject.Traits))
					{
						if (!subject.Traits.ContainsKey("Visibility")) subject.Traits.Add("Visibility", new Trait(double.MaxValue, 0, visibility));
					}
				}
				using (interaction.Lock(subject.Traits["Visibility"])) subject.Traits["Visibility"].SetValue(visibility);
			}
			return interaction;
		}
	
		protected bool Validate (HengeInteraction interaction)
		{	
			bool result = false;
			if (!interaction.Finished)
			{
				if (interaction.Protagonist != null && interaction.Antagonist != null)
				{
						if (interaction.Protagonist.Traits.ContainsKey("Health") && interaction.Protagonist.Traits["Health"].Flavour=="Dead")
						{
							interaction.Failure("You are dead", false);
						}
						else result = true;
				}
				else interaction.Failure("Interactors are invalid", true);
			}
			return result;
		}
		
		protected abstract IInteraction Apply(HengeInteraction interaction);
		protected abstract double Visibility(HengeInteraction interaction, out Component subject);
	}
}
