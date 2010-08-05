using System;

using Henge.Data.Entities;


namespace Henge.Rules.Interference.Move
{
	public class ScaleInterferer : HengeRule, IInterferer
	{
		public override bool Valid(Component subject)
		{
			return subject is Location;
		}

		protected override double Visibility (HengeInteraction interaction, out Component subject)
		{
			//Do not change visibility
			subject = null;
			return -1;
		}
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			//Here's where we should do default climbing up and down stuff	
			if (this.Validate(interaction))
			{
				if (interaction.Arguments.ContainsKey("Transition"))
				{
					int deltaZ = (int)interaction.Arguments["Transition"];
					if (deltaZ > 0) this.ClimbUp(interaction, deltaZ);
					if (deltaZ < 0) this.ClimbDown(interaction, deltaZ);
				}
			}
			return interaction;			
		}
		
		protected virtual HengeInteraction ClimbUp (HengeInteraction interaction, int height)
		{
			//Climb up stuff
			interaction.Log+=string.Format("You climb up {0} ", height);
			return interaction;
		}
		
		protected virtual HengeInteraction ClimbDown (HengeInteraction interaction, int height)
		{
			//Climb down stuff
			interaction.Log+=string.Format("You climb down {0} ", height);
			return interaction;
		}
	}
}
