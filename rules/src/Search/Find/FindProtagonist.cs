using System;
using System.Collections.Generic;

using Henge.Data.Entities;
using Henge.Rules.Protagonist.Search;

namespace Henge.Rules.Protagonist.Search.Find
{
	public class FindProtagonist : SearchProtagonist
	{
		protected override double CalculateDifficulty(Item item)
		{
			return (1 - item.Traits["Visibility"].Value);
		}
	}
}