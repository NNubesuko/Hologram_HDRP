using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KataokaLib.System {

    public abstract class TriggerObject : MonoBehaviour {

        protected virtual void OnTriggerEnter2DEvent(Collider2D collider) {
        }

        protected virtual void OnTriggerStay2DEvent(Collider2D collider) {
        }

        protected virtual void OnTriggerEnterAndStay2DEvent(Collider2D collider) {
        }

        protected virtual void OnTriggerExit2DEvent(Collider2D collider) {
        }

        private void OnTriggerEnter2D(Collider2D collider) {
            OnTriggerEnter2DEvent(collider);
            OnTriggerEnterAndStay2DEvent(collider);
        }

        private void OnTriggerStay2D(Collider2D collider) {
            OnTriggerStay2DEvent(collider);
            OnTriggerEnterAndStay2DEvent(collider);
        }

        private void OnTriggerExit2D(Collider2D collider) {
            OnTriggerExit2DEvent(collider);
        }

    }

}
