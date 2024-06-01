using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MyMod;
namespace MyFirstMod;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class AliasPlugin : BaseUnityPlugin
{
    internal static GlobalConfig BoundConfig { get; private set; } = null!; 
    public static AliasPlugin Instance { get; private set; } = null!;
    internal static Harmony? Harmony { get; set; }

    internal new static ManualLogSource Logger { get; private set; } = null!;

    private void Awake()
    {
        Logger = base.Logger;
        Instance = this;

        BoundConfig = new GlobalConfig(base.Config); 
        BoundConfig.SetCOnfig();

        Patch();

        Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
    }

    internal static void Patch()
    {
        Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

        Logger.LogDebug("Patching...");

        //Harmony.PatchAll();
        Harmony.PatchAll(typeof(AliasPatch));

        Logger.LogDebug("Finished patching!");
    }

    internal static void Unpatch()
    {
        Logger.LogDebug("Unpatching...");

        Harmony?.UnpatchSelf();

        Logger.LogDebug("Finished unpatching!");
    }
}
