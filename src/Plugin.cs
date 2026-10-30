using System.IO;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;

namespace GwentAccess
{
    [BepInPlugin(Guid, Name, Version)]
    public class Plugin : BasePlugin
    {
        public const string Guid = "gwentaccess";
        public const string Name = "Gwent Access";
        public const string Version = "0.26.2";

        private static ManualLogSource _log;
        public static void LogInfo(string msg) => _log?.LogInfo(msg);

        public override void Load()
        {
            _log = Log;
            string dir = Path.GetDirectoryName(typeof(Plugin).Assembly.Location);
            Log.LogInfo($"{Name} {Version} starting from {dir}");

            if (Speech.Init(dir, LogInfo))
                Speech.Say("Gwent Access loaded. F3 and F2 move, F4 clicks, F1 card details, F6 keys, F8 reads the screen.");

            Hotkeys.Start();

            try
            {
                var harmony = new HarmonyLib.Harmony(Guid);
                int ok = 0, bad = 0;
                foreach (var t in typeof(Plugin).Assembly.GetTypes())
                {
                    if (t.GetCustomAttributes(typeof(HarmonyLib.HarmonyPatch), false).Length == 0) continue;
                    try { harmony.CreateClassProcessor(t).Patch(); ok++; }
                    catch (System.Exception pe) { bad++; Log.LogError("Patch " + t.Name + " failed: " + pe.Message); }
                }
                Log.LogInfo("Patches applied: " + ok + ", failed: " + bad);
                Log.LogInfo("Menu speech patches applied.");
            }
            catch (System.Exception e) { Log.LogError("Patching failed: " + e); }
        }
    }
}
