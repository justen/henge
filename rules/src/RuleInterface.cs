
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace Henge.Rules
{


	public class RuleInterface
	{
      public static Rulebook LoadRules()
        {
			Rulebook rulebook = new Rulebook();
			DirectoryInfo info = new DirectoryInfo("rules/");

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
								var rule = (IRule)Activator.CreateInstance(type);
								
								if (rule != null)
								{
									rulebook.Add(rule, rule.Interaction, rule.Ruletype);
								}
							}
						}
					}
				}
			}

            return rulebook;
        }
	}
}
