using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {
    
	public static bool HexalExists(int x, int y, int z)
	{
		Chunk chunk = MapData.GetChunk (
			Mathf.FloorToInt(x / Chunk.CHUNK_SIZE), 
			Mathf.FloorToInt(y / Chunk.CHUNK_SIZE), 
			Mathf.FloorToInt(z / Chunk.CHUNK_SIZE));

		if (chunk == null)
			return false;

		return chunk.ContainsHexal (
			(int)(x - chunk.ChunkPosition.x * Chunk.CHUNK_SIZE),
			(int)(y - chunk.ChunkPosition.y * Chunk.CHUNK_SIZE),
			(int)(z - chunk.ChunkPosition.z * Chunk.CHUNK_SIZE));
	}
}
