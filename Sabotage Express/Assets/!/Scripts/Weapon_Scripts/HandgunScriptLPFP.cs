using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HandgunScriptLPFP : MonoBehaviour {

	//Animator component attached to weapon
	Animator anim;

	[Header("Gun Camera")]
	//Main gun camera
	public Camera gunCamera;

	[Header("Gun Camera Options")]
	//How fast the camera field of view changes when aiming 
	[Tooltip("How fast the camera field of view changes when aiming.")]
	public float fovSpeed = 15.0f;
	//Default camera field of view
	[Tooltip("Default value for camera field of view (40 is recommended).")]
	public float defaultFov = 40.0f;

	[Header("UI Weapon Name")]
	[Tooltip("Name of the current weapon, shown in the game UI.")]
	public string weaponName;
	private string storedWeaponName;

	[Header("Weapon Attachments (Only use one scope attachment)")]
	[Space(10)]
	//Toggle weapon attachments (loads at start)
	//Toggle scope 02
	public bool scope2;
	public Sprite scope2Texture;
	public float scope2TextureSize = 0.01f;
	//Scope 02 camera fov
	[Range(5, 40)]
	public float scope2AimFOV = 25;
	[Space(10)]
	//Toggle scope 03
	public bool scope3;
	public Sprite scope3Texture;
	public float scope3TextureSize = 0.025f;
	//Scope 03 camera fov
	[Range(5, 40)]
	public float scope3AimFOV = 20;
	[Space(10)]
	//Toggle iron sights
	public bool ironSights;
	public bool alwaysShowIronSights;
	//Iron sights camera fov
	[Range(5, 40)]
	public float ironSightsAimFOV = 16;
	[Space(10)]
	//Toggle silencer
	public bool silencer;
	//Weapon attachments components
	[System.Serializable]
	public class weaponAttachmentRenderers 
	{
		[Header("Scope Model Renderers")]
		[Space(10)]
		//All attachment renderer components
		public SkinnedMeshRenderer scope2Renderer;
		public SkinnedMeshRenderer scope3Renderer;
		public SkinnedMeshRenderer ironSightsRenderer;
		public SkinnedMeshRenderer silencerRenderer;
		[Header("Scope Sight Mesh Renderers")]
		[Space(10)]
		//Scope render meshes
		public GameObject scope2RenderMesh;
		public GameObject scope3RenderMesh;
		[Header("Scope Sight Sprite Renderers")]
		[Space(10)]
		//Scope sight textures
		public SpriteRenderer scope2SpriteRenderer;
		public SpriteRenderer scope3SpriteRenderer;
	}
	public weaponAttachmentRenderers WeaponAttachmentRenderers;

	[Header("Weapon Sway")]
	//Enables weapon sway
	[Tooltip("Toggle weapon sway.")]
	public bool weaponSway;

	public float swayAmount = 0.02f;
	public float maxSwayAmount = 0.06f;
	public float swaySmoothValue = 4.0f;

	private Vector3 initialSwayPosition;

	[Header("Weapon Settings")]

	public float sliderBackTimer = 1.58f;
	private bool hasStartedSliderBack;

	//Eanbles auto reloading when out of ammo
	[Tooltip("Enables auto reloading when out of ammo.")]
	public bool autoReload;
	//Delay between shooting last bullet and reloading
	public float autoReloadDelay;
	//Check if reloading
	private bool isReloading;

	//Holstering weapon
	private bool hasBeenHolstered = false;
	//If weapon is holstered
	private bool holstered;
	//Check if running
	private bool isRunning;
	//Check if aiming
	private bool isAiming;
	//Check if walking
	private bool isWalking;
	//Check if inspecting weapon
	private bool isInspecting;

	//How much ammo is currently left
	private int currentAmmo;
	//Totalt amount of ammo
	[Tooltip("How much ammo the weapon should have.")]
	public int ammo;
	//Check if out of ammo
	private bool outOfAmmo;

	[Header("Bullet Settings")]
	//Bullet
	[Tooltip("How much force is applied to the bullet when shooting.")]
	public float bulletForce = 400;
	[Tooltip("How long after reloading that the bullet model becomes visible " +
		"again, only used for out of ammo reload aniamtions.")]
	public float showBulletInMagDelay = 0.6f;
	[Tooltip("The bullet model inside the mag, not used for all weapons.")]
	public SkinnedMeshRenderer bulletInMagRenderer;

	[Header("Grenade Settings")]
	public float grenadeSpawnDelay = 0.35f;

	[Header("Muzzleflash Settings")]
	public bool randomMuzzleflash = false;
	//min should always bee 1
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

	[Header("Muzzleflash Light Settings")]
	public Light muzzleflashLight;
	public float lightDuration = 0.02f;

	[Header("Audio Source")]
	//Main audio source
	private AudioSource mainAudioSource;
	//Audio source used for shoot sound
	private AudioSource shootAudioSource;

	[Header("UI Components")]
	public Text currentWeaponText;
	public Text currentAmmoText;
	public Text totalAmmoText;

	[System.Serializable]
	public class prefabs
	{  
		[Header("Prefabs")]
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
		//Set the animator component
		anim = GetComponent<Animator>();
		//Set current ammo to total ammo value
		currentAmmo = ammo;

		muzzleflashLight.enabled = false;
		
	}
	private ItemSpawner itemSpawner;
	private Transform bulletSpawnPointPlayer;
	private void Start () {
		GameObject parentObject = transform.parent.parent.gameObject;
		if (parentObject != null)
		{
			//Debug.Log("Found Player GameObject");
			//Debug.Log(parentObject);
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
		//Weapon sway
		if (weaponSway == true) {
			float movementX = -Input.GetAxis ("Mouse X") * swayAmount;
			float movementY = -Input.GetAxis ("Mouse Y") * swayAmount;
			//Clamp movement to min and max values
			movementX = Mathf.Clamp 
				(movementX, -maxSwayAmount, maxSwayAmount);
			movementY = Mathf.Clamp 
				(movementY, -maxSwayAmount, maxSwayAmount);
			//Lerp local pos
			Vector3 finalSwayPosition = new Vector3 
				(movementX, movementY, 0);
			transform.localPosition = Vector3.Lerp 
				(transform.localPosition, finalSwayPosition + 
				initialSwayPosition, Time.deltaTime * swaySmoothValue);
		}
	}
	
	private void Update () {

		//Aiming
		//Toggle camera FOV when right click is held down
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
			//When right click is released
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

		//If randomize muzzleflash is true, genereate random int values
		if (randomMuzzleflash == true) {
			randomMuzzleflashValue = Random.Range (minRandomValue, maxRandomValue);
		}
		
		//Set current ammo text from ammo int
		currentAmmoText.text = currentAmmo.ToString ();

		//Continosuly check which animation 
		//is currently playing
		AnimationCheck ();

		//Play knife attack 1 animation when Q key is pressed
		// if (Input.GetKeyDown (KeyCode.Q) && !isInspecting) 
		// {
		// 	anim.Play ("Knife Attack 1", 0, 0f);
		// }
		// //Play knife attack 2 animation when F key is pressed
		// if (Input.GetKeyDown (KeyCode.F) && !isInspecting) 
		// {
		// 	anim.Play ("Knife Attack 2", 0, 0f);
		// }
		// 	
		// //Throw grenade when pressing G key
		// if (Input.GetKeyDown (KeyCode.G) && !isInspecting) 
		// {
		// 	StartCoroutine (GrenadeSpawnDelay ());
		// 	//Play grenade throw animation
		// 	anim.Play("GrenadeThrow", 0, 0.0f);
		// }

		//If out of ammo
		if (currentAmmo == 0) 
		{
			//Show out of ammo text
			currentWeaponText.text = "OUT OF AMMO";
			//Toggle bool
			outOfAmmo = true;
			//Auto reload if true
			if (autoReload == true && !isReloading) 
			{
				StartCoroutine (AutoReload ());
			}
				
			//Set slider back
			anim.SetBool ("Out Of Ammo Slider", true);
			//Increase layer weight for blending to slider back pose
			anim.SetLayerWeight (1, 1.0f);
		} 
		else 
		{
			//When ammo is full, show weapon name again
			currentWeaponText.text = itemSpawner.gunName;
			outOfAmmo = false;
			anim.SetLayerWeight (1, 0.0f);
		}

		//Shooting 
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
				
			//Remove 1 bullet from ammo
			currentAmmo -= 1;
			
			shootAudioSource.clip = SoundClips.shootSound;
			shootAudioSource.Play ();
			

			//Light flash start
			StartCoroutine(MuzzleFlashLight());

			if (!isAiming) //if not aiming
			{
				anim.Play ("Fire", 0, 0f);
				if (!silencer) 
				{
					muzzleParticles.Emit (1);
				}

				if (enableSparks == true) 
				{
					//Emit random amount of spark particles
					sparkParticles.Emit (Random.Range (1, 6));
				}
			} 
			else //if aiming
			{
				//If random muzzle is false
				if (!randomMuzzleflash && !silencer) {
					muzzleParticles.Emit (1);
					//If random muzzle is true
				} 
				else if (randomMuzzleflash == true) 
				{
					//Only emit if random value is 1
					if (randomMuzzleflashValue == 1) 
					{
						if (enableSparks == true) 
						{
							//Emit random amount of spark particles
							sparkParticles.Emit (Random.Range (1, 6));
						}
						if (enableMuzzleflash == true && !silencer) 
						{
							muzzleParticles.Emit (1);
							//Light flash start
							StartCoroutine (MuzzleFlashLight ());
						}
					}
				}
			}
				
			//Spawn bullet at bullet spawnpoint
			var bullet = (Transform)Instantiate (
				Prefabs.bulletPrefab,
				bulletSpawnPoint.transform.position,
				bulletSpawnPoint.transform.rotation);

			//Add velocity to the bullet
			bullet.GetComponent<Rigidbody>().velocity = 
				bullet.transform.forward * bulletForce;
			
			

			// //Spawn casing prefab at spawnpoint
			// Instantiate (Prefabs.casingPrefab, 
			// 	Spawnpoints.casingSpawnPoint.transform.position, 
			// 	Spawnpoints.casingSpawnPoint.transform.rotation);
		}

		//Inspect weapon when pressing T key
		// if (Input.GetKeyDown (KeyCode.T)) 
		// {
		// 	anim.SetTrigger ("Inspect");
		// }
		//
		// //Toggle weapon holster when pressing E key
		// if (Input.GetKeyDown (KeyCode.E) && !hasBeenHolstered) 
		// {
		// 	holstered = true;
		//
		// 	mainAudioSource.clip = SoundClips.holsterSound;
		// 	mainAudioSource.Play();
		//
		// 	hasBeenHolstered = true;
		// } 
		// else if (Input.GetKeyDown (KeyCode.E) && hasBeenHolstered) 
		// {
		// 	holstered = false;
		//
		// 	mainAudioSource.clip = SoundClips.takeOutSound;
		// 	mainAudioSource.Play ();
		//
		// 	hasBeenHolstered = false;
		// }

		//Holster anim toggle
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
		//Wait set amount of time
		yield return new WaitForSeconds (sliderBackTimer);
		//Set slider back
		anim.SetBool ("Out Of Ammo Slider", false);
		//Increase layer weight for blending to slider back pose
		anim.SetLayerWeight (1, 0.0f);

		hasStartedSliderBack = false;
	}

	private IEnumerator GrenadeSpawnDelay () {
		//Wait for set amount of time before spawning grenade
		yield return new WaitForSeconds (grenadeSpawnDelay);
		//Spawn grenade prefab at spawnpoint
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
		//Wait for set amount of time
		yield return new WaitForSeconds (autoReloadDelay);

		if (outOfAmmo == true) {
			//Play diff anim if out of ammo
			anim.Play ("Reload Out Of Ammo", 0, 0f);

			mainAudioSource.clip = SoundClips.reloadSoundOutOfAmmo;
			mainAudioSource.Play ();

			//If out of ammo, hide the bullet renderer in the mag
			//Do not show if bullet renderer is not assigned in inspector
			if (bulletInMagRenderer != null) 
			{
				bulletInMagRenderer.GetComponent
				<SkinnedMeshRenderer> ().enabled = false;
				//Start show bullet delay
				StartCoroutine (ShowBulletInMag ());
			}
		} 
		//Restore ammo when reloading
		currentAmmo = ammo;
		outOfAmmo = false;
	}

	//Reload
	private void Reload () {
		
		if (outOfAmmo == true) 
		{
			//Play diff anim if out of ammo
			anim.Play ("Reload Out Of Ammo", 0, 0f);

			mainAudioSource.clip = SoundClips.reloadSoundOutOfAmmo;
			mainAudioSource.Play ();

			//If out of ammo, hide the bullet renderer in the mag
			//Do not show if bullet renderer is not assigned in inspector
			if (bulletInMagRenderer != null) 
			{
				bulletInMagRenderer.GetComponent
				<SkinnedMeshRenderer> ().enabled = false;
				//Start show bullet delay
				StartCoroutine (ShowBulletInMag ());
			}
		} 
		else 
		{
			//Play diff anim if ammo left
			anim.Play ("Reload Ammo Left", 0, 0f);

			mainAudioSource.clip = SoundClips.reloadSoundAmmoLeft;
			mainAudioSource.Play ();

			//If reloading when ammo left, show bullet in mag
			//Do not show if bullet renderer is not assigned in inspector
			if (bulletInMagRenderer != null) 
			{
				bulletInMagRenderer.GetComponent
				<SkinnedMeshRenderer> ().enabled = true;
			}
		}
		//Restore ammo when reloading
		currentAmmo = ammo;
		outOfAmmo = false;
	}

	//Enable bullet in mag renderer after set amount of time
	private IEnumerator ShowBulletInMag () {
		//Wait set amount of time before showing bullet in mag
		yield return new WaitForSeconds (showBulletInMagDelay);
		bulletInMagRenderer.GetComponent<SkinnedMeshRenderer> ().enabled = true;
	}

	//Show light when shooting, then disable after set amount of time
	private IEnumerator MuzzleFlashLight () 
	{
		muzzleflashLight.enabled = true;
		yield return new WaitForSeconds (lightDuration);
		muzzleflashLight.enabled = false;
	}

	//Check current animation playing
	private void AnimationCheck () 
	{
		//Check if reloading
		//Check both animations
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Reload Out Of Ammo") || 
			anim.GetCurrentAnimatorStateInfo (0).IsName ("Reload Ammo Left")) 
		{
			isReloading = true;
		} 
		else 
		{
			isReloading = false;
		}

		//Check if inspecting weapon
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