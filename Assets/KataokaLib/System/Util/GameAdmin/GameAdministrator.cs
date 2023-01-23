using UnityEngine;

namespace KataokaLib.System {

    public class GameAdministrator {

        public static void SetFPS(int fps) {
            Application.targetFrameRate = fps;
        }

        public static void DisplayCursor() {
            Cursor.visible = true;
        }

        public static void HiddenCursor() {
            Cursor.visible = false;
        }

        public static void QuitGame() {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

    }

}