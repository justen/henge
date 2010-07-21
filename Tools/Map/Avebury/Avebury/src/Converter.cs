using System;
using System.Xml;
using System.Collections.Generic;

namespace Avebury
{
	public sealed class Converter
	{
		private static readonly Converter instance = new Converter();
		public static Converter Instance { get { return instance; }}
		static Converter() {}
		private Converter (){}
	
		public XmlDocument Create(Image map, string name)
		{
			if (map==null) throw new Exception("Map image could not be opened");
			//create a new world document
			XmlDocument result = new XmlDocument();
			result.AppendChild(result.CreateXmlDeclaration("1.0", "UTF-8", "yes"));
			result.AppendChild(result.CreateDocumentType("avebury", null, null, null));

			//set up the world, key and first map elements
			XmlNode root = result.CreateElement("world");
			result.AppendChild(root);
			XmlNode key = result.CreateElement("key");
			root.AppendChild(key);
			XmlNode mapNode = result.CreateElement("map");
			mapNode.Attributes.Append(result.CreateAttribute("name"));
			mapNode.Attributes["name"].Value = name;
			root.AppendChild(mapNode);
			
			//Parse the bitmap to populate the map and key elements 
			this.GenerateMap(mapNode, key, map);
			
			//All done, return the document
			return result;
		}
		
		public XmlDocument Convert(Image map, string name, XmlDocument world)
		{
			bool worldFound = false;
			if (map==null) throw new Exception("Map image could not be opened");
			if (world==null) 
			{
				world = new XmlDocument();
				world = this.Create(map, name);
			}
			else
			{
				if (world.DocumentType.Name=="avebury")
				{
					foreach (XmlNode root in world.ChildNodes)
					{
						if (!worldFound &&(root.Name == "world"))
						{
							worldFound = true;
							//Find the key and specified map nodes (or create them if not present)
							XmlNode key = null;
							XmlNode mapNode = null;
							foreach (XmlNode child in root)
							{
								if (child.Name=="key") key = child;
								if ((child.Name=="map") && (child.Attributes["name"].Value==name)) mapNode = child;
							}
							
							if (key==null)
							{
								key = world.CreateElement("key");
								world.AppendChild(key);
							}
							if (mapNode!=null) mapNode.RemoveAll();
							else 
							{
								mapNode = world.CreateElement("map");
								world.AppendChild(mapNode);
							}
							mapNode.Attributes.Append(world.CreateAttribute("name"));
							mapNode.Attributes["name"].Value = name;
						
							
							//parse bitmap, and add it to the world;
							this.GenerateMap(mapNode, key, map);
							
						}  
					}
					if (!worldFound) throw new Exception("Avebury: Malformed document - missing world node");
				}
				else throw new Exception("Avebury: Invalid XML destination document");
			}
			return world;
		}
		
		private Dictionary<string, XmlNode> Collate (XmlNode key, Mapper mapper)
		{
			Dictionary<string, XmlNode> result = new Dictionary<string, XmlNode>();
			foreach ( XmlNode child in key)
			{
				if (key.Name == "terrain")
				{
					foreach (XmlNode appearance in child)
					{
						bool def = true;
						foreach (XmlNode properties in appearance)
						{
							if (properties.Name == "condition") def = false;
						}
						if (def) 
						{
							result.Add(appearance.Attributes["colour"].Value, child);
							mapper.AddMapping(child.Attributes["id"].Value, appearance.Attributes["colour"].Value);
							break;
						}
					}
				}
			}
			return result;
		}
		
		private void GenerateMap (XmlNode map, XmlNode key, Image source)
		{
			XmlDocument doc = map.OwnerDocument;
			Mapper mapper = new Mapper();
			Dictionary<string, XmlNode> keys = this.Collate(key, mapper);
			
			for (int x = 0; x < source.Width; x++)
			{
				for (int y = 0; y<source.Height; y++)
				{			
					int elevation = source.GetAlpha(x, y);
					int red = source.GetRed(x, y);
					int green = source.GetGreen(x, y);
					int blue = source.GetBlue(x, y);
					string colourString = string.Format("#{0}{1}{2}", red.ToString("x").PadLeft(2,'0'), 
					                                    green.ToString("x").PadLeft(2,'0'), 
					                                    blue.ToString("x").PadLeft(2,'0'));
					if (!keys.ContainsKey(colourString))
					{
						XmlNode newTerrain = key.OwnerDocument.CreateElement("terrain");
						newTerrain.Attributes.Append(doc.CreateAttribute("id"));
						newTerrain.Attributes["id"].Value = mapper.Id(colourString);
						XmlNode appearance = key.OwnerDocument.CreateElement("appearance");
						appearance.Attributes.Append(doc.CreateAttribute("type"));
						appearance.Attributes.Append(doc.CreateAttribute("description"));
						appearance.Attributes.Append(doc.CreateAttribute("short_description"));
						appearance.Attributes.Append(doc.CreateAttribute("colour"));
						
						appearance.Attributes["type"].Value = "firmament";
						appearance.Attributes["description"].Value = "nothingness";
						appearance.Attributes["short_description"].Value = "nothingness";
						appearance.Attributes["colour"].Value = colourString;
						
						//If we were to handle conditional appearances here, it'd just be a case of adding multiple 
						//appearance elements, each containing at least one "condition" element specifying the conditions
						//that must be met to "see" that appearance. we're assuming that any conditional appearances
						//apply *on top* of the default appearance.
						
						newTerrain.AppendChild(appearance);
						keys.Add(colourString, newTerrain);
					}
					XmlNode location = key.OwnerDocument.CreateElement("location");
					location.Attributes.Append(doc.CreateAttribute("type"));
					location.Attributes.Append(doc.CreateAttribute("coordinates"));
					location.Attributes.Append(doc.CreateAttribute("name"));
					location.Attributes.Append(doc.CreateAttribute("detail"));
					location.Attributes["type"].Value = mapper.Id(colourString);
					location.Attributes["coordinates"].Value = string.Format("{0},{1},{2}", x, y, elevation);
					map.AppendChild(location);
				}
			}
			key.RemoveAll();
			foreach ( XmlNode node in keys.Values )
			{
				key.AppendChild(node);	
			}
		}
		 
	}
}

