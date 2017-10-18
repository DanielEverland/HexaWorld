using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

	[SerializeField]
	private GameObject ChunkPrefab;

	private static HexalBuffer _buffer = new HexalBuffer();
    private static GameObject _chunkPrefab;
    
    private void Awake()
    {
        _chunkPrefab = ChunkPrefab;
    }
    private void Update()
	{
		MapData.Poll ();
	}
	public static void RenderChunk(Chunk chunk)
	{
		CreateChunkGameObject (chunk);

		for (int x = 0; x < chunk.Hexals.GetLength(0); x++)
		{
			for (int y = 0; y < chunk.Hexals.GetLength(1); y++)
			{
				for (int z = 0; z < chunk.Hexals.GetLength(2); z++)
				{
					_buffer.HexalPosition = new Vector3 (x, y, z);
					_buffer.HexalType = chunk.Hexals [x, y, z];

					if(_buffer.HexalType != 0)
						RenderHexal();
				}
			}
		}

		ApplyMeshData ();
	}
	private static void CreateChunkGameObject(Chunk chunk)
	{
		GameObject gameObject = Instantiate (_chunkPrefab);
		chunk.ChunkObject = gameObject;

		gameObject.name = string.Format ("Chunk: {0}", chunk.ChunkPosition);
		gameObject.transform.position = GetChunkWorldPosition(chunk.ChunkPosition) * Chunk.CHUNK_SIZE;

		_buffer.Mesh = new Mesh();
		_buffer.MeshRenderer = gameObject.GetComponent<MeshRenderer> ();
		_buffer.MeshFilter = gameObject.GetComponent<MeshFilter> ();
		_buffer.Chunk = chunk;
		_buffer.GameObject = gameObject;
		_buffer.Vertices = new List<Vector3> ();
		_buffer.Triangles = new List<int> ();
		_buffer.VerticeCount = 0;
	}
	private static Vector3 GetChunkWorldPosition(Vector3 chunkDataPosition)
	{
		chunkDataPosition.z *= Chunk.HexalSize.z;
		chunkDataPosition.y *= Chunk.HexalSize.y;

		return chunkDataPosition;
	}
	private static Vector3 GetHexalWorldPosition(Vector3 hexalDataPosition)
	{
		if (hexalDataPosition.z % 2 == 0)
		{
			hexalDataPosition.x -= Chunk.HexalSize.y;
		}

		hexalDataPosition.y *= Chunk.HexalSize.y;
		hexalDataPosition.z *= Chunk.HexalSize.z;

		return hexalDataPosition;
	}
	private static void RenderHexal()
	{
		Vector3 worldPosition = GetHexalWorldPosition (_buffer.HexalPosition);

		_buffer.Vertices.AddRange(new List<Vector3>(12)
		{
			new Vector3(-Chunk.HexalSize.x / 2  , Chunk.HexalSize.y / 2  	, 0.25f)	+ worldPosition,
			new Vector3(Chunk.HexalSize.x / 2   , Chunk.HexalSize.y / 2   	, 0.5f)		+ worldPosition,
			new Vector3(Chunk.HexalSize.x / 2	, Chunk.HexalSize.y / 2     , 0.25f)	+ worldPosition,
                                  
			new Vector3(-Chunk.HexalSize.x / 2  , Chunk.HexalSize.y / 2  	, -0.25f)	+ worldPosition,
			new Vector3(Chunk.HexalSize.x / 2   , Chunk.HexalSize.y / 2   	, -0.5f)	+ worldPosition,
			new Vector3(Chunk.HexalSize.x / 2   , Chunk.HexalSize.y / 2     , -0.25f)	+ worldPosition,


			new Vector3(-Chunk.HexalSize.x / 2  , -Chunk.HexalSize.y / 2  	, 0.25f)	+ worldPosition,
			new Vector3(Chunk.HexalSize.x / 2   , -Chunk.HexalSize.y / 2   	, 0.5f)		+ worldPosition,
			new Vector3(Chunk.HexalSize.x / 2   , -Chunk.HexalSize.y / 2    , 0.25f)	+ worldPosition,

			new Vector3(-Chunk.HexalSize.x / 2  , -Chunk.HexalSize.y / 2  	, -0.25f)	+ worldPosition,
			new Vector3(Chunk.HexalSize.x / 2   , -Chunk.HexalSize.y / 2   	, -0.5f)	+ worldPosition,
			new Vector3(Chunk.HexalSize.x / 2   , -Chunk.HexalSize.y / 2    , -0.25f)	+ worldPosition,
		});

		AddTriangles (
			(int)(_buffer.Chunk.ChunkPosition.x * Chunk.CHUNK_SIZE + _buffer.HexalPosition.x),
			(int)(_buffer.Chunk.ChunkPosition.y * Chunk.CHUNK_SIZE + _buffer.HexalPosition.y),
			(int)(_buffer.Chunk.ChunkPosition.z * Chunk.CHUNK_SIZE + _buffer.HexalPosition.z));

		_buffer.VerticeCount += 12;
	}
	/// <summary>
	/// Adds triangles to mesh.
	/// </summary>
	/// <param name="x">The global x coordinate.</param>
	/// <param name="y">The global y coordinate.</param>
	/// <param name="z">The global z coordinate.</param>
	private static void AddTriangles(int x, int y, int z)
	{
		int vertexCount = _buffer.VerticeCount;

		//Top
		if (!Utility.HexalExists (x, y + 1, z))
		{
			_buffer.Triangles.AddRange (new List<int> (12) {
				vertexCount + 0, vertexCount + 1, vertexCount + 2,
				vertexCount + 0, vertexCount + 2, vertexCount + 5,

				vertexCount + 5, vertexCount + 3, vertexCount + 0,
				vertexCount + 5, vertexCount + 4, vertexCount + 3,
			});
		}

		//Bottom
		if (!Utility.HexalExists (x, y - 1, z))
		{
			_buffer.Triangles.AddRange (new List<int> (12) {
				vertexCount + 8, vertexCount + 7, vertexCount + 6,
				vertexCount + 11, vertexCount + 8, vertexCount + 6,

				vertexCount + 6, vertexCount + 9, vertexCount + 11,
				vertexCount + 9, vertexCount + 10, vertexCount + 11,
			});
		}

		//0-1 Side
		if (!Utility.HexalExists (x - 1, y, z + 1)) {
			_buffer.Triangles.AddRange (new List<int> (6) {
				vertexCount + 7, vertexCount + 1, vertexCount + 0,
				vertexCount + 0, vertexCount + 6, vertexCount + 7,
			});
		}

		//1-2 Side
		if (!Utility.HexalExists (x + 1, y, z + 1)) {
			_buffer.Triangles.AddRange (new List<int> (6) {
				vertexCount + 7, vertexCount + 2, vertexCount + 1,
				vertexCount + 2, vertexCount + 7, vertexCount + 8,
			});
		}

		//2-3 Side
		if (!Utility.HexalExists (x + 1, y, z)) {
			_buffer.Triangles.AddRange (new List<int> (6) {
				vertexCount + 5, vertexCount + 2, vertexCount + 8,
				vertexCount + 5, vertexCount + 8, vertexCount + 11,
			});
		}

		//3-4 Side
		if (!Utility.HexalExists (x + 1, y, z - 1)) {
			_buffer.Triangles.AddRange (new List<int> (6) {
				vertexCount + 4, vertexCount + 5, vertexCount + 11,
				vertexCount + 11, vertexCount + 10, vertexCount + 4,
			});
		}

		//4-5 Side
		if (!Utility.HexalExists (x - 1, y, z - 1)) {
			_buffer.Triangles.AddRange (new List<int> (6) {
				vertexCount + 3, vertexCount + 4, vertexCount + 10,
				vertexCount + 10, vertexCount + 9, vertexCount + 3,
			});
		}

		//5-6 Side
		if (!Utility.HexalExists (x - 1, y, z)) {
			_buffer.Triangles.AddRange (new List<int> (6) {
				vertexCount + 3, vertexCount + 6, vertexCount + 0,
				vertexCount + 3, vertexCount + 9, vertexCount + 6,
			});
		}
	}
	private static void ApplyMeshData()
	{
		_buffer.Mesh.vertices = _buffer.Vertices.ToArray();
		_buffer.Mesh.triangles = _buffer.Triangles.ToArray();

		_buffer.Mesh.RecalculateNormals ();
		_buffer.Mesh.RecalculateTangents ();
		_buffer.Mesh.RecalculateBounds ();

		_buffer.MeshFilter.mesh = _buffer.Mesh;
	}
	private struct HexalBuffer
	{
		public Chunk Chunk;
		public GameObject GameObject;

		public Vector3 HexalPosition;
		public byte HexalType; 
		public int VerticeCount;

		public Mesh Mesh;
		public MeshRenderer MeshRenderer;
		public MeshFilter MeshFilter;
		public List<Vector3> Vertices;
		public List<int> Triangles;
	}
}
