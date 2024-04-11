using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RocketLauncherScriptLPFP : MonoBehaviour {

	
	Animator anim;

	
	public Camera gunCamera;

	
	public float fovSpeed = 15.0f;
	
	public float defaultFov = 40.0f;

	public float aimFov = 18.0f;

	
	public string weaponName;

	
	[Space(10)]
	public SkinnedMeshRenderer projectileRenderer;

	
	public bool weaponSway;

	public float swayAmount;
	public float maxSwayAmount;
	public float swaySmoothValue;

	private Vector3 initialSwayPosition;

	

	public float autoReloadDelay;
	public float showProjectileDelay;

	
	private bool isReloading;

	
	//private bool hasBeenHolstered = false;
	
	private bool holstered;
	
	private bool isRunning;
	
	private bool isAiming;
	
	private bool isWalking;
	
	private bool isInspecting;

	
	private int currentAmmo;
	
	private int ammo = 1;
	
	private bool outOfAmmo;

	
	public float grenadeSpawnDelay;
	
	public bool randomMuzzleflash = false;
	private int minRandomValue = 1;

	[Range(2, 25)]
	public int maxRandomValue = 5;

	private int randomMuzzleflashValue;

	public bool enableMuzzleFlash;
	public ParticleSystem muzzleParticles;
	public bool enableSparks;
	public ParticleSystem sparkParticles;
	public int minSparkEmission = 1;
	public int maxSparkEmission = 7;

	public Light muzzleFlashLight;
	public float lightDuration = 0.02f;

	public AudioSource mainAudioSource;
	public AudioSource shootAudioSource;

	public Text timescaleText;
	public Text currentWeaponText;
	public Text currentAmmoText;
	public Text totalAmmoText;

	[System.Serializable]
	public class prefabs
	{  
		public Transform projectilePrefab;
		public Transform grenadePrefab;
	}
	public prefabs Prefabs;
	
	
	private Transform grenadeSpawnPoint;
	private Transform bulletSpawnPoint;
	


	[System.Serializable]
	public class soundClips
	{
		public AudioClip shootSound;
		public AudioClip takeOutSound;
		public AudioClip holsterSound;
		public AudioClip reloadSound;
		public AudioClip aimSound;
	}
	public soundClips SoundClips;

	private bool soundHasPlayed = false;

	private void Awake () 
	{
		anim = GetComponent<Animator>();
		currentAmmo = ammo;

		muzzleFlashLight.enabled = false;
	}
	private ItemSpawner itemSpawner;
	private Transform bulletSpawnPointPlayer;
	private void Start () 
	{
		currentWeaponText.text = weaponName;
		totalAmmoText.text = ammo.ToString();

		initialSwayPosition = transform.localPosition;

		shootAudioSource.clip = SoundClips.shootSound;
		
		GameObject parentObject = transform.parent.parent.gameObject;
		if (parentObject != null)
		{
			Debug.Log("Found Player GameObject");
			Debug.Log(parentObject);
			itemSpawner = parentObject.GetComponent<ItemSpawner>();
			bulletSpawnPointPlayer = itemSpawner.bulletSpawnPoint;
			if (bulletSpawnPointPlayer != null)
			{
				float spawnDistanceForward = 1.0f;
				bulletSpawnPoint= bulletSpawnPointPlayer;
				bulletSpawnPoint.position +=bulletSpawnPoint.forward * spawnDistanceForward;
				

				Debug.Log("Found bulletSpawnPoint on Player GameObject");
			}
			else
			{
				Debug.LogError("bulletSpawnPoint not found on Player GameObject");
			}
			

		}
	}

	private void LateUpdate () 
	{
		if (weaponSway == true) 
		{
			float movementX = -Input.GetAxis ("Mouse X") * swayAmount;
			float movementY = -Input.GetAxis ("Mouse Y") * swayAmount;
			movementX = Mathf.Clamp 
				(movementX, -maxSwayAmount, maxSwayAmount);
			movementY = Mathf.Clamp 
				(movementY, -maxSwayAmount, maxSwayAmount);
			Vector3 finalSwayPosition = new Vector3 
				(movementX, movementY, 0);
			transform.localPosition = Vector3.Lerp 
				(transform.localPosition, finalSwayPosition + 
					initialSwayPosition, Time.deltaTime * swaySmoothValue);
		}
	}
	
	private void Update () 
	{
		//Aiming
		if(Input.GetButton("Fire2") && !isReloading && !isRunning && !isInspecting) 
		{
			isAiming = true;
			
			gunCamera.fieldOfView = Mathf.Lerp (gunCamera.fieldOfView,
			aimFov, fovSpeed * Time.deltaTime);

			anim.SetBool ("Aim", true);

			if (!soundHasPlayed) 
			{
				mainAudioSource.clip = SoundClips.aimSound;
				mainAudioSource.Play ();

				soundHasPlayed = true;
			}
		} 
		else 
		{
			gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView,
				defaultFov,fovSpeed * Time.deltaTime);

			isAiming = false;

			anim.SetBool ("Aim", false);

			soundHasPlayed = false;
		}
		//Aim end

		if (randomMuzzleflash == true) 
		{
			randomMuzzleflashValue = Random.Range (minRandomValue, maxRandomValue);
		}
		

		currentAmmoText.text = currentAmmo.ToString ();

		
		AnimationCheck ();
		

		if (currentAmmo <= 0 && !outOfAmmo) 
		{
			outOfAmmo = true;
			StartCoroutine (AutoReload ());
			StartCoroutine (ShowProjectileDelay ());
		}
			
		//Shooting 
		if (Input.GetMouseButtonDown (0) && !outOfAmmo && !isReloading && !isInspecting && !isRunning) 
		{
			anim.Play ("Fire", 0, 0f);
		
			muzzleParticles.Emit (1);

			Instantiate (
				Prefabs.projectilePrefab, 
				bulletSpawnPoint.transform.position, 
				bulletSpawnPoint.transform.rotation);
				
			currentAmmo -= 1;

			shootAudioSource.clip = SoundClips.shootSound;
			shootAudioSource.Play ();

			StartCoroutine(MuzzleFlashLight());

			if (!isAiming) 
			{
				anim.Play ("Fire", 0, 0f);

				muzzleParticles.Emit (1);

				if (enableSparks == true) 
				{
					sparkParticles.Emit (Random.Range (1, 6));
				}
			} 
			else 
			{
				anim.Play ("Aim Fire", 0, 0f);

				if (!randomMuzzleflash) {
					muzzleParticles.Emit (1);
					
				} 
				else if (randomMuzzleflash == true) 
				{
					if (randomMuzzleflashValue == 1) 
					{
						if (enableSparks == true) 
						{
							sparkParticles.Emit (Random.Range (1, 6));
						}
						if (enableMuzzleFlash == true) 
						{
							muzzleParticles.Emit (1);
							StartCoroutine (MuzzleFlashLight ());
						}
					}
				}
			}
		}
		

		if (Input.GetKey (KeyCode.W) && !isRunning || 
			Input.GetKey (KeyCode.A) && !isRunning || 
			Input.GetKey (KeyCode.S) && !isRunning || 
			Input.GetKey (KeyCode.D) && !isRunning) 
		{
			anim.SetBool ("Walk", true);
		} else {
			anim.SetBool ("Walk", false);
		}

		if ((Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.LeftShift))) 
		{
			isRunning = true;
		} 
		else 
		{
			isRunning = false;
		}
		
		if (isRunning == true) 
		{
			anim.SetBool ("Run", true);
		} 
		else 
		{
			anim.SetBool ("Run", false);
		}
	}

	private IEnumerator ShowProjectileDelay	()
	{
		projectileRenderer.GetComponent<SkinnedMeshRenderer> ().enabled = false;
		yield return new WaitForSeconds (showProjectileDelay);
		projectileRenderer.GetComponent<SkinnedMeshRenderer>().enabled = true;
	}

	private IEnumerator GrenadeSpawnDelay () 
	{
		yield return new WaitForSeconds (grenadeSpawnDelay);
		Instantiate(Prefabs.grenadePrefab, 
			bulletSpawnPoint.transform.position, 
			bulletSpawnPoint.transform.rotation);
	}

	private IEnumerator AutoReload () {
		yield return new WaitForSeconds (autoReloadDelay);
	
		if (outOfAmmo == true) 
		{
			anim.Play ("Reload", 0, 0f);

			mainAudioSource.clip = SoundClips.reloadSound;
			mainAudioSource.Play ();
		} 
		currentAmmo = ammo;
		outOfAmmo = false;
	}

	private IEnumerator MuzzleFlashLight () 
	{
		muzzleFlashLight.enabled = true;
		yield return new WaitForSeconds (lightDuration);
		muzzleFlashLight.enabled = false;
	}

	private void AnimationCheck () 
	{
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Reload")) 
		{
			isReloading = true;
		} 
		else 
		{
			isReloading = false;
		}

		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Inspect")) 
		{
			isInspecting = true;
		} 
		else 
		{
			isInspecting = false;
		}
	}
}