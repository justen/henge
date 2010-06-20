using System;


namespace Henge.Rules
{
	public abstract class HengeRule : Rule
	{
		public override IInteraction Apply (IInteraction interaction)
		{
			return this.Apply(interaction as HengeInteraction);
		}
	
		
		protected abstract HengeInteraction Apply(HengeInteraction interaction);
	}
}
