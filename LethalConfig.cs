
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using MyMod;
using MyFirstMod;
using LethalConfig;
using System.Collections.Generic;
using LethalConfig.ConfigItems;
using System;
public class LethalConfigAlias
{
    private string addKey = "";
    private string addValue = "";
    public void Init()
    {
        ListKeysValues keysValues = GlobalConfig.entriesList;
        var configEntryKeys = GlobalConfig.config.Bind(Text.LethalConfig.display, Text.LethalConfig.keys, keysValues.keysStr, Text.LethalConfig.descAllKeys);

        var configAddEntryKeys = GlobalConfig.config.Bind(Text.LethalConfig.addRemove, Text.LethalConfig.keys, "",  Text.LethalConfig.addRemoveAKey);
        configAddEntryKeys.BoxedValue = "";
        configAddEntryKeys.Value = "";
        var configAddEntryValues = GlobalConfig.config.Bind(Text.LethalConfig.addRemove, Text.LethalConfig.values, "", Text.LethalConfig.addAValue);
        configAddEntryValues.BoxedValue = "";
        configAddEntryValues.Value = "";

        LethalConfigManager.AddConfigItem(new GenericButtonConfigItem(Text.LethalConfig.addRemove, Text.LethalConfig.add, Text.LethalConfig.addAnAlias, Text.LethalConfig.button, () => 
        {
            if (configAddEntryKeys.Value == "" || configAddEntryValues.Value == "")
            {
                return;
            }
            configEntryKeys.Value +=  ";" + configAddEntryKeys.Value;
            configEntryKeys.BoxedValue +=  ";" + configAddEntryKeys.Value;
            GlobalConfig.entriesList.Add(new KeyValue(configAddEntryKeys.Value, configAddEntryValues.Value));
            GlobalConfig.entriesConfig.Value += "; " + configAddEntryKeys.Value + " : " + configAddEntryValues.Value + " ";
            GlobalConfig.entriesConfig.BoxedValue += "; " + configAddEntryKeys.Value + " : " + configAddEntryValues.Value + " ";
            configAddEntryKeys.Value = "";
            configAddEntryKeys.BoxedValue = "";
            configAddEntryValues.Value = "";
            configAddEntryValues.BoxedValue = "";
            GlobalConfig.config.Save();
        }));

        LethalConfigManager.AddConfigItem(new GenericButtonConfigItem(Text.LethalConfig.addRemove, Text.LethalConfig.remove, Text.LethalConfig.removeAnAlias, Text.LethalConfig.button, () => 
        {
            if (configAddEntryKeys.Value == "")
            {
                return;
            }

            if (!GlobalConfig.entriesList.RemoveByKey(configAddEntryKeys.Value))
            {
                configAddEntryKeys.Value = Text.LethalConfig.unbind;
                configAddEntryKeys.BoxedValue = Text.LethalConfig.unbind;
                configAddEntryValues.Value = Text.LethalConfig.unbind;
                configAddEntryValues.BoxedValue = Text.LethalConfig.unbind;
                GlobalConfig.config.Save();
                return;
            }

            GlobalConfig.entriesList.RemoveByKey(configAddEntryKeys.Value);

            configEntryKeys.Value =  GlobalConfig.entriesList.keysStr;
            configEntryKeys.BoxedValue =  GlobalConfig.entriesList.keysStr;
            GlobalConfig.entriesConfig.Value = GlobalConfig.entriesList.keysValuesStr;
            GlobalConfig.entriesConfig.BoxedValue = GlobalConfig.entriesList.keysValuesStr;
            configAddEntryKeys.Value = Text.LethalConfig.sucess;
            configAddEntryKeys.BoxedValue = Text.LethalConfig.sucess;
            configAddEntryValues.Value = "";
            configAddEntryValues.BoxedValue = "";
            GlobalConfig.config.Save();
        }));

        var searchConfigEntryKey = GlobalConfig.config.Bind(Text.LethalConfig.search, Text.LethalConfig.key, "", Text.LethalConfig.searchKey);
        var searchConfigEntryValue = GlobalConfig.config.Bind(Text.LethalConfig.search, Text.LethalConfig.value, "", Text.LethalConfig.resultValue);

        LethalConfigManager.AddConfigItem(new GenericButtonConfigItem(Text.LethalConfig.search, Text.LethalConfig.search, Text.LethalConfig.searchDesc, Text.LethalConfig.button, () => 
        {
            searchConfigEntryValue.BoxedValue = GlobalConfig.entriesList.SearchByKey(searchConfigEntryKey.Value);
        }));
    }
}