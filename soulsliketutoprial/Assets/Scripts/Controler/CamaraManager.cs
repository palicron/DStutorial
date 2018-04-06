using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{

    public class CamaraManager : MonoBehaviour
    {

        public static CamaraManager singleton;

        public bool lockOn;
        public float followSpeed = 9;
        public float mouseSpeed = 2;
        public float controllerSpeed = 7;
        public Transform target;
        public EnemyTarget lockonTarget;
        public Transform lockOnTransform;
        [HideInInspector]
        public Transform pivot;
        [HideInInspector]
        public Transform camTrans;
        private float turnSmoothing = 1f;
        public float minAngle = -35;
        public float maxAngle = 35;
        private float smoothX;
        private float smoothY;
        private float smoothSpeddX;
        private float smoothSpeddY;

        private bool usedRightAxis;
        private StateManager states;
        [SerializeField]
        private float lookAngle;
        [SerializeField]
        private float tiltAngle;

        private void Awake()
        {
            singleton = this;


        }


        public void Init(StateManager st)
        {
            states = st;
            target = states.transform;

            camTrans = Camera.main.transform;
            pivot = camTrans.parent;
        }
        public void Tick(float d)
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");

            float c_h = Input.GetAxis("RightAxis X");
            float c_v = Input.GetAxis("RightAxis Y");

            if(lockonTarget!=null)
            {
                if (lockOnTransform == null)
                {
                    lockOnTransform = lockonTarget.GetTarget();
                    states.lockOnTranform = lockOnTransform;
                }
                if(Mathf.Abs(c_h)>0.6f)
                {
                    if (!usedRightAxis)
                    {
                        lockOnTransform = lockonTarget.GetTarget((c_h>0));
                        states.lockOnTranform = lockOnTransform;
                        usedRightAxis = true;
                    }
           
                }
            }
       
            if(usedRightAxis)
            {
                if(Mathf.Abs(c_h)<0.6f)
                {
                    usedRightAxis = false;
                }
            }
            float targetspeed = mouseSpeed;
            if (c_h != 0 || c_v != 0)
            {
                h = c_h;
                v = c_v;
                targetspeed = controllerSpeed;
            }

            FollowTarget(d);
            HandleRotations(d, v, h, targetspeed);
        }
        private void FollowTarget(float d)
        {
            float speed = d * followSpeed;
            Vector3 tp = Vector3.Lerp(transform.position, target.position, followSpeed * speed);
            transform.position = tp;

        }
        private void HandleRotations(float d, float v, float h, float targetSpeed)
        {
            if (turnSmoothing > 0)
            {
                smoothX = Mathf.SmoothDamp(smoothX, h, ref smoothSpeddX, turnSmoothing);
                smoothY = Mathf.SmoothDamp(smoothY, v, ref smoothSpeddY, turnSmoothing);
            }
            else
            {
                smoothX = h;
                smoothY = v;
            }

            tiltAngle -= smoothY * targetSpeed;
            tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);
            pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);

           

            if (lockOn && lockonTarget !=null)
            {
                Vector3 targetDir = lockOnTransform.position - transform.position;
                targetDir.Normalize();
                //targetDir.y = 0;
                if (targetDir == Vector3.zero)
                    targetDir = transform.forward;
                Quaternion targetRot = Quaternion.LookRotation(targetDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, d*9);
                lookAngle = transform.eulerAngles.y;
                return;
            }
       
            lookAngle += smoothX * targetSpeed;
            transform.rotation = Quaternion.Euler(0, lookAngle, 0);

          
        }

    }

}
