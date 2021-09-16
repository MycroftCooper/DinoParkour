using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCallBack : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject.Find("GameController").GetComponent<SoundsController>().PlayVoice("Sounds/Jump");
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject.Find("DinoPrefab").GetComponent<DinoController>().State = DinoController.DinoState.Run;
    }
}
