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
        public bool rollinput;
        public bool itemInput;
        [Header("stats")]
        public float moveSpeed = 2;
        public float runSpeed = 3.5f;
        public float rotateSpeed = 5;
        public float toGround = 0.5f;
        public float rollSpeed = 1f;

        [Header("States")]
        public bool onGround;
        public bool run;
        public bool canMove;
        public bool lockOn;
        public bool inAction;
        public bool isTwoHanded;
        public bool usingItem;


        [Header("Other")]
        public EnemyTarget lockOnTarget;
        public Transform lockOnTranform;
        public AnimationCurve roll_curve;

        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public Rigidbody rib;
        [HideInInspector]
        public ActionManager actionManager;
        [HideInInspector]
        public float delta;
        [HideInInspector]
        public LayerMask ignoreLayers;
        [HideInInspector]
        public AnimatorHook a_hook;
        [HideInInspector]
        public InventoryManager inventoryManager;
        private float _actionDelay;

        public void Init()
        {
            setUpAnimator();
            rib = this.GetComponent<Rigidbody>();
            rib.angularDrag = 999;
            rib.drag = 4;
            rib.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            inventoryManager = GetComponent<InventoryManager>();
            inventoryManager.Init(this);
            actionManager = GetComponent<ActionManager>();
            actionManager.Init(this);

            a_hook = activeModel.GetComponent<AnimatorHook>();

            if (a_hook == null)
                a_hook = activeModel.AddComponent<AnimatorHook>();

            a_hook.Int(this, null);
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

            usingItem = anim.GetBool("interacting");

            DetectAction();
            DetecItemAction();
            inventoryManager.rightHandWeapon.weaponModel.SetActive(!usingItem);
            if (inAction)
            {
                anim.applyRootMotion = true;
                _actionDelay += delta;
                if (_actionDelay > 0.13f)
                {
                    inAction = false;
                    _actionDelay = 0;

                }
                return;
            }
            canMove = anim.GetBool("canMove");

            if (!canMove)
                return;
            //a_hook.rm_multi = 1;
            a_hook.CloseRoll();
            handleRolls();
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
            if (usingItem)
            {
                run = false;
                moveAmount = Mathf.Clamp(moveAmount, 0, 0.5f);
            }
            if (run)
                tspeed = runSpeed;

            if (onGround)
                rib.velocity = moveDir * (tspeed * moveAmount);
            if (run)
                lockOn = false;


            Vector3 targetDir = (!lockOn) ? moveDir :
                (lockOnTranform != null) ? lockOnTranform.transform.position - transform.position :
                moveDir;
            targetDir.y = 0;
            if (targetDir == Vector3.zero)
                targetDir = transform.forward;
            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, delta * moveAmount * rotateSpeed);
            transform.rotation = targetRotation;

            anim.SetBool("lockOn", lockOn);
            if (!lockOn)
                HandleMovementAnimation();
            else
                HandleLockOnAnimation(moveDir);
        }
        public void DetectAction()
        {

            if (!canMove || usingItem)
                return;
            if (!rb && !rt && !lt && !lb)
                return;

            string tagetAnimation = null;
           
            Action slot = actionManager.GetActionSlot(this);
            if (slot == null)
                return;
            tagetAnimation = slot.targetAnim;
            canMove = false;
            inAction = true;
            anim.SetBool("mirror", slot.mirror);
            anim.CrossFade(tagetAnimation, 0.14f);
            // rib.drag = 4;

        }
        public void DetecItemAction()
        {
            if (!canMove || usingItem)
                return;
            if (!itemInput)
                return;

            ItemAction slot = actionManager.consumableItem;

            string tagetAnimation = slot.targetanim;

            // inventoryManager.curWeapon.weaponModel.SetActive(false);
            usingItem = true;
            anim.Play(tagetAnimation);
        }
        public void HandleMovementAnimation()
        {
            anim.SetBool("run", run);
            anim.SetFloat("Vertical", moveAmount, 0.4f, delta);
        }
        public void HandleLockOnAnimation(Vector3 moveDir)
        {
            Vector3 relativeDir = transform.InverseTransformDirection(moveDir);
            float h = relativeDir.x;
            float v = relativeDir.z;
            anim.SetFloat("Vertical", v, 0.2f, delta);
            anim.SetFloat("Horizontal", h, 0.2f, delta);
        }
        private void handleRolls()
        {
            if (!rollinput || usingItem)
                return;


            float v = vertical;
            float h = horizontal;
            v = (moveAmount > 0.3f) ? 1 : 0;
            h = 0;
            /*  if (!lockOn)
              {
                  v = (moveAmount>0.3f)?1:0;
                  h = 0;
              }
              else
              {

                  if(Mathf.Abs(v)<0.3f)
                      v = 0;
                  if (Mathf.Abs(h) < 0.3f)
                      h = 0;
              }*/
            if (v != 0)
            {
                if (moveDir == Vector3.zero)
                    moveDir = transform.forward;
                Quaternion targetRot = Quaternion.LookRotation(moveDir);
                transform.rotation = targetRot;
                a_hook.InitForRoll();
                a_hook.rm_multi = rollSpeed;
            }
            else
            {
                a_hook.rm_multi = 1.3f;
            }

            anim.SetFloat("Vertical", v);
            anim.SetFloat("Horizontal", h);
            canMove = false;
            inAction = true;
            anim.CrossFade("Rolls", 0.14f);

        }
        public void HandleTwoHanded()
        {
            anim.SetBool("two_handed", isTwoHanded);
            if (isTwoHanded)
                actionManager.UpdateActionsTwoHanded();
            else
                actionManager.UpdateActionsOneHanded();
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
                tagetposition.y = tagetposition.y + 0.04512596f;
                transform.position = tagetposition;
            }


            return r;
        }

    }

}
