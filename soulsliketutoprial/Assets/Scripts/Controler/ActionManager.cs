using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

    public class ActionManager : MonoBehaviour
    {

        public List<Action> actionSlots = new List<Action>();
        StateManager states;
        public void Init(StateManager st)
        {
            states = st;

            UpdateActionsOneHanded();
        }

        public void UpdateActionsOneHanded()
        {
            Weapon w = states.inventoryManager.curWeapon;


            for (int i = 0; i < w.actions.Count; i++)
            {
                Action a = GetAction(w.actions[i].input);
                a.targetAnim = w.actions[i].targetAnim;
            }
        }

        public void UpdateActionsTwoHanded()
        {
            Weapon w = states.inventoryManager.curWeapon;


            for (int i = 0; i < w.two_HandedAction.Count; i++)
            {
                Action a = GetAction(w.two_HandedAction[i].input);
                a.targetAnim = w.two_HandedAction[i].targetAnim;
            }
        }
        ActionManager()
        {
            for (int i = 0; i < 4; i++)
            {
                Action a = new Action();
                a.input = (ActionInput)i;
                actionSlots.Add(a);

            }
        }

        public Action GetActionSlot(StateManager st)
        {
            ActionInput a_input = GetActionInput(st);
            return GetAction(a_input);
        }

        public Action GetAction(ActionInput inp)
        {

            for (int i = 0; i < actionSlots.Count; i++)
            {
                if (actionSlots[i].input == inp)
                {
                    return actionSlots[i];
                }


            }

            return null;
        }
        public ActionInput GetActionInput(StateManager st)
        {


            if (st.rb)
                return ActionInput.rb;
            if (st.lb)
                return ActionInput.lb;
            if (st.rt)
                return ActionInput.rt;
            if (st.lt)
                return ActionInput.lt;

            return ActionInput.rb;

        }


    }
    public enum ActionInput
    {
        rb, lb, rt, lt
    }
    [System.Serializable]
    public class Action
    {
        public ActionInput input;
        public string targetAnim;
    }


}
