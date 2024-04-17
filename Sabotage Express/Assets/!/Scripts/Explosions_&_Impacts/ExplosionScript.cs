using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {

	public float despawnTime = 10.0f;
	public float lightDuration = 0.02f;
	public Light lightFlash;

	public AudioClip[] explosionSounds;
	public AudioSource audioSource;

	private void Start () {
		StartCoroutine (DestroyTimer ());
		StartCoroutine (LightFlash ());

		audioSource.clip = explosionSounds
			[Random.Range(0, explosionSounds.Length)];
		audioSource.Play();
	}

	private IEnumerator LightFlash () {
		lightFlash.GetComponent<Light>().enabled = true;
		yield return new WaitForSeconds (lightDuration);
		lightFlash.GetComponent<Light>().enabled = false;
	}

	private IEnumerator DestroyTimer () {
		yield return new WaitForSeconds (despawnTime);
		Destroy (gameObject);
	}
}