using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data;
using Henge.Data.Entities;


namespace Henge.Rules
{
	public class PropertyCache
	{
		Component subject;
		Actor actor;
		bool energyCached	= false;
		double energy		= 0.0;

		DataProvider db;
		public Dictionary<Skill, double> SkillBonuses {get; private set;}
		
		
		public PropertyCache(DataProvider db, Component subject)
		{
			this.db				= db;
			this.subject		= subject;
			this.actor			= subject as Actor;
			this.SkillBonuses 	= new Dictionary<Skill, double>();
			this.energyCached 	= this.actor == null;
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
						double newReserve	= reserve.Value - gain;
						double newEnergy	= energy.Value + gain;
						
						using (this.db.Lock(this.actor, reserve, energy))
						{
							reserve.SetValue(newReserve);
							energy.SetValue(newEnergy);
							this.actor.LastModified = DateTime.Now;
						}
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
		
		public double Conspicuousness
		{
			get { return this.subject.Traits.ContainsKey("Conspicuousness") ? this.subject.Traits["Conspicuousness"].Value : Constants.BaseConspicuousness; }
		}
		
		public double Visibility
		{
				get { return this.subject.Traits.ContainsKey("Visibility") ? this.subject.Traits["Visibility"].Value : Constants.StandardVisibility; }
		}
		
		public double Weight
		{
			get { return this.subject.Traits.ContainsKey("Weight") ? this.subject.Traits["Weight"].Value : Constants.ActorBaseWeight; }
		}
		
		public double Capacity
		{
			get { return this.subject.Traits.ContainsKey("Capacity") ? this.subject.Traits["Capacity"].Value : 0; }
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
						
						using (this.db.Lock(energy))
						{
							energy.SetValue(energy.Value - amount);
						}
						
						return true;
					}
				}
				
			}
			
			return false;
		}
		
		//Method for burning energy in non-Strength related activities
		//Unlike Strength-based energy usage, this *doesn't* perform a 
		//skill check, but can *only* be performed if you have the 
		//requisite amount of energy. If you set overdraw to true, this function
		//will blindly use the amount of energy you specified (handy if the Rule
		//is checking energy levels first)
		public bool BurnEnergy(double amount, bool overdraw)
		{
			if (this.actor != null)
			{
				if (overdraw || this.Energy >= amount)
				{
					Trait energy = subject.Traits["Energy"];
					this.energy -= amount;
					
					using (this.db.Lock(energy))
					{
						energy.SetValue(energy.Value - amount);
					}
					
					return true;
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
						if (!this.SkillBonuses.ContainsKey(skill))		this.SkillBonuses.Add(skill, increase);
						else if (increase > this.SkillBonuses[skill])	this.SkillBonuses[skill] = increase;
					}
				}	
			}
			
			return result;
		}	
	}
}
