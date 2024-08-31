﻿using LethalMenu.Cheats;
using LethalMenu.Language;
using LethalMenu.Manager;
using LethalMenu.Menu.Core;
using LethalMenu.Util;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.Netcode;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LethalMenu.Menu.Popup
{
    internal class ItemManagerWindow : PopupMenu
    {
        private Vector2 scrollPos = Vector2.zero;
        private string s_scrapValue = "100";
        private string s_search = "";

        public ItemManagerWindow(int id) : base("ItemManager.Title", new Rect(50f, 50f, 577f, 300f), id) { }

        public override void DrawContent(int windowID)
        {
            //if (!LethalMenu.localPlayer.IsHost)
            //{
            //    UI.Label("General.HostRequired", Settings.c_error);
            //    return;
            //}

            scrollPos = GUILayout.BeginScrollView(scrollPos);

            GUILayout.BeginHorizontal();
            UI.Textbox("General.Search", ref s_search);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            UI.Textbox("ItemManager.ScrapValue", ref s_scrapValue, @"[^0-9]");
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            UI.ButtonGrid(StartOfRound.Instance.allItemsList.itemsList, (i) => Localization.Localize("Items."+i.name), s_search, (i) => SpawnItem(i), 3);
            //UI.ButtonGrid(StartOfRound.Instance.allItemsList.itemsList, (i) => i.name, s_search, (i) => SpawnItem(i), 3);

            GUILayout.EndScrollView();
            GUI.DragWindow();
        }
        private void SpawnItem(Item item)
        {
            Vector3 position = GameNetworkManager.Instance.localPlayerController.playerEye.transform.position;
            GameObject gameObject = Object.Instantiate(item.spawnPrefab, position, Quaternion.identity, StartOfRound.Instance.propsContainer);
            int value = int.TryParse(s_scrapValue, out value) ? value : UnityEngine.Random.Range(15, 100);
            gameObject.GetComponent<GrabbableObject>().SetScrapValue(value);
            gameObject.GetComponent<GrabbableObject>().fallTime = 0.0f;
            gameObject.GetComponent<NetworkObject>().Spawn();
            HUDManager.Instance.DisplayTip($"Lethal Menu", $"生成了物品: {Localization.Localize("Items." + item.name)}! 物品价值: {value}!");
        }
    }
}