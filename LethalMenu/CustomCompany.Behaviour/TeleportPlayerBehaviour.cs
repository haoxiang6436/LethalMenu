using System;
using System.Collections;
using System.Linq;
using GameNetcodeStuff;
using UnityEngine;

namespace LethalMenu.CustomCompany.Behaviour
{
    public static class TeleportPlayerBehaviour
    {

        public static void TeleportPlayer(PlayerControllerB player)
        {
            bool flag = TeleportPlayerBehaviour.coroutine != null;
            if (flag)
            {
                CustomCompanyManager.Instance.StopCoroutine(TeleportPlayerBehaviour.coroutine);
                TeleportPlayerBehaviour.coroutine = null;
            }
            TeleportPlayerBehaviour.coroutine = CustomCompanyManager.Instance.StartCoroutine(TeleportPlayerBehaviour.TeleportPlayerSync(player));
        }

        private static IEnumerator TeleportPlayerSync(PlayerControllerB player)
        {
            ShipTeleporter teleporter = TeleportPlayerBehaviour.GetShipTeleporter();
            bool flag = teleporter && player;
            if (flag)
            {
                int locals1 = TeleportPlayerBehaviour.SearchForPlayerInRadar(player);
                StartOfRound.Instance.mapScreen.SwitchRadarTargetAndSync(locals1);
                yield return new WaitForSeconds(0.15f);
                yield return new WaitUntil(() => StartOfRound.Instance.mapScreen.targetTransformIndex == locals1);
                teleporter.PressTeleportButtonOnLocalClient();
            }
            else
            {
                yield return null;
            }
            yield break;
        }

        private static ShipTeleporter GetShipTeleporter()
        {
            bool flag = TeleportPlayerBehaviour.shipTeleporter != null;
            bool flag2 = flag;
            ShipTeleporter result;
            if (flag2)
            {
                result = TeleportPlayerBehaviour.shipTeleporter;
            }
            else
            {
                ShipTeleporter[] array = UnityEngine.Object.FindObjectsOfType<ShipTeleporter>();
                ShipTeleporter shipTeleporter = null;
                foreach (ShipTeleporter shipTeleporter2 in array)
                {
                    bool isInverseTeleporter = shipTeleporter2.isInverseTeleporter;
                    bool flag3 = !isInverseTeleporter;
                    if (flag3)
                    {
                        shipTeleporter = shipTeleporter2;
                        break;
                    }
                }
                TeleportPlayerBehaviour.shipTeleporter = shipTeleporter;
                result = TeleportPlayerBehaviour.shipTeleporter;
            }
            return result;
        }
        private static ShipTeleporter GetShipInverseTeleporter()
        {
            bool flag2 = TeleportPlayerBehaviour.shipInverseTeleporter != null;
            ShipTeleporter result;
            if (flag2)
            {
                result = TeleportPlayerBehaviour.shipInverseTeleporter;
            }
            else
            {
                ShipTeleporter[] array = UnityEngine.Object.FindObjectsOfType<ShipTeleporter>();
                ShipTeleporter shipInverseTeleporter = null;
                foreach (ShipTeleporter shipTeleporter2 in array)
                {
                    if (shipTeleporter2.isInverseTeleporter)
                    {
                        shipInverseTeleporter = shipTeleporter2;
                        break;
                    }
                }
                TeleportPlayerBehaviour.shipInverseTeleporter = shipInverseTeleporter;
                result = TeleportPlayerBehaviour.shipInverseTeleporter;
            }
            return result;
        }

        private static int SearchForPlayerInRadar(PlayerControllerB player)
        {
            int result = -1;
            for (int i = 0; i < StartOfRound.Instance.mapScreen.radarTargets.Count<TransformAndName>(); i++)
            {
                bool flag = StartOfRound.Instance.mapScreen.radarTargets[i].transform.gameObject.GetComponent<PlayerControllerB>() != player;
                bool flag2 = !flag;
                if (flag2)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }

        public static void StartShipInverseTeleporter()
        {
            ShipTeleporter shipTeleporter = TeleportPlayerBehaviour.GetShipInverseTeleporter();
            shipTeleporter.PressTeleportButtonOnLocalClient();
        }

        private static ShipTeleporter shipTeleporter;
        private static ShipTeleporter shipInverseTeleporter; 

        private static Camera cmr;

        private static Coroutine coroutine;
    }
}
