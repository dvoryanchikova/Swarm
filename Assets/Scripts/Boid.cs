using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {

	public Vector3 velocity, v2_separation, v3_alignment;
	private float radius = 10f;
	private float maxSpeed = 3f;
	public float distance, separationCount;
	private float sight;
	Vector3 centerMass, mouse;
	private Collider2D[] boids;
	private Vector3 v1_cohesion, v4_target;

	public Transform target;

	private void Start()
	{
		InvokeRepeating("Velocity", 0, 0.2f);
	}

	void Velocity()
	{
		velocity = Vector3.zero;
		boids = Physics2D.OverlapCircleAll(transform.position, radius);
		Rule1 (boids);
		Rule2 (boids);
		Rule3 (boids);
		Rule4 ();
		velocity += v1_cohesion + v2_separation + 1.5f*v3_alignment + v4_target;
		velocity = Vector3.ClampMagnitude (velocity, maxSpeed);
	
	}

	void Update()
	{
		//Velocity ();
		if (transform.position.magnitude > 10)
		{
			velocity += -transform.position.normalized;
		}
		transform.position += velocity * Time.deltaTime;
		//Debug.DrawRay(transform.position, cohesion, Color.magenta);
	}

	void Rule1(Collider2D[] birds)
	{
		v1_cohesion = Vector3.zero;
		foreach(var bird in birds)
		{
			v1_cohesion += bird.transform.position;
		}
		centerMass = v1_cohesion / birds.Length;
		v1_cohesion = centerMass - transform.position;
		v1_cohesion = Vector3.ClampMagnitude(v1_cohesion, maxSpeed);

	}

	void Rule2(Collider2D[] birds)
	{
		separationCount = 0;
		distance = 2f;
		v2_separation = Vector3.zero;
		foreach(var bird in birds)
		{
			if(bird.gameObject != gameObject && (bird.transform.position - transform.position).magnitude < distance)
			{
				v2_separation += (transform.position - bird.transform.position) / (transform.position - bird.transform.position).magnitude;
				separationCount++;
			}
		}
		if (separationCount > 0)
		{
			v2_separation = v2_separation / separationCount;
			v2_separation = Vector3.ClampMagnitude(v2_separation, maxSpeed);
		}
	}

	void Rule3(Collider2D[] birds)
	{
		v3_alignment = Vector3.zero;
		foreach(var bird in birds)
		{
				v3_alignment += bird.GetComponent<Boid> ().velocity;
		}
		v3_alignment /= birds.Length;
		v3_alignment = Vector3.ClampMagnitude(v3_alignment, maxSpeed);

	}

	void Rule4()
	{
		mouse = new Vector3(Camera.main.ScreenToWorldPoint (Input.mousePosition).x,Camera.main.ScreenToWorldPoint (Input.mousePosition).y, 0f);
		v4_target = Vector3.zero;
		v4_target = mouse - centerMass;
		v4_target = Vector3.ClampMagnitude (v4_target, maxSpeed);

	}
}