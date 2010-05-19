using System;

using Henge.Data.Entities;


namespace Henge.Rules
{
	public interface IRule
	{
		string Interaction	{ get; }
		double Priority(HengeEntity actor);
		Interaction Apply(Interaction interaction);
	}
}
