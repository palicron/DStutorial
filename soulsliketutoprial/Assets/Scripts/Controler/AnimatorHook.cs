using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class AnimatorHook : MonoBehaviour
    {

        private Animator anim;
        private StateManager states;

        public void Int(StateManager st)
        {
            this.states = st;
            anim = states.anim;
        }
        private void OnAnimatorMove()
        {
            if (states.canMove)
                return;
            states.rib.drag = 0;
            float multiplier = 1;
            Vector3 delta = anim.deltaPosition;
            delta.y = 0;
            Vector3 v = (delta * multiplier) / states.delta;
            states.rib.velocity = v;

        }


    }

}
