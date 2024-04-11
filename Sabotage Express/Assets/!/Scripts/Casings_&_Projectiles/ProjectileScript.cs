using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

	private bool explodeSelf;
	public bool useConstantForce;
	public float constantForceSpeed;
	public float explodeAfter;
	private bool hasStartedExplode;

	private bool hasCollided;

	public Transform explosionPrefab;

	public float force = 5000f;
	public float despawnTime = 30f;

	public float radius = 50.0F;
	public float power = 250.0F;

	public bool usesParticles;
	public ParticleSystem smokeParticles;
	public ParticleSystem flameParticles;
	public float destroyDelay;
	public int rocketDamage = 100;

	private void Start () 
	{
		if (!useConstantForce) 
		{
			GetComponent<Rigidbody> ().AddForce 
				(gameObject.transform.forward * force);
		}

		StartCoroutine (DestroyTimer ());
	}
		
	private void FixedUpdate()
	{
		if(GetComponent<Rigidbody>().velocity != Vector3.zero)
			GetComponent<Rigidbody>().rotation = 
				Quaternion.LookRotation(GetComponent<Rigidbody>().velocity);  

		if (useConstantForce == true && !hasStartedExplode) {
			GetComponent<Rigidbody>().AddForce 
				(gameObject.transform.forward * constantForceSpeed);

			StartCoroutine (ExplodeSelf ());

			hasStartedExplode = true;
		}
	}

	private IEnumerator ExplodeSelf () 
	{
		yield return new WaitForSeconds (explodeAfter);
		if (!hasCollided) {
			Instantiate (explosionPrefab, transform.position, transform.rotation);
		}
		gameObject.GetComponent<MeshRenderer> ().enabled = false;
		gameObject.GetComponent<Rigidbody> ().isKinematic = true;
		gameObject.GetComponent<BoxCollider>().isTrigger = true;
		if (usesParticles == true) {
			flameParticles.GetComponent <ParticleSystem> ().Stop ();
			smokeParticles.GetComponent<ParticleSystem> ().Stop ();
		}
		yield return new WaitForSeconds (destroyDelay);
		Destroy (gameObject);
	}

	private IEnumerator DestroyTimer () 
	{
		yield return new WaitForSeconds (despawnTime);
		Destroy (gameObject);
	}

	private IEnumerator DestroyTimerAfterCollision () 
	{
		yield return new WaitForSeconds (destroyDelay);
		Destroy (gameObject);
	}

	private void OnCollisionEnter (Collision collision) 
	{
		if (collision.gameObject.tag == "Enemy")
		{
			EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
			if (enemyHealth != null)
			{
				enemyHealth.TakeDamage(rocketDamage);
			
			}
			
			Destroy(gameObject);
		}
		if (collision.gameObject.tag == "Player" ) 
		{
			Debug.LogWarning("Collides with player");

			Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
			return;
		}
		if (collision.gameObject.CompareTag("Gun") ) {
			Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
			return;
		}
		// if (collision.gameObject.layer == LayerMask.NameToLayer("InvisibleToSelf")) {
		// 	// Ignore the collision
		// 	Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
		// 	return;
		// }
		
		hasCollided = true;

		gameObject.GetComponent<MeshRenderer> ().enabled = false;
		gameObject.GetComponent<Rigidbody> ().isKinematic = true;
		gameObject.GetComponent<BoxCollider>().isTrigger = true;

		if (usesParticles == true) {
			flameParticles.GetComponent <ParticleSystem> ().Stop ();
			smokeParticles.GetComponent<ParticleSystem> ().Stop ();
		}
		
		StartCoroutine (DestroyTimerAfterCollision ());

		Instantiate(explosionPrefab,collision.contacts[0].point,
			Quaternion.LookRotation(collision.contacts[0].normal));

		if (collision.gameObject.tag == "Target" && 
		    	collision.gameObject.GetComponent<TargetScript>().isHit == false) {
			
			Instantiate(explosionPrefab,collision.contacts[0].point,
			            Quaternion.LookRotation(collision.contacts[0].normal));

			collision.gameObject.transform.gameObject.GetComponent
				<Animation> ().Play("target_down");
			collision.gameObject.transform.gameObject.GetComponent
				<TargetScript>().isHit = true;
		}

		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
		foreach (Collider hit in colliders) {
			Rigidbody rb = hit.GetComponent<Rigidbody> ();

			if (rb != null)
				rb.AddExplosionForce (power * 50, explosionPos, radius, 3.0F);

			if (hit.GetComponent<Collider>().tag == "Target" && 
			    	hit.GetComponent<TargetScript>().isHit == false) {

				hit.gameObject.GetComponent<Animation> ().Play("target_down");
				hit.gameObject.GetComponent<TargetScript>().isHit = true;
			}

			if (hit.transform.tag == "ExplosiveBarrel") {
				
				hit.transform.gameObject.GetComponent<ExplosiveBarrelScript>().explode = true;
			}

			if (hit.GetComponent<Collider>().tag == "GasTank") 
			{
				hit.gameObject.GetComponent<GasTankScript> ().isHit = true;
				hit.gameObject.GetComponent<GasTankScript> ().explosionTimer = 0.05f;
			}
		}
	}
}