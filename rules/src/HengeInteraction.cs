using System;
using System.Linq;

using Henge.Data.Entities;


namespace Henge.Rules
{
	public class HengeInteraction : Interaction
	{
		public double Energy	{ get; set; }
		public double Strength	{ get; set; }
		public double Impedance { get; set; }
		
		
		public HengeInteraction(Actor protagonist, Component antagonist) : base(protagonist, antagonist)
		{
			// Get the protagonist energy level here accounting for transferal since the last modification time
			Trait energy 	= protagonist.Traits["Energy"];
			Skill fitness	= protagonist.Skills.ContainsKey("Fitness") ? protagonist.Skills["Fitness"] : null;
			this.Energy		= energy.Value;
			this.Strength	= protagonist.Skills.ContainsKey("Strength") ? protagonist.Skills["Strength"].Value : Constants.DefaultSkill;
			
			if (fitness != null && fitness.Value > 0)
			{
				double time		= (DateTime.Now - protagonist.LastModified).TotalSeconds;
				double gain 	= Constants.MaxEnergyGain * fitness.Value * time;
				Trait reserve 	= protagonist.Traits["Reserve"];
				
				if ((energy.Value + gain) > energy.Maximum) gain = energy.Maximum - energy.Value;
				if (gain > reserve.Value)					gain = reserve.Value;
				
				this.Energy += gain;
				
				this.Deltas.Add((success) => {
					reserve.Value	-= gain;
					energy.Value	+= gain;
					protagonist.LastModified = DateTime.Now;
					return true;
				});
			}
		}
			
	
		// Attempt to use the specified amount of energy of the protagonist. Return false if the protagonist is already at zero energy or less,
		// or if the amount of energy to be used is greater than the protagonists strength skill.
		public bool UseEnergy(double amount)
		{
			if (this.Energy > 0)
			{
				if (this.UseEnergy(this.Protagonist, amount))
				{
					this.Energy -= amount;
					return true;
				}
			}
			
			return false;	
		}
		
		
		// Attempt to use the specified amount of energy of the actor. Return false if the actor is already at zero energy or less,
		// or if the amount of energy to be used is greater than the actorts strength skill.
		public bool UseEnergy(Actor subject, double amount)
		{
			Trait energy = subject.Traits["Energy"];
			
			if (this.SkillCheck(subject, "Strength", amount / energy.Maximum))
			{
				this.Deltas.Add((success) => {
					energy.Value -= amount;
					return true;
				});
				
				return true;
			}
			
			return false;
		}
		
		
		// Performs a SkillCheck. actor is the entity whose skill is being tested, name is the name of the skill and difficulty is 
		// the difficulty of the task (and should be in the range 0.0-1.0). If the test is passed the function returns true and increases
		// the skill of the actor. If the test is failed, the function returns false. If it's only just failed, the actor gets a small
		// skill increase.
		public bool SkillCheck(Actor actor, string name, double difficulty)
		{
			bool result = false;
			if (difficulty < 0 ) difficulty = 0;
			
			if (actor.Skills.ContainsKey(name))
			{
				double increase	= 0;
				Skill skill		= actor.Skills[name];
				
				if (skill.Value >= difficulty)
				{
					increase	= Constants.SkillAcquisition * ((skill.Value > 0) ?  difficulty / skill.Value : Constants.AlmostPassed);
					result		= true;	
				}
				else
				{
					double miss = difficulty - skill.Value;
					if (miss < Constants.AlmostPassed) increase = Constants.CommiserationPrize * (Constants.AlmostPassed - miss) / Constants.AlmostPassed;
				}
				
				if (increase > 0)
				{
					this.Deltas.Add((success) => {
						skill.Add(increase);
						
						if (skill.Value == 1.0)
						{
							foreach (string s in skill.Children)
							{
								if (!actor.Skills.ContainsKey(s)) actor.Skills.Add(s, new Skill { Value = Constants.SkillGrantDefault });
							}
						}
						return true;
					});
				}
			}		
			
			return result;
		}	
	}
}
