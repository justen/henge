
using System;
using System.Collections.Generic;
using Henge.Engine.Ruleset;
using Henge.Data.Entities;

namespace Henge.Engine
{


	public sealed class Interactor
	{
		Rulebook Rulebook
		private Interactor ()
		{
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
		
		public void Interact(Actor protagonist, HengeEntity antagonist, string Interaction)
		{
	
		}
		
		public void Interact(Actor protagonist, IList<HengeEntity> antagonists, string Interaction)
		{
			//string protagonistRule = protagonist.GetProtagonistRule(Interaction);
		}
	}
}
