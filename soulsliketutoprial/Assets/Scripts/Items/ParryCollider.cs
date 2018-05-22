using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA { 
public class ParryCollider : MonoBehaviour {

        private void OnTriggerEnter(Collider other)
        {
            EnemyStates e_st = other.transform.GetComponentInParent<EnemyStates>();
            if (e_st == null)
                return;
            if (!e_st.canBeParried || e_st.isInvicible)
                return;

            e_st.CheckForParry(transform.root);


        }
    }

}
