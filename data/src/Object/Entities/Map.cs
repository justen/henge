using System;
using System.Collections.Generic;


namespace Henge.Data.Entities
{
	public class Coordinates : ObjectEntity
	{
		public virtual int X { get; set; }
		public virtual int Y { get; set; }
		public virtual int Z { get; set; }
		
		
		public Coordinates() {}
		
		
		public Coordinates(Coordinates c)
		{
			this.X = c.X;
			this.Y = c.Y;
			this.Z = c.Z;
		}
		
		
		public override bool Equals(object obj)
		{
			return (obj is Coordinates) ? ((Coordinates)obj).X == X && ((Coordinates)obj).Y == Y && ((Coordinates)obj).Z == Z : false;
		}
		
		
		public override int GetHashCode ()
		{
			return string.Format("{0};{1};{2}", X, Y, Z).GetHashCode();
		}
	}
	
	
	public class Map : ObjectEntity
	{
		public virtual IDictionary<Coordinates, Location> Locations	{ get; set; }
	    public virtual string Name									{ get; set; }
		
		
		public Map()
		{
			this.Locations = new Dictionary<Coordinates, Location>();
		}
		
		
		public Location GetLocation(int x, int y, int z)
		{
			return this.GetLocation(new Coordinates { X = x, Y = y, Z = z });
		}
		
		public Location GetLocation(Coordinates coordinates)
		{
			Location result;
			return this.Locations.TryGetValue(coordinates, out result) ? result : null;
		}
	}
}