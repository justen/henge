using System;

using Henge.Data.Entities;


namespace Henge.Rules.Protagonist.Give
{
	public class GiveItemProtagonist : HengeRule, IProtagonist
	{
		public override bool Valid(Component subject)
		{
			//you have to be an actor
			return subject is Actor;
		}
		
		
		protected override double Visibility(HengeInteraction interaction, out Component subject)
		{
			//Set our visibility back to default * conspicuousness if interacting with an Actor, or
			//do not change if it's an inanimate object;
			subject = interaction.Antagonist;
			if (interaction.Antagonist is Actor)
			{
				return (Constants.StandardVisibility * interaction.SubjectCache.Conspicuousness);
			}
			else return -1;
		}
		
		
		protected override IInteraction Apply(HengeInteraction interaction)
		{
			if (this.Validate(interaction))
			{
				Actor protagonist		= interaction.Protagonist as Actor;
				Component antagonist	= interaction.Antagonist;
				
				if (interaction.Arguments.ContainsKey("Item"))
				{
					Item item = interaction.Arguments["Item"] as Item;
					
					if (item != null)
					{
						if (item.Owner == protagonist)
						{
							string itemDescription	= item.Inspect(protagonist).ShortDescription;
							double weight			= item.Traits["Weight"].Value;
							
							if (antagonist is Location)
							{
								double visibility 		= Constants.StandardVisibility * item.Traits["Conspicuousness"].Value;
								Trait weightTrait 		= protagonist.Traits["Weight"];
								Trait visibilityTrait	= item.Traits["Visibility"];
								
								using (interaction.Lock(protagonist.Inventory, weightTrait, item, visibilityTrait, antagonist.Inventory))
								{
									protagonist.Inventory.Remove(item);
									weightTrait.SetValue(weightTrait.Value - weight);
									item.Owner = antagonist;
									item.Traits["Visibility"].SetValue(visibility);
									antagonist.Inventory.Add(item);
								}
								interaction.Success(string.Format("You put the {0} down", itemDescription));													
							}
							else
							{
								if (interaction.AntagonistCache.Capacity < antagonist.Inventory.Count)
								{
									if (antagonist is Actor)
									{
									
										double load = 0;
										foreach (Item thing in antagonist.Inventory) load += thing.Traits["Weight"].Value;
										if (interaction.AntagonistCache.Strength >= (weight + load) * Constants.WeightToLiftStrength)	
										{
											//delta to switch ownership of item
											Trait protagonistWeight = protagonist.Traits["Weight"];
											Trait antagonistWeight	= antagonist.Traits["Weight"];
											
											using (interaction.Lock(protagonist.Inventory, protagonistWeight, item, antagonist.Inventory, antagonistWeight))
											{
												protagonist.Inventory.Remove(item);
												protagonistWeight.SetValue(protagonistWeight.Value - weight);
												item.Owner = antagonist;
												antagonist.Inventory.Add(item);
												antagonistWeight.SetValue(antagonistWeight.Value - weight);
											}
											if (antagonist is Npc)
											{
												interaction.Success(string.Format("You give the {0} to the {1}", itemDescription, protagonist.Inspect(protagonist).ShortDescription));
											}
											else
											{
												interaction.Success(string.Format("You hand the {0} to {1}", itemDescription, protagonist.Name));
											}
										}
										else interaction.Failure(string.Format("The recipient is not strong enough to carry the {0}", itemDescription), false);
									}
									else
									{
										Trait protagonistWeight = protagonist.Traits["Weight"];
										
										using (interaction.Lock(protagonist.Inventory, protagonistWeight, item, antagonist.Inventory))
										{
											protagonist.Inventory.Remove(item);
											protagonist.Traits["Weight"].SetValue(protagonist.Traits["Weight"].Value - weight);
											item.Owner = antagonist;
											antagonist.Inventory.Add(item);
										}
										interaction.Success(string.Format("You place the {0} in the {1}", itemDescription, antagonist.Inspect(protagonist).ShortDescription));
									}
								}
								else interaction.Failure(string.Format("The recipient is unable to take the {0}", itemDescription), false);
							}
						}
						interaction.Failure("You don't have that item", true);
					}
					interaction.Failure("Item doesn't exist", true);
				}
				interaction.Failure("No item to give", true);
			}
			return interaction;
		}
	}
}

