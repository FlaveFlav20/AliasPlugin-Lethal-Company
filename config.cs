using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using Dissonance.Config;
using LethalConfig;
using LethalConfig.ConfigItems;
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
    public static List<KeyValue> keyValues = [];
    public static List<ConfigEntry<string>> listConfigAliasKey = [];
    public static List<ConfigEntry<string>> listConfigAliasValue = [];
    public string keysStr;
    public string valuesStr;
    public string keysValuesStr;

    public ListKeysValues(string keysStr, string valuesStr, string keysValuesStr)
    {
        this.keysStr = keysStr;
        this.valuesStr = valuesStr;
        this.keysValuesStr = keysValuesStr;
    }

    public int Append(KeyValue keyValue, bool addToConfig = false)
    {

        string key = keyValue.key.TrimEnd();
        key = keyValue.key.TrimStart();

        if (this.SearchByKey(key) != "None")
        {
            return -1;
        }
        keyValue.key = key;
        ListKeysValues.keyValues.Add(keyValue);
        this.keysStr += "; " + keyValue.key + " ";
        this.valuesStr += "; " + keyValue.value + " ";
        this.keysValuesStr += "; " + keyValue.key + " : " + keyValue.value + " "; 

        int index = ListKeysValues.keyValues.Count() - 1;
      
        ListKeysValues.listConfigAliasKey.Add(GlobalConfig.config.Bind(Text.LethalConfig.alias + index, Text.LethalConfig.key, "", ""));
        ListKeysValues.listConfigAliasKey[index].Value = keyValue.key;
        ListKeysValues.listConfigAliasKey[index].BoxedValue = keyValue.key;

        ListKeysValues.listConfigAliasValue.Add(GlobalConfig.config.Bind(Text.LethalConfig.alias + index, Text.LethalConfig.value, "", ""));
        ListKeysValues.listConfigAliasValue[index].Value = keyValue.value;
        ListKeysValues.listConfigAliasValue[index].BoxedValue = keyValue.value;

        LethalConfigManager.AddConfigItem(new GenericButtonConfigItem(Text.LethalConfig.alias + index, Text.LethalConfig.remove, Text.LethalConfig.removeAnAlias, Text.LethalConfig.button, () => 
        {
            if (index >= ListKeysValues.listConfigAliasKey.Count())
            {
                return;
            }
            GlobalConfig.entriesList.RemoveByKey(ListKeysValues.listConfigAliasKey[index].Value);
        }));
        return index;
    }

    public string SearchByKey(string key)
    {
        key = key.TrimEnd();
        key = key.TrimStart();

        foreach (KeyValue elem in ListKeysValues.keyValues)
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
        key = key.TrimEnd();
        key = key.TrimStart();
        int index = 0;
        while (index < ListKeysValues.keyValues.Count)
        {
            if (ListKeysValues.keyValues[index].key == key)
            {
                break;
            }
            index++;
        }
        if (index >= ListKeysValues.keyValues.Count)
        {
            return false;
        }

        keysStr = keysStr.Replace(ListKeysValues.keyValues[index].key, "");
        valuesStr = valuesStr.Replace(ListKeysValues.keyValues[index].value, "");
        keysValuesStr = keysValuesStr.Replace(ListKeysValues.keyValues[index].key + " : " + ListKeysValues.keyValues[index].value, "");
        GlobalConfig.entriesConfig.Value = keysValuesStr;
        GlobalConfig.entriesConfig.BoxedValue = keysValuesStr;

        ListKeysValues.keyValues.RemoveAt(index);

        for (int i = 0; i < ListKeysValues.keyValues.Count; i++)
        {
            ListKeysValues.listConfigAliasKey[i].Value = ListKeysValues.keyValues[i].key;
            ListKeysValues.listConfigAliasKey[i].BoxedValue = ListKeysValues.keyValues[i].key;

            ListKeysValues.listConfigAliasValue[i].Value = ListKeysValues.keyValues[i].value;
            ListKeysValues.listConfigAliasValue[i].BoxedValue = ListKeysValues.keyValues[i].value;
        }

        var lastKey = GlobalConfig.config.Bind(Text.LethalConfig.alias + ListKeysValues.keyValues.Count(), Text.LethalConfig.key, "", "");
        lastKey.Value = "";
        lastKey.BoxedValue = "";

        var lastValue = GlobalConfig.config.Bind(Text.LethalConfig.alias + ListKeysValues.keyValues.Count(), Text.LethalConfig.value, "", "");
        lastValue.Value = "";
        lastValue.BoxedValue = "";

        GlobalConfig.config.Save();

        return true;
    }
}

public class GlobalConfig
{    
    public static ConfigEntry<string> entriesConfig;
    public static ListKeysValues entriesList = new ListKeysValues("", "", "");
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
            entriesList.Append(new KeyValue(key, value));
            AliasPlugin.Logger.LogDebug("Content config" + ListKeysValues.keyValues.Count);
        }

        for (int i = 0; i < 8; i++)
        {
            var configKey = GlobalConfig.config.Bind(Text.LethalConfig.alias + (keyValue.Count + i), Text.LethalConfig.key, "", "");
            configKey.Value = "";
            configKey.BoxedValue = "";

            var configValue = GlobalConfig.config.Bind(Text.LethalConfig.alias + (keyValue.Count + i), Text.LethalConfig.value, "", "");
            configValue.Value = "";
            configValue.BoxedValue = "";

        LethalConfigManager.AddConfigItem(new GenericButtonConfigItem(Text.LethalConfig.alias + (keyValue.Count + i), Text.LethalConfig.remove, Text.LethalConfig.removeAnAlias, Text.LethalConfig.button, () => 
        {
            return;
        }));
        }
        
        AliasPlugin.Logger.LogDebug("Content Init end " + ListKeysValues.keyValues.Count);
    }
}
