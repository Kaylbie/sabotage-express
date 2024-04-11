using UnityEngine;
using System.Collections;

public class DebrisScript : MonoBehaviour {

	public AudioClip[] debrisSounds;
	public AudioSource audioSource;

	private void OnCollisionEnter (Collision collision) {
		if (collision.relativeVelocity.magnitude > 50) 
		{
			audioSource.clip = debrisSounds
				[Random.Range (0, debrisSounds.Length)];
			audioSource.Play ();
		}
	}
}