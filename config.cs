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
    public static int number = 0;
    public string keysStr;
    public string valuesStr;
    public string keysValuesStr;

    public ListKeysValues(string keysStr, string valuesStr, string keysValuesStr)
    {
        this.keysStr = keysStr;
        this.valuesStr = valuesStr;
        this.keysValuesStr = keysValuesStr;
        LethalConfigManager.SkipAutoGenFor("NULL");
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
      
        ListKeysValues.listConfigAliasKey.Add(GlobalConfig.config.Bind("Alias-" + index, "Key", "", ""));
        ListKeysValues.listConfigAliasKey[index].Value = keyValue.key;
        ListKeysValues.listConfigAliasKey[index].BoxedValue = keyValue.key;

        ListKeysValues.listConfigAliasValue.Add(GlobalConfig.config.Bind("Alias-" + index, "Value", "", ""));
        ListKeysValues.listConfigAliasValue[index].Value = keyValue.value;
        ListKeysValues.listConfigAliasValue[index].BoxedValue = keyValue.value;
        
        ListKeysValues.number++;
        
        MyFirstMod.AliasPlugin.Logger.LogInfo("Number:" + number);
        

        LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("Alias-" + index, Text.LethalConfig.remove, Text.LethalConfig.removeAnAlias, Text.LethalConfig.button, () => 
        {
            MyFirstMod.AliasPlugin.Logger.LogInfo(index);
            MyFirstMod.AliasPlugin.Logger.LogInfo(ListKeysValues.listConfigAliasKey.Count());
            if (index >= ListKeysValues.listConfigAliasKey.Count())
            {
                return;
            }
            MyFirstMod.AliasPlugin.Logger.LogInfo("Heu..." + ListKeysValues.listConfigAliasKey[index].Value);
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
        MyFirstMod.AliasPlugin.Logger.LogInfo("REMOOVEEEEEEEEEEEEEEEEEEEEEEEE1");
        while (index < ListKeysValues.number)
        {
            if (ListKeysValues.keyValues[index].key == key)
            {
                break;
            }
            index++;
        }
        if (index >= ListKeysValues.keyValues.Count)
        {
            MyFirstMod.AliasPlugin.Logger.LogInfo("REMOOVEEEEEEEEEEEEEEEEEEEEEEEE2");
            return false;
        }

        ListKeysValues.number--;

        MyFirstMod.AliasPlugin.Logger.LogInfo("Index:" + index);

        keysStr = keysStr.Replace(ListKeysValues.keyValues[index].key, "");
        valuesStr = valuesStr.Replace(ListKeysValues.keyValues[index].value, "");
        keysValuesStr = keysValuesStr.Replace(ListKeysValues.keyValues[index].key + " : " + ListKeysValues.keyValues[index].value, "");
        GlobalConfig.entriesConfig.Value = keysValuesStr;
        GlobalConfig.entriesConfig.BoxedValue = keysValuesStr;

        MyFirstMod.AliasPlugin.Logger.LogInfo("Length:" + ListKeysValues.keyValues.Count());
        ListKeysValues.keyValues.RemoveAt(index);
        MyFirstMod.AliasPlugin.Logger.LogInfo("Length:" + ListKeysValues.keyValues.Count());

        MyFirstMod.AliasPlugin.Logger.LogInfo("REMOOVEEEEEEEEEEEEEEEEEEEEEEEE");

        for (int i = 0; i < ListKeysValues.keyValues.Count; i++)
        {
            ListKeysValues.listConfigAliasKey[i].Value = ListKeysValues.keyValues[i].key;
            ListKeysValues.listConfigAliasKey[i].BoxedValue = ListKeysValues.keyValues[i].key;

            ListKeysValues.listConfigAliasValue[i].Value = ListKeysValues.keyValues[i].value;
            ListKeysValues.listConfigAliasValue[i].BoxedValue = ListKeysValues.keyValues[i].value;

            MyFirstMod.AliasPlugin.Logger.LogInfo(i + ":" + ListKeysValues.keyValues[i].key + ":" + ListKeysValues.keyValues[i].value);
        }

        var lastKey = GlobalConfig.config.Bind("Alias-" + (ListKeysValues.keyValues.Count() - 1), "Key", "", "");
        lastKey.Value = "";
        lastKey.BoxedValue = "";

        var lastValue = GlobalConfig.config.Bind("Alias-" + (ListKeysValues.keyValues.Count() - 1), "Value", "", "");
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

            AliasPlugin.Logger.LogInfo("Index:" + i);
            AliasPlugin.Logger.LogInfo("Key:" + key);
            AliasPlugin.Logger.LogInfo("Value:" + value);
            AliasPlugin.Logger.LogInfo("LEEENGTH:" + ListKeysValues.keyValues.Count);
            entriesList.Append(new KeyValue(key, value));
            AliasPlugin.Logger.LogInfo("LEEENGTH:" + ListKeysValues.keyValues.Count);
            AliasPlugin.Logger.LogDebug("Content config" + ListKeysValues.keyValues.Count);
        }
        
        AliasPlugin.Logger.LogDebug("Content Init end " + ListKeysValues.keyValues.Count);
    }
}