using UnityEngine;
using System.Collections;

public class GrenadeScript : MonoBehaviour {

	public float grenadeTimer = 5.0f;

	public Transform explosionPrefab;

	public float radius = 25.0F;
	public float power = 350.0F;

	public float minimumForce = 1500.0f;
	public float maximumForce = 2500.0f;
	private float throwForce;

	public AudioSource impactSound;

	private void Awake () 
	{
		
		throwForce = Random.Range
			(minimumForce, maximumForce);

		GetComponent<Rigidbody>().AddRelativeTorque 
		   (Random.Range(500, 1500), //X 
			Random.Range(0,0), 		 //Y 
			Random.Range(0,0)  		 //Z 
			* Time.deltaTime * 5000);
	}

	private void Start () 
	{
		GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * throwForce);

		StartCoroutine (ExplosionTimer ());
	}

	private void OnCollisionEnter (Collision collision) 
	{
		impactSound.Play ();
	}

	private IEnumerator ExplosionTimer () 
	{
		yield return new WaitForSeconds(grenadeTimer);

		RaycastHit checkGround;
		if (Physics.Raycast(transform.position, Vector3.down, out checkGround, 50))
		{
			Instantiate (explosionPrefab, checkGround.point, 
				Quaternion.FromToRotation (Vector3.forward, checkGround.normal)); 
		}

		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
		foreach (Collider hit in colliders) {
			Rigidbody rb = hit.GetComponent<Rigidbody> ();

			if (rb != null)
				rb.AddExplosionForce (power * 5, explosionPos, radius, 3.0F);
			
			if (hit.GetComponent<Collider>().tag == "Target" 
			    	&& hit.gameObject.GetComponent<TargetScript>().isHit == false) 
			{
				hit.gameObject.GetComponent<Animation> ().Play("target_down");
				hit.gameObject.GetComponent<TargetScript>().isHit = true;
			}

			if (hit.GetComponent<Collider>().tag == "ExplosiveBarrel") 
			{
				hit.gameObject.GetComponent<ExplosiveBarrelScript> ().explode = true;
			}

			if (hit.GetComponent<Collider>().tag == "GasTank") 
			{
				hit.gameObject.GetComponent<GasTankScript> ().isHit = true;
				hit.gameObject.GetComponent<GasTankScript> ().explosionTimer = 0.05f;
			}
		}

		Destroy (gameObject);
	}
}