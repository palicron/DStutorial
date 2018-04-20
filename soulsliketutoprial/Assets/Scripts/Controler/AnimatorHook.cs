using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class AnimatorHook : MonoBehaviour
    {

        private Animator anim;
        private StateManager states;
        public float rm_multi;
     
        private bool rolling;
        private float roll_t;
        public void Int(StateManager st)
        {
            this.states = st;
            anim = states.anim;
          
        }

        public void InitForRoll()
        {
            rolling = true;
            roll_t = 0;
        }
        public void CloseRoll()
        {
            if (!rolling)
                return;
            rm_multi = 1;
            roll_t = 0; 
            rolling = false;
        }
        private void OnAnimatorMove()
        {
            if (states == null)
                return;
            if (states.canMove)
                return;
            states.rib.drag = 0;

            if (rm_multi == 0)
                rm_multi = 1;

            if (!rolling)
            {
                Vector3 delta = anim.deltaPosition;
                delta.y = 0;
                Vector3 v = (delta * rm_multi) / states.delta;
                states.rib.velocity = v;
            }
            else
            {
                roll_t += states.delta /0.6f;
                if (roll_t > 1)
                    roll_t = 1;
                float zValue = states.roll_curve.Evaluate(roll_t);
                Vector3 v1= Vector3.forward * zValue;
                Vector3 relative = transform.TransformDirection(v1);
                Vector3 v2 = (relative * rm_multi);
                states.rib.velocity = v2;
            }
           

        }

        public void OpenDamageColliders()
        {
            if (states == null)
                return;
            states.inventoryManager.curWeapon.w_hook.OpenDamageColliders();
        }
        public void CloseDamageColliders()
        {
            if (states == null)
                return;
            states.inventoryManager.curWeapon.w_hook.CloseDamageColliders();
        }
    }

}
