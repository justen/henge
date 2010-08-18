using System;
using System.Collections.Generic;

using Coincidental;


namespace Henge.Data.Entities
{
	public class Location : Component, IInhabitable
	{
		[Indexed]
		public virtual ulong Index					{ get; set; }
		public virtual int X 						{ get; set; }
		public virtual int Y 						{ get; set; }
		public virtual int Z 						{ get; set; }
	    public virtual IList<Avatar> Inhabitants	{ get; set; }
		public virtual IList<Edifice> Structures	{ get; set; }
		public virtual IList<Npc> Fauna				{ get; set; }
	    public virtual IList<Region> Regions		{ get; set; }	// List of the Regions this Location is in
	    public virtual Map Map						{ get; set; }	// The Map this location exists in
		public virtual IList<Location> Visible		{ get; set; }
		public virtual bool InvertVisibility		{ get; set; }
		public virtual IList<Trait> TracesIn		{ get; set; }
		public virtual IList<Trait> TracesOut		{ get; set; }
		
		public Location(int x, int y, int z, ComponentType type) : base(type)
		{
			this.X					= x;
			this.Y					= y;
			this.Z					= z;
			this.Index				= ((ulong)x << 32) | (ulong)y;
			this.Inhabitants 		= new List<Avatar>();
			this.Structures			= new List<Edifice>();
			this.Fauna				= new List<Npc>();
			this.Regions 			= new List<Region>();
			this.Visible			= new List<Location>();
			this.TracesIn			= new List<Trait>();
			this.TracesOut			= new List<Trait>();
			this.InvertVisibility	= true;
		}
		
		
		public Location()
		{
			this.Inhabitants 		= new List<Avatar>();
			this.Structures			= new List<Edifice>();
			this.Fauna				= new List<Npc>();
			this.Regions 			= new List<Region>();
			this.Visible			= new List<Location>();
			this.TracesIn			= new List<Trait>();
			this.TracesOut			= new List<Trait>();
			this.InvertVisibility	= true;
		}
		
		
		public bool CanSee(Location target)
		{
			return this.Visible.Contains(target) ^ this.InvertVisibility;	
		}
	}
}