using UnityEngine;
using System.Collections;

public class CasingScript : MonoBehaviour {

	public float minimumXForce;		
	public float maximumXForce;
	public float minimumYForce;
	public float maximumYForce;
	public float minimumZForce;
	public float maximumZForce;
	public float minimumRotation;
	public float maximumRotation;
	public float despawnTime;

	public AudioClip[] casingSounds;
	public AudioSource audioSource;

	public float speed = 2500.0f;

	private void Awake () 
	{
		GetComponent<Rigidbody>().AddRelativeTorque (
			Random.Range(minimumRotation, maximumRotation), //X Axis
			Random.Range(minimumRotation, maximumRotation), //Y Axis
			Random.Range(minimumRotation, maximumRotation)  //Z Axis
			* Time.deltaTime);

		GetComponent<Rigidbody>().AddRelativeForce (
			Random.Range (minimumXForce, maximumXForce),  //X Axis
			Random.Range (minimumYForce, maximumYForce),  //Y Axis
			Random.Range (minimumZForce, maximumZForce)); //Z Axis		     
	}

	private void Start () 
	{
		StartCoroutine (RemoveCasing ());
		transform.rotation = Random.rotation;
		StartCoroutine (PlaySound ());
	}

	private void FixedUpdate () 
	{
		transform.Rotate (Vector3.right, speed * Time.deltaTime);
		transform.Rotate (Vector3.down, speed * Time.deltaTime);
	}

	private IEnumerator PlaySound () 
	{
		yield return new WaitForSeconds (Random.Range(0.25f, 0.85f));
		audioSource.clip = casingSounds
			[Random.Range(0, casingSounds.Length)];
		audioSource.Play();
	}

	private IEnumerator RemoveCasing () 
	{
		yield return new WaitForSeconds (despawnTime);
		Destroy (gameObject);
	}
}