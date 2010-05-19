
using System;
using System.Collections.Generic;
using Henge.Rules;
using Henge.Data.Entities;

namespace Henge.Engine
{


	public sealed class Interactor
	{
		private Rulebook rulebook;
		private static readonly Interactor instance = new Interactor();
		
		static Interactor ()
		{

		}
		
		private Interactor()
		{
			this.rulebook = Loader.LoadRules();			
		}
		
	    public static Interactor Instance
	    {
	        get
	        {
	            return instance;
	        }
	    }
		
		
		//special case when there are no Interferers
		public Interaction Interact(Actor protagonist, HengeEntity antagonist, string interaction)
		{
			Interaction result = new Interaction() {Antagonist = antagonist, Protagonist = protagonist};
			if (this.rulebook.Section(interaction).ApplyRule<AntagonistRule>(result, antagonist))
			{
				this.rulebook.Section(interaction).ApplyRule<ProtagonistRule>(result, protagonist);
			}
			return result;
		}
		
		
		public Interaction Interact(Actor protagonist, IList<HengeEntity> interferers, HengeEntity antagonist, string interaction)
		{
			Interaction result = new Interaction() {Antagonist = antagonist, Interferers = interferers, Protagonist = protagonist};
			Section rules = this.rulebook.Section(interaction);
			if (rules.ApplyRule<AntagonistRule>(result, antagonist))
			{
				bool success = true;
				foreach (HengeEntity interferer in interferers)
				{
					success = rules.ApplyRule<InterferenceRule>(result, interferer);
					if(!success) break;
				}
				if (success) this.rulebook.Section(interaction).ApplyRule<ProtagonistRule>(result, protagonist);
			}
			return result;
		}
	}
}
