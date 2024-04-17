using UnityEngine;
using System.Collections;

public class TargetScript : MonoBehaviour {

	float randomTime;
	bool routineStarted = false;

	public bool isHit = false;

	public float minTime;
	public float maxTime;

	public AudioClip upSound;
	public AudioClip downSound;

	public AudioSource audioSource;
	
	private void Update () {
		
		randomTime = Random.Range (minTime, maxTime);

		if (isHit == true) 
		{
			if (routineStarted == false) 
			{
				gameObject.GetComponent<Animation> ().Play("target_down");

				audioSource.GetComponent<AudioSource>().clip = downSound;
				audioSource.Play();

				StartCoroutine(DelayTimer());
				routineStarted = true;
			} 
		}
	}

	//Time before the target pops back up
	private IEnumerator DelayTimer () {
		//Wait for random amount of time
		yield return new WaitForSeconds(randomTime);
		//Animate the target "up" 
		gameObject.GetComponent<Animation> ().Play ("target_up");

		//Set the upSound as current sound, and play it
		audioSource.GetComponent<AudioSource>().clip = upSound;
		audioSource.Play();

		//Target is no longer hit
		isHit = false;
		routineStarted = false;
	}
}