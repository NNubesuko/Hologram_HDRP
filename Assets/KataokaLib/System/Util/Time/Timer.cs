using UnityEngine;

namespace KataokaLib.System {

    public class Timer {

        public bool Elapsed { get; private set; } = false;

        private float currentTime = 0f;
        private readonly float second;
        public delegate void TimerDelegate();

        public Timer(float second = 0f) {
            this.second = second;
        }

        public void Update(TimerDelegate method) {
            currentTime += Time.deltaTime;

            if (currentTime >= second) {
                currentTime = second;
                method();
            }
        }

        public void Reset() {
            currentTime = 0;
            Elapsed = false;
        }

    }

}
