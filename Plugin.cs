using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using KnifeSprint.Patches;

namespace KnifeSprint
{
    [BepInPlugin("com.20fpsguy.knifesprint", "KnifeSprint", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LogSource;
        public static ConfigEntry<float> SpeedMultiplier;

        private void Awake()
        {
            LogSource = Logger;

            SpeedMultiplier = Config.Bind(
                "General",
                "Sprint Speed Multiplier",
                1.15f,
                new ConfigDescription(
                    "How much faster you sprint with a knife out.",
                    new AcceptableValueRange<float>(1.0f, 2.0f))
            );

            new ClampedSpeedPatch().Enable();
            new ApplyMotionPatch().Enable();

            LogSource.LogInfo("KnifeSprint loaded.");
        }
    }
}
