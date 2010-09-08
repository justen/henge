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
		private Dictionary<Map, Dictionary<string, Location>> Locations					= new Dictionary<Map, Dictionary<string, Location>>();
		private Dictionary<string, Map> Maps							= new Dictionary<string, Map>();
		private Dictionary<string, string> orphanRegions	= new Dictionary<string, string>();
		private Dictionary <string, Henge.Data.Entities.Region> mapRegions 		= new Dictionary<string, Henge.Data.Entities.Region>();
		private Dictionary<string[], Edifice> pendingPortals = new Dictionary<string[], Edifice>();
		private Dictionary<Npc, string> orphanedNPCs = new Dictionary<Npc, string>();
		private Dictionary<string, Avatar> avatars = new Dictionary<string, Avatar>();
		private Dictionary<string, User> users = new Dictionary<string, User>();
		private Dictionary<string, Component> traitRefs = new Dictionary<string, Component>();
		private Dictionary<Trait, string> traits = new Dictionary<Trait, string>();
		
		public List<Entity> Data 
		{
			get
			{	
				
	
				if (this.pendingPortals.Count>0)
				{
					string exception = string.Format("Avebury - {0} broken portals: \n", this.pendingPortals.Count);
					foreach (Edifice edifice in this.pendingPortals.Values)
					{
						exception += string.Format("{0} at {1}, {2} in {3} is invalid\n", edifice.Name, edifice.Location.X, edifice.Location.Y, edifice.Location.Map.Name);
					}
					throw new Exception (exception);
				}
				List<Entity> result = new List<Entity>();
				result.AddRange(this.AbstractTypes.Values.Cast<Entity>());
				result.AddRange(this.users.Values.Cast<Entity>());
				result.AddRange(this.Maps.Values.Cast<Entity>());
				foreach (Dictionary<string, Location> locationList in this.Locations.Values)
				{
					result.AddRange(locationList.Values.Cast<Entity>());
				}
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
							case "users": this.ParseUsers(child);		break;
							case "map":		maps.Add(child);			break;
						}
					}
					foreach (XmlNode node in maps) this.ParseMap(node);	
				}
			}
			List<string[]> connected = new List<string[]>();
			foreach (KeyValuePair<string[], Edifice>edifice in this.pendingPortals)
			{
				string mapName = edifice.Key[0];
				string locationName = edifice.Key[1];
				if (this.Maps.ContainsKey(mapName))
				{
	
					Map map = this.Maps[mapName];
					
					if (this.Locations[map].ContainsKey(locationName))
					{
						edifice.Value.Portal = this.Locations[map][locationName];	
						connected.Add(edifice.Key);
					}
				}
			}
			foreach(string[] key in connected)
			{
				this.pendingPortals.Remove(key);	
			}
			
			foreach(KeyValuePair<Npc, string> pair in this.orphanedNPCs)
			{
				if (this.avatars.ContainsKey(pair.Value))
				{
					this.avatars[pair.Value].Pets.Add(pair.Key);
					pair.Key.Master = this.avatars[pair.Value];
				}
			}
		}
		

		
		private void ParseUsers(XmlNode users)
		{
			foreach(XmlNode user in users)
			{
				if (user.Name=="user")
				{
					string name = user.Attributes["name"].Value;
					User instance =  new User()
					{ 
						Name = name,
						Clan = (user.Attributes["clan"]!=null)? user.Attributes["clan"].Value : name,
						Email = (user.Attributes["email"]!=null)? user.Attributes["email"].Value : string.Empty,
						Enabled = user.Attributes["enabled"]!=null,
						Password = user.Attributes["password"].Value
					};
					foreach (XmlNode child in user)
					{
						if (child.Name=="role")
						{
							instance.Roles.Add(child.Attributes["name"].Value);
						}
						if (child.Name=="avatar")
						{
							Avatar avatar = this.ParseAvatar(child);	
							instance.Avatars.Add(avatar);
							this.avatars.Add(avatar.Name, avatar);
						}
					}
					this.users.Add(name, instance);
						
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
			result.BaseTick = this.GetTicks(node);
			foreach (XmlNode child in node)
			{
				if (child.Name=="skill") this.BaseSkills(child, result);
			}
			
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
					location = this.PopulateInhabitants(location, child);
					foreach (XmlNode trace in child)
					{
						if (trace.Name=="trace")
						{
							Dictionary<string, Trait> traits = this.GetTraits(trace);	
							if (trace.Attributes["direction"].Value=="in")
							{
								((List<Trait>)location.TracesIn).AddRange(traits.Values);
							}
							if (trace.Attributes["direction"].Value=="out")
							{
								((List<Trait>)location.TracesOut).AddRange(traits.Values);
							}
						}
					}
					foreach (Avatar character in location.Inhabitants)
					{
						character.Location = location;
						
					}
					foreach (Npc animal in location.Fauna)
					{
						animal.Location = location;
					}
					location = this.PopulateEdifices(location, child);
					List<string> regions = this.GetRegions(child);
					foreach(string region in regions)
					{
						location.Regions.Add(this.mapRegions[region]);
						this.mapRegions[region].Locations.Add(location);
					}
					if (!this.Locations.ContainsKey(location.Map)) this.Locations.Add(location.Map, new Dictionary<string, Location>());
					this.Locations[location.Map].Add(string.Format("{0},{1}", coords[0], coords[1]), location);
				}
			}
			foreach (string orphan in this.orphanRegions.Keys)
			{
				if (this.mapRegions.ContainsKey(orphan) && this.mapRegions.ContainsKey(this.orphanRegions[orphan]))
			   	{
					this.mapRegions[orphan].Parent = this.mapRegions[this.orphanRegions[orphan]];					
				}
			}
			foreach(KeyValuePair<Trait, string> trait in this.traits)
			{
				if (this.traitRefs.ContainsKey(trait.Value))
				{
					trait.Key.Subject = this.traitRefs[trait.Value];	
				}
			}
			this.Maps.Add(map.Name, map);
		}
		
		private Avatar ParseAvatar(XmlNode definition)
		{
			Avatar result = new Avatar(this.Type(definition));
			this.ParseActor(result, definition);
			foreach (XmlNode child in definition)
			{
				if (child.Name=="ancestor")
				{
					result.Ancestors.Add(child.Attributes["details"].Value);
				}
			}
			return result;
		}
		
		private T ParseActor<T> (T actor, XmlNode definition) where T : Actor
		{
			actor =  this.CommonParameters(actor, definition);
			actor = this.ParseSkills(actor, definition);	
			
			return actor;
		}
		
		private Npc ParseNPC(XmlNode definition)
		{
			Npc result = new Npc(this.Type(definition));
			result = this.ParseActor(result, definition);
			if (definition.Attributes["master"]!=null)
			{
				this.orphanedNPCs.Add(result, definition.Attributes["master"].Value);	
			}
			return result;
		}
		
		private T ParseSkills<T> (T actor, XmlNode definition) where T : Actor
		{
			T result = actor;
			
			foreach (XmlNode child in definition)
			{
				if (child.Name=="skill")
				{
					result = this.AddSkill(result, child);
				}
			}
			
			return result;
		}
		
		private void BaseSkills(XmlNode definition, ComponentType owner)
		{
			Skill skill = new Skill()
			{
				Value = (definition.Attributes["value"]==null)? 0.0 : double.Parse(definition.Attributes["value"].Value)
			};
			foreach (XmlNode child in definition)
			{
				if (child.Name=="skill")
				{
					skill.Children.Add(child.Attributes["name"].Value);
					this.BaseSkills(child, owner);
				}
			}
			owner.BaseSkills.Add(definition.Attributes["name"].Value, skill);
		}
					
		private T AddSkill<T> (T owner, XmlNode definition) where T : Actor
		{
			Skill skill = new Skill()
			{
				Value = (definition.Attributes["value"]==null)? 0.0 : double.Parse(definition.Attributes["value"].Value)
			};
			foreach (XmlNode child in definition)
			{
				if (child.Name=="skill")
				{
					skill.Children.Add(child.Attributes["name"].Value);
					owner = this.AddSkill(owner, child);
				}
			}
			if (owner.Skills.ContainsKey(definition.Attributes["name"].Value))
			{
				owner.Skills[definition.Attributes["name"].Value] = skill;
			}
			else 
			{
				owner.Skills.Add(definition.Attributes["name"].Value, skill);
			}
			return 	owner;
		}
		
		private T PopulateInhabitants<T> (T component, XmlNode definition) where T : Component, IInhabitable
		{
			T result = component;
			foreach (XmlNode child in definition)
			{
				if (child.Name=="avatar")
				{
					result.Inhabitants.Add(this.avatars[child.Attributes["name"].Value]);
					this.avatars.Remove(child.Attributes["name"].Value);
				}
				
				if (child.Name=="npc")
				{
					result.Fauna.Add(this.ParseNPC(child));
				}
			}
			return result;
		}
		
		private Location PopulateEdifices(Location location, XmlNode definition)
		{
			foreach (XmlNode child in definition)
			{
				if (child.Name=="edifice")
				{
					Edifice edifice = new Edifice(this.Type(child));
					edifice = this.CommonParameters(edifice, child);
					foreach (XmlNode portal in child)
					{
						if (portal.Name=="portal")	
						{
						
							this.pendingPortals.Add(new string[2]{portal.Attributes["map"].Value, portal.Attributes["coordinates"].Value}, edifice);
							break;
						}
					}
					edifice.Location = location;
					location.Structures.Add(edifice);
					
					edifice = this.PopulateInhabitants(edifice, child);
					foreach (Avatar character in edifice.Inhabitants)
					{
						character.Location = edifice.Location;
					}
					foreach (Npc animal in location.Fauna)
					{
						animal.Location = edifice.Location;
					}
				}
			}
			return location;
		}
		
		private T CommonParameters<T> (T component, XmlNode definition) where T : Component
		{
			if (definition.Attributes["reference"]!=null)
			{
				//this is component which is referenced by a Trait
				this.traitRefs.Add(definition.Attributes["reference"].Value, component);
			}
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
			List<Tick> ticks = this.GetTicks(definition);
			foreach (Tick tick in ticks)
			{
				component.Ticks.Add(tick);	
			}
			component.UpdateNextTick();
			return component;
		}
		
		private List<Tick> GetTicks(XmlNode node)
		{
			List<Tick> result = new List<Tick>();
			foreach (XmlNode child in node)
			{
				if (child.Name=="tick")
				{
					Tick tick = new Tick()
					{
						Name = child.Attributes["name"].Value,
						Scheduled = DateTime.Now +  TimeSpan.Parse(child.Attributes["offset"].Value),
						Period = int.Parse(child.Attributes["period"].Value)
					};
					result.Add(tick);
				}
			}
			return result;
		}
		
		private ComponentType Type (XmlNode node)
		{
			ComponentType result = null;
			string typename = node.Attributes["type"].Value;
			if (this.AbstractTypes.ContainsKey(typename) )
			{
				this.GeneralTypes.Add(typename, this.AbstractTypes[typename]);
				this.AbstractTypes.Remove(typename);
			}
			if (this.GeneralTypes.ContainsKey(typename))
			{
				result = this.GeneralTypes[typename];
			}
			else
			{
				throw new Exception(string.Format("Avebury loader exception: Undefined type {0} in node {1}", typename, node.ToString()));
			}
			return result;
		}
		
		private List<Item> GetInventory(XmlNode owner)
		{
			List<Item> result = new List<Item>();
			foreach (XmlNode child in owner)
			{
				if (child.Name == "item")
				{
					Item item =  new Item(this.Type(child));
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
					
					if (child.Attributes["subject"]!=null)
					{
						this.traits.Add(trait, child.Attributes["subject"].Value);	
					}
					result.Add(child.Attributes["name"].Value, trait); 	
				}
			}
			
			return result;
		}
	}
}
