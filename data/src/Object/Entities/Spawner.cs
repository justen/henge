using System;
using System.Collections.Generic;

namespace Henge.Data.Entities
{
	public class Spawner : ObjectEntity
	{
		//ComponentType to spawn the object with
		public virtual ComponentType ComponentType 	{ get; set; }
		//Type of the object to be spawned
		public virtual Type		Class			{ get; set; }
		//Conditions to be met in order for the object to be spawned using this Spawner
		public virtual Dictionary<Condition, double> Conditions	{ get; set; }
		
		//Elevation range over which this spawner applies
		public virtual int 	  MinZ	{ get; set; }
		public virtual int 	  MaxZ	{ get; set; }
		
		//Rate at which this spawn works (range of 0 to 1, where 0 is "never spawns" and 1 is "spawns every tick")
		public virtual double SpawnRate		{ get; set; }
	}
}

