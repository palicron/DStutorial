using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Helper : MonoBehaviour
    {
        [Range(-1, 1)]
        public float vertical;
        [Range(-1, 1)]
        public float horizontal;
        Animator anim;
        public string[] oh_attacks;
        public string[] th_attacks;
        public bool playAnim;
        public bool twoHanded;
        public bool enableRM;
        public bool useItem;
        public bool interancting;
        public bool lockOn;
        // Use this for initialization
        void Start()
        {
            this.anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
           

            enableRM = !anim.GetBool("canMove");
            anim.applyRootMotion = enableRM;
            interancting = anim.GetBool("interacting");
            anim.SetBool("two_handed", twoHanded);
            anim.SetBool("lockOn", lockOn);
            if (enableRM)
                return;

            if(!lockOn)
            {
                horizontal = 0;
                vertical = Mathf.Clamp01(vertical);
            }
            if (useItem)
            {
               // playAnim = false;
               // twoHanded = false;
                anim.CrossFade("use_item", 0.8f);
                useItem = false;
            }
            if(interancting)
            {
                playAnim = false;
                vertical = Mathf.Clamp(vertical, 0, 0.5f);
            }
            if (playAnim)
            {
                string targetAnim;
                if(twoHanded)
                {
                    int r = Random.Range(0, th_attacks.Length);
                    targetAnim = th_attacks[r];
                }
                else
                {
                    int r = Random.Range(0, oh_attacks.Length);
                    targetAnim = oh_attacks[r];
                    if(vertical >0.5f)
                    {
                        targetAnim = "oh_attack_3";
                    }
                }
                vertical = 0;
                anim.CrossFade(targetAnim, 0.2f);
              
                playAnim = false;
            }
            anim.SetFloat("Vertical", vertical);
            anim.SetFloat("Horizontal", horizontal);
        }
    }
}

