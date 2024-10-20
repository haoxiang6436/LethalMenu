﻿using GameNetcodeStuff;
using LethalMenu.Manager;
using LethalMenu.Menu.Core;
using LethalMenu.Util;
using System.Linq;
using UnityEngine;
using Steamworks;
using Steamworks.Data;
using Unity.Netcode;
using Object = UnityEngine.Object;


namespace LethalMenu.Menu.Tab
{
    internal class DebugTab : MenuTab
    {
        private Vector2 scrollPos = Vector2.zero;
        public DebugTab() : base("Debug") { }

        public override void Draw()
        {
            GUILayout.BeginVertical();
            MenuContent();
            GUILayout.EndVertical();
        }

        private void MenuContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            if (GUILayout.Button("Clear Debug Message")) Settings.debugMessage = "";
            GUILayout.TextArea(Settings.debugMessage, GUILayout.Height(50));

            UI.Button("LookAt Closest Item", () =>
            {
                GrabbableObject item = LethalMenu.items.Where(i => !i.isInShipRoom).OrderBy(
                    i => Vector3.Distance(i.transform.position, LethalMenu.localPlayer.transform.position)
                ).FirstOrDefault();

                if (item == null) return;

                LethalMenu.localPlayer.transform.LookAt(item.transform.position);
            });

            UI.Button("LookAt Closest Player", () =>
            {
                PlayerControllerB player = LethalMenu.players.Where(p => p != LethalMenu.localPlayer).OrderBy(
                        p => Vector3.Distance(p.transform.position, LethalMenu.localPlayer.transform.position)
                ).FirstOrDefault();

                if (player == null) return;

                LethalMenu.localPlayer.transform.LookAt(player.transform.position);
            });

            GUILayout.Label("Debug Menu");

            GUILayout.BeginHorizontal();
            GUILayout.Label("Leaderboard");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Execute"))
            {
                Leaderboard();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Debug NetworkObjectReferences");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Execute"))
            {
                NetworkManager.Singleton.SpawnManager.SpawnedObjects.ToList().ForEach(s =>
                {
                    Debug.Log($"Name: {s.Value.gameObject.name} ID: {s.Key}");
                });
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Debug GetSpawnableMapObjects");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Execute"))
            {
                GameUtil.GetSpawnableMapObjects().ToList().ForEach(o => Debug.Log(o.prefabToSpawn.name));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Goto Not Spawned");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Execute"))
            {
                LethalMenu.localPlayer.TeleportPlayer(StartOfRound.Instance.notSpawnedPosition.position);
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Raycast Colliders");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Execute"))
            {
                foreach (RaycastHit hit in CameraManager.ActiveCamera.transform.SphereCastForward())
                {
                    Collider collider = hit.collider;
                    Settings.debugMessage = ("Hit: " + collider.name + " =>" + collider.gameObject.name + "\n");
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Garage");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Execute"))
            {
                LethalMenu.animatedTriggers.Where(t => t.name == "Cube" && t.transform.parent.name == "Cutscenes").ToList().ForEach(t => t.triggerAnimator?.SetTrigger("doorFall"));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Test");
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Execute"))
            {
               RadMechAI enemy = Object.FindAnyObjectByType<RadMechAI>();

                foreach (var item in enemy.GetComponentsInParent<Renderer>())
                {
                    Debug.LogWarning(item.name + "=>" + item.GetType());

                }

                foreach (var item in enemy.GetComponentsInChildren<Renderer>())
                {
                    Debug.LogError(item.name + "=>" + item.GetType());

                }
            }
    GUILayout.EndHorizontal();
            GUILayout.EndScrollView(); 
        }

        private async void Leaderboard()
        {
            int weekNum = GameNetworkManager.Instance.GetWeekNumber();
            Leaderboard? leaderboardAsync = await SteamUserStats.FindOrCreateLeaderboardAsync(string.Format("challenge{0}", weekNum), LeaderboardSort.Descending, LeaderboardDisplay.Numeric);

            LeaderboardUpdate? nullable = await leaderboardAsync.Value.ReplaceScore(int.MaxValue);

            Settings.debugMessage = (nullable.Value.OldGlobalRank + " => " + nullable.Value.NewGlobalRank);
        }
    }
}