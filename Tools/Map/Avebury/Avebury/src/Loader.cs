using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;

using Henge.Data.Entities;

namespace Avebury
{
	public class Loader
	{
		private Dictionary<string, ComponentType> Types;
		private List<Location> Locations;
		private List<Entity> Maps ;
		private List<Entity> UnlinkedEntities = new List<Entity>();
		public List<Entity> Data 
		{
			get
			{	
				this.Maps.AddRange(UnlinkedEntities);
				this.UnlinkedEntities = new List<Entity>();
				return this.Maps;
			}
		}
		
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
				if (child.Name=="type")
				{
					ComponentType type = new ComponentType() { Id = child.Attributes["id"].Value };
					foreach (XmlNode subnode in child)
					{
						if (subnode.Name=="appearance")
						{
							
							Dictionary<string, string> parameters = new Dictionary<string, string>();
							foreach (XmlNode param in subnode)
							{
								if (param.Name=="parameter") parameters.Add(param.Attributes["name"].Value, param.Attributes["value"].Value);
							}
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
							type.Appearance.Add(appearance);
						}
					}
					type.BaseTraits = this.GetTraits(child);
					this.Types.Add(child.Attributes["id"].Value, type);
					this.UnlinkedEntities.Add(type);
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
					if (this.UnlinkedEntities.Contains(type)) this.UnlinkedEntities.Remove(type);
					string[]coords = child.Attributes["coordinates"].Value.Split(new Char[] {','});
		
					Location location = new Location( int.Parse(coords[0]),int.Parse(coords[1]), int.Parse(coords[2]),  type  ) 
														{	Detail = child.Attributes["detail"].Value,
															Map = map,
															Name = child.Attributes["name"].Value};

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
