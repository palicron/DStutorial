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
        [Header("stats")]
        public float moveSpeed = 2;
        public float runSpeed = 3.5f;
        public float rotateSpeed = 5;
        public float toGround = 0.5f;
        public bool lockOn;
        [Header("States")]
        public bool onGround;
        public bool run;
        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public Rigidbody rb;
        [HideInInspector]
        public float delta;
        [HideInInspector]
        public LayerMask ignoreLayers;

        public void Init()
        {
            setUpAnimator();
            rb = this.GetComponent<Rigidbody>();
            rb.angularDrag = 999;
            rb.drag = 4;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
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

            if (moveAmount > 0 || !onGround)
            {
                rb.drag = 0;
            }
            else
            {
                rb.drag = 4;
            }
            float tspeed = moveSpeed;
            if (run)
                tspeed = runSpeed;

            if (onGround)
                rb.velocity = moveDir * (tspeed * moveAmount);
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

        public void HandleMovementAnimation()
        {
            anim.SetBool("run", run);
            anim.SetFloat("Vertical", moveAmount, 0.4f, delta);
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
