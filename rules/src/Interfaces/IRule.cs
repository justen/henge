
using System;
using Henge.Data.Entities;

namespace Henge.Engine.Ruleset
{


	public interface IRule
	{
		string Name 		{get;}
		string Ruletype		{get;}
		int Priority (HengeEntity actor, string interaction)
	}
}
