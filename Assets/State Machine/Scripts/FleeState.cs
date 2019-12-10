using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : State {

    Transform destination;
    GameObject fleeTo;

    public FleeState(StateController stateController) : base(stateController) { }

    public override void CheckTransitions()
    {
        if (!stateController.CheckIfInRange("Player"))
        {
            stateController.SetState(new PatrolState(stateController));
        }
    }
    public override void Act()
    {
        if (stateController.enemyToFlee != null)
        {
            if (GameObject.FindGameObjectsWithTag("flee") != null)
            {
                foreach (GameObject f in GameObject.FindGameObjectsWithTag("flee"))
                {
                    stateController.DestroyObject(f);
                }
            }

            fleeTo = new GameObject();
            fleeTo.tag = "flee";

            fleeTo.transform.position = (stateController.enemyToFlee.transform.position + stateController.transform.position) * 2;
            stateController.ai.SetTarget(fleeTo.transform);
        }
    }
    public override void OnStateEnter()
    {
        
        stateController.ChangeColor(Color.yellow);
        stateController.ai.agent.speed = 1.5f;
    }
}
