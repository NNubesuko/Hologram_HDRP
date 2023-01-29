using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceAdmin : MonoBehaviour {

    private void Awake() {
        Cursor.visible = false;

        int count = Mathf.Min(Display.displays.Length, 2);
        for (int i = 0; i < count; ++i) {
            Display.displays[i].Activate();
        }
    }

}
