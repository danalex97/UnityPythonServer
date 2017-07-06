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
		Debug.Log("Starting the game...");

		BuildStaticScene();
		InvokeRepeating("UpdateMapState", 0.0f, 1.0f);
	}

	void BuildStaticScene()
	{
		Debug.Log("Initial map.");
		StartCoroutine(GetFixedObjects());
	}

	IEnumerator GetFixedObjects()
	{
		UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:4000");
		yield return request.Send();

		var objectList = JSON.Parse(request.downloadHandler.text)["objects"];
		Debug.Log(objectList.Count);

		for (int i = 0; i < objectList.Count; i++)
		{
			var obj = objectList[i];

			string id = "object" + Convert.ToString(obj["id"].AsInt);
			GameObject avatar = GameObject.Find(id);

			float x = (float) obj["x"].AsInt;
			float y = (float) obj["y"].AsInt;

			if (avatar == null)
			{
				// Create new game object.
				avatar = GameObject.CreatePrimitive(PrimitiveType.Cube);
				avatar.transform.position = new Vector3(x, 0.5f, y);
				avatar.name = id;
			}
			avatar.transform.position = new Vector3(x, 0.5f, y);
			Debug.Log("Object " + id + " is at position (" + x + ", " + y + ")");
		}
	}

	// Wrap request in a coroutine.
	void UpdateMapState()
	{
		Debug.Log("Updating map.");
		StartCoroutine(RequestMapState());
	}

	// Gets a list of the players with their positions.
	IEnumerator RequestMapState()
	{
		//string requestAddress = String.Format("{0}:{0}", url, port);
		UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:4000");
		yield return request.Send();

		var playersList = JSON.Parse(request.downloadHandler.text)["players"];
		Debug.Log(playersList.Count);

		for (int i = 0; i < playersList.Count; i++)
		{
			var player = playersList[i];

			string id = "player" + Convert.ToString(player["id"].AsInt);
			GameObject avatar = GameObject.Find(id);

			float x = (float) player["x"].AsInt;
			float y = (float) player["y"].AsInt;

			if (avatar == null)
			{
				// Create new game object.
				avatar = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				avatar.transform.position = new Vector3(x, 0.5f, y);
				avatar.name = id;
			}
			avatar.transform.position = new Vector3(x, 0.5f, y);
			Debug.Log("Player " + id + " is at position (" + x + ", " + y + ")");
		}
	}
}
