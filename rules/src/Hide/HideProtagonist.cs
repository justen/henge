using System;

using Henge.Data.Entities;


namespace Henge.Rules.Protagonist.Hide
{
	public class HideProtagonist : HengeRule, IProtagonist
	{	
		
		//This rule allows you to hide *something* in the current location.
		public override bool Valid(Component subject)
		{
			//you have to be an actor
			return subject is Actor;
		}
		
		
		protected override double Visibility(HengeInteraction interaction)
		{
			//Unless hiding ourself, we should probably become Standardly Visible:
			if (interaction.Antagonist != interaction.Protagonist)
			{
				return Constants.StandardVisibility * interaction.SubjectCache.Conspicuousness;
			}
			else return -1;
		}
		
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			if (!interaction.Finished)
			{
				Actor protagonist		= interaction.Protagonist as Actor;
				Component antagonist	= interaction.Antagonist;
				
				//Hiding a Location is dumb, and will just fail.
				if (antagonist is Location)
				{
					interaction.Failure("You cannot hide a location", true);	
				}
				else
				{
					//You can try to hide pretty much anything else.
					//I suspect hiding a building is going to be a bit very impossible, 
					//but we can handle that by simply giving Edifices very, very large Conspicuousness values.
					//Difficulty of hiding something depends upon
					// 1) Conspicuousness of the thing being hidden
					// 2) Things interfering
					// - Up to this point, the running total is cached in interaction.Impedance -
					// 3) The "Cover" trait of the Location - even if you're useless at hiding stuff, this could make it easier
					// 4) The protagonists "Hide" skill
					
					double visibility = Constants.StandardVisibility;
					
					//This is going to cost us Some Energy. The better we hid it, the more effort it took, so...
					double maxEnergy	= interaction.Protagonist.Traits["Energy"].Maximum;
					double best			= interaction.ProtagonistCache.Energy / maxEnergy;
					double randomNumber	= Constants.RandomNumber;
					randomNumber		= (randomNumber > best) ? best : randomNumber;
					
					if (randomNumber > 0)
					{
						interaction.ProtagonistCache.BurnEnergy(randomNumber * maxEnergy, true);
						//So, the worst-case visibility is now known.
						//difficulty is in Impedance, need to modify it by the cover available:
						if(interaction.ProtagonistCache.SkillCheck("Hide", interaction.Impedance / protagonist.Location.Traits["Cover"].Value))
						{
							//You've hidden it. But how well?
							//Let's add our rng result to the hide skill and divide by two
							visibility -= visibility * (interaction.Protagonist.Skills.ContainsKey("Hide") ? 0.5 * (randomNumber + interaction.Protagonist.Skills["Hide"].Value) : 0.5 * randomNumber);
							
							if (antagonist is Avatar)	interaction.Success(string.Format("You attempt to conceal {0}", antagonist.Name));	
							else 						interaction.Success(string.Format("You attempt to conceal the {0}", antagonist.Inspect(protagonist).ShortDescription));	
						}
						else interaction.Failure("Your skill at concealing things isn't up to the task", false);
					}
					else interaction.Failure("You are too tired", false);
					
					visibility *= interaction.AntagonistCache.Conspicuousness;
					
					if (antagonist is Item && (antagonist as Item).Owner == protagonist)
					{
						Item item = antagonist as Item;
						interaction.Deltas.Add((success) => {
							protagonist.Location.Inventory.Add(item);
							item.Owner = protagonist.Location;
							protagonist.Traits["Weight"].Value -= item.Traits["Weight"].Value;
							protagonist.Inventory.Remove(item);
							return true;
						});
					}	
					interaction.Deltas.Add((success) => {
						if (!antagonist.Traits.ContainsKey("Visibility")) antagonist.Traits.Add("Visibility", new Trait());
						antagonist.Traits["Visibility"].Value = visibility;
						return true;
					});
				}
			}			
			return interaction;
		}
	}
}

