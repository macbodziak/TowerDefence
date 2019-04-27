using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(AudioSource))]
abstract public class ArmedUnit : MonoBehaviour {

	[SerializeField] bool isEnemy; 
	[SerializeField] GameObject turret; 
	[Range(0,100f)] [SerializeField] protected float range;
	[Range(0,100f)] [SerializeField] protected float reloadTime;
	[SerializeField] AudioClip fireWeaponSound;
	protected Destructable target;
	bool readyToFire = true;
	int fireWeaponAnimHash;
	Animator animator;
	AudioSource audioSource;

	void Awake()
	{
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		if(audioSource == null)
		{
			audioSource = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
		}
		Assert.IsNotNull(animator);
		Assert.IsNotNull(turret);
		fireWeaponAnimHash = Animator.StringToHash("FireWeapon");
	}
	
	abstract protected void FireWeapon();

	void DetectEnemy()
	{
		List<Destructable> list;
		if(isEnemy)
		{
			list = GameController.Instance.playerUnits;
		}
		else
		{
			list = GameController.Instance.enemyUnits;
		}
		
		foreach(var unit in list)
		{
			if(Vector3.Distance(transform.position, unit.transform.position) <= range)
			{
				target = unit;
				target.RegisterEngagingEnemy(this);
				break;
			}
		}
	}

	void Update()
	{
		if(target != null && TargetInRange())
		{
			RotateTurret();
			
			if(readyToFire)
			{
				StartCoroutine(Reload());
				Attack();
			}
		}
		else
		{
			DetectEnemy();
		}
	}

	public void Untarget(Destructable destructable)
	{
		if(target == destructable)
		{
			target = null;
		}
	}

	bool TargetInRange()
	{
		if(target == null)
		{
			return false;
		}
		return (Vector3.Distance(transform.position, target.transform.position) <= range);
	}

	IEnumerator Reload()
	{
		readyToFire = false;
		yield return new WaitForSeconds(reloadTime);
		readyToFire = true;
		yield return null;
	}

	void Attack()
	{
		Color debugColor;
		if(isEnemy)
			debugColor = Color.red;
		else
			debugColor = Color.green;
		Debug.DrawLine(transform.position, target.transform.position, debugColor, 0.2f, true);

		//audio & visual effects
		FireWeapon();
	}

	void RotateTurret()
	{
		Vector3 targetDir = target.transform.position - turret.transform.position;
		Quaternion newRot =  Quaternion.LookRotation(targetDir);
		turret.transform.rotation = Quaternion.Lerp(turret.transform.rotation, newRot, Time.deltaTime * 4f);
	}

	protected void PlayWeaponEffect()
	{
		audioSource.PlayOneShot(fireWeaponSound);
		animator.SetTrigger(fireWeaponAnimHash);
	}
}
