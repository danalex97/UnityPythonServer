using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class MapBuilder : MonoBehaviour 
{
	// Request map dimensions and static objects, i.e. walls.
	IEnumerator GetFixedObjects()
	{
		UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:4000/start");
		yield return request.Send();

		var map = JSON.Parse (request.downloadHandler.text);

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

			Debug.Log("Object " + id + " is at position (" + x + ", " + y + ")");
		}
	}

	// Wrap initialisation request in coroutine.
	void BuildStaticScene()
	{
		Debug.Log("Initialising map...");
		StartCoroutine(GetFixedObjects());
	}
}