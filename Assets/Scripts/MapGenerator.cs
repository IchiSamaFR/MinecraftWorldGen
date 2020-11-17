using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class MapGenerator : MonoBehaviour
{
    [Header("Chunks Size")]
    public bool Speed = true;

    [Header("Chunks Size")]
    public int width = 16;
    public int length = 16;
    public int height = 20;
    public float scale = 0.5f;

    [Header("Chunk")]
    public GameObject chunkPrefab;
    public int viewDistance = 3;

    [Header("Seed")]
    public int seed;

    List<Chunk> mapChunks = new List<Chunk>();
    GameObject[,] chunks = new GameObject[128, 128];

    public GameObject player;
    Vector3 playerPos;
    
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown("x"))
        {
            ChunkCreator();
        }
        if (Input.GetKeyDown("r"))
        {
            seed = Random.Range(0, 100000);
        }
        CheckPlayerPos();
    }

    void CheckPlayerPos()
    {
        int playerX = (int)(player.transform.position.x / 16);
        int playerZ = (int)(player.transform.position.z / 16);

        if(player.transform.position.x < 0)
        {
            playerX -= 1;
        }
        if (player.transform.position.z < 0)
        {
            playerZ -= 1;
        }

        if (playerPos.x != playerX || playerPos.z != playerZ)
        {
            playerPos = new Vector3(playerX, 0, playerZ);
            ChunkCreator();
        }
    }

    void ChunkCreator()
    {
        DeleteMap();

        int xCorrector = 0;
        int zCorrector = 0;
        /*
        if (player.transform.position.x < 0)
        {
            xCorrector -= 1;
        }
        if (player.transform.position.z < 0)
        {
            zCorrector -= 1;
        }*/

        for (int x = 0; x < viewDistance * 2 + 1; x++)
        {
            for (int z = 0; z < viewDistance * 2 + 1; z++)
            {
                if(!(CheckChunkInList((int)(x - viewDistance + playerPos.x + xCorrector), (int)(z - viewDistance + playerPos.z + zCorrector)) >= 0))
                {
                    GameObject newChunk = Instantiate(chunkPrefab, this.transform);
                    newChunk.transform.position = new Vector3((x - viewDistance + playerPos.x + xCorrector) * width, 0, (z - viewDistance + playerPos.z + zCorrector) * length);
                    chunks[x, z] = newChunk;
                    newChunk.GetComponent<Chunk>().Set(width, length, height, scale, (x - viewDistance + (int)playerPos.x + xCorrector), (z - viewDistance + (int)playerPos.z + zCorrector), seed);

                    newChunk.GetComponent<Chunk>().MapGen();

                    mapChunks.Add(newChunk.GetComponent<Chunk>());
                } else
                {
                    mapChunks[CheckChunkInList((int)(x - viewDistance + playerPos.x + xCorrector), (int)(z - viewDistance + playerPos.z + zCorrector))].gameObject.SetActive(true);
                }
            }
        }
    }

    int CheckChunkInList(int x, int z)
    {
        int count = 0;
        foreach (Chunk entity in mapChunks)
        {
            if(entity.xPos == x && entity.zPos == z)
            {
                return count;
            }
            count++;
        }
        return -1;
    }

    void DeleteMap()
    {
        List<GameObject> toDelete = new List<GameObject>();
        foreach (Chunk entity in mapChunks)
        {
            if (!(entity.xPos - playerPos.x >= - viewDistance && entity.xPos - playerPos.x <= viewDistance) ||
                !(entity.zPos - playerPos.z >= -viewDistance && entity.zPos - playerPos.z <= viewDistance))
            {
                entity.gameObject.SetActive(false);
                //toDelete.Add(entity.gameObject);
            }
        }

        foreach (GameObject entity in toDelete)
        {
            mapChunks.Remove(entity.GetComponent<Chunk>());
            Destroy(entity);
        }
    }
}
