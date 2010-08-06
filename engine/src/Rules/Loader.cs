using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

using Henge.Data;
using Henge.Rules;


namespace Henge.Rules
{
	public class Loader
	{
      	public static Rulebook LoadRules(DataProvider db, string applicationPath)
        {			
			string path					= Path.Combine(applicationPath, "rules");
			DirectoryInfo info			= new DirectoryInfo(path);
			List<IRule> rules			= new List<IRule>();
			List<IModifier> modifiers	= new List<IModifier>();
			Type interaction			= null;

			foreach(FileInfo file in info.GetFiles("*.dll"))
			{
				if (!file.Name.Equals("Henge.Data.dll")) 
				{	                    
					Assembly asm = Assembly.LoadFrom(Path.Combine(path, file.Name));

                    if (asm != null)
					{
						foreach (Type type in asm.GetExportedTypes())
						{
							//Console.WriteLine(type.ToString());
							
							if (type.GetInterface("Henge.Rules.IRule") != null && !type.IsAbstract)
							{
								IRule rule = Activator.CreateInstance(type) as IRule;
								if (rule != null) rules.Add(rule);
							}
							
							if (type.GetInterface("Henge.Rules.IModifier") != null && !type.IsAbstract)
							{
								IModifier modifier = Activator.CreateInstance(type) as IModifier;
								if (modifier != null) modifiers.Add(modifier);
							}
							
							if (type.GetInterface("Henge.Rules.IInteraction") != null)
							{
								interaction = type;
							}
						}
					}
				}
			}
			
			modifiers.ForEach(m => m.Initialise(db));
			
            return new Rulebook(rules, modifiers, interaction);
        }
	}
}
