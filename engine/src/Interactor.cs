
using System;
using System.Collections.Generic;
using Henge.Rules;
using Henge.Data.Entities;

namespace Henge.Engine
{


	public sealed class Interactor
	{
		private Rulebook rulebook;
		
		
		private Interactor ()
		{
			this.rulebook = Loader.LoadRules();
		}
		
		
	    public static Interactor Instance
	    {
	        get
	        {
	            return Internal.instance;
	        }
	    }
		
		
		class Internal
		{
			// Explicit static constructor to tell C# compiler
			// not to mark type as beforefieldinit
			static Internal()
			{
				
			}
			
			internal static readonly Interactor instance = new Interactor();
		}
		
		//special case when there are no Interferers
		public void Interact(Actor protagonist, HengeEntity antagonist, string interaction)
		{
			Interaction result = this.rulebook.Chapter(interaction).Rule<IAntagonistRule>(antagonist).ConcludeInteraction(this.rulebook.Chapter(interaction).Rule<IProtagonistRule>(protagonist).BeginInteraction(protagonist, antagonist));
		}
		
		
		public void Interact(Actor protagonist, IList<HengeEntity> antagonists, string interaction)
		{
			//string protagonistRule = protagonist.GetProtagonistRule(Interaction);
		}
	}
}
