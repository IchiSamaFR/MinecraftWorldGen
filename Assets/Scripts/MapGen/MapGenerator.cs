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
    GameObject[,] chunksX = new GameObject[128, 128];
    GameObject[,] chunksZ = new GameObject[128, 128];
    GameObject[,] chunksXZ = new GameObject[128, 128];

    public GameObject player;
    Vector3 playerPos;


    [Header("Stats")]
    public float timeToCheck = 0.2f;
    float timerCheck;
    public bool preGen;
    public int preGendChunks = 4;
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
        if(Time.time < timerCheck + timeToCheck )
        {
            return;
        }

        timerCheck = Time.time;

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

    public void _initmap_()
    {
        for (int x = 0; x < preGendChunks * 2 + 1; x++)
        {
            for (int z = 0; z < preGendChunks * 2 + 1; z++)
            {
                int _xPos = (int)(x - preGendChunks + playerPos.x);
                int _zPos = (int)(z - preGendChunks + playerPos.z);
                CreateChunk(_xPos, _zPos);
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
                int _xPos = (int)(x - viewDistance + playerPos.x);
                int _zPos = (int)(z - viewDistance + playerPos.z);
                CreateChunk(_xPos, _zPos);
            }
        }
    }

    public void CreateChunk(int _xPos, int _zPos)
    {
        Chunk _c = GetChunk(_xPos, _zPos);
        if (!_c)
        {
            GameObject newChunk = Instantiate(chunkPrefab, this.transform);

            newChunk.transform.position = new Vector3(_xPos * width, 0, _zPos * length);
            if (_xPos < 0 && _zPos < 0)
            {
                chunks[-_xPos, -_zPos] = newChunk;
            }
            else if (_xPos < 0)
            {
                chunksZ[-_xPos, _zPos] = newChunk;
            }
            else if (_zPos < 0)
            {
                chunksX[_xPos, -_zPos] = newChunk;
            }
            else
            {
                chunksXZ[_xPos, _zPos] = newChunk;
            }

            Chunk _chunk = newChunk.GetComponent<Chunk>();
            _chunk.Set(this, width, length, height, scale, _xPos, _zPos, seed);

            ThreadChunks.instance.RequestBlock(() => _chunk.ArrayGen());

            mapChunks.Add(newChunk.GetComponent<Chunk>());
        }
        else
        {
            _c.gameObject.SetActive(true);
        }
    }

    public int CheckChunkInList(int x, int z)
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
    public Chunk GetChunk(int _xPos, int _zPos)
    {
        if (_xPos < 0 && _zPos < 0 && chunks[-_xPos, -_zPos] != null)
        {
            return (chunks[-_xPos, -_zPos].GetComponent<Chunk>());
        }
        else if (_xPos < 0 && _zPos >= 0 && chunksZ[-_xPos, _zPos] != null)
        {
            return (chunksZ[-_xPos, _zPos].GetComponent<Chunk>());
        }
        else if (_xPos >= 0 && _zPos < 0 && chunksX[_xPos, -_zPos] != null)
        {
            return (chunksX[_xPos, -_zPos].GetComponent<Chunk>());
        }
        else if(_xPos >= 0 && _zPos >= 0 && chunksXZ[_xPos, _zPos] != null)
        {
            return (chunksXZ[_xPos, _zPos].GetComponent<Chunk>());
        }

        return null;
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
