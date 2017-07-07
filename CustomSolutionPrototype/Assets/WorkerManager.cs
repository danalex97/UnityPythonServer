using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class WorkerManager : MonoBehaviour
{
	// Setup to ping the server every second.
	void Start()
	{
		Debug.Log("Starting the game...");

		BuildStaticScene();
		InvokeRepeating("UpdateMapState", 0.0f, 1.0f);
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
		UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:4000/update");
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

			AvatarController controller = avatar.GetComponent<AvatarController>();
			Vector3 nextPosition = new Vector3(x, 0.5f, y);
			controller.SetNextPosition(nextPosition);

			Debug.Log("Moved " + id + " to position (" + x + ", " + y + ")");
		}
	}

	// Wrap initialisation request in coroutine.
	public void BuildStaticScene()
	{
		Debug.Log("Initialising map...");
		StartCoroutine(GetFixedObjects());
	}

	// Request map dimensions and static objects, i.e. walls.
	IEnumerator GetFixedObjects()
	{
		Debug.Log("afdsgsdfgdsg");
		UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:4000/start");
		yield return request.Send();

		var map = JSON.Parse(request.downloadHandler.text);
		Debug.Log(map);

		// Create plane (floor).
		float minX = (float) map["minX"].AsInt;
		float minY = (float) map["minY"].AsInt;
		float maxX = (float) map["maxX"].AsInt;
		float maxY = (float) map["maxY"].AsInt;

		GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
		floor.transform.position = new Vector3(minX + maxX, 0.0f, minY + maxY);
		floor.transform.localScale = new Vector3((maxX - minX) / 10.0f, 1.0f, (maxY - minY) / 10.0f);

		// Create cubes (walls).
		var objectList = map["objects"];

		for (int i = 0; i < objectList.Count; i++)
		{
			var obj = objectList[i];

			string id = "object" + Convert.ToString(obj["id"].AsInt);

			float x = (float) obj["x"].AsInt;
			float y = (float) obj["y"].AsInt;

			// Create new game object.
			GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
			wall.transform.position = new Vector3(x, 0.5f, y);
			wall.name = id;

			Debug.Log(id + " is at position (" + x + ", " + y + ")");
		}

		// Create spheres (avatars)
		var playersList = map["players"];

		for (int i = 0; i < playersList.Count; i++) 
		{
			var player = playersList[i];

			string id = "player" + Convert.ToString(player["id"].AsInt);

			float x = (float) player["x"].AsInt;
			float y = (float) player["y"].AsInt;

			// Create new game object.
			GameObject avatar = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			avatar.transform.position = new Vector3(x, 0.5f, y);
			avatar.name = id;

			avatar.AddComponent<AvatarController>();

			Debug.Log(id + " is at position (" + x + ", " + y + ")");
		}
	}
}
