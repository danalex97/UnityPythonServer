using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class WorkerManager : MonoBehaviour
{
	private const string url = "http://127.0.0.1";
	private const int port = 4000;

	// Setup to ping the server every second.
	void Start() 
	{
		InvokeRepeating("UpdateMapState", 0.0f, 1.0f);
	}
	
	// Gets a list of the players with their positions.
	void UpdateMapState()
	{
		var playersList = JSON.Parse(CurrentRawMapState());

		foreach (var player in playersList) 
		{
			string id = player["id"].AsString;
			GameObject avatar = GameObject.Find(id);

			int x = player["x"].AsInt;
			int y = player["y"].AsInt;

			if (avatar == null) 
			{
				// Create new game object.
				avatar = GameObject.CreatePrimitive(PrimitiveType.Cube);
				avatar.transform.position = Vector3(x, 0.5, y);
				avatar.name = id;
			}
			avatar.transform.Translate (x, 0.5, y);
			Debug.Log("Player " + id + " is at position (" + x + ", " + y + ")");
		}
	}

	// Perform GET request to the server.
	string CurrentRawMapState()
	{
		WWW request = new WWW(String.Format("{0}:{0}", url, port));
		yield return request.text;
	}
}
