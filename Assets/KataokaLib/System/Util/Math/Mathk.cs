using UnityEngine;

namespace KataokaLib.System {

    public class Mathk {

        public static T KeepValueWithinRange<T>(T value, T MIN, T MAX) {
            value = Mathf.Max((dynamic)value, (dynamic)MIN);
            value = Mathf.Min((dynamic)value, (dynamic)MAX);

            return value;
        }

        public static int Normalize(int x, int min, int max) {
            int cycle = max - min;
            x = (x - min) % cycle + min;

            if (x < min)
                x += cycle;

            return x;
        }

    }

}