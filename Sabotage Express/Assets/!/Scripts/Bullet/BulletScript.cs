using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	[Range(5, 100)]
	public float destroyAfter;
	public bool destroyOnImpact = false;
	public float minDestroyTime;
	public float maxDestroyTime;

	public Transform [] bloodImpactPrefabs;
	public Transform [] metalImpactPrefabs;
	public Transform [] dirtImpactPrefabs;
	public Transform []	concreteImpactPrefabs;

	public int bulletDamage = 25;

	private void Start () 
	{
		//Start destroy timer
		StartCoroutine (DestroyAfter ());
	}

	//If the bullet collides with anything
	private void OnCollisionEnter (Collision collision) 
	{
		
		if (collision.gameObject.tag == "Enemy")
		{
			EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
			if (enemyHealth != null)
			{
				enemyHealth.TakeDamageServerRpc(bulletDamage);
			
			}
			Instantiate (bloodImpactPrefabs [Random.Range 
					(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
			//Destroy bullet object
			Destroy(gameObject);
		}
		
		if (collision.gameObject.tag == "Player" ) 
		{
			//Physics.IgnoreCollision (collision.collider);
			Debug.LogWarning("Collides with player");

			Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
			return;
		}
		if (collision.gameObject.CompareTag("Gun")) {
			// Ignore 
			Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
			return;
		}
		//Debug.LogWarning("Collides with something"+collision.gameObject.name);
		if (!destroyOnImpact) 
		{
			StartCoroutine (DestroyTimer ());
		}
		//destroy bullet on impact
		else 
		{
			Destroy (gameObject);
		}



		if (collision.transform.tag == "Metal") 
		{
			Instantiate (metalImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
			Destroy(gameObject);
		}

		if (collision.transform.tag == "Dirt") 
		{
			Instantiate (dirtImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
			Destroy(gameObject);
		}

		if (collision.transform.tag == "Concrete") 
		{
			Instantiate (concreteImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
			Destroy(gameObject);
		}

		if (collision.transform.tag == "Target") 
		{
			collision.transform.gameObject.GetComponent
				<TargetScript>().isHit = true;
			Destroy(gameObject);
		}
			
		if (collision.transform.tag == "ExplosiveBarrel") 
		{
			collision.transform.gameObject.GetComponent
				<ExplosiveBarrelScript>().explode = true;
			Destroy(gameObject);
		}

		if (collision.transform.tag == "GasTank") 
		{
			collision.transform.gameObject.GetComponent
				<GasTankScript> ().isHit = true;
			Destroy(gameObject);
		}
	}

	private IEnumerator DestroyTimer () 
	{
		yield return new WaitForSeconds
			(Random.Range(minDestroyTime, maxDestroyTime));
		Destroy(gameObject);
	}

	private IEnumerator DestroyAfter () 
	{
		yield return new WaitForSeconds (destroyAfter);
		Destroy (gameObject);
	}
}