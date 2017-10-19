using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {

    public static Vector3 GetChunkWorldPosition(Vector3 chunkDataPosition)
    {
        return Vector3.Scale(chunkDataPosition, Chunk.HexalSize) * Chunk.CHUNK_SIZE;
    }
    public static Vector3 GetHexalWorldPosition(Vector3 hexalDataPosition)
    {
        if (hexalDataPosition.z % 2 == 0)
        {
            hexalDataPosition.x -= Chunk.HexalSize.y;
        }

        return Vector3.Scale(hexalDataPosition, Chunk.HexalSize);
    }
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
