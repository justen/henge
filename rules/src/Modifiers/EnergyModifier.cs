using System;

using Henge.Data;
using Henge.Data.Entities;


namespace Henge.Rules
{
	public class EnergyModifier : HengeModifier
	{
		public override string Target { get { return "Energy"; }}
		
		
		public override Trait Apply(Actor actor)
		{
			Trait energy = null;
			
			if (actor.Traits.ContainsKey("Energy"))
			{
				energy 			= actor.Traits["Energy"];
				double value 	= energy.Value;

				if (value < energy.Maximum)
				{
					Console.WriteLine(string.Format("Updating energy: Starting with {0}", energy.Value));
					
					double time 	= (DateTime.Now - actor.LastModified).TotalSeconds;
					double gain 	= Constants.EnergyGain * time;
					Trait reserve	= actor.Traits["Reserve"];
					
					if ((value + gain) > energy.Maximum) 	gain = energy.Maximum - value;
					if (gain > reserve.Value)				gain = reserve.Value;
					
					using (this.db.Lock(actor, energy, reserve))
					{
						// Check that another thread has not already performed an update during
						// the above calculations. If it has then it is safe to not bother performing
						// an update since the energy level should be accurate enough.
						if (energy.Value == value)
						{
							reserve.SetValue(reserve.Value - gain);
							energy.SetValue(value + gain);
							actor.LastModified = DateTime.Now;
						}
					}
					
					Console.WriteLine(string.Format("New energy: {0}", energy.Value));
				}
			}
			
			return energy;
		}
	}
}
