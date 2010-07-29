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
		
		
		protected override double Visibility(HengeInteraction interaction, out Component subject)
		{
			//Unless hiding ourself, we should probably become Standardly Visible:
			subject = interaction.Protagonist;
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
			
						//So, the worst-case visibility is now known.
						//difficulty is in Impedance, need to modify it by the cover available:
						switch (interaction.ProtagonistCache.SkillCheck("Hide", interaction.Impedance / protagonist.Location.Traits["Cover"].Value, randomNumber * maxEnergy, randomNumber * maxEnergy, EnergyType.Concentration))
						{
						case SkillResult.PassSufficient:
							//You've hidden it. But how well?
							//Let's add our rng result to the hide skill and divide by two
							visibility -= visibility * (interaction.Protagonist.Skills.ContainsKey("Hide") ? 0.5 * (randomNumber + interaction.Protagonist.Skills["Hide"].Value) : 0.5 * randomNumber);
							
							if (antagonist is Avatar)	interaction.Success(string.Format("You attempt to conceal {0}", antagonist.Name));	
							else 						interaction.Success(string.Format("You attempt to conceal the {0}", antagonist.Inspect(protagonist).ShortDescription));	
							break;
							
						case SkillResult.PassExhausted:
							interaction.Failure("You are too tired", false);
							break;
								
						default: interaction.Failure("Your skill at concealing things isn't up to the task", false);
							break;
						}
						visibility *= interaction.AntagonistCache.Conspicuousness;		
						if (antagonist is Item && (antagonist as Item).Owner == protagonist)
						{
							Item item 				= antagonist as Item;
							Trait protagonistWeight = protagonist.Traits["Weight"];
							
							using (interaction.Lock(protagonist.Location.Inventory, item, protagonistWeight, protagonist.Inventory))
							{
								protagonist.Location.Inventory.Add(item);
								item.Owner = protagonist.Location;
								protagonist.Traits["Weight"].SetValue(protagonist.Traits["Weight"].Value - item.Traits["Weight"].Value);
								protagonist.Inventory.Remove(item);
							}
						}
						using (interaction.Lock(antagonist.Traits))
						{
							if (antagonist.Traits.ContainsKey("Visibility")) 
							{
								// Not sure how safe it is to use recursive locking?
								using (interaction.Lock(antagonist.Traits["Visibility"]))
								{
									antagonist.Traits["Visibility"].SetValue(visibility);
								}
							}
							else antagonist.Traits.Add("Visibility", new Trait(double.MaxValue, 0, visibility));
					
						}
					}
					else
					{
						interaction.Failure("You can see nowhere to hide that", false);	
					}
				}			
			}
			return interaction;
		}
	}
}

