using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data.Entities;


namespace Henge.Rules
{
	public class PropertyCache
	{
		Component subject;
		Actor actor;
		bool energyCached 		= false;
		double energy			= 0.0;
		IList<Func<bool, bool>> deltas = null;
		
		
		public PropertyCache(IList<Func<bool, bool>> deltas, Component subject)
		{
			this.deltas		= deltas;
			this.subject	= subject;
			this.actor		= subject as Actor;
			
			if (this.actor == null) this.energyCached = true;
		}
		
		
		public double Energy
		{
			get
			{
				if (!this.energyCached)
				{
					// Get the protagonist energy level here accounting for transferal since the last modification time
					Trait energy 	= this.actor.Traits["Energy"];
					Skill fitness	= this.actor.Skills.ContainsKey("Fitness") ? this.actor.Skills["Fitness"] : null;
					this.energy		= energy.Value;
					
					if (fitness != null && fitness.Value > 0)
					{
						double time		= (DateTime.Now - this.actor.LastModified).TotalSeconds;
						double gain 	= Constants.MaxEnergyGain * fitness.Value * time;
						Trait reserve 	= this.actor.Traits["Reserve"];
						
						if ((energy.Value + gain) > energy.Maximum) gain = energy.Maximum - energy.Value;
						if (gain > reserve.Value)					gain = reserve.Value;
						
						this.energy += gain;
						
						this.deltas.Add((success) => {
							reserve.Value	-= gain;
							energy.Value	+= gain;
							this.actor.LastModified = DateTime.Now;
							return true;
						});
					}
					
					this.energyCached = true;
				}
				
				return this.energy;
			}

		}
		
		
		public double Strength
		{
			get { return (this.actor != null) ? (this.actor.Skills.ContainsKey("Strength") ? this.actor.Skills["Strength"].Value : Constants.DefaultSkill) : 0.0; }
		}
		
		
		public double Weight
		{
			get { return this.subject.Traits.ContainsKey("Weight") ? this.subject.Traits["Weight"].Value : Constants.ActorBaseWeight; }
		}
		
		
		// Attempt to use the specified amount of energy of the actor. Return false if the actor is already at zero energy or less,
		// or if the amount of energy to be used is greater than the actorts strength skill.
		public bool UseEnergy(double amount)
		{
			if (this.actor != null)
			{
				if (this.Energy > 0)
				{
					Trait energy = subject.Traits["Energy"];
					
					if (this.SkillCheck("Strength", amount / energy.Maximum))
					{
						this.energy -= amount;
						
						this.deltas.Add((success) => {
							energy.Value -= amount;
							return true;
						});
						
						return true;
					}
				}
				
			}
			
			return false;
		}
		
		
		// Performs a SkillCheck. actor is the entity whose skill is being tested, name is the name of the skill and difficulty is 
		// the difficulty of the task (and should be in the range 0.0-1.0). If the test is passed the function returns true and increases
		// the skill of the actor. If the test is failed, the function returns false. If it's only just failed, the actor gets a small
		// skill increase.
		public bool SkillCheck(string name, double difficulty)
		{
			bool result = false;
			
			if (this.actor != null)
			{
				if (difficulty < 0 ) difficulty = 0;
				
				if (this.actor.Skills.ContainsKey(name))
				{
					double increase	= 0;
					Skill skill		= this.actor.Skills[name];
					
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
						this.deltas.Add((success) => {
							skill.Add(increase);
							
							if (skill.Value == 1.0)
							{
								foreach (string s in skill.Children)
								{
									if (!this.actor.Skills.ContainsKey(s)) this.actor.Skills.Add(s, new Skill { Value = Constants.SkillGrantDefault });
								}
							}
							return true;
						});
					}
				}	
			}
			
			return result;
		}	
	}
}
