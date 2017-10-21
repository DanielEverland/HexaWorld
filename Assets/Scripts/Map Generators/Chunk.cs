using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {

	public Chunk(Vector3 position)
	{
		_chunkPosition = position;
		_hexals = new byte[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];

		for (int x = 0; x < Hexals.GetLength(0); x++) {
			for (int y = 0; y < Hexals.GetLength(1); y++) {
				for (int z = 0; z < Hexals.GetLength(2); z++) {

                    //if (Utility.GetHexalWorldPosition(new Vector3(0, (int)(position.y * CHUNK_SIZE + y), 0)).y >= 0)
                        //continue;

					_hexals [x, y, z] = 1;
				}
			}
		}

        NotifyNeighborsOfCreation();
	}

	public const byte CHUNK_SIZE = 8;
    public static Vector3 HexalSize { get { return _hexalSize; } }
    
    private static readonly Vector3 _hexalSize = new Vector3(1, 0.5f, 0.75f);

	public GameObject ChunkObject { get; set; }
	public Vector3 ChunkPosition { get { return _chunkPosition; } }
	public byte[,,] Hexals { get { return _hexals; } }

	private readonly Vector3 _chunkPosition;

    public int NeighborCount { get { return NeighboringChunks.Count; } }

    private Dictionary<Vector3, Chunk> NeighboringChunks = new Dictionary<Vector3, Chunk>();

	private byte[,,] _hexals;
    private int _lastDrawFrame;

    private const bool DEBUG_DRAW_NEIGHBORS = true;
    private const bool DEBUG_DRAW_NEIGHBORS_DEPTH_TEST = true;
    private const bool DEBUG_DRAW_NEIGHBORS_ONLY_MISSING = true;

    private const bool DEBUG_DRAW_OUTLINE = true;
    private const bool DEBUG_DRAW_OUTLINE_DEPTH_TEST = true;

    public void DebugChunk()
    {
        if(DEBUG_DRAW_NEIGHBORS)
            DrawNeighbors();

        if (DEBUG_DRAW_OUTLINE)
            DrawOutline();
    }
    private void DrawOutline()
    {
        Utility.DrawCube(
            Utility.GetChunkWorldPosition(_chunkPosition) + HexalSize * ((float)CHUNK_SIZE / 2),
            HexalSize * CHUNK_SIZE,
            Color.blue,
            0,
            DEBUG_DRAW_OUTLINE_DEPTH_TEST);
    }
    private void DrawNeighbors()
    {
        if(Time.frameCount - _lastDrawFrame != 0)
        {
            _lastDrawFrame = Time.frameCount;
        }
        else return;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2)) != 1)
                        continue;

                    Color color = Color.red;

                    if(NeighboringChunks.ContainsKey(ChunkPosition + new Vector3(x, y, z)))
                    {
                        color = Color.green;

                        if (DEBUG_DRAW_NEIGHBORS_ONLY_MISSING)
                            continue;
                    }

                    Vector3 start = Utility.GetChunkWorldPosition(_chunkPosition + Vector3.one / 2);
                    Vector3 end = Utility.GetChunkWorldPosition(_chunkPosition + Vector3.one / 2 + new Vector3(x, y, z) / 2);

                    Debug.DrawLine(start, end, color, 0, DEBUG_DRAW_NEIGHBORS_DEPTH_TEST);
                }
            }
        }
    }
    private void NotifyNeighborsOfCreation()
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2)) != 1)
                        continue;
                    
                    PollNeighbor(new Vector3(x, y, z) + ChunkPosition);   
                }
            }
        }
    }
    private void PollNeighbor(Vector3 neighborPosition)
    {
        if (MapData.Chunks.ContainsKey(neighborPosition))
        {
            AddNeighbor(MapData.Chunks[neighborPosition], true);
        }
    }
    private void AddNeighbor(Chunk chunk, bool notifyNeighbor = false)
    {
        if(NeighboringChunks.ContainsKey(chunk.ChunkPosition))
        {
            NeighboringChunks[chunk.ChunkPosition] = chunk;
        }
        else
        {
            NeighboringChunks.Add(chunk.ChunkPosition, chunk);
        }

        if(notifyNeighbor)
        {
            chunk.AddNeighbor(this, false);
        }
    }
    public List<Vector3> GetMissingNeighbors()
    {
        List<Vector3> list = new List<Vector3>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2)) != 1)
                        continue;

                    Vector3 neighborPosition = new Vector3(x, y, z) + ChunkPosition;

                    if(!MapData.Chunks.ContainsKey(neighborPosition))
                    {
                        list.Add(neighborPosition);
                    }
                }
            }
        }

        return list;
    }
	public bool ContainsHexal(Vector3 localPosition)
	{
		return ContainsHexal ((int)localPosition.x, (int)localPosition.y, (int)localPosition.z);
	}
	public bool ContainsHexal(int x, int y, int z)
	{
		if (
			_hexals.GetLength (0) > x && x >= 0 &&
			_hexals.GetLength (1) > y && y >= 0 &&
			_hexals.GetLength (2) > z && z >= 0)
		{
			return _hexals [x, y, z] != 0;
		}

		return false;
	}
	public byte GetHexal(Vector3 localPosition)
	{
		return GetHexal ((int)localPosition.x, (int)localPosition.y, (int)localPosition.z);
	}
	public byte GetHexal(int x, int y, int z)
	{
		if (!ContainsHexal (x, y, z))
			throw new System.ArgumentOutOfRangeException (string.Format ("Hexal ({0}, {1}, {2}) is out of range. Chunk size is {3}", x, y, z, CHUNK_SIZE));

		return _hexals [x, y, z];
	}

	public override string ToString ()
	{
		return string.Format ("[Chunk: Offset={0}]", ChunkPosition);
	}
}
