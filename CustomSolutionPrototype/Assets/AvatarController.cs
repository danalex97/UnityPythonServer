using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour 
{
	private Vector3 currPosition;
	private Vector3 nextPosition;
	private float startTime;

	public AvatarController()
	{
		startTime = Time.time;
		currPosition = transform.position;
		nextPosition = transform.position;
	}

	// Use this for initialization
	void Start() 
	{
		
	}
	
	// Move the player
	void Update() 
	{
		if (nextPosition != currPosition) 
		{
			float step = Time.time - startTime;
			transform.position = Vector3.Lerp(currPosition, nextPosition, step);
		}	
	}

	public void SetNextPosition(Vector3 position)
	{
		currPosition = transform.position;
		nextPosition = position;
		startTime = Time.time;
	}
}
