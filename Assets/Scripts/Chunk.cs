using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Chunk : MonoBehaviour
{
    [Header("Needed")]
    public int width;
    public int length;
    public int height;
    public float scale = 1;

    [Header("Seed")]
    public int xPos;
    public int zPos;

    [Header("Seed")]
    public int seed;

    [Header("Others")]
    public GameObject protoBlock;
    public GameObject grassTilePrefab;


    [Header("Others")]
    public bool timelapse = true;
    public float timeBetweenBlocks = 0.05f;

    List<GameObject> chunkBlocks = new List<GameObject>();
    public GameObject[,,] blocks = new GameObject[16, 21, 16];

    void Start()
    {

    }

    public void Set(int width, int length, int height, float scale, int xPos, int zPos, int seed)
    {
        this.width = width;
        this.length = length;
        this.height = height;
        this.scale = scale;
        this.xPos = xPos;
        this.zPos = zPos;
        this.seed = seed;
    }

    public bool GetAt(int x, int y, int z)
    {
        if(x < 0 || y < 0 || z < 0 ||
           x >= width || y >= height || z >= length)
        {
            return true;
        } else if(blocks[x, y, z])
        {
            return true;
        }

        return false;
    }

    public void MapGen()
    {
        if (timelapse)
        {
            StartCoroutine("IE_MapGen");
        }
        else
        {
            List<Vector3> mapBlocks = new List<Vector3>();

            mapBlocks = NoiseMap.Create(width, length, height, scale, xPos, zPos, seed);

            int h = 0;
            while (h < height)
            {
                foreach (Vector3 tile in mapBlocks)
                {
                    if (tile.y >= h)
                    {

                        GameObject cube = Instantiate(grassTilePrefab, this.transform);
                        cube.transform.position = this.transform.position + new Vector3(tile.x, h, tile.z);
                        chunkBlocks.Add(cube);
                        blocks[(int)tile.x, (int)h, (int)tile.z] = cube;
                    }
                }
                h++;
            }
        }
    }
    IEnumerator IE_MapGen()
    {
        WaitForSeconds wait = new WaitForSeconds(timeBetweenBlocks);

        List<Vector3> mapBlocks = new List<Vector3>();

        mapBlocks = NoiseMap.Create(width, length, height, scale, xPos, zPos, seed);

        int h = 0;
        while (h < height)
        {
            foreach (Vector3 tile in mapBlocks)
            {
                if (tile.y >= h)
                {
                    if (tile.y == h)
                    {
                        GameObject cube = Instantiate(protoBlock, this.transform);
                        cube.transform.position = this.transform.position + new Vector3(tile.x, h, tile.z);
                        chunkBlocks.Add(cube);
                        cube.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                        cube.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
                    }
                    else
                    {
                        GameObject cube = Instantiate(protoBlock, this.transform);
                        cube.transform.position = this.transform.position + new Vector3(tile.x, h, tile.z);
                        chunkBlocks.Add(cube);
                        cube.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                        cube.transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
                    }
                }
            }
            h++;
            yield return wait;
        }
    }

    public void SpeedMapGen()
    {
        if (timelapse)
        {
            StartCoroutine("IE_SpeedMapGen");
        }
        else
        {
            List<Vector3> mapBlocks = new List<Vector3>();

            mapBlocks = NoiseMap.Create(width, length, height, scale, xPos, zPos, seed);

            foreach (Vector3 tile in mapBlocks)
            {
                GameObject cube = Instantiate(grassTilePrefab, this.transform);
                cube.transform.position = this.transform.position + new Vector3(tile.x, tile.y, tile.z);
                chunkBlocks.Add(cube);

                blocks[(int)tile.x, (int)tile.y, (int)tile.z] = cube;
            }
        }
    }

    IEnumerator IE_SpeedMapGen()
    {
        WaitForSeconds wait = new WaitForSeconds(timeBetweenBlocks);

        List<Vector3> mapBlocks = new List<Vector3>();

        mapBlocks = NoiseMap.Create(width, length, height, scale, xPos, zPos, seed);

        
        int h = 0;
        while (h <= height)
        {
            foreach (Vector3 tile in mapBlocks)
            {
                if (tile.y == h)
                {
                    GameObject cube = null;

                    cube = Instantiate(grassTilePrefab, this.transform);
                    cube.transform.position = this.transform.position + new Vector3(tile.x, tile.y, tile.z);
                    chunkBlocks.Add(cube);
                    blocks[(int)tile.x, (int)h, (int)tile.z] = cube;
                    
                    cube.GetComponent<Block>().pos = new Vector3(tile.x, tile.y, tile.z);
                    cube.GetComponent<Block>().chunk = this;
                    
                }
            }
            yield return wait;
            h++;
        }

    }
}
