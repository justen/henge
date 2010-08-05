using System;

using Henge.Data.Entities;


namespace Henge.Rules.Antagonist.Hide
{
	public class HideAntagonist : HengeRule, IAntagonist
	{
		public override bool Valid(Component subject)
		{
			//you can't try to hide a Location
			return !(subject is Location);
		}
		
		
		protected override double Visibility(HengeInteraction interaction, out Component subject)
		{
			//Don't change visibility - leave that for the Protagonist rule
			subject = null;
			return -1;
		}
		
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			if (this.Validate(interaction))
			{
				Component antagonist = interaction.Antagonist;
				
				if (antagonist is Item)
				{
					Item item = antagonist as Item;
					
					if (interaction.Protagonist != item.Owner && item.Owner != interaction.Protagonist.Location)
					{
						interaction.Failure("You do not posess that item", true);	
					}
				}
				else
				{
					if (interaction.Protagonist.Location != (antagonist as MapComponent).Location)
					{
						if (antagonist is Avatar)
						{
							interaction.Failure(string.Format("You are no longer in the same location as {0}", ((Avatar)antagonist).Name), false);	
						}
						else
						{
							interaction.Failure(string.Format("You are no longer in the same location as the {0}", antagonist.Inspect(interaction.Protagonist).ShortDescription), false);
						}
					}
				}
				interaction.Impedance = interaction.AntagonistCache.Conspicuousness;
				
				//if we want to add interferers, we can do. Just have them modify the Impedance
			}
			return interaction;
		}
	}
}

