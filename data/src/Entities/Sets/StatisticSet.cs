using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{
	public class StatisticSet : Entity
	{
		public virtual IList<Statistic> Statistics { get; set; }
		
		
		public StatisticSet()
		{
			this.Statistics = new List<Statistic>();
		}
	}
}