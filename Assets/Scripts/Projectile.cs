using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Projectile : MonoBehaviour {

	[SerializeField] int damage;
	[SerializeField] int AP;
	[SerializeField] float horizontalSpeed;
	[SerializeField] float angle;
	protected Destructable target;
	Rigidbody rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		Assert.IsNotNull(rb);
	}

	public void SetTarget(Destructable argTarget)
	{
		//init movment
		target = argTarget;
		Vector3 targetVector = GetTargetVector(target.transform.position);

		rb.AddForce(targetVector,ForceMode.VelocityChange);
		
	}

	Vector3 GetTargetVector(Vector3 targetPosition)
	{
		Vector3 direction = targetPosition - transform.position;
		float h = direction.y;
		direction.y = 0f;
		float distance = direction.magnitude;
		direction.y = distance * Mathf.Tan(angle * Mathf.Deg2Rad);
		distance += h / Mathf.Tan(angle * Mathf.Deg2Rad);
		float  totalVelocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * angle * Mathf.Deg2Rad));
		return totalVelocity * direction.normalized;
	}

	void FixedUpdate()
	{
		//move the motherfucking projectile towards its target
		//account for the target not exisitng anymore (sold, destroyed before reaching it)
		//maybe make it move in an arch
	}

	void OnCollisionEnter(Collision collisionInfo)
	{
		//check for the right label (Destructable) and deal damage to the target!!! :)
	}

}
