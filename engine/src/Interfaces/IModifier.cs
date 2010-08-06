using System;

using Henge.Data;
using Henge.Data.Entities;


namespace Henge.Rules
{
	public interface IModifier
	{
		string Target { get; }
		
		void Initialise(DataProvider db);
		
		Trait Apply(Actor actor);
	}
}
