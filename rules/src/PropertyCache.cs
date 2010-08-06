using System;
using System.Linq;
using System.Collections.Generic;

using Henge.Data;
using Henge.Data.Entities;
using Henge.Engine;


namespace Henge.Rules
{
	public enum SkillResult
	{
		PassSufficient,
		PassExhausted,
		FailSufficient,
		FailExhausted
	}
	
	
	public class PropertyCache
	{
		private enum Success
		{
			Full,
			Partial,
			Fail
		}
		
		Component subject;
		Actor actor;
		private Trait energy	= null;

		DataProvider db;
		public Dictionary<Skill, double> SkillBonuses {get; private set;}
		
		
		public PropertyCache(DataProvider db, Component subject)
		{
			this.db				= db;
			this.subject		= subject;
			this.actor			= subject as Actor;
			this.SkillBonuses 	= new Dictionary<Skill, double>();
			
		}
		
		
		public double Energy
		{
			get
			{
				if (this.energy == null) this.energy = Interactor.Instance.UpdateTrait("Energy", this.actor);
				
				return this.energy.Value;
			}
			
			protected set
			{
				if (this.energy != null)
				{
					Console.WriteLine(string.Format("Setting energy to {0}", value));
					using (db.Lock(this.energy)) this.energy.SetValue(value);	
				}
			}
			
		}
		
		
		public double Strength
		{
			get { return (this.actor != null) ? (this.actor.Skills.ContainsKey("Strength") ? this.actor.Skills["Strength"].Value : Constants.DefaultSkill) : 0.0; }
		}
		
		public double Health
		{
			get { return this.subject.Traits.ContainsKey("Health") ? this.subject.Traits["Health"].Value : 0; }
		}
		
		public double Constitution
		{
			get { return this.subject.Traits.ContainsKey("Constitution") ? this.subject.Traits["Constitution"].Value : 0; }
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
		
		
//		// Attempt to use the specified amount of energy of the actor. Return false if the actor is already at zero energy or less,
//		// or if the amount of energy to be used is greater than the actorts strength skill.
//		public bool UseEnergy(double amount)
//		{
//			if (this.actor != null)
//			{
//				if (this.Energy > 0)
//				{
//					Console.WriteLine(string.Format("Energy at {0}, using {1}", this.Energy, amount));
//					return this.SkillCheck("Strength", amount / this.energy.Maximum, amount, amount/10.0);
//				}
//				
//			}
//			
//			return false;
//		}
		
		//Method for burning energy in non-Strength related activities
		//Unlike Strength-based energy usage, this *doesn't* perform a 
		//skill check, but can *only* be performed if you have the 
		//requisite amount of energy. If you set overdraw to true, this function
		//will blindly use the amount of energy you specified (handy if the Rule
		//is checking energy levels first)
		public bool UseEnergy(double amount, EnergyType limitingFactor)
		{
			bool result = false;
			
			if (amount == 0) return true;
			
			if (this.Energy > 0 && this.actor != null)
			{
				double increase		= 0;
				string skilltype	= limitingFactor.ToString();
				
				if (skilltype != "None")
				{
					Success success = this.SkillCheck(skilltype, amount / this.energy.Maximum, out increase);
					
					if (success != PropertyCache.Success.Fail)
					{
						result = success == PropertyCache.Success.Full;

						if (increase != 0)
						{
							Skill skill	= this.actor.Skills[skilltype];
							increase *= Constants.BaseEnergyUseSkillMultiplier * amount/Constants.EnergySpan;
							
							if (!this.SkillBonuses.ContainsKey(skill))		this.SkillBonuses.Add(skill, increase);
							else if (increase > this.SkillBonuses[skill])	this.SkillBonuses[skill] = increase;
						}
					}
					
				}
				else result = true;
				
				if (result) this.Energy -= amount;
			}
			return result;
		}
		
		
		private Success SkillCheck(string name, double difficulty, out double increase)
		{
			Success result	= Success.Fail;
			increase		= 0;
			difficulty		= (difficulty < 0) ? 0 : difficulty;

			if (this.actor.Skills.ContainsKey(name))
			{
				Skill skill	= this.actor.Skills[name];
				
				if (skill.Value >= difficulty)
				{
					increase	= Constants.SkillAcquisition * ((skill.Value > 0) ?  difficulty / skill.Value : Constants.AlmostPassed);
					result		= Success.Full;	
				}
				else
				{			
					double miss = difficulty - skill.Value;
				//	double almostPassed = Constants.AlmostPassed + Constants.AlmostPassed * (Constants.RandomNumber - 0.5);
					if (miss < Constants.AlmostPassed)
					{
						increase = Constants.CommiserationPrize * (Constants.AlmostPassed - miss) / Constants.AlmostPassed;
						result = PropertyCache.Success.Partial;
					}
				}
			}
			return result;
		}	
			
		// Performs a SkillCheck. actor is the entity whose skill is being tested, name is the name of the skill and difficulty is 
		// the difficulty of the task (and should be in the range 0.0-1.0). If the test is passed the function returns true and increases
		// the skill of the actor. If the test is failed, the function returns false. If it's only just failed, the actor gets a small
		// skill increase.
		public SkillResult SkillCheck(string name, double difficulty, double successTariff, double failTariff, EnergyType energyType)
		{
			SkillResult result	= SkillResult.FailExhausted;
			double tariff		= failTariff;
			double increase		= 0;
			
			if (this.actor != null)
			{
				Success success = this.SkillCheck(name, difficulty, out increase);
				
				if (success != Success.Fail)
				{
					if (success == Success.Full)
					{
						tariff = successTariff;
						result = SkillResult.PassExhausted;
					}
				}
				
				if (this.UseEnergy(tariff, energyType))
				{
					result--;
					if (increase != 0)
					{
						Skill skill = this.actor.Skills[name];
						increase *= tariff/Constants.EnergySpan;
						
						if (!this.SkillBonuses.ContainsKey(skill))		this.SkillBonuses.Add(skill, increase);
						else if (increase > this.SkillBonuses[skill])	this.SkillBonuses[skill] = increase;
					}
					
				}
			}
			
			return result;
		}	
	}
}
