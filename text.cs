using MyFirstMod;
using Unity.Multiplayer.Tools.NetStatsMonitor;

namespace Text
{
    class GlobalConfig
    {
        public static string default_value = " vm : VIEW MONITOR ; sw : SWITCH ; s : SWITCH ; p : PING ; t : TRANSMIT ; sc : SCAN ; st : STORE ; m : MOONS ; tcb : THE COMPANY BUILDING ; exp : EXPERIMENTATION ; ass : ASSURANCE ; v : VOW ; ma : MARCH ; off : OFFENSE ; ad : ADAMANCE ; re : REND ; di : DINE ; ti : TITAN";
        public static string warningMessageBadElement = "[" + MyPluginInfo.PLUGIN_NAME + "] : An element cannot be initialized. Continue to the next one. Element:";
        public static string configSection = "All alias";
        public static string KeyConfig = "List alias";
        public static string description = "To put your alias -> \"my alias : command \"";
    }

    class init
    {
        public static string finishLoaded = $"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!";
        public static string finishPatch = "Finished patching!";
    }

    class LethalConfig
    {
        public static string display = "Display";
        public static string keys = "Keys";
        public static string key = "Key";
        public static string values = "Values";
        public static string value = "value";
        public static string resultValue = "Result value";
        public static string descAllKeys = "All keys with structure -> key : value";
        public static string addRemove = "Add/Remove";
        public static string add = "Add";
        public static string addAnAlias = "Add an alias. Be careful, key and value must not be empty";
        public static string addRemoveAKey = "Add/Remove a key";
        public static string addAValue = "Add a value";
        public static string remove = "Remove";
        public static string removeAnAlias = "Remove an alias";
        public static string search = "Search";
        public static string searchKey = "Search a value from a key";
        public static string searchDesc = "Click to search. Key must not be empty";
        public static string button = "click";
        public static string unbind = "Unbind";
        public static string sucess = "Sucess";
    }
}