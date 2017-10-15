using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {

	public Chunk(Vector3 position)
	{
		_offset = position;
		_hexals = new byte[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];

		for (int x = 0; x < Hexals.GetLength(0); x++) {
			for (int y = 0; y < Hexals.GetLength(1); y++) {
				for (int z = 0; z < Hexals.GetLength(2); z++) {
					_hexals [x, y, z] = 1;
				}
			}
		}
	}

	public const byte CHUNK_SIZE = 8;

	public Vector3 Offset { get { return _offset; } }
	public byte[,,] Hexals { get { return _hexals; } }

	private byte[,,] _hexals;

	private readonly Vector3 _offset;

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
		return string.Format ("[Chunk: Offset={0}]", Offset);
	}
}
