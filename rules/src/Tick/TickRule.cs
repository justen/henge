using System;
namespace Henge.Rules
{
	public abstract class TickRule : HengeRule
	{
		protected override bool Validate (HengeInteraction interaction)
		{	
			bool result = false;
			if (!interaction.Finished)
			{
				if (interaction.Antagonist != null)
				{
						if (interaction.Antagonist.Traits.ContainsKey("Health") && interaction.Antagonist.Traits["Health"].Flavour=="Dead")
						{
							interaction.Failure("You are dead", false);
						}
						else result = true;
				}
				else interaction.Failure("Interactors are invalid", true);
			}
			return result;
		}
	}
}

