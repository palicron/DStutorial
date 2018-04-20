using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class EnemyStates : MonoBehaviour
    {
        public float healt;
        private Animator anim;

        public bool isInvicible;
        EnemyTarget enTarget;

        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
            enTarget = GetComponent<EnemyTarget>();
            enTarget.Init(anim);
        }


        private void Update()
        {
            if(isInvicible)
            {
                isInvicible = !anim.GetBool("canMove");
            }
           
        }
        public void DoDmg(float v)
        {
            if (isInvicible)
                return;

            healt -= v;
            isInvicible = true;
            anim.Play("damage 1");
        }

    }
}

