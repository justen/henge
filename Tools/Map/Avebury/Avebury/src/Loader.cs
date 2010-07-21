using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

using Henge.Data.Entities;

namespace Avebury
{
	public class Loader
	{
		public Dictionary<string, ComponentType> Types {get; protected set;}
		public List<Location> Locations {get; protected set;}
		public List<Entity> Maps {get; protected set;}
		
		public Loader (string applicationPath)
		{
			this.Types = new Dictionary<string, ComponentType>();
			this.Locations = new List<Location>();
			this.Maps = new List<Entity>();
			string path			= Path.Combine(applicationPath, "maps");
			DirectoryInfo info	= new DirectoryInfo(path);
			List<XmlNode> maps = new List<XmlNode>();
			foreach(FileInfo file in info.GetFiles("*.xml"))
			{
				XmlDocument map = new XmlDocument();
				map.Load(file.FullName);
				if ((map.DocumentType.Name=="avebury")&&(map.DocumentElement.Name=="world"))
				{
					foreach (XmlNode child in map.DocumentElement)
					{
						if (child.Name=="key") this.ParseKey(child);
						if (child.Name=="map") maps.Add(child);
					}
					foreach (XmlNode node in maps)
					{
						this.ParseMap(node);	
					}
				}
			}
		}
		
		private void ParseKey(XmlNode root)
		{
			foreach(XmlNode child in root)
			{
				if (child.Name=="terrain")
				{
					ComponentType terrain = new ComponentType() { Id = string.Format("terrain.{0}", child.Attributes["id"].Value) };
					foreach (XmlNode subnode in child)
					{
						if (subnode.Name=="appearance")
						{
							
							Dictionary<string, string> parameters = new Dictionary<string, string>();
							parameters.Add("colour", subnode.Attributes["colour"].Value);
							Appearance appearance = new Appearance(){ 	Description = subnode.Attributes["description"].Value, 
																	ShortDescription = subnode.Attributes["short_description"].Value, 
																	Type = subnode.Attributes["type"].Value,
																	Parameters = parameters };
							foreach (XmlNode condition in subnode)
							{
								if (subnode.Name=="condition")
								{
									appearance.Conditions.Add(new Condition(){ 	Invert 	= condition.Attributes["invert"]!=null,
																			   	Maximum = double.Parse(condition.Attributes["maximum"].Value),
																				Minimum = double.Parse(condition.Attributes["minimum"].Value),
																				Trait	= condition.Attributes["trait"].Value});																													
								}
							}
							terrain.Appearance.Add(appearance);
						}
					}
					terrain.BaseTraits = this.GetTraits(child);
					this.Types.Add(child.Attributes["id"].Value, terrain);
				}
			}
			
		}
		
		private void ParseMap(XmlNode root)
		{
			Map map = new Map() { Name = root.Attributes["name"].Value };
			foreach (XmlNode child in root)
			{
				if (child.Name=="location")
				{
					ComponentType type = this.Types[child.Attributes["type"].Value];
					string[]coords = child.Attributes["coordinates"].Value.Split(new Char[] {','});
		
					Location location = new Location( int.Parse(coords[0]),int.Parse(coords[1]), int.Parse(coords[2]) ) 
														{	Detail = child.Attributes["detail"].Value,
															Map = map,
															Name = child.Attributes["name"].Value,
															Type = type };
					foreach (KeyValuePair<string, Trait> trait in type.BaseTraits) 
					{
						location.Traits.Add(trait);	
					}
					Dictionary<string, Trait> traits = this.GetTraits(child);
					foreach(KeyValuePair<string, Trait> trait in traits)
					{
						if (location.Traits.ContainsKey(trait.Key))
						{
							location.Traits[trait.Key] = trait.Value;
						}
						else location.Traits.Add(trait);
					}
					map.Locations.Add(location.Coordinates, location);
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
				if (child.Name=="trait")
				{
					Trait trait = new Trait();
					if (child.Attributes["flavour"]!=null)
					{
						trait.Flavour = child.Attributes["flavour"].Value;
					}
					if (child.Attributes["expiry"]!=null)
					{
						trait.Expiry = DateTime.Parse(child.Attributes["expiry"].Value);
					}
					if (child.Attributes["maximum"]!=null)
					{
						trait.Maximum = double.Parse(child.Attributes["maximum"].Value);
					}
					if (child.Attributes["minimum"]!=null)
					{
						trait.Minimum = double.Parse(child.Attributes["minimum"].Value);
					}
					if (child.Attributes["value"]!=null)
					{
						trait.Value = double.Parse(child.Attributes["value"].Value);
					}
					//TODO: Setting the trait subject on import is not currently supported
					result.Add(child.Attributes["name"].Value, trait); 	
				}
			}
			return result;
		}
	}
}
