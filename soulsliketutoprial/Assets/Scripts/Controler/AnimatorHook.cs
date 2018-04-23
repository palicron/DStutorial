using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class AnimatorHook : MonoBehaviour
    {

        private Animator anim;
        private StateManager states;
        private EnemyStates eStates;
        private Rigidbody rib;

        public float rm_multi;
        private float delta;
        private bool rolling;
        private float roll_t;

        private AnimationCurve roll_curve;
        public void Int(StateManager st,EnemyStates eSt)
        {
            this.states = st;
            this.eStates = eSt;
            if(st != null)
            {
                anim = states.anim;
                rib = states.rib;
                roll_curve = states.roll_curve;
          
            }
       
            if(eSt!=null)
            {
                anim = eStates.anim;
                rib = eStates.rib;
                delta = eSt.delta;
            }
                

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
            if (states == null && eStates == null)
                return;

            if (rib == null)
                return;

            if (states != null)
            {
                if (states.canMove)
                    return;

                delta = states.delta;

            }

            if (eStates != null)
            {
                if (!eStates.canMove)
                    return;
                delta = eStates.delta;
                Debug.Log("carajo");
            }

              rib.drag = 0;

            if (rm_multi == 0)
                rm_multi = 1;

            if (!rolling)
            {
                Vector3 delta2 = anim.deltaPosition;
                delta2.y = 0;
                Vector3 v = (delta2 * rm_multi) / delta;
                rib.velocity = v;
            }
            else
            {
                roll_t += delta / 0.6f;
                if (roll_t > 1)
                    roll_t = 1;
                if (states == null)
                    return;
                float zValue = roll_curve.Evaluate(roll_t);
                Vector3 v1 = Vector3.forward * zValue;
                Vector3 relative = transform.TransformDirection(v1);
                Vector3 v2 = (relative * rm_multi);
                rib.velocity = v2;
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
