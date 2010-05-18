using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;


namespace Henge.Rules
{
	public class Loader
	{
      	public static Rulebook LoadRules()
        {
			DirectoryInfo info	= new DirectoryInfo("rules/");
			List<IRule> rules	= new List<IRule>();
			
			foreach(FileInfo file in info.GetFiles("*.dll"))
			{
				if (!file.Name.Equals("Henge.Rules.dll")) 
				{	                    
					Assembly asm = Assembly.LoadFrom(Path.GetFullPath("rules/" + file.Name));

                    if (asm != null)
					{
						foreach(Type type in asm.GetExportedTypes())
						{
							if (type.GetInterface("Henge.Rules.IRule") != null)
							{
								IRule rule = (IRule)Activator.CreateInstance(type);
								
								if (rule != null) rules.Add(rule);
							}
						}
					}
				}
			}
			
            return new Rulebook(rules);
        }
	}
}
