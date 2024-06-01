using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using BepInEx.Logging;
using MyFirstMod;

namespace MyMod;

class GlobalConfig
{
    // We define our config variables in a public scope
    public static ConfigEntry<string> entriesConfig;
    public static List<List<string>> entriesList = new List<List<string>>();

    private string warningMessageBadElement = "[" + MyPluginInfo.PLUGIN_NAME + "] : An element cannot be initialized. Continue to the next one. Element:";

    string default_value = "vm : VIEW MONITOR ; sw : SWITCH ; s : SWITCH ; p : PING ; t : TRANSMIT ; sc : SCAN ; st : STORE ; m : MOONS ; tcb : THE COMPANY BUILDING ; exp : EXPERIMENTATION ; ass : ASSURANCE ; v : VOW ; ma : MARCH ; off : OFFENSE ; ad : ADAMANCE ; re : REND ; di : DINE ; ti : TITAN";

    public GlobalConfig(ConfigFile cfg)
    {
        cfg.SaveOnConfigSet = false;

        entriesConfig = cfg.Bind(
            "List",                          // Config section
            "GreetingText",                     // Key of this config
            default_value,                    // Default value
            "Greeting text upon game launch"    // Description
        ); 

        cfg.Save(); 
        cfg.SaveOnConfigSet = true; 
        SetCOnfig();
    }

    public void SetCOnfig()
    {
        List<string> keyValue = entriesConfig.Value.Split(';', System.StringSplitOptions.RemoveEmptyEntries).ToList<string>();
        AliasPlugin.Logger.LogDebug("Content Init start " + keyValue.Count);
        for (int i = 0; i < keyValue.Count; i++)
        {
            List<string> elem = keyValue[i].Split(':', System.StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            AliasPlugin.Logger.LogDebug("Content Init inter " + elem.Count);
            if (elem.Count != 2)
            {
                AliasPlugin.Logger.LogWarning(warningMessageBadElement + elem.ToString());
                continue;
            }

            string key = elem[0];
            key = key.TrimStart();
            key = key.TrimEnd();

            string value = elem[1];
            value = value.TrimStart();
            value = value.TrimEnd();

            entriesList.Add(new List<string>([new string(key), new string(value)]));
            AliasPlugin.Logger.LogDebug("LAlala" + entriesList.Count);
        }
        
        AliasPlugin.Logger.LogDebug("Content Init end " + entriesList.Count);
    }
}