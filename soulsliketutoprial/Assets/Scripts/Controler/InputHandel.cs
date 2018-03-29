using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{ 
public class InputHandel : MonoBehaviour {

        [SerializeField]
        private float horizontal;
        [SerializeField]
        private float vertical;

        StateManager states;
        CamaraManager camaraManager;

        private float delta;
	// Use this for initialization
	void Start () {
            states = this.GetComponent<StateManager>();
            states.Init();

            camaraManager = CamaraManager.singleton;
            camaraManager.Init(this.transform);
    }

        private void FixedUpdate()
        {
            delta = Time.fixedDeltaTime;
            getInput();
        }

        private void Update()
        {
            delta = Time.deltaTime;
            camaraManager.Tick(delta);
        }

        private void getInput()
        {
            vertical = Input.GetAxis("Vertical");
            horizontal = Input.GetAxis("Horizontal");
        }

        private void UpdateStates()
        {
            states.horizontal = horizontal;
            states.vertical = vertical;
            states.Tick(delta);
        }
    }
}
