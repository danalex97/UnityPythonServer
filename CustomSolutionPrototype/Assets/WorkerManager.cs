using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class WorkerManager : MonoBehaviour
	{
	// Use this for initialization
	void Start () 
	{
		InvokeRepeating("GetAction", 0.0f, 1.0f);

	}
	
	// Get movement
	void GetAction ()
	{
		WWW request = new WWW ("http://127.0.0.1:4000");
		
		while (!request.isDone) {}

		var gameData = JSON.Parse(request.text);
		int x = gameData["x"].AsInt;
		int y = gameData["y"].AsInt;

		transform.Translate (x * Time.deltaTime, 0, 0);
		transform.Translate (0, y * Time.deltaTime, 0);
	
		Debug.Log("x: " + x + ", y: " + y);
	}
}
