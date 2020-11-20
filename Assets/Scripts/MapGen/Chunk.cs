using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    MapGenerator map;

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

    public void Set(MapGenerator map, int width, int length, int height, float scale, int xPos, int zPos, int seed)
    {
        this.map = map;
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
        else if (x < 0)
        {
            Chunk _c = map.GetChunk(xPos - 1, zPos);
            if(_c != null)
            {
                return _c.GetAt(x + width, y, z);
            }
            else
            {
                return false;
            }
        }
        else if (x >= width)
        {
            Chunk _c = map.GetChunk(xPos + 1, zPos);
            if (_c != null)
            {
                return _c.GetAt(x - width, y, z);
            }
            else
            {
                return false;
            }
        }
        else if (z < 0)
        {
            Chunk _c = map.GetChunk(xPos, zPos - 1);
            if (_c != null)
            {
                return _c.GetAt(x, y, z + length);
            }
            else
            {
                return false;
            }
        }
        else if (z >= length)
        {
            Chunk _c = map.GetChunk(xPos, zPos + 1);
            if (_c != null)
            {
                return _c.GetAt(x, y, z - length);
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    public bool ThisGetAt(int x, int y, int z)
    {
        if (x >= 0 && y >= 0 && z >= 0 &&
           x < width && y < height && z < length
           && blocksString[x, y, z] != "air")
        {
            return true;
        }
        else
        {
            return false;
        }
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

        MapGen();
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
        if(!ThisGetAt(x, y, z))
        {
            if (x < 0)
            {
                Chunk _c = map.GetChunk(xPos - 1, zPos);
                if (_c != null)
                {
                    _c.RefreshBlock(x + width, y, z);
                }
            }
            else if (x >= width)
            {
                Chunk _c = map.GetChunk(xPos + 1, zPos);
                if (_c != null)
                {
                    _c.RefreshBlock(x - width, y, z);
                }
            }
            else if (z < 0)
            {
                Chunk _c = map.GetChunk(xPos, zPos - 1);
                if (_c != null)
                {
                    _c.RefreshBlock(x, y, z + length);
                }
            }
            else if (z >= length)
            {
                Chunk _c = map.GetChunk(xPos, zPos + 1);
                if (_c != null)
                {
                    _c.RefreshBlock(x, y, z - length);
                }
            }
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

        int[] vec = new int[] { x, y, z };
        if (!(_down && _up && _forward && _back && _right && _left) && !blocks[vec[0], vec[1], vec[2]])
        {
            GameObject cube = Instantiate(grassTilePrefab, this.transform);
            cube.transform.position = this.transform.position + new Vector3(vec[0], vec[1], vec[2]);
            blocks[vec[0], vec[1], vec[2]] = cube;

            Block b = cube.GetComponent<Block>();
            b.pos = vec;
            b.chunk = this;

            ThreadBlocks.instance.RequestBlock(() => b.BuildMesh(_left, _right, _forward, _back, _up, _down));
        }
        else if (!(_down && _up && _forward && _back && _right && _left) && blocks[vec[0], vec[1], vec[2]])
        {
            Block b = blocks[vec[0], vec[1], vec[2]].GetComponent<Block>();

            ThreadBlocks.instance.RequestBlock(() => b.BuildMesh(_left, _right, _forward, _back, _up, _down));
        }
        else if(blocks[vec[0], vec[1], vec[2]])
        {
            Destroy(blocks[vec[0], vec[1], vec[2]]);
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
        BreakBlock(_block.pos[0], _block.pos[1], _block.pos[2]);
    }

    public void PlaceBlock(int x, int y, int z)
    {
        if (x < 0)
        {
            Chunk _c = map.GetChunk(xPos - 1, zPos);
            _c.PlaceBlock(x + width, y, z);
            return;
        }
        else if (x >= width)
        {
            Chunk _c = map.GetChunk(xPos + 1, zPos);
            _c.PlaceBlock(x - width, y, z);
            return;
        }
        else if (y < 0 || y >= height)
        {
            return;
        }
        else if (z < 0)
        {
            Chunk _c = map.GetChunk(xPos, zPos - 1);
            _c.PlaceBlock(x, y, z + length);
            return;
        }
        else if (z >= length)
        {
            Chunk _c = map.GetChunk(xPos, zPos + 1);
            _c.PlaceBlock(x, y, z - length);
            return;
        }

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
    public void PlaceBlock(int[] _pos)
    {
        PlaceBlock(_pos[0], _pos[1], _pos[2]);
    }
    
}
