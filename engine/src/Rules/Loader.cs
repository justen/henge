using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using Henge.Rules;


namespace Henge.Rules
{
	public class Loader
	{
      	public static Rulebook LoadRules()
        {
			DirectoryInfo info	= new DirectoryInfo("rules/");
			List<IRule> rules	= new List<IRule>();
			IRule rule;
			foreach(FileInfo file in info.GetFiles("*.dll"))
			{
				if (!file.Name.Equals("Henge.Data.dll")) 
				{	                    
					Assembly asm = Assembly.LoadFrom(Path.GetFullPath("rules/" + file.Name));

                    if (asm != null)
					{
						foreach(Type type in asm.GetExportedTypes())
						{
							if (type.GetInterface("Henge.Rules.IRule") != null)
							{
								rule = (IRule)Activator.CreateInstance(type);
								if (rule != null) rules.Add((IRule)rule);
							}
						}
					}
				}
			}
			
            return new Rulebook(rules);
        }
	}
}
