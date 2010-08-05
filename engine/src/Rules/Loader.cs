using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Henge.Rules;


namespace Henge.Rules
{
	public class Loader
	{
      	public static Rulebook LoadRules(string applicationPath)
        {			
			string path			= Path.Combine(applicationPath, "rules");
			DirectoryInfo info	= new DirectoryInfo(path);
			List<IRule> rules	= new List<IRule>();
			Type interaction	= null;

			foreach(FileInfo file in info.GetFiles("*.dll"))
			{
				if (!file.Name.Equals("Henge.Data.dll")) 
				{	                    
					Assembly asm = Assembly.LoadFrom(Path.Combine(path, file.Name));

                    if (asm != null)
					{
						foreach (Type type in asm.GetExportedTypes())
						{
							Console.WriteLine(type.ToString());
							if (type.GetInterface("Henge.Rules.IRule") != null && !type.IsAbstract)
							{
								IRule rule = (IRule)Activator.CreateInstance(type);
								if (rule != null) rules.Add((IRule)rule);
							}
							
							if (type.GetInterface("Henge.Rules.IInteraction") != null)
							{
								interaction = type;
							}
						}
					}
				}
			}
			
            return new Rulebook(rules, interaction);
        }
	}
}
