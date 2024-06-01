using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using BepInEx.Logging;
using DunGen;
using MyFirstMod;

namespace MyMod;

struct KeyValue
{
    public string key;
    public string value;

    public KeyValue(string key, string value)
    {
        this.key = key;
        this.value = value;
    }
};

class GlobalConfig
{    
    public static ConfigEntry<string> entriesConfig;
    public static List<KeyValue> entriesList = new List<KeyValue>();
    
    public GlobalConfig(ConfigFile cfg)
    {
        cfg.SaveOnConfigSet = false;

        entriesConfig = cfg.Bind(
            Text.GlobalConfig.configSection,                
            Text.GlobalConfig.KeyConfig,                     
            Text.GlobalConfig.default_value,                    
            Text.GlobalConfig.description
        ); 

        cfg.Save(); 
        cfg.SaveOnConfigSet = true; 
        SetCOnfig();
    }

    public void SetCOnfig()
    {
        List<string> keyValue = entriesConfig.Value.Split(';', System.StringSplitOptions.RemoveEmptyEntries).ToList<string>();
        for (int i = 0; i < keyValue.Count; i++)
        {
            List<string> elem = keyValue[i].Split(':', System.StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            if (elem.Count != 2)
            {
                AliasPlugin.Logger.LogWarning(Text.GlobalConfig.warningMessageBadElement + elem.ToString());
                continue;
            }

            string key = elem[0];
            key = key.TrimStart();
            key = key.TrimEnd();

            string value = elem[1];
            value = value.TrimStart();
            value = value.TrimEnd();

            entriesList.Add(new KeyValue(key, value));
            AliasPlugin.Logger.LogDebug("LAlala" + entriesList.Count);
        }
        
        AliasPlugin.Logger.LogDebug("Content Init end " + entriesList.Count);
    }
}