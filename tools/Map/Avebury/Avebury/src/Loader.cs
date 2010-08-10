using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

using Henge.Data.Entities;

namespace Avebury
{
	public class Loader
	{
		private Dictionary<string, ComponentType> KeyTypes	= new Dictionary<string, ComponentType>();
		private Dictionary<string, ComponentType> GeneralTypes	= new Dictionary<string, ComponentType>();
		private Dictionary<string, ComponentType> AbstractTypes = new Dictionary<string, ComponentType>();
		private List<Location> Locations					= new List<Location>();
		private List<Entity> Maps							= new List<Entity>();
		private Dictionary<string, string> orphanRegions	= new Dictionary<string, string>();
		private Dictionary <string, Henge.Data.Entities.Region> mapRegions 		= new Dictionary<string, Henge.Data.Entities.Region>();
		
		public List<Entity> Data 
		{
			get
			{	
				List<Entity> result = new List<Entity>();
				result.AddRange(this.AbstractTypes.Values.Cast<Entity>());
				result.AddRange(this.Maps.Cast<Entity>());
				result.AddRange(this.Locations.Cast<Entity>());

				return result;
			}
		}
		
		
		public Loader(string applicationPath)
		{
			string path			= Path.Combine(applicationPath, "maps");
			DirectoryInfo info	= new DirectoryInfo(path);
			List<XmlNode> maps	= new List<XmlNode>();
			
			
			foreach(FileInfo file in info.GetFiles("*.xml"))
			{
				XmlDocument map = new XmlDocument();
				map.Load(file.FullName);
				
				if (map.DocumentType.Name=="avebury" && map.DocumentElement.Name=="world")
				{
					foreach (XmlNode child in map.DocumentElement)
					{
						switch (child.Name)
						{
							case "key":		this.ParseKey(child);		break;
							case "general": this.ParseGeneral(child);	break;
							case "map":		maps.Add(child);			break;
						}
					}
					foreach (XmlNode node in maps) this.ParseMap(node);	
				}
			}
		}
		
		
		private void ParseKey(XmlNode root)
		{
			foreach(XmlNode child in root)
			{
				if (child.Name == "type") this.KeyTypes.Add(child.Attributes["id"].Value, this.ParseType(child));
			}
		}
		
		
		private void ParseGeneral(XmlNode root)
		{
			//Gets "abstract" regions - i.e., regions not currently mapped to any locations.
			//These can be used as parents of real regions.
			//Orphaned abstract regions won't end up in the db.
			this.GetRegions(root);
			
			foreach (XmlNode child in root)
			{
				if (child.Name == "type")
				{
					ComponentType type = this.ParseType(child);
					this.AbstractTypes.Add(type.Id, type);
				}
			}
		}
		
		
		private ComponentType ParseType(XmlNode node)
		{
			ComponentType result = new ComponentType() { Id = node.Attributes["id"].Value };
					
			foreach (XmlNode subnode in node)
			{
				if (subnode.Name == "appearance")
				{
					Dictionary<string, string> parameters = new Dictionary<string, string>();
					
					foreach (XmlNode param in subnode)
					{
						if (param.Name == "parameter") parameters.Add(param.Attributes["name"].Value, param.Attributes["value"].Value);
					}
					
					Appearance appearance = new Appearance(){ 	
						Description			= subnode.Attributes["description"].Value, 
						ShortDescription	= subnode.Attributes["short_description"].Value, 
						Type				= subnode.Attributes["type"].Value,
						Priority			= (subnode.Attributes["priority"] != null) ? int.Parse(subnode.Attributes["priority"].Value) : -1,
						Parameters			= parameters 
					};
					
					foreach (XmlNode condition in subnode)
					{
						if (subnode.Name=="condition")
						{
							appearance.Conditions.Add(new Condition(){ 	
								Invert 	= condition.Attributes["invert"] != null,
							   	Maximum = double.Parse(condition.Attributes["maximum"].Value),
								Minimum = double.Parse(condition.Attributes["minimum"].Value),
								Trait	= condition.Attributes["trait"].Value
							});																													
						}
					}
					result.Appearance.Add(appearance);
				}
			}
			result.BaseTraits = this.GetTraits(node);
			
			return result;
		}
		
		
		
		private void ParseMap(XmlNode root)
		{
			Map map 			= new Map() { Name = root.Attributes["name"].Value };
			map.LocationTypes 	= (from t in this.KeyTypes select t.Value).ToList();
			
			foreach (XmlNode child in root)
			{
				if (child.Name=="location")
				{
					ComponentType type	= this.KeyTypes[child.Attributes["type"].Value];
					string []coords 	= child.Attributes["coordinates"].Value.Split(new Char[] {','});
		
					Location location = new Location(int.Parse(coords[0]), int.Parse(coords[1]), int.Parse(coords[2]),  type) {
						Map		= map
					};
				
					location = this.CommonParameters(location, child);
					
					List<string> regions = this.GetRegions(child);
					foreach(string region in regions)
					{
						location.Regions.Add(this.mapRegions[region]);
						this.mapRegions[region].Locations.Add(location);
					}
					this.Locations.Add(location);
				}
			}
			foreach (string orphan in this.orphanRegions.Keys)
			{
				if (this.mapRegions.ContainsKey(orphan) && this.mapRegions.ContainsKey(this.orphanRegions[orphan]))
			   	{
					this.mapRegions[orphan].Parent = this.mapRegions[this.orphanRegions[orphan]];					
				}
			}
			this.Maps.Add(map);
		}
		
		private T CommonParameters<T> (T component, XmlNode definition) where T : Component
		{
			if (definition.Attributes["created"]!=null)
			{
				component.Created = DateTime.Parse(definition.Attributes["created"].Value);
			}
			
			if (definition.Attributes["modified"]!=null)
			{
			 	component.LastModified = DateTime.Parse(definition.Attributes["modified"].Value);
			}
			if (definition.Attributes["name"]!=null)
			{
				component.Name = definition.Attributes["name"].Value;
			}
			if (definition.Attributes["detail"]!=null)
			{
				component.Detail = definition.Attributes["detail"].Value;	
			}
			Dictionary<string, Trait> traits = this.GetTraits(definition);
			foreach(KeyValuePair<string, Trait> trait in traits)
			{
				if (component.Traits.ContainsKey(trait.Key)) 	component.Traits[trait.Key] = trait.Value;
				else 											component.Traits.Add(trait);
			}
			List<Item> contents = this.GetInventory(definition);
			foreach (Item thing in contents)
			{
					thing.Owner = component;
					component.Inventory.Add(thing);
			}
			return component;
		}
		
		private List<Item> GetInventory(XmlNode owner)
		{
			List<Item> result = new List<Item>();
			foreach (XmlNode child in owner)
			{
				if (child.Name == "item")
				{
					string typename = child.Attributes["type"].Value;
					if (this.AbstractTypes.ContainsKey(typename) )
					{
						this.GeneralTypes.Add(typename, this.AbstractTypes[typename]);
						this.AbstractTypes.Remove(typename);
					}
					ComponentType type	= this.GeneralTypes[typename];
					Item item =  new Item(type);
					item = this.CommonParameters(item, child);
					result.Add(item);
				}
			}
			return result;
		}
		
		private List<string> GetRegions(XmlNode location)
		{
			List<string> result = new List<string>();
			foreach	(XmlNode child in location)
			{
				if (child.Name == "region")
				{
					string description = child.Attributes["description"]==null? string.Empty : child.Attributes["description"].Value;
					string name = child.Attributes["name"]==null? string.Empty : child.Attributes["name"].Value;
					string parent = child.Attributes["parent"]==null? string.Empty : child.Attributes["parent"].Value;
					if (name!=null)
					{
						if (!this.mapRegions.ContainsKey(name))
						{
							Henge.Data.Entities.Region region = new Henge.Data.Entities.Region()
							{	
								Description = description,
								Name = name	
							};
							if (this.mapRegions.ContainsKey(parent))
							{
								region.Parent = mapRegions[parent];	
							}
							else this.orphanRegions.Add(name, parent);
							this.mapRegions.Add(region.Name, region);
						}
						result.Add(name);
					}
				}
			}
			return result;
		}
		
		
		private Dictionary<string, Trait> GetTraits(XmlNode source)
		{
			Dictionary<string, Trait> result = new Dictionary<string, Trait>();
			
			foreach (XmlNode child in source)
			{
				if (child.Name == "trait")
				{
					double max = double.MaxValue;
					double min = double.MinValue;
					double val = 0;
					if (child.Attributes["maximum"]!=null)
					{
						max = double.Parse(child.Attributes["maximum"].Value);
					}
					if (child.Attributes["minimum"]!=null)
					{
						min = double.Parse(child.Attributes["minimum"].Value);
					}
					if (child.Attributes["value"]!=null)
					{
						val = double.Parse(child.Attributes["value"].Value);
					}
					Trait trait = new Trait(max, min, val);
					if (child.Attributes["flavour"]!=null)
					{
						trait.Flavour = child.Attributes["flavour"].Value;
					}
					if (child.Attributes["expiry"]!=null)
					{
						trait.Expiry = DateTime.Parse(child.Attributes["expiry"].Value);
					}
					
					//TODO: Setting the trait subject on import is not currently supported
					result.Add(child.Attributes["name"].Value, trait); 	
				}
			}
			
			return result;
		}
	}
}
