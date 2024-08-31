using LethalMenu.Language;
using LethalMenu.Manager;
using LethalMenu.Menu.Core;
using LethalMenu.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LethalMenu.Menu.Popup
{
    internal class WeatherManagerWindow : PopupMenu
    {
        private Vector2 scrollPos = Vector2.zero;

        public WeatherManagerWindow(int id) : base("WeatherManager.Title", new Rect(50f, 50f, 350f, 250f), id) { }

        public override void DrawContent(int windowID)
        {
            if (!LethalMenu.localPlayer.IsHost)
            {
                UI.Label("General.HostRequired", Settings.c_error);
                return;
            }

            scrollPos = GUILayout.BeginScrollView(scrollPos);

            if ((bool)StartOfRound.Instance)
            {
                SelectableLevel level = StartOfRound.Instance.currentLevel;
                UI.Header("MoonManager.CurrentMoon");
                UI.Label("MoonManager.Moon", StartOfRound.Instance.currentLevel.PlanetName);
                UI.Label("MoonManager.Weather", Localization.Localize("weather." + StartOfRound.Instance.currentLevel.currentWeather.ToString()));
                LevelWeatherType[] weathers = (LevelWeatherType[])Enum.GetValues(typeof(LevelWeatherType));
                List<LevelWeatherType> weathertypes = new List<LevelWeatherType>(weathers);
                foreach (var weather in weathertypes)
                {
                    UI.Button(Localization.Localize("weather." + weather.ToString()), () => {
                        level.currentWeather = weather;
                        HUDManager.Instance.DisplayTip($"Lethal Menu", $"���������� {Localization.Localize("weather." + level.currentWeather)} ���������Ч");
                    }, "WeatherManager.Change");
                }
            }
            GUILayout.EndScrollView();
            GUI.DragWindow();
        }
    }
}
