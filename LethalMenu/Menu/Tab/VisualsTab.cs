using LethalMenu.Language;
using LethalMenu.Menu.Core;
using LethalMenu.Types;
using LethalMenu.Util;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

namespace LethalMenu.Menu.Tab
{
    internal class VisualsTab : MenuTab
    {
        private Vector2 scrollPos = Vector2.zero;
        public VisualsTab() : base("VisualsTab.Title") { }
        private float f_leftWidth;

        public override void Draw()
        {
            f_leftWidth = HackMenu.Instance.contentWidth * 0.5f - HackMenu.Instance.spaceFromLeft;

            GUILayout.BeginVertical(GUILayout.Width(f_leftWidth));
            ESPMenuContent();
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUILayout.Width(f_leftWidth));
            OtherVisualsContent();
            ESPSettingsContent();
            GUILayout.EndVertical();
        }

        private void ESPMenuContent()
        {
            UI.Header("ESP");

            scrollPos = GUILayout.BeginScrollView(scrollPos);

            UI.Hack(Hack.ToggleAllESP, "VisualsTab.ToggleAll");
            UI.Toggle("VisualsTab.UseScrapTiers", ref Settings.b_useScrapTiers, "General.Disable", "General.Enable");
            UI.Toggle("VisualsTab.VCDisplay", ref Settings.b_VCDisplay, "General.Disable", "General.Enable");
            UI.Toggle("VisualsTab.HPDisplay", ref Settings.b_HPDisplay, "General.Disable", "General.Enable");
            UI.Hack(Hack.ObjectESP, "VisualsTab.ObjectESP");
            UI.Hack(Hack.EnemyESP, "VisualsTab.EnemyESP");
            UI.Hack(Hack.PlayerESP, "VisualsTab.PlayerESP");
            UI.Hack(Hack.DoorESP, "VisualsTab.EntExtDoorsESP");
            UI.Hack(Hack.LandmineESP, "VisualsTab.LandmineESP");
            UI.Hack(Hack.TurretESP, "VisualsTab.TurretESP");
            UI.Hack(Hack.ShipESP, "VisualsTab.ShipESP");
            UI.Hack(Hack.SteamHazardESP, "VisualsTab.SteamHazardESP");
            UI.Hack(Hack.BigDoorESP, "VisualsTab.BigDoorESP");
            UI.Hack(Hack.DoorLockESP, "VisualsTab.LockedDoorESP");
            UI.Hack(Hack.BreakerESP, "VisualsTab.BreakerESP");
            UI.Hack(Hack.SpikeRoofTrapESP, "VisualsTab.SpikeRoofTrapESP");

            GUILayout.EndScrollView();
        }

        private void OtherVisualsContent()
        {
            UI.Header("VisualsTab.OtherVisuals");
            UI.Hack(Hack.AlwaysShowClock, "VisualsTab.ShowClock");
            UI.Hack(Hack.SimpleClock, "VisualsTab.SimpleClock");
            UI.Hack(Hack.Crosshair, "VisualsTab.Crosshair");
            UI.Hack(Hack.Breadcrumbs, "VisualsTab.Breadcrumbs");
            UI.Hack(Hack.NoFog, "VisualsTab.NoFog");
            UI.Hack(Hack.NoVisor, "VisualsTab.NoVisor");
            UI.Hack(Hack.NoFieldOfDepth, "VisualsTab.NoFieldOfDepth");
            UI.HackSlider(Hack.FOV, "VisualsTab.FOV", Settings.f_fov.ToString("0.0"), ref Settings.f_fov, 10f, 180f);
        }
        private void ESPSettingsContent()
        {
            UI.Header("SettingsTab.ESP", true);
            UI.SubHeader("SettingsTab.Chams");

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width((f_leftWidth * 0.465f)));
            UI.Checkbox("SettingsTab.Objects", ref Settings.b_chamsObject);
            UI.Checkbox("SettingsTab.Enemies", ref Settings.b_chamsEnemy);
            UI.Checkbox("SettingsTab.Players", ref Settings.b_chamsPlayer);
            UI.Checkbox("SettingsTab.Landmines", ref Settings.b_chamsLandmine);
            UI.Checkbox("SettingsTab.Breaker", ref Settings.b_chamsBreaker);
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width((f_leftWidth * 0.465f)));
            UI.Checkbox("SettingsTab.Turrets", ref Settings.b_chamsTurret);
            UI.Checkbox("SettingsTab.Ship", ref Settings.b_chamsShip);
            UI.Checkbox("SettingsTab.SteamValves", ref Settings.b_chamsSteamHazard);
            UI.Checkbox("SettingsTab.BigDoors", ref Settings.b_chamsBigDoor);
            UI.Checkbox("SettingsTab.LockedDoors", ref Settings.b_chamsDoorLock);
            UI.Checkbox("SettingsTab.SpikeRoofTrap", ref Settings.b_chamsSpikeRoofTrap);
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            UI.SubHeader("SettingsTab.EnemyTypes", true);

            List<EnemyAIType> types = Enum.GetValues(typeof(EnemyAIType)).Cast<EnemyAIType>().ToList();

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width((f_leftWidth * 0.465f)));
            for (int i = 0; i < types.Count / 2; i++)
            {
                EnemyAIType type = types[i];
                if (type == EnemyAIType.Unknown) continue;
                //UI.Checkbox(type.ToString(), type.IsESPEnabled(), () => type.ToggleESP());
                UI.Checkbox(Localization.Localize("Enemies." + type.ToString()), type.IsESPEnabled(), () => type.ToggleESP());
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUILayout.Width((f_leftWidth * 0.465f)));
            for (int i = types.Count / 2; i < types.Count; i++)
            {
                EnemyAIType type = types[i];
                if (type == EnemyAIType.Unknown) continue;
                //UI.Checkbox(type.ToString(), type.IsESPEnabled(), () => type.ToggleESP());
                UI.Checkbox(Localization.Localize("Enemies." + type.ToString()), type.IsESPEnabled(), () => type.ToggleESP());
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

    }
}
