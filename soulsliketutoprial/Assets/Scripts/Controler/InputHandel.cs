using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class InputHandel : MonoBehaviour
    {

        private float horizontal;
        private float vertical;
        private bool b_input;
        private bool a_input;
        private bool x_input;
        private bool y_input;

        private bool rb_input;
        private float rt_axis;
        private bool rt_input;
        private bool lb_input;
        private float lt_axis;
        private bool lt_input;
        private bool leftAxis_down;
        private bool rightAxis_down;

        private float b_timer;
        private float rt_timer;
        private float lt_timer;

        StateManager states;
        CamaraManager camaraManager;

        private float delta;
        // Use this for initialization
        void Start()
        {
            states = this.GetComponent<StateManager>();
            states.Init();

            camaraManager = CamaraManager.singleton;
            camaraManager.Init(states);

        }

        private void FixedUpdate()
        {

            delta = Time.fixedDeltaTime;
            getInput();
            UpdateStates();
            states.FixedTick(delta);
            camaraManager.Tick(delta);



        }

        private void Update()
        {
            delta = Time.deltaTime;
            states.Tick(delta);
            ResetInputs();

        }

        private void getInput()
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
            b_input = Input.GetButton("B");
            a_input = Input.GetButton("A");
            x_input = Input.GetButton("X");
            y_input = Input.GetButtonUp("Y");
            rt_input = Input.GetButton("RT");
            rt_axis = Input.GetAxis("RT");
            if (rt_axis != 0)
                rt_input = true;

            lt_input = Input.GetButton("LT");
            lt_axis = Input.GetAxis("LT");
            if (lt_axis != 0)
                lt_input = true;
            rb_input = Input.GetButton("RB");
            lb_input = Input.GetButton("LB");

            rightAxis_down = Input.GetButtonUp("L");

            if (b_input)
                b_timer += delta;


        }

        private void UpdateStates()
        {
            states.horizontal = horizontal;
            states.vertical = vertical;

            Vector3 v = vertical * camaraManager.transform.forward;
            Vector3 h = horizontal * camaraManager.transform.right;

            states.moveDir = (v + h).normalized;
            float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
            states.moveAmount = Mathf.Clamp01(m);

            if (x_input)
                b_input = false;



            if (b_input && b_timer > 0.5f)
            {
                states.run = (states.moveAmount > 0);

            }
            //else
            //{
            //    states.run = false;
            //}

            if (!b_input && b_timer > 0 && b_timer < 0.5f)
                states.rollinput = true;

            states.itemInput = x_input;
            states.rt = rt_input;
            states.lt = lt_input;
            states.rb = rb_input;
            states.lb = lb_input;
            if (y_input)
            {
                states.isTwoHanded = !states.isTwoHanded;
                states.HandleTwoHanded();
            }
            if(states.lockOnTarget != null)
            {
                if (states.lockOnTarget.eStates.isDead)
                {
                    states.lockOn = false;
                    states.lockOnTarget = null;
                    states.lockOnTranform = null;
                    camaraManager.lockOn = false;
                    camaraManager.lockonTarget = null;
                }
            }
     
            if (rightAxis_down)
            {
                states.lockOn = !states.lockOn;
                if (states.lockOnTarget == null)
                {
                    states.lockOn = false;
                }
           
                    
                camaraManager.lockonTarget = states.lockOnTarget;
                states.lockOnTranform = camaraManager.lockOnTransform;
                camaraManager.lockOn = states.lockOn;

            }

        }

        private void ResetInputs()
        {
            if (!b_input)
                b_timer = 0;

            if (states.rollinput)
                states.rollinput = false;

            if (states.run)
                states.run = false;
        }
    }
}
