using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HandgunScriptLPFP : MonoBehaviour {

	Animator anim;

	public Camera gunCamera;

	
	public float fovSpeed = 15.0f;
	public float defaultFov = 40.0f;

	
	public string weaponName;
	private string storedWeaponName;

	
	
	public bool scope2;
	public Sprite scope2Texture;
	public float scope2TextureSize = 0.01f;
	[Range(5, 40)]
	public float scope2AimFOV = 25;
	[Space(10)]
	public bool scope3;
	public Sprite scope3Texture;
	public float scope3TextureSize = 0.025f;
	[Range(5, 40)]
	public float scope3AimFOV = 20;
	[Space(10)]
	public bool ironSights;
	public bool alwaysShowIronSights;
	[Range(5, 40)]
	public float ironSightsAimFOV = 16;
	[Space(10)]
	public bool silencer;
	[System.Serializable]
	public class weaponAttachmentRenderers 
	{
		[Space(10)]
		public SkinnedMeshRenderer scope2Renderer;
		public SkinnedMeshRenderer scope3Renderer;
		public SkinnedMeshRenderer ironSightsRenderer;
		public SkinnedMeshRenderer silencerRenderer;
		[Space(10)]
		public GameObject scope2RenderMesh;
		public GameObject scope3RenderMesh;
		[Space(10)]
		public SpriteRenderer scope2SpriteRenderer;
		public SpriteRenderer scope3SpriteRenderer;
	}
	public weaponAttachmentRenderers WeaponAttachmentRenderers;

	
	public bool weaponSway;

	public float swayAmount = 0.02f;
	public float maxSwayAmount = 0.06f;
	public float swaySmoothValue = 4.0f;

	private Vector3 initialSwayPosition;


	public float sliderBackTimer = 1.58f;
	private bool hasStartedSliderBack;

	public bool autoReload;
	public float autoReloadDelay;
	private bool isReloading;

	
	private bool holstered;
	private bool isRunning;
	private bool isAiming;
	private bool isWalking;
	private bool isInspecting;

	
	private int currentAmmo;
	
	public int ammo;
	
	private bool outOfAmmo;

	
	
	public float bulletForce = 400;
	
	public float showBulletInMagDelay = 0.6f;
	
	public SkinnedMeshRenderer bulletInMagRenderer;

	
	public float grenadeSpawnDelay = 0.35f;

	
	public bool randomMuzzleflash = false;
	private int minRandomValue = 1;

	[Range(2, 25)]
	public int maxRandomValue = 5;

	private int randomMuzzleflashValue;

	public bool enableMuzzleflash = true;
	public ParticleSystem muzzleParticles;
	public bool enableSparks = true;
	public ParticleSystem sparkParticles;
	public int minSparkEmission = 1;
	public int maxSparkEmission = 7;

	
	public Light muzzleflashLight;
	public float lightDuration = 0.02f;

	
	private AudioSource mainAudioSource;
	
	private AudioSource shootAudioSource;
	
	public Text currentWeaponText;
	public Text currentAmmoText;
	public Text totalAmmoText;

	[System.Serializable]
	public class prefabs
	{  
		public Transform bulletPrefab;
		public Transform grenadePrefab;
	}
	public prefabs Prefabs;
	
	private Transform grenadeSpawnPoint;
	private Transform bulletSpawnPoint;
	[System.Serializable]
	public class soundClips
	{
		public AudioClip shootSound;
		public AudioClip silencerShootSound;
		public AudioClip takeOutSound;
		public AudioClip holsterSound;
		public AudioClip reloadSoundOutOfAmmo;
		public AudioClip reloadSoundAmmoLeft;
		public AudioClip aimSound;
	}
	public soundClips SoundClips;

	private bool soundHasPlayed = false;

	private void Awake () 
	{
		anim = GetComponent<Animator>();
		currentAmmo = ammo;

		muzzleflashLight.enabled = false;
		
	}
	private ItemSpawner itemSpawner;
	private Transform bulletSpawnPointPlayer;
	private void Start () {
		GameObject parentObject = transform.parent.parent.gameObject;
		if (parentObject != null)
		{
			
			itemSpawner = parentObject.GetComponent<ItemSpawner>();
			bulletSpawnPointPlayer = itemSpawner.bulletSpawnPoint;
			currentWeaponText.text = itemSpawner.gunName;
			bulletSpawnPointPlayer = itemSpawner.bulletSpawnPoint;
			mainAudioSource = parentObject.GetComponent<AudioSource>();
			shootAudioSource = parentObject.GetComponent<AudioSource>();
			
			bulletSpawnPoint= bulletSpawnPointPlayer;
			
		}
		totalAmmoText.text = ammo.ToString();
		
		initialSwayPosition = transform.localPosition;

		shootAudioSource.clip = SoundClips.shootSound;
		
		
	}

	private void LateUpdate () {
		if (weaponSway == true) {
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
	
	private void Update () {

		//Aiming
		if(Input.GetButton("Fire2") && !isReloading && !isRunning && !isInspecting) 
		{
			if (ironSights == true) 
			{
				gunCamera.fieldOfView = Mathf.Lerp (gunCamera.fieldOfView,
					ironSightsAimFOV, fovSpeed * Time.deltaTime);
			}
			isAiming = true;
			if (ironSights == true) 
			{
				anim.SetBool ("Aim", true);
			}
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
			if (ironSights == true) 
			{
				anim.SetBool ("Aim", false);
			}
			soundHasPlayed = false;

		}
		//Aiming end

		
		if (randomMuzzleflash == true) {
			randomMuzzleflashValue = Random.Range (minRandomValue, maxRandomValue);
		}
		
		currentAmmoText.text = currentAmmo.ToString ();
		
		AnimationCheck ();
		

		if (currentAmmo == 0) 
		{
			
			currentWeaponText.text = "OUT OF AMMO";
		
			outOfAmmo = true;
			
			if (autoReload == true && !isReloading) 
			{
				StartCoroutine (AutoReload ());
			}
				
			
			anim.SetBool ("Out Of Ammo Slider", true);
			
			anim.SetLayerWeight (1, 1.0f);
		} 
		else 
		{
			currentWeaponText.text = itemSpawner.gunName;
			outOfAmmo = false;
			anim.SetLayerWeight (1, 0.0f);
		}

		//Shoot
		if (Input.GetMouseButtonDown (0) && !outOfAmmo && !isReloading && !isInspecting && !isRunning) 
		{
			muzzleParticles.transform.position = bulletSpawnPoint.transform.position; 
			sparkParticles.transform.position =	bulletSpawnPoint.transform.position;
			muzzleflashLight.transform.position = bulletSpawnPoint.transform.position;
			anim.Play ("Fire", 0, 0f);
			if (!silencer) 
			{
				muzzleParticles.Emit (1);
			}
				
			
			currentAmmo -= 1;
			
			shootAudioSource.clip = SoundClips.shootSound;
			shootAudioSource.Play ();
			

			StartCoroutine(MuzzleFlashLight());

			if (!isAiming) 
			{
				anim.Play ("Fire", 0, 0f);
				if (!silencer) 
				{
					muzzleParticles.Emit (1);
				}

				if (enableSparks == true) 
				{
					sparkParticles.Emit (Random.Range (1, 6));
				}
			} 
			else 
			{
				if (!randomMuzzleflash && !silencer) {
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
						if (enableMuzzleflash == true && !silencer) 
						{
							muzzleParticles.Emit (1);
							
							StartCoroutine (MuzzleFlashLight ());
						}
					}
				}
			}
				
			var bullet = (Transform)Instantiate (
				Prefabs.bulletPrefab,
				bulletSpawnPoint.transform.position,
				bulletSpawnPoint.transform.rotation);

			bullet.GetComponent<Rigidbody>().velocity = 
				bullet.transform.forward * bulletForce;
			
			

			
		}

		
		if (holstered == true) 
		{
			anim.SetBool ("Holster", true);
		} 
		else 
		{
			anim.SetBool ("Holster", false);
		}

		//Reload 
		if (Input.GetKeyDown (KeyCode.R) && !isReloading && !isInspecting) 
		{
			//Reload
			Reload ();

			if (!hasStartedSliderBack) 
			{
				hasStartedSliderBack = true;
				StartCoroutine (HandgunSliderBackDelay());
			}
		}
		
		//Run anim toggle
		if (isRunning == true) {
			anim.SetBool ("Run", true);
		} else {
			anim.SetBool ("Run", false);
		}
	}

	private IEnumerator HandgunSliderBackDelay () {
		yield return new WaitForSeconds (sliderBackTimer);
		anim.SetBool ("Out Of Ammo Slider", false);
		anim.SetLayerWeight (1, 0.0f);

		hasStartedSliderBack = false;
	}

	private IEnumerator GrenadeSpawnDelay () {
		yield return new WaitForSeconds (grenadeSpawnDelay);
		Instantiate(Prefabs.grenadePrefab, 
			bulletSpawnPoint.transform.position, 
			bulletSpawnPoint.transform.rotation);
	}

	private IEnumerator AutoReload () {

		if (!hasStartedSliderBack) 
		{
			hasStartedSliderBack = true;

			StartCoroutine (HandgunSliderBackDelay());
		}
		yield return new WaitForSeconds (autoReloadDelay);

		if (outOfAmmo == true) {
			anim.Play ("Reload Out Of Ammo", 0, 0f);

			mainAudioSource.clip = SoundClips.reloadSoundOutOfAmmo;
			mainAudioSource.Play ();

			if (bulletInMagRenderer != null) 
			{
				bulletInMagRenderer.GetComponent
				<SkinnedMeshRenderer> ().enabled = false;
				StartCoroutine (ShowBulletInMag ());
			}
		} 
		currentAmmo = ammo;
		outOfAmmo = false;
	}

	//Reload
	private void Reload () {
		
		if (outOfAmmo == true) 
		{
			anim.Play ("Reload Out Of Ammo", 0, 0f);

			mainAudioSource.clip = SoundClips.reloadSoundOutOfAmmo;
			mainAudioSource.Play ();

			if (bulletInMagRenderer != null) 
			{
				bulletInMagRenderer.GetComponent
				<SkinnedMeshRenderer> ().enabled = false;
				StartCoroutine (ShowBulletInMag ());
			}
		} 
		else 
		{
			anim.Play ("Reload Ammo Left", 0, 0f);

			mainAudioSource.clip = SoundClips.reloadSoundAmmoLeft;
			mainAudioSource.Play ();

			
			if (bulletInMagRenderer != null) 
			{
				bulletInMagRenderer.GetComponent
				<SkinnedMeshRenderer> ().enabled = true;
			}
		}
		currentAmmo = ammo;
		outOfAmmo = false;
	}

	
	private IEnumerator ShowBulletInMag () {
		yield return new WaitForSeconds (showBulletInMagDelay);
		bulletInMagRenderer.GetComponent<SkinnedMeshRenderer> ().enabled = true;
	}

	private IEnumerator MuzzleFlashLight () 
	{
		muzzleflashLight.enabled = true;
		yield return new WaitForSeconds (lightDuration);
		muzzleflashLight.enabled = false;
	}

	
	private void AnimationCheck () 
	{
		
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Reload Out Of Ammo") || 
			anim.GetCurrentAnimatorStateInfo (0).IsName ("Reload Ammo Left")) 
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