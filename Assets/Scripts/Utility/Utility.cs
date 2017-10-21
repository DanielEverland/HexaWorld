using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {

    public static void DrawCube(Vector3 center, Vector3 size)
    {
        DrawCube(center, size, Color.white);
    }
    public static void DrawCube(Vector3 center, Vector3 size, Color color, float duration = 0, bool depthTest = true)
    {
        Vector3 bottomBackRight     =   center - new Vector3(0.5f   * size.x,   -0.5f   * size.y,   -0.5f   * size.z);
        Vector3 bottomBackLeft      =   center - new Vector3(-0.5f  * size.x,   -0.5f   * size.y,   -0.5f   * size.z);
        Vector3 bottomForwardRight  =   center - new Vector3(0.5f   * size.x,   -0.5f   * size.y,   0.5f    * size.z);
        Vector3 bottomForwardLeft   =   center - new Vector3(-0.5f  * size.x,   -0.5f   * size.y,   0.5f    * size.z);

        Vector3 topBackRight        =   center - new Vector3(0.5f   * size.x,   0.5f    * size.y,   -0.5f   * size.z);
        Vector3 topBackLeft         =   center - new Vector3(-0.5f  * size.x,   0.5f    * size.y,   -0.5f   * size.z);
        Vector3 topForwardRight     =   center - new Vector3(0.5f   * size.x,   0.5f    * size.y,   0.5f    * size.z);
        Vector3 topForwardLeft      =   center - new Vector3(-0.5f  * size.x,   0.5f    * size.y,   0.5f    * size.z);

        Debug.DrawLine(bottomBackLeft,      bottomBackRight,    color, duration, depthTest);
        Debug.DrawLine(bottomBackRight,     bottomForwardRight, color, duration, depthTest);
        Debug.DrawLine(bottomForwardRight,  bottomForwardLeft,  color, duration, depthTest);
        Debug.DrawLine(bottomForwardLeft,   bottomBackLeft,     color, duration, depthTest);

        Debug.DrawLine(bottomBackLeft,      topBackLeft,        color, duration, depthTest);
        Debug.DrawLine(bottomBackRight,     topBackRight,       color, duration, depthTest);
        Debug.DrawLine(bottomForwardRight,  topForwardRight,    color, duration, depthTest);
        Debug.DrawLine(bottomForwardLeft,   topForwardLeft,     color, duration, depthTest);

        Debug.DrawLine(topBackLeft,         topBackRight,       color, duration, depthTest);
        Debug.DrawLine(topBackRight,        topForwardRight,    color, duration, depthTest);
        Debug.DrawLine(topForwardRight,     topForwardLeft,     color, duration, depthTest);
        Debug.DrawLine(topForwardLeft,      topBackLeft,        color, duration, depthTest);
    }
    public static float ClampEulerAngle(float angle, float min, float max)
    {
        angle = angle % 360;

        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
        }

        return Mathf.Clamp(angle, min, max);
    }
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
    public static Vector3 WorldToChunkSpace(float x, float y, float z)
    {
        return WorldToChunkSpace(new Vector3(x, y, z));
    }
    public static Vector3 WorldToChunkSpace(Vector3 worldSpace)
    {
        return new Vector3(
            WorldToChunkSpaceAxis(worldSpace.x),
            WorldToChunkSpaceAxis(worldSpace.y),
            WorldToChunkSpaceAxis(worldSpace.z));
    }
    public static float WorldToChunkSpaceAxis(float global)
    {
        float local = global % Chunk.CHUNK_SIZE;

        if (local < 0)
            local += Chunk.CHUNK_SIZE;

        return local;
    }
    public static bool HexalExists(int x, int y, int z)
	{
		Chunk chunk = MapData.GetChunk (
			Mathf.FloorToInt(x / Chunk.CHUNK_SIZE), 
			Mathf.FloorToInt(y / Chunk.CHUNK_SIZE), 
			Mathf.FloorToInt(z / Chunk.CHUNK_SIZE));
        
		if (chunk == null)
			return false;
                
        return chunk.ContainsHexal(WorldToChunkSpace(x, y, z));
	}
}
