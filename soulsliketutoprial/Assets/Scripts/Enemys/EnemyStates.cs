using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class EnemyStates : MonoBehaviour
    {
        public float healt;
        public Animator anim;
        public bool canMove;
        public Rigidbody rib;
        public bool isInvicible;
        EnemyTarget enTarget;
        AnimatorHook a_hook;
        public float delta;
        public bool isDead;
        List<Rigidbody> ragdollRigids = new List<Rigidbody>();
        List<Collider> ragDollColiders = new List<Collider>();
        private void Start()
        {
            healt = 100;
            anim = GetComponentInChildren<Animator>();
            enTarget = GetComponent<EnemyTarget>();
            enTarget.Init(this);
            a_hook = anim.GetComponent<AnimatorHook>();
     
            rib = GetComponent<Rigidbody>();
            if (a_hook == null)
                a_hook = anim.gameObject.AddComponent<AnimatorHook>();
            a_hook.Int(null, this);
            InitRagdoll();
        }

        private void InitRagdoll()
        {
            Rigidbody[] rigs = GetComponentsInChildren<Rigidbody>();
            for(int i =0; i<rigs.Length;i++)
            {
                if (rigs[i] == rib)
                    continue;
                ragdollRigids.Add(rigs[i]);
                rigs[i].isKinematic = true;

                Collider col = rigs[i].gameObject.GetComponent<Collider>();
                col.isTrigger = true;
                ragDollColiders.Add(col);

            }
        }

        public void EnableRagDoll()
        {

            for (int i = 0; i < ragdollRigids.Count; i++)
            {
                ragdollRigids[i].isKinematic = false;
                ragDollColiders[i].isTrigger = false;
            }
            Collider controllerColider = rib.gameObject.GetComponent<Collider>();
            controllerColider.enabled = false;
            rib.isKinematic = true;
            StartCoroutine("CloseAnimator");
        }

        private IEnumerator CloseAnimator()
        {
            yield return new WaitForEndOfFrame();
            anim.enabled = false;
            this.enabled = false;

        }
        private void Update()
        {
           
            delta = Time.deltaTime;
            canMove = !anim.GetBool("canMove");

            if(healt<=0)
            {
                if(!isDead)
                {
                    isDead = true;
                    EnableRagDoll();
                }
            }
            if (isInvicible)
            {
                isInvicible = !canMove;
               
            }
            if(canMove)
            {
                anim.applyRootMotion = true;
            }
               
           
        }
        public void DoDmg(float v)
        {
            if (isInvicible)
                return;
           
            healt -= v;
            isInvicible = true;
           
            anim.Play("damage 1");
            anim.applyRootMotion = true;


        }

    }
}

