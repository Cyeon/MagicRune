using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TravisGameAssets
{
	[System.Serializable]
	public class StatusEffect
	{
		public string effectName;
		
		public GameObject effectPrefab;
		
		public int animation;
		
		public GameObject auraPrefab;
		
		public float auraDelay;
		
		public Vector3 standingHitPosition;
		public Vector3 standingAuraPosition;
		public Vector3 stunnedHitPosition;
		public Vector3 stunnedAuraPosition;
		public Vector3 woundedHitPosition;
		public Vector3 woundedAuraPosition;
	}
	
	
	public class StatusEffectsPreview : MonoBehaviour
	{
		public Animator skeletonAnimator;
		AnimatorControllerParameter[] animParameters;
		
		[SerializeField]
		private int currentAnimation = 0;
		
		public List<StatusEffect> buffs;
		public List<StatusEffect> debuffs;
		public List<StatusEffect> ailments;

		public Text buffsNameLabel;
		public Text debuffsNameLabel;
		public Text ailmentsNameLabel;
		
		public Text buffsApplyIcon;
		public Text debuffsApplyIcon;
		public Text ailmentsApplyIcon;
		
		public Transform cameraPivot;
		public float cameraRotationSpeed = 10f;
		
		public MeshRenderer floor;
		
		public Image rotationIcon;
		public Image floorIcon;
		public Image slowMotionIcon;
		public Image lightingIcon;
		
		public GameObject lightGO;
		
		public GameObject aurasRoot;
		
		private string checkMark = "<color=green>✓</color>";
		private string crossMark = "<color=red>X</color>";

		private int buffIndex;
		private int appliedBuffIndex = -1;
		private int debuffIndex;
		private int appliedDebuffIndex = -1;
		private int ailmentIndex;
		private int appliedAilmentIndex = -1;

		private Vector3 initCamPosition;
		private Quaternion initCamRotation;

	    private float zoomSpeed = 0.5f;
		private float minZ = -2f;
		private float maxZ = -10f;
		
		private bool cameraRotating;
		private bool floorVisible;
		private bool slowMotion;
		private bool lighting;
		
		private Camera fXCamera;
		
		private IEnumerator spawnAuraCO;
		
		void Awake()
		{
			fXCamera = Camera.main.transform.GetChild(0).GetComponent<Camera>();
		}

		void Start()
		{
			animParameters = skeletonAnimator.parameters;
			
			buffIndex = 0;
			debuffIndex = 0;
			ailmentIndex = 0;
			
			cameraRotating = false;
			floorVisible = true;
			slowMotion = false;
			lighting = true;
			
			initCamPosition = Camera.main.transform.position;
			initCamRotation = Camera.main.transform.rotation;
			
			
			RefreshStatusUI();
		}

		void Update()
		{
			
			if(Input.GetKeyDown("1"))
			{
				ToggleRotation();
			}
			
			if(Input.GetKeyDown("2"))
			{
				ToggleFloor();
			}
			
			if(Input.GetKeyDown("3"))
			{
				ToggleSlowMotion();
			}
			
			if(Input.GetKeyDown("4"))
			{
				ToggleLighting();
			}

			if(Input.GetKeyDown("q"))
			{
				PreviousBuff();
			}
			
			if(Input.GetKeyDown("w"))
			{
				ApplyBuff();
			}
			
			if(Input.GetKeyDown("e"))
			{
				NextBuff();
			}
			
			if(Input.GetKeyDown("a"))
			{
				PreviousDebuff();
			}
			
			if(Input.GetKeyDown("s"))
			{
				ApplyDebuff();
			}
			
			if(Input.GetKeyDown("d"))
			{
				NextDebuff();
			}
			
			if(Input.GetKeyDown("z"))
			{
				PreviousAilment();
			}
			
			if(Input.GetKeyDown("x"))
			{
				ApplyAilment();
			}
			
			if(Input.GetKeyDown("c"))
			{
				NextAilment();
			}

			float scrollInput = Input.mouseScrollDelta.y;
			if(Camera.main.transform.localPosition.z <= maxZ)
			{
				if(scrollInput < 0)
				{
					scrollInput = 0;
				}
			}
			if(Camera.main.transform.localPosition.z >= minZ)
			{
				if(scrollInput > 0)
				{
					scrollInput = 0;
				}
				
			}
			MoveCamera(scrollInput);

			if(Input.GetMouseButtonDown(1))
			{
				
				Camera.main.transform.position = initCamPosition;
				Camera.main.transform.rotation = initCamRotation;
				
				if(cameraRotating)
				{
					ToggleRotation();
				}
			}
			
			
			if(cameraRotating)
			{	
				cameraPivot.Rotate(Vector3.up * (cameraRotationSpeed * Time.deltaTime));
			}
		}

	// Misc 
	
		void MoveCamera(float scroll)
		{
			Camera.main.transform.position += cameraPivot.position +(Camera.main.transform.forward * scroll * zoomSpeed);
		}
	
		void RefreshStatusUI(int type = -1)
		{
			buffsNameLabel.text = buffs[buffIndex].effectName;
			debuffsNameLabel.text = debuffs[debuffIndex].effectName;
			ailmentsNameLabel.text = ailments[ailmentIndex].effectName;

			buffsApplyIcon.text = crossMark;
			debuffsApplyIcon.text = crossMark;
			ailmentsApplyIcon.text = crossMark;

			switch(type)
			{
				case 0: //BUFF
					if(appliedBuffIndex == buffIndex)
					{
						buffsApplyIcon.text = checkMark;
					}else{
						buffsApplyIcon.text = crossMark;
					}
				break;
				case 1: //DEBUFF
					if(appliedDebuffIndex == debuffIndex)
					{
						debuffsApplyIcon.text = checkMark;
					}else{
						debuffsApplyIcon.text = crossMark;
					}
				break;
				case 2: //AILMENT
					if(appliedAilmentIndex == ailmentIndex)
					{
						ailmentsApplyIcon.text = checkMark;
					}else{
						ailmentsApplyIcon.text = crossMark;
					}
				break;
			}
		}
		
		public void ApplyAnimation(int animationParameter)
		{
			skeletonAnimator.SetTrigger(animParameters[animationParameter].name);
			currentAnimation = animationParameter;
		}

	//Scene Buttons
		
		public void ToggleRotation()
		{
			cameraRotating = !cameraRotating;
			
			var newColor = rotationIcon.color;
			newColor.a = cameraRotating ? 1f : 0.33f;
			rotationIcon.color = newColor;
			
			rotationIcon.GetComponent<Outline>().enabled = cameraRotating;
			rotationIcon.GetComponent<Shadow>().enabled = cameraRotating;
		}
		
		public void ToggleFloor()
		{
			floorVisible = !floorVisible;
			
			floor.enabled = floorVisible;
			
			var newColor = floorIcon.color;
			newColor.a = floorVisible ? 1f : 0.33f;
			floorIcon.color = newColor;
			
			floorIcon.GetComponent<Outline>().enabled = floorVisible;
			floorIcon.GetComponent<Shadow>().enabled = floorVisible;
		}
		
		public void ToggleSlowMotion()
		{
			slowMotion = !slowMotion;
			if(slowMotion)
			{
				Time.timeScale = 0.5f;
			}else{
				Time.timeScale = 1.0f;
			}
		
			var newColor = slowMotionIcon.color;
			newColor.a = slowMotion ? 1f : 0.33f;
			slowMotionIcon.color = newColor;
			
			slowMotionIcon.GetComponent<Outline>().enabled = slowMotion;
			slowMotionIcon.GetComponent<Shadow>().enabled = slowMotion;
		}
		
		public void ToggleLighting()
		{
			lighting = !lighting;
			lightGO.SetActive(lighting);
			
			var newColor = lightingIcon.color;
			newColor.a = lighting ? 1f : 0.33f;
			lightingIcon.color = newColor;
			
			lightingIcon.GetComponent<Outline>().enabled = lighting;
			lightingIcon.GetComponent<Shadow>().enabled = lighting;
		}
		


	//Buff, Debuff and Ailment Buttons
	
		public void ApplyBuff()
		{
			Transform spawnedBuff = SpawnBuff().transform;

			//Buff Hit Position
			Vector3 newPosition = spawnedBuff.position;
			switch(currentAnimation)
			{
				//Idle and Buff animations
				case 0:
				case 1:
					newPosition = buffs[buffIndex].standingHitPosition;
				break;
				
				//Stunned animation
				case 2:
					newPosition = buffs[buffIndex].stunnedHitPosition;
				break;
				
				//Wounded animation
				case 3:
					newPosition = buffs[buffIndex].woundedHitPosition;
				break;
				
			}

			spawnedBuff.transform.position = newPosition;

			ApplyAnimation(buffs[buffIndex].animation);

			appliedBuffIndex = buffIndex;


			//Buff Aura Position
			switch(currentAnimation)
			{
				//Idle and Buff animations
				case 0:
				case 1:
					newPosition = buffs[buffIndex].standingAuraPosition;
				break;
				
				//Stunned animation
				case 2:
					newPosition = buffs[buffIndex].stunnedAuraPosition;
				break;
				
				//Wounded animation
				case 3:
					newPosition = buffs[buffIndex].woundedAuraPosition;
				break;
				
			}

			CleanAuras();
			if(buffs[buffIndex].auraPrefab != null)
			{
				StartCoroutine(SpawnAura(buffs[buffIndex].auraPrefab, newPosition, buffs[buffIndex].auraDelay));
			}
			
			RefreshStatusUI(0);
			
		}
		
		public void NextBuff()
		{
			buffIndex++;
			if(buffIndex >= buffs.Count)
			{
				buffIndex = 0;
			}
			RefreshStatusUI(0);
		}
		
		public void PreviousBuff()
		{
			buffIndex--;
			if(buffIndex < 0)
			{
				buffIndex = buffs.Count - 1;
			}
			RefreshStatusUI(0);
		}
		
		public void ApplyDebuff()
		{
			Transform spawnedDebuff = SpawnDebuff().transform;

			//Debuff Hit Position
			Vector3 newPosition = spawnedDebuff.position;
			switch(currentAnimation)
			{
				//Idle and Debuff animations
				case 0:
				case 1:
					newPosition = debuffs[debuffIndex].standingHitPosition;
				break;
				
				//Stunned animation
				case 2:
					newPosition = debuffs[debuffIndex].stunnedHitPosition;
				break;
				
				//Wounded animation
				case 3:
					newPosition = debuffs[debuffIndex].woundedHitPosition;
				break;
				
			}

			spawnedDebuff.transform.position = newPosition;

			ApplyAnimation(debuffs[debuffIndex].animation);

			appliedDebuffIndex = debuffIndex;

			//Debuff Aura Position
			switch(currentAnimation)
			{
				//Idle and Buff animations
				case 0:
				case 1:
					newPosition = debuffs[debuffIndex].standingAuraPosition;
				break;
				
				//Stunned animation
				case 2:
					newPosition = debuffs[debuffIndex].stunnedAuraPosition;
				break;
				
				//Wounded animation
				case 3:
					newPosition = debuffs[debuffIndex].woundedAuraPosition;
				break;
				
			}

			CleanAuras();
			if(debuffs[debuffIndex].auraPrefab != null)
			{
				StartCoroutine(SpawnAura(debuffs[debuffIndex].auraPrefab, newPosition, debuffs[debuffIndex].auraDelay));
			}
			
			RefreshStatusUI(1);
			
		}
		
		public void NextDebuff()
		{
			debuffIndex++;
			if(debuffIndex >= debuffs.Count)
			{
				debuffIndex = 0;
			}
			RefreshStatusUI(1);
		}
		
		public void PreviousDebuff()
		{
			debuffIndex--;
			if(debuffIndex < 0)
			{
				debuffIndex = debuffs.Count - 1;
			}
			RefreshStatusUI(1);
		}
	
		public void ApplyAilment()
		{
			Transform spawnedAilment = SpawnAilment().transform;

			//Ailment Hit Position
			Vector3 newPosition = spawnedAilment.position;
			switch(currentAnimation)
			{
				//Idle and Ailment animations
				case 0:
				case 1:
					newPosition = ailments[ailmentIndex].standingHitPosition;
				break;
				
				//Stunned animation
				case 2:
					newPosition = ailments[ailmentIndex].stunnedHitPosition;
				break;
				
				//Wounded animation
				case 3:
					newPosition = ailments[ailmentIndex].woundedHitPosition;
				break;
				
			}

			spawnedAilment.transform.position = newPosition;

			ApplyAnimation(ailments[ailmentIndex].animation);

			appliedAilmentIndex = ailmentIndex;

			//Ailment Aura Position
			switch(currentAnimation)
			{
				//Idle and Buff animations
				case 0:
				case 1:
					newPosition = ailments[ailmentIndex].standingAuraPosition;
				break;
				
				//Stunned animation
				case 2:
					newPosition = ailments[ailmentIndex].stunnedAuraPosition;
				break;
				
				//Wounded animation
				case 3:
					newPosition = ailments[ailmentIndex].woundedAuraPosition;
				break;
				
			}

			CleanAuras();
			if(ailments[ailmentIndex].auraPrefab != null)
			{
				StartCoroutine(SpawnAura(ailments[ailmentIndex].auraPrefab, newPosition, ailments[ailmentIndex].auraDelay));
			}
			
			RefreshStatusUI(2);
		}
		
		public void NextAilment()
		{
			ailmentIndex++;
			if(ailmentIndex >= ailments.Count)
			{
				ailmentIndex = 0;
			}
			RefreshStatusUI(2);
		}
		
		public void PreviousAilment()
		{
			ailmentIndex--;
			if(ailmentIndex < 0)
			{
				ailmentIndex = ailments.Count - 1;
			}
			RefreshStatusUI(2);
		}
		
	//Spawn Buff, Debuff and Ailment
	
		private GameObject SpawnBuff()
		{
			GameObject spawnedBuff = Instantiate(buffs[buffIndex].effectPrefab);
			spawnedBuff.SetActive(true);
			return spawnedBuff;
		}
		
		private GameObject SpawnDebuff()
		{
			GameObject spawnedDebuff = Instantiate(debuffs[debuffIndex].effectPrefab);
			spawnedDebuff.SetActive(true);
			return spawnedDebuff;
		}
		
		private GameObject SpawnAilment()
		{
			GameObject spawnedAilment = Instantiate(ailments[ailmentIndex].effectPrefab);
			spawnedAilment.SetActive(true);
			return spawnedAilment;
		}

		private IEnumerator SpawnAura(GameObject auraToSpawn, Vector3 newPosition, float delay)
		{
			yield return new WaitForSeconds(delay);
			
			CleanAuras();
			
			GameObject newAuraToSpawn = Instantiate(auraToSpawn, aurasRoot.transform);
			
			newAuraToSpawn.transform.position = newPosition;
			
			newAuraToSpawn.SetActive(true);
			
		}
		
		private void CleanAuras()
		{
			int totalAuras = aurasRoot.transform.childCount;
			for(int i = 0; i < totalAuras; i++)
			{
				Destroy(aurasRoot.transform.GetChild(0).gameObject);
			}
		}
	}
}