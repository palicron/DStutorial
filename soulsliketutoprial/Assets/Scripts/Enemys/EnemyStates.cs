using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class EnemyStates : MonoBehaviour
    {
        private Animator anim;

        EnemyTarget enTarget;

        private void Start()
        {
            anim = GetComponentInChildren<Animator>();
            enTarget = GetComponent<EnemyTarget>();
            enTarget.Init(anim);
        }


    }
}

