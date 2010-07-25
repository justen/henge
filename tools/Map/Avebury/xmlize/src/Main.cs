using wx;
using System;
using System.IO;
using System.Xml;

using System.Collections.Generic; 

namespace xmlize
{
	class Xmlize
	{
		public static void Main (string[] args)
		{
			Console.WriteLine ("Excavating...");
			Dictionary<string, string> arguments = Xmlize.GetOpt(args);
			if (Xmlize.Validate(arguments))
			{
				Dictionary<string, Image> maps = new Dictionary<string, Image>();
				int i = 1;
				string output = arguments["0"];
				while (arguments.ContainsKey(i.ToString()))
				{
					if (File.Exists(output))
					{
						
						maps.Add(output, new Image(output));
					}
					output = arguments[i.ToString()];
					i++;
				}
				XmlDocument target = null;
				if (!arguments.ContainsKey("-o") && File.Exists(output))
				{
					target = new XmlDocument();
					target.Load(output);
				}
				foreach (string map in maps.Keys)
				{
					Console.WriteLine(string.Format("Converting {0}", map));
					target = Avebury.Converter.Instance.Convert(Xmlize.PrepImage(maps[map]), map, target);
					Console.WriteLine(string.Format("{0} converted", map));
					maps[map].Dispose();
				}
				if (target != null) target.Save(output);
			}
			else Xmlize.Usage();
		}
		
		private static Avebury.Image PrepImage(wx.Image input)
		{
			Avebury.Image result = new Avebury.Image(input.Width, input.Height);
			for (int x = 0; x<input.Width; x++)
			{
				for (int y = 0; y<input.Height; y++)
				{
					result.SetRed(x, y, input.GetRed(x, y));
					result.SetGreen(x, y, input.GetGreen(x, y));
					result.SetBlue(x, y, input.GetBlue(x, y));
					result.SetAlpha(x, y, input.GetAlpha(x, y));
				}
			}
			return result;
		}
		
		private static bool Validate(Dictionary<string, string> arguments)
		{
			bool result = true;
			int options = 0;
			foreach (string key in arguments.Keys)
			{
				if ( (key.IndexOf("-") == 0) )
				{
					if (key=="-o") 
					{
						options++;
					} else result = false;
				}				
			}
			if (arguments.Count < options+2)  result = false;
			return result;
		}
		
		private static void Usage()
		{
			Console.WriteLine("xmlize: Command-line wrapper for Avebury, the Henge map generator");
			Console.WriteLine("Usage: xmlize -[o] input(s) output");
			Console.WriteLine(" -o: Overwrite any existing output files");
			Console.WriteLine("input: Map image(s)");
			Console.WriteLine("output: XML map file to create/modify");
		}
		
		private static Dictionary<string, string> GetOpt(string[] args)
		{
			Dictionary <string, string> result = new Dictionary<string, string>();
			int i = 0;
			int others = 0;
			while(i < args.Length)
			{
				if (args[i].IndexOf('-')==0)
				{
					//switch or argument?
					if ((i+2<args.Length) && (args[i+1].IndexOf('-')==0))
					{
						//argument
						result.Add(args[i], args[i+1]);
						i+=2;
					}
					else
					{
						//switch	
						result.Add(args[i], string.Empty);
						i++;
					}
				}
				else 
				{
					result.Add(others.ToString(), args[i]);
					i++; 
					others++;
				}
			}
			return result;
		}
	}
}

