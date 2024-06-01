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
        //__instance.screenText.SetTextWithoutNotify(text);

        AliasPlugin.Logger.LogDebug("Ptn aller: " + GlobalConfig.entriesList.Count);

        string text = __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded);

        int length = text.Length;

        if (length <= 0)
        {
            return;
        }

        AliasPlugin.Logger.LogDebug("Text:" + text);
        
        for (int i = 0; i < GlobalConfig.entriesList.Count; i++)
        {
            AliasPlugin.Logger.LogDebug("Iteration:" + GlobalConfig.entriesList[i][0] + '-' + text);
            if (text.Length > GlobalConfig.entriesList[i][0].Length)
            {
                continue;
            }

            /*if (text.StartsWith(MyMod.MyTestConfig.entriesList[i][0] + " "))
            {
                text = MyMod.MyTestConfig.entriesList[i][1] + text.Substring(MyMod.MyTestConfig.entriesList[i][0].Length);
                MyFirstMod.MyFirstMod.Logger.LogDebug("Wait");
            }*/
            
            text = text.Replace(GlobalConfig.entriesList[i][0]
                                    , GlobalConfig.entriesList[i][1]);
        
            /*if (text.EndsWith(" " + MyMod.MyTestConfig.entriesList[i][0]))
            {
                text = text.Substring(0, text.Length - MyMod.MyTestConfig.entriesList[i][1].Length)
                                        + MyMod.MyTestConfig.entriesList[i][1];
                MyFirstMod.MyFirstMod.Logger.LogDebug("Wait2");
            }*/
            AliasPlugin.Logger.LogDebug("Fin Iteration:" + GlobalConfig.entriesList[i][1] + "-" + text);
        }

        AliasPlugin.Logger.LogDebug("Ptn yesHeu:" + __instance.screenText.text.Substring(0, __instance.screenText.text.Length - __instance.textAdded));

        __instance.screenText.text = __instance.screenText.text.Substring(0, __instance.screenText.text.Length - __instance.textAdded)
                                        + text;

        __instance.textAdded = __instance.textAdded + (text.Length - length);

        __instance.modifyingText = saveBool;

        AliasPlugin.Logger.LogDebug("Ptn yesHa:" + __instance.screenText.text);
        //__instance.screenText.interactable = false;
        return;
    }
}
