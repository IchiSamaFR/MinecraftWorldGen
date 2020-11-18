using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

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
    
    public GameObject[,,] blocks = new GameObject[16, 21, 16];
    public string[,,] blocksString = new string[16, 21, 16];

    bool arrayGenerated = false;
    bool blockGenerated = false;
    
    void Start()
    {

    }

    private void Update()
    {
        if (arrayGenerated && !blockGenerated)
        {
            MapGen();
            blockGenerated = true;
        }
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
        if(x >= 0 && y >= 0 && z >= 0 &&
           x < width && y < height && z < length
           && blocksString[x, y, z] != "air")
        {
            return true;
        }

        return false;
    }

    public void ArrayGen()
    {
        List<Vector3> mapBlocks = new List<Vector3>();

        mapBlocks = NoiseMap.Create(width, length, height, scale, xPos, zPos, seed);

        /*  Geneartion de la map
         *  Incrémentation de 1 a chaque fois que ca monte
         *  Si le pixel est moins haut que la hauteur alors ajouter un block
         */
        int h = 0;
        while (h < height)
        {
            foreach (Vector3 tile in mapBlocks)
            {
                if (tile.y >= h)
                {
                    blocksString[(int)tile.x, (int)h, (int)tile.z] = "block";
                }
                else
                {
                    blocksString[(int)tile.x, (int)h, (int)tile.z] = "air";
                }
            }
            h++;
        }

        arrayGenerated = true;
    }

    public void MapGen()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < length; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    if(blocksString[x, y, z] == null)
                    {
                        blocksString[x, y, z] = "air";
                    }
                    else if (blocksString[x, y, z] != "air")
                    {
                        RefreshBlock(x, y, z);
                    }
                }
            }
        }
    }

    ThreadBlocks thread = new ThreadBlocks(); 
    public void RefreshBlock(int x, int y, int z)
    {
        if(!GetAt(x, y, z))
        {
            return;
        }

        bool _left = false;
        bool _right = false;

        bool _forward = false;
        bool _back = false;

        bool _up = false;
        bool _down = false;

        if (GetAt(x - 1, y, z))
            _left = true;
        if (GetAt(x + 1, y, z))
            _right = true;

        if (GetAt(x, y, z + 1))
            _back = true;
        if (GetAt(x, y, z - 1))
            _forward = true;

        if (GetAt(x, y + 1, z))
            _up = true;
        if (GetAt(x, y - 1, z))
            _down = true;

        Vector3 vec = new Vector3(x, y, z);
        if (!(_down && _up && _forward && _back && _right && _left) && !blocks[(int)vec.x, (int)vec.y, (int)vec.z])
        {
            GameObject cube = Instantiate(grassTilePrefab, this.transform);
            cube.transform.position = this.transform.position + vec;
            blocks[(int)vec.x, (int)vec.y, (int)vec.z] = cube;

            Block b = cube.GetComponent<Block>();
            b.pos = vec;
            b.chunk = this;

            b.BuildMesh(_left, _right, _forward, _back, _up, _down);
        }
        else if (!(_down && _up && _forward && _back && _right && _left) && blocks[(int)vec.x, (int)vec.y, (int)vec.z])
        {
            Block b = blocks[(int)vec.x, (int)vec.y, (int)vec.z].GetComponent<Block>();

            b.BuildMesh(_left, _right, _forward, _back, _up, _down);
        }
        else if(blocks[(int)vec.x, (int)vec.y, (int)vec.z])
        {
            Destroy(blocks[(int)vec.x, (int)vec.y, (int)vec.z]);
        }
    }

    public void BreakBlock(int x, int y, int z)
    {
        Destroy(blocks[x, y, z]);
        blocksString[x, y, z] = "air";
        blocks[x, y, z] = null;

        RefreshBlock(x, y, z);
        RefreshBlock(x - 1, y, z);
        RefreshBlock(x + 1, y, z);
        RefreshBlock(x, y, z + 1);
        RefreshBlock(x, y, z - 1);
        RefreshBlock(x, y + 1, z);
        RefreshBlock(x, y - 1, z);
    }
    public void BreakBlock(Block _block)
    {
        BreakBlock((int)_block.pos.x, (int)_block.pos.y, (int)_block.pos.z);
    }
    public void PlaceBlock(int x, int y, int z)
    {
        blocksString[x, y, z] = "block";
        blocks[x, y, z] = null;

        RefreshBlock(x, y, z);
        RefreshBlock(x - 1, y, z);
        RefreshBlock(x + 1, y, z);
        RefreshBlock(x, y, z + 1);
        RefreshBlock(x, y, z - 1);
        RefreshBlock(x, y + 1, z);
        RefreshBlock(x, y - 1, z);
    }
    public void PlaceBlock(Vector3 _pos)
    {
        PlaceBlock((int)_pos.x, (int)_pos.y, (int)_pos.z);
    }


    /*
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
                    
                cube.GetComponent<Block>().pos = new Vector3(tile.x, tile.y, tile.z);
                cube.GetComponent<Block>().chunk = this;
            }
        }

        foreach(GameObject bl in chunkBlocks)
        {
            bl.GetComponent<Block>().BuildMesh();
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
    */
}
