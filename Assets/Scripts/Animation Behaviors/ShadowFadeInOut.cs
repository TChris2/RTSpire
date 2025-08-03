using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// Starts shadow fade in or out at the start of an animation
public class ShadowFadeInOut : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Gets ShadowFade script from object
        ShadowFade sFade = animator.GetComponent<ShadowFade>();
        // Starts loop to fade in or out shadow and sends animation length info
        sFade.FadeStart(stateInfo.length);
    }
}
