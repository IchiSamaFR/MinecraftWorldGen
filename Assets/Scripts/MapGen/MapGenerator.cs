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


    [Header("Stats")]
    public bool preGen;
    public float waitTimer = 0.2f;

    void Start()
    {
        if(preGen)
            _initmap_();
    }

    void Update()
    {
        if (preGen)
        {
            if (Time.time > waitTimer)
            {
                CheckPlayerPos();
            }
        }
        else
        {
            CheckPlayerPos();
        }

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

        if (playerPos.x != playerX || playerPos.x == 0 || playerPos.z != playerZ || playerPos.z == 0)
        {
            playerPos = new Vector3(playerX, 0, playerZ);
            ChunkCreator();
        }
    }

    void _initmap_()
    {
        int ToGen = 3;
        for (int x = 0; x < ToGen * 2 + 1; x++)
        {
            for (int z = 0; z < ToGen * 2 + 1; z++)
            {
                if (!(CheckChunkInList((int)(x - ToGen + playerPos.x), (int)(z - ToGen + playerPos.z)) >= 0))
                {
                    GameObject newChunk = Instantiate(chunkPrefab, this.transform);
                    newChunk.transform.position = new Vector3((x - ToGen + playerPos.x) * width, 0, (z - ToGen + playerPos.z) * length);
                    chunks[x, z] = newChunk;
                    Chunk _chunk = newChunk.GetComponent<Chunk>();
                    _chunk.Set(width, length, height, scale, (x - ToGen + (int)playerPos.x), (z - ToGen + (int)playerPos.z), seed);

                    _chunk.ArrayGen();

                    mapChunks.Add(newChunk.GetComponent<Chunk>());
                }
                else
                {
                    mapChunks[CheckChunkInList((int)(x - ToGen + playerPos.x), (int)(z - ToGen + playerPos.z))].gameObject.SetActive(true);
                }
            }
        }
    }

    ThreadBlocks thread = new ThreadBlocks();
    void ChunkCreator()
    {
        DeleteMap();
        
        for (int x = 0; x < viewDistance * 2 + 1; x++)
        {
            for (int z = 0; z < viewDistance * 2 + 1; z++)
            {
                if(!(CheckChunkInList((int)(x - viewDistance + playerPos.x), (int)(z - viewDistance + playerPos.z)) >= 0))
                {
                    GameObject newChunk = Instantiate(chunkPrefab, this.transform);
                    newChunk.transform.position = new Vector3((x - viewDistance + playerPos.x) * width, 0, (z - viewDistance + playerPos.z) * length);
                    chunks[x, z] = newChunk;
                    Chunk _chunk = newChunk.GetComponent<Chunk>();
                    _chunk.Set(width, length, height, scale, (x - viewDistance + (int)playerPos.x), (z - viewDistance + (int)playerPos.z), seed);

                    System.Action _action = () => _chunk.ArrayGen();
                    thread.AddToThread(_action);

                    mapChunks.Add(newChunk.GetComponent<Chunk>());
                } else
                {
                    mapChunks[CheckChunkInList((int)(x - viewDistance + playerPos.x), (int)(z - viewDistance + playerPos.z))].gameObject.SetActive(true);
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
