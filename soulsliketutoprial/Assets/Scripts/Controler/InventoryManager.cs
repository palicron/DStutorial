﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class InventoryManager : MonoBehaviour
    {
        public Weapon rightHandWeapon;
        public bool hasLeftHandWepaon = true;
        public Weapon leftHandWeapon;
        private StateManager states;

        public GameObject parryColider;
        public void Init(StateManager st)
        {
            states = st;
            EquipWeapon(rightHandWeapon, false);
            EquipWeapon(rightHandWeapon, true);
            CloseAllDamageColliders();
            CloseParryCollider();
        }

        public void EquipWeapon(Weapon w, bool isLeft =false)
        {
            string targetIdle = w.oh_idle;
            targetIdle += (isLeft) ? "_l" : "_r";
            states.anim.SetBool("mirror", isLeft);
            states.anim.Play("changeWeapon");
            states.anim.Play(targetIdle);
        }
  
        public void OpenAllDamageColliders()
        {
            if (rightHandWeapon.w_hook != null)
                rightHandWeapon.w_hook.OpenDamageColliders();
            if (leftHandWeapon.w_hook != null)
                leftHandWeapon.w_hook.OpenDamageColliders();
        }

        public void CloseAllDamageColliders()
        {
            if (rightHandWeapon.w_hook != null)
                rightHandWeapon.w_hook.CloseDamageColliders();
            if (leftHandWeapon.w_hook != null)
                leftHandWeapon.w_hook.CloseDamageColliders();
        }

        public void CloseParryCollider()
        {
            parryColider.SetActive(false);
        }
        public void OpenParryCollider()
        {
            parryColider.SetActive(false);
        }
    }


 
    [System.Serializable]
    public class Weapon
    {
        public string oh_idle;
        public string th_idle;
        public List<Action> actions;
        public List<Action> two_HandedAction;
        public bool leftHandMirror;
        public GameObject weaponModel;
        public WeaponHook w_hook;

        public Action GetAction(List<Action> l,ActionInput inp)
        {
            for (int i = 0; i < l.Count;i++)
            {
                if(l[i].input==inp)
                {
                    return l[i];
                }
            }
            return null;
            }
        }
    }

