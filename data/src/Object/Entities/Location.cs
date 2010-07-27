using System;
using System.Collections.Generic;

using Coincidental;


namespace Henge.Data.Entities
{
	public class Location : Component
	{
	    public virtual IList<Avatar> Inhabitants		{ get; set; }
		public virtual IList<Edifice> Structures		{ get; set; }
		public virtual IList<Npc> Fauna					{ get; set; }
		//public virtual Coordinates Coordinates			{ get; set; }
		[Indexed]
		public virtual int X { get; set; }
		[Indexed]
		public virtual int Y { get; set; }
		public virtual int Z { get; set; }

	    //List of the Regions this Location is in
	    public virtual IList<Region> Regions			{ get; set; }
	    //the Map this location exists in
	    public virtual Map Map							{ get; set; }
		
		public virtual IList<Location> Visible			{ get; set; }
		public virtual bool InvertVisibility			{ get; set; }
		
		
		public Location(int x, int y, int z, ComponentType type) : base(type)
		{
			//this.Coordinates		= new Coordinates { X = x, Y = y, Z = z };
			this.X					= x;
			this.Y					= y;
			this.Z					= z;
			this.Inhabitants 		= new List<Avatar>();
			this.Structures			= new List<Edifice>();
			this.Fauna				= new List<Npc>();
			this.Regions 			= new List<Region>();
			this.Visible			= new List<Location>();
			this.InvertVisibility	= true;
		}
		
		
		public Location()
		{
			this.Inhabitants 		= new List<Avatar>();
			this.Structures			= new List<Edifice>();
			this.Fauna				= new List<Npc>();
			this.Regions 			= new List<Region>();
			this.Visible			= new List<Location>();
			this.InvertVisibility	= true;
		}
		
		
		public bool CanSee(Location target)
		{
			return this.Visible.Contains(target) ^ this.InvertVisibility;	
		}
	}
}