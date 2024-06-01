using MyFirstMod;

namespace Text
{
    class GlobalConfig
    {
        public static string default_value = "vm : VIEW MONITOR ; sw : SWITCH ; s : SWITCH ; p : PING ; t : TRANSMIT ; sc : SCAN ; st : STORE ; m : MOONS ; tcb : THE COMPANY BUILDING ; exp : EXPERIMENTATION ; ass : ASSURANCE ; v : VOW ; ma : MARCH ; off : OFFENSE ; ad : ADAMANCE ; re : REND ; di : DINE ; ti : TITAN";
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
}