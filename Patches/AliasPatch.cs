using System;
using System.Text;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MyFirstMod;
using TMPro;
using Unity.Netcode;
using UnityEngine.UI;
using System.Collections;
using UnityEngine;
using MyMod;



[HarmonyPatch(typeof(Terminal))]
class AliasPatch
{ 
    [HarmonyPatch(typeof(Terminal), nameof(Terminal.ParsePlayerSentence))]
    [HarmonyPrefix]
    private static void TestAlias(ref Terminal __instance)
    {
        bool saveBool = __instance.modifyingText;
        __instance.modifyingText = true;
        string text = __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded);
        int length = text.Length;

        if (length <= 0)
            return;
        
        MyFirstMod.AliasPlugin.Logger.LogInfo("Length: " + ListKeysValues.keyValues.Count);

        for (int i = 0; i < ListKeysValues.number; i++)
        {
            MyFirstMod.AliasPlugin.Logger.LogInfo(ListKeysValues.keyValues[i].key);
            if (text.Length > ListKeysValues.keyValues[i].key.Length)
                continue;
            
            text = text.Replace(ListKeysValues.keyValues[i].key, ListKeysValues.keyValues[i].value);
        }

        __instance.screenText.text = __instance.screenText.text.Substring(0, __instance.screenText.text.Length - __instance.textAdded) + text;
        __instance.textAdded = __instance.textAdded + (text.Length - length);
        __instance.modifyingText = saveBool;
        return;
    }
}
