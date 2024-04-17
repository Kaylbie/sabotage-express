using UnityEngine;
using System.Collections;

public class PlaySoundScript : StateMachineBehaviour {

	public AudioClip soundClip;

	override public void OnStateEnter
		(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		animator.gameObject.GetComponent<AudioSource> ().clip = soundClip;
		animator.gameObject.GetComponent<AudioSource> ().Play ();
	}
}