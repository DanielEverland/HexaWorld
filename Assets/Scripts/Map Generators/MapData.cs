using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public static class MapData {

	public static Dictionary<Vector3, Chunk> Chunks { get { return _chunks; } }

	private static Vector3 CurrentPosition
	{
		get
		{
			return Game.Player.transform.position;
		}
	}

	private const byte CHUNK_RADIUS = 3;

	private static Dictionary<Vector3, Chunk> _chunks = new Dictionary<Vector3, Chunk>();
    private static Vector3 _currentChunkPosition;

	public static void Poll()
	{
        _currentChunkPosition = CurrentPosition.ToChunkPosition();

        CheckChunk(_currentChunkPosition);
	}
    private static void CheckChunk(Vector3 chunkPosition)
    {
        if (!Chunks.ContainsKey(chunkPosition))
        {
            PollAdd(chunkPosition);
        }
        else
        {
            CheckNeighbors(chunkPosition);
        }
    }
    private static void PollAdd(Vector3 chunkPosition)
    {
        if(Vector3.Distance(chunkPosition, _currentChunkPosition) <= CHUNK_RADIUS)
        {
            AddChunk(chunkPosition);
        }
    }
    private static void AddChunk(Vector3 chunkPosition)
    {
        Chunk newChunk = new Chunk(chunkPosition);

        Chunks.Add(chunkPosition, newChunk);

        MapGenerator.RenderChunk(newChunk);

        CheckNeighbors(chunkPosition);
    }
    private static void CheckNeighbors(Vector3 chunkPosition)
    {
        Chunk chunk = Chunks[chunkPosition];

        if (chunk.NeighborCount != 6)
        {
            foreach (Vector3 possibleChunk in chunk.GetMissingNeighbors())
            {
                CheckChunk(possibleChunk);
            }
        }
    }
	public static Chunk GetChunk(int x, int y, int z)
	{
		return GetChunk (new Vector3 (x, y, z));
	}
	public static Chunk GetChunk(Vector3 chunkPosition)
	{
        if (Chunks.ContainsKey(chunkPosition))
        {
            return Chunks[chunkPosition];
        }

        return null;
	}
}