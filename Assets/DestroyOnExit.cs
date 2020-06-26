using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnExit : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FindObjectOfType<EngineScript>().GetService<SpawnService>().DestroyObject(animator.gameObject);
        //Destroy(animator.gameObject, stateInfo.length);
    }
}
