using LethalMenu.Menu.Core;
using UnityEngine;

namespace LethalMenu.Menu.Tab
{
    internal class GeneralTab : MenuTab
    {
        Vector2 scrollPos;
        Texture2D avatar;

        public GeneralTab() : base("GeneralTab.Title")
        {
            GetImage("https://icyrelic.com/img/Avatar2.jpg", Avatar);
        }

        public void Avatar(Texture2D texture)
        {
            avatar = texture;
        }

        public override void Draw()
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            MenuContent();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }

        private void MenuContent()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            GUILayout.BeginHorizontal();
            GUILayout.Label(avatar, GUILayout.Width(100), GUILayout.Height(100));
            GUILayout.Label("感谢使用LethalMenu\n\n如果你有任何建议，请在论坛帖子上留言\n如果你发现任何漏洞，请提供一些重现问题的步骤并留言\n");
            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            foreach (string line in Settings.Changelog.changes)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label);

                if (line.StartsWith("v")) style.fontStyle = FontStyle.Bold;
                GUILayout.Label(line.StartsWith("v") ? "更新日志： " + line : line, style);
            }

            GUILayout.EndScrollView();
        }
    }
}
