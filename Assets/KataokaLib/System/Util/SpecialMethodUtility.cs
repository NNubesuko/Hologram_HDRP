using UnityEngine.Events;

namespace KataokaLib.System {

    public class SpecialMethodUtility {

        private bool oneTime = true;

        public void OneTimeMethod(UnityAction action) {
            if (oneTime) {
                oneTime = false;
                action();
            }
        }
        
        public void OneTimeMethod(UnityAction action, bool reset) {
            if (reset) {
                oneTime = true;
            }

            if (oneTime) {
                oneTime = false;
                action();
            }
        }

        public void Reset() {
            oneTime = true;
        }

    }

}
