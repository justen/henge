using System;

using Henge.Data.Entities;


namespace Henge.Rules.Interference.Move.Autodetect
{
	public class Dive : ScaleInterferer, IInterferer
	{
		public override bool Valid(Component subject)
		{
			return (subject is Location) && subject.Traits.ContainsKey("Movement") && subject.Traits["Movement"].Flavour == "Swim";
		}

		protected override IInteraction Apply(HengeInteraction interaction)
		{
			//Here's where we should do diving, if we're moving down
			if (this.Validate(interaction))
			{
				if (interaction.Arguments.ContainsKey("Transition"))
				{
					int height = (int)interaction.Arguments["Transition"];
					if (height >0) this.ClimbUp(interaction, height);
					if (height <0) this.DiveIn(interaction, height);
				}
			}
			return interaction;			
		}
		
		protected void DiveIn(HengeInteraction interaction, int height)
		{
			interaction.Log+=string.Format("You dive down {0} ", height);	
		}
	}
}