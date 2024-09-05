using LethalMenu.Language;
using LethalMenu.Manager;
using LethalMenu.Menu.Core;
using LethalMenu.Types;
using LethalMenu.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LethalMenu.Menu.Popup
{
    internal class StoreManagerWindow : PopupMenu
    {
        private Vector2 scrollPos = Vector2.zero;
        private Terminal terminal;
        private string str_buyNum = "1";
        private static int itemIndex = -1;
        private static string itemName = string.Empty;
        private string s_search = "";

        public StoreManagerWindow(int id) : base("UnlockableManager.Title", new Rect(50f, 50f, 562f, 225f), id) { }

        public override void DrawContent(int windowID)
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);
            UI.Header(Localization.Localize("StoreManager." + itemName));
            UI.TextboxAction("StoreManager.BuyNum", ref str_buyNum, @"", 30,
            new UIButton("General.Execute", () => {
                if (itemIndex == -1) {
                    HUDManager.Instance.DisplayTip("Lethal Menu", $"请选择需要购买的装备");
                    return;
                    };
                terminal = RoundHandler.GetTerminal();
                int num = int.Parse(str_buyNum);
                while (num > 0)
                {
                    var ls = new List<int>();
                    ls.AddRange(Enumerable.Repeat(itemIndex, num > 10 ? 10 : num));
                    num -= 10;
                    terminal.BuyItemsServerRpc(ls.ToArray(), terminal.groupCredits, 0);
                }
                itemIndex = -1;
                itemName = string.Empty;
            }));
            List<Item> itemList = RoundHandler.GetTerminal().buyableItemsList.ToList();
            Dictionary<string, int> itemNameToIndex = new Dictionary<string, int>();
            for (int i = 0; i < itemList.Count; i++)
            {
                itemNameToIndex[itemList[i].itemName] = i;
            }
            UI.ButtonGrid<Item>(itemList, i => Localization.Localize("StoreManager." + i.itemName), s_search, (i) => {
                if (itemNameToIndex.ContainsKey(i.itemName))
                {
                    itemIndex = itemNameToIndex[i.itemName];
                    itemName = i.itemName;
                }
            }, 3);
            GUILayout.EndScrollView();
            GUI.DragWindow();
        }
    }
}
