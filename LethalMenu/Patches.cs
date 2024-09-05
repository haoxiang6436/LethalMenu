using GameNetcodeStuff;
using HarmonyLib;
using LethalMenu.Cheats;
using LethalMenu.Menu.Tab;
using LethalMenu.Util;
using Steamworks;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;


namespace LethalMenu
{
    [HarmonyPatch]
    internal class Patches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameNetworkManager), nameof(GameNetworkManager.Disconnect))]
        public static void Disconnect(GameNetworkManager __instance)
        {
            ServerTab.ClearPlayerOptions();
            SpectatePlayer.Reset();
            Freecam.Reset();
            LethalMenu.Instance.LMUsers.Clear();
            Shoplifter.Clear();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlayerControllerB), "SendNewPlayerValuesClientRpc")]
        public static void SendNewPlayerValuesClientRpc(PlayerControllerB __instance)
        {
            MenuUtil.RunLMUser();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), "OnClientConnect")]
        public static void OnClientConnect(StartOfRound __instance)
        {
            MenuUtil.RunLMUser();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GameNetworkManager), "StartClient")]
        public static void StartClient(SteamId id)
        {
            Settings.s_lobbyid = id;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(DepositItemsDesk), nameof(DepositItemsDesk.AttackPlayersServerRpc))]
        public static void CompanyAttackPrefix(ref bool ___attacking, ref bool ___inGrabbingObjectsAnimation, ref bool __state)
        {
            __state = ___inGrabbingObjectsAnimation;
            ___attacking = false;
            ___inGrabbingObjectsAnimation = false;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(DepositItemsDesk), nameof(DepositItemsDesk.AttackPlayersServerRpc))]
        public static void CompanyAttackPostfix(ref bool ___inGrabbingObjectsAnimation, ref bool __state)
        {
            ___inGrabbingObjectsAnimation = __state;
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(DepositItemsDesk), nameof(DepositItemsDesk.PlaceItemOnCounter))]
        public static IEnumerable<CodeInstruction> CompanyInfiniteSell(IEnumerable<CodeInstruction> instructions)
        {
            foreach (CodeInstruction instruction in instructions)
            {
                yield return instruction.opcode == OpCodes.Ldc_I4_S && instruction.operand.ToString().Equals("12") ?
                    new CodeInstruction(OpCodes.Ldc_I4, int.MaxValue) : instruction;
            }
        } 
    }
    // Token: 0x02000009 RID: 9
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRound_Patch
    {
        // Token: 0x0600003D RID: 61 RVA: 0x00003AEC File Offset: 0x00001CEC
        [HarmonyPatch("Debug_ReviveAllPlayersServerRpc")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Debug_ReviveAllPlayersServerRpcTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> list = instructions.ToList<CodeInstruction>();
            list[73].opcode = OpCodes.Nop;
            return list;
        }

        // Token: 0x0600003E RID: 62 RVA: 0x00003B18 File Offset: 0x00001D18
        [HarmonyPatch("Debug_ReviveAllPlayersClientRpc")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Debug_ReviveAllPlayersClientRpcTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> list = instructions.ToList<CodeInstruction>();
            list[58].opcode = OpCodes.Nop;
            return list;
        }
    }

}
