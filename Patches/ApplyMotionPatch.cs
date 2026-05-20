using Comfort.Common;
using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Reflection;
using UnityEngine;

namespace KnifeSprint.Patches
{
    public class ApplyMotionPatch : ModulePatch
    {
        private static FieldInfo _playerField;

        protected override MethodBase GetTargetMethod()
        {
            _playerField = AccessTools.Field(typeof(MovementContext), "_player");
            return AccessTools.Method(typeof(MovementState), nameof(MovementState.ApplyMotion));
        }

        [PatchPrefix]
        private static void Prefix(MovementState __instance, ref Vector3 motion)
        {
            if (!__instance.MovementContext.IsSprintEnabled) return;

            var gameWorld = Singleton<GameWorld>.Instance;
            if (gameWorld?.MainPlayer == null) return;

            var player = _playerField.GetValue(__instance.MovementContext) as Player;
            if (player != gameWorld.MainPlayer) return;

            var hc = player.HandsController;
            if (hc == null) return;
            if (hc.GetType().Name.IndexOf("Knife", StringComparison.OrdinalIgnoreCase) < 0) return;

            motion *= Plugin.SpeedMultiplier.Value;
        }
    }
}
