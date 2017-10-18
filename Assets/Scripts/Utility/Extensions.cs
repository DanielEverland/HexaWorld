using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {

	private const byte MAX_ITEM_OUTPUT = 255;

	public static Vector3 ToChunkPosition(this Vector3 vector)
	{
		return new Vector3 (
			Mathf.FloorToInt (vector.x / Chunk.CHUNK_SIZE),
			Mathf.FloorToInt (vector.y / Chunk.CHUNK_SIZE),
			Mathf.FloorToInt (vector.z / Chunk.CHUNK_SIZE));
	}
	public static void Output(this object[] list)
	{
		if(list.Length > MAX_ITEM_OUTPUT)
		{
			Debug.Log ("List truncated. Max length is " + MAX_ITEM_OUTPUT);
		}

		for (int i = 0; i < Mathf.Min(list.Length, MAX_ITEM_OUTPUT); i++) {

			Debug.Log (list [i]);
		}
	}
	public static void Output(this object[,] list)
	{
		if(list.Length > MAX_ITEM_OUTPUT)
		{
			Debug.Log ("List truncated. Max length is " + MAX_ITEM_OUTPUT);
		}

		for (int x = 0; x < list.GetLength(0); x++) {
			for (int y = 0; y < list.GetLength(1); y++) {

				if (x + y > MAX_ITEM_OUTPUT)
					return;

				Debug.Log (list [x, y]);
			}
		}
	}
	public static void Output(this object[,,] list)
	{
		if(list.Length > MAX_ITEM_OUTPUT)
		{
			Debug.Log ("List truncated. Max length is " + MAX_ITEM_OUTPUT);
		}

		for (int x = 0; x < list.GetLength(0); x++) {
			for (int y = 0; y < list.GetLength(1); y++) {
				for (int z = 0; z < list.GetLength (2); z++) {

					if (x + y + z > MAX_ITEM_OUTPUT)
						return;

					Debug.Log (string.Format("Index ({0}, {1}, {2}) - {3}", x, y, z, list [x, y, z]));
				}
			}
		}
	}
	public static void Output(this object[,,,] list)
	{
		if(list.Length > MAX_ITEM_OUTPUT)
		{
			Debug.Log ("List truncated. Max length is " + MAX_ITEM_OUTPUT);
		}

		for (int x = 0; x < list.GetLength(0); x++) {
			for (int y = 0; y < list.GetLength(1); y++) {
				for (int z = 0; z < list.GetLength (2); z++) {
					for (int w = 0; w < list.GetLength (3); w++) {

						if (x + y + z + w > MAX_ITEM_OUTPUT)
							return;

						Debug.Log (list [x, y, z, w]);
					}
				}
			}
		}
	}
	public static void Output<T>(this IEnumerable<T> list)
	{
		int listCount = list.Count ();

		if (listCount > MAX_ITEM_OUTPUT) {
			Debug.Log ("List truncated. Max length is " + MAX_ITEM_OUTPUT);
		}

		for (int i = 0; i < Mathf.Min(listCount, MAX_ITEM_OUTPUT); i++) {

			Debug.Log (list.ElementAt (i));
		}
	}
}
