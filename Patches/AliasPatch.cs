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
        
        for (int i = 0; i < GlobalConfig.entriesList.keyValues.Count; i++)
        {
            if (text.Length > GlobalConfig.entriesList.keyValues[i].key.Length)
                continue;
            
            text = text.Replace(GlobalConfig.entriesList.keyValues[i].key, GlobalConfig.entriesList.keyValues[i].value);
        }

        __instance.screenText.text = __instance.screenText.text.Substring(0, __instance.screenText.text.Length - __instance.textAdded) + text;
        __instance.textAdded = __instance.textAdded + (text.Length - length);
        __instance.modifyingText = saveBool;
        return;
    }
}
