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
		private List<ComponentType> GeneralTypes			= new List<ComponentType>();
		private List<Location> Locations					= new List<Location>();
		private List<Entity> Maps							= new List<Entity>();
		
		public List<Entity> Data 
		{
			get
			{	
				List<Entity> result = new List<Entity>();
				result.AddRange(this.GeneralTypes.Cast<Entity>());
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
			foreach (XmlNode child in root)
			{
				if (child.Name == "type") this.GeneralTypes.Add(this.ParseType(child));
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
						Name 	= child.Attributes["name"].Value,
						Detail	= child.Attributes["detail"].Value,
						Map		= map
					};

					Dictionary<string, Trait> traits = this.GetTraits(child);
					foreach(KeyValuePair<string, Trait> trait in traits)
					{
						if (location.Traits.ContainsKey(trait.Key)) location.Traits[trait.Key] = trait.Value;
						else 										location.Traits.Add(trait);
					}

					this.Locations.Add(location);
				}
			}
			this.Maps.Add(map);
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
