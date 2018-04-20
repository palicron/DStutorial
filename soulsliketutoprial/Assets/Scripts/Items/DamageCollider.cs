using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

public class DamageCollider : MonoBehaviour {

        private void OnTriggerEnter(Collider other)
        {
            EnemyStates eStates = other.transform.GetComponentInParent<EnemyStates>();
            if (eStates == null)
                return;

            eStates.DoDmg(5);

        }
    }

}
