using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LethalMenu.CustomCompany
{
    public class CustomCompanyManager : MonoBehaviour
    {
        public static CustomCompanyManager Instance
        {
            get
            {
                bool flag = CustomCompanyManager.instance == null;
                if (flag)
                {
                    CustomCompanyManager.Init();
                }
                return CustomCompanyManager.instance;
            }
        }
        public static void Init()
        {
            bool flag = CustomCompanyManager.instance != null;
            if (!flag)
            {
                GameObject gameObject = new GameObject("CustomCompanyManager");
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                gameObject.hideFlags = HideFlags.HideAndDontSave;
                CustomCompanyManager.instance = gameObject.AddComponent<CustomCompanyManager>();
            }
        }
        private static CustomCompanyManager instance;
    }
}
