using System.IO;
using System.Reflection;
using System;
using UnityEngine;
using System.Linq;
using LethalMenu.Handler;
using LethalMenu.Menu.Core;
using Object = UnityEngine.Object;


namespace LethalMenu
{
    public class Loader : MonoBehaviour
    {
        private static GameObject Load;

        public static void Init()
        {
            MenuFragment.InjectNotification(Load);
            if (Load != null) return;      
            ChamHandler.ChamsSetEnabled(true);
            LoadHarmony();
            Loader.Load = new GameObject();
            Load.AddComponent<LethalMenu>();
            Object.DontDestroyOnLoad(Loader.Load);
        }

        public static void LoadHarmony()
        {
            String name = "LethalMenu.Resources.0Harmony.dll";
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream(name);
            byte[] rawAssembly = new byte[stream.Length];
            stream.Read(rawAssembly, 0, (int)stream.Length);
            AppDomain.CurrentDomain.Load(rawAssembly);
        }

        public static void Unload()
        {
            HackExtensions.ToggleFlags.Keys.ToList().ForEach(h => HackExtensions.ToggleFlags[h] = false);
            ChamHandler.ChamsSetEnabled(false);
            if (Cursor.visible) Hack.ToggleCursor.Execute();
            //LethalMenu.harmony.UnpatchAll("LethalMenu");
            Object.Destroy(Load);
        }
    }
}
