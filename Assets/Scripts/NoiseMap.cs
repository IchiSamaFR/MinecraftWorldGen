using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseMap
{
    public static List<Vector3> Create(int width, int length, int height, float scale, int xPos, int zPos, int seed)
    {
        List<Vector3> mapTiles = new List<Vector3>();

        float z = 0.0f;
        float newXPos = xPos * scale;
        float newZPos = zPos * scale;

        while (z < length)
        {
            float x = 0.0f;
            while (x < width)
            {
                float xCoord = seed + newXPos + x / width * scale;
                float zCoord = seed + newZPos + z / length * scale;
                float yCoord = Mathf.PerlinNoise(xCoord, zCoord) * height;

                if(yCoord < 0)
                {
                    yCoord = 0;
                } else if (yCoord > height)
                {
                    yCoord = height;
                }

                mapTiles.Add(new Vector3((int)x, (int)yCoord, (int)z));
                x++;
            }
            z++;
        }

        return mapTiles;
    }
}
