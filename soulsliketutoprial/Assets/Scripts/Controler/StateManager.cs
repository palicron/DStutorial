using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class StateManager : MonoBehaviour
    {

        [Header("Init")]
        public GameObject activeModel;
        [Header("Inputs")]
        public float vertical;
        public float horizontal;
        public float moveAmount;
        public Vector3 moveDir;
        public bool rt, lt, rb, lb;
        public bool twoHanded;
        [Header("stats")]
        public float moveSpeed = 2;
        public float runSpeed = 3.5f;
        public float rotateSpeed = 5;
        public float toGround = 0.5f;

       

        [Header("States")]
        public bool onGround;
        public bool run;
        public bool canMove;
        public bool lockOn;
        public bool inAction;
        public bool isTwoHanded;
        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public Rigidbody rib;
        [HideInInspector]
        public float delta;
        [HideInInspector]
        public LayerMask ignoreLayers;
        [HideInInspector]
        public AnimatorHook a_hook;
        private float _actionDelay;

        public void Init()
        {
            setUpAnimator();
            rib = this.GetComponent<Rigidbody>();
            rib.angularDrag = 999;
            rib.drag = 4;
            rib.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            a_hook = activeModel.AddComponent<AnimatorHook>();
            a_hook.Int(this);
            gameObject.layer = 8;
            ignoreLayers = ~(1 << 9);
            anim.SetBool("onGround", false);

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
            onGround = OnGround();
            anim.SetBool("onGround", onGround);
        }
        public void FixedTick(float d)
        {
            delta = d;


            
            DetectAction();

            if (inAction)
            {
                anim.applyRootMotion = true;
                _actionDelay += delta;
                if(_actionDelay>0.13f)
                {
                    inAction = false;
                    _actionDelay = 0;

                }
                return;
            }
            canMove = anim.GetBool("canMove");

            if (!canMove )
                   return;

            anim.applyRootMotion = false;
            if (moveAmount > 0 || !onGround)
            {
                rib.drag = 0;
            }
            else
            {
                rib.drag = 4;
            }
            float tspeed = moveSpeed;
            if (run)
                tspeed = runSpeed;

            if (onGround)
                rib.velocity = moveDir * (tspeed * moveAmount);
            if (run)
                lockOn = false;
            if(!lockOn)
            {

            Vector3 targetDir = moveDir;
            targetDir.y = 0;
            if (targetDir == Vector3.zero)
                targetDir = transform.forward;
            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, delta * moveAmount * rotateSpeed);
            transform.rotation = targetRotation;

            }

            HandleMovementAnimation();
        }
        public void DetectAction()
        {

            if (!canMove)
                return;
            if (!rb && !rt && !lt && !lb)
                return;
          
            string tagetAnimation=null;

            if (rb)
                tagetAnimation = "oh_attack_1";
            if (rt)
                tagetAnimation = "oh_attack_2";
            if (lt)
                tagetAnimation = "oh_attack_3";
            if (lb)
                tagetAnimation = "oh_attack_3";
            canMove = false;
               inAction = true;
               anim.CrossFade(tagetAnimation,0.14f);
           // rib.drag = 4;

        }
        public void HandleMovementAnimation()
        {
            anim.SetBool("run", run);
            anim.SetFloat("Vertical", moveAmount, 0.4f, delta);
        }
        public void HandleTwoHanded()
        {
            anim.SetBool("two_handed", isTwoHanded);
        }
        public bool OnGround()
        {
            bool r = false;

            Vector3 origin = transform.position + (Vector3.up * toGround);
            Vector3 dir = -Vector3.up;
            float dis = toGround + 0.3f;
            RaycastHit hit;
            if (Physics.Raycast(origin, dir, out hit, dis, ignoreLayers))
            {
                r = true;
                Vector3 tagetposition = hit.point;
                transform.position = tagetposition;
            }


            return r;
        }

    }

}
