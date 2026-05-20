using Comfort.Common;
using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Reflection;

namespace KnifeSprint.Patches
{
    public class ClampedSpeedPatch : ModulePatch
    {
        private static FieldInfo _playerField;

        protected override MethodBase GetTargetMethod()
        {
            _playerField = AccessTools.Field(typeof(MovementContext), "_player");
            return AccessTools.PropertyGetter(typeof(MovementContext), "ClampedSpeed");
        }

        [PatchPostfix]
        private static void Postfix(MovementContext __instance, ref float __result)
        {
            if (!__instance.IsSprintEnabled) return;
            if (_playerField == null) return;

            var gameWorld = Singleton<GameWorld>.Instance;
            if (gameWorld?.MainPlayer == null) return;

            var player = _playerField.GetValue(__instance) as Player;
            if (player != gameWorld.MainPlayer) return;

            var hc = player.HandsController;
            if (hc == null) return;
            if (hc.GetType().Name.IndexOf("Knife", StringComparison.OrdinalIgnoreCase) < 0) return;

            __result *= Plugin.SpeedMultiplier.Value;
        }
    }
}
