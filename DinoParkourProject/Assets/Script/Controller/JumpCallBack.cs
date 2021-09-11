using UnityEngine;

public class JumpCallBack : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject.Find("GameController").GetComponent<SoundsController>().PlayVoice("Sounds/Jump");
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int stateIndex = animator.GetInteger("DinoState");
        if (stateIndex != 1 && stateIndex != 4)
            GameObject.Find("DinoPrefab").GetComponent<DinoController>().State = DinoController.DinoState.Run;
    }
}
