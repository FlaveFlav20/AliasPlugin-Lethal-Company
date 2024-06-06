using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using MyFirstMod;
using UnityEngine.UIElements;

namespace MyMod;

public struct KeyValue
{
    public string key;
    public string value;

    public KeyValue(string key, string value)
    {
        this.key = key;
        this.value = value;
    }
};

public struct ListKeysValues
{
    public List<KeyValue> keyValues;
    public string keysStr;
    public string valuesStr;
    public string keysValuesStr;

    public ListKeysValues(List<KeyValue> keyValues, string keysStr, string valuesStr, string keysValuesStr)
    {
        this.keyValues = keyValues;
        this.keysStr = keysStr;
        this.valuesStr = valuesStr;
        this.keysValuesStr = keysValuesStr;
    }

    public void Add(KeyValue keyValue, bool addToConfig = false)
    {
        this.keyValues.Add(keyValue);
        this.keysStr += "; " + keyValue.key + " ";
        this.valuesStr += "; " + keyValue.value + " ";
        this.keysValuesStr += "; " + keyValue.key + " : " + keyValue.value + " "; 
    }

    public string SearchByKey(string key)
    {
        foreach (KeyValue elem in this.keyValues)
        {
            if (elem.key == key)
            {
                return elem.value;
            }
        }
        return "None";
    }

    public bool RemoveByKey(string key)
    {
        int index = 0;
        while (index < this.keyValues.Count)
        {
            if (this.keyValues[index].key == key)
            {
                break;
            }
            index++;
        }
        if (index >= this.keyValues.Count)
        {
            return false;
        }

        keysStr = keysStr.Replace(this.keyValues[index].key, "");
        valuesStr = valuesStr.Replace(this.keyValues[index].value, "");
        keysValuesStr = keysValuesStr.Replace(this.keyValues[index].key + " : " + this.keyValues[index].value, "");
        this.keyValues.RemoveAt(index);
        return true;   
    }
}

public class GlobalConfig
{    
    public static ConfigEntry<string> entriesConfig;
    public static ListKeysValues entriesList = new ListKeysValues([], "", "", "");
    public static ConfigFile config;
    
    public GlobalConfig(ConfigFile cfg)
    {
        config = cfg;
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
            AliasPlugin.Logger.LogDebug("Content config" + entriesList.keyValues.Count);
        }
        
        AliasPlugin.Logger.LogDebug("Content Init end " + entriesList.keyValues.Count);
    }
}