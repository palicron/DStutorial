using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

public class StateManager : MonoBehaviour {

        public float vertical;
        public float horizontal;

        public GameObject activeModel;
        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public Rigidbody rb;
        [HideInInspector]
        public float delta;

        public void Init()
        {
            setUpAnimator();
            rb = this.GetComponent<Rigidbody>();
        }

        private void setUpAnimator()
        {
            if (activeModel == null)
            {
                anim = this.GetComponentInChildren<Animator>();
                if (anim == null)
                    Debug.Log("Not model found");
                else
                    activeModel = anim.gameObject;
            }
            if (anim == null && activeModel != null)
                anim = activeModel.GetComponent<Animator>();

            anim.applyRootMotion = false;
        }

        public void Tick(float d)
        {
            delta = d;
        }
}

}
