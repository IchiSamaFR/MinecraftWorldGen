
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Chunk chunk;
    public Vector3 pos;

    List<int> triangles = new List<int>();
    List<Vector3> vertices = new List<Vector3>();

    void Start()
    {
        if (!chunk)
        {
            BuildMesh();
        }
    }

    bool meshCreated = false;
    bool isBuilded = false;
    private void Update()
    {
        if (!isBuilded && meshCreated)
        {
            Build();
        }
    }

    void Build()
    {
        Mesh mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = mesh;
        
        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();


        mesh.Optimize();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        isBuilded = true;
    }

    public void BuildMesh(bool left, bool right, bool forward, bool backward, bool up, bool down)
    {
        isBuilded = false;
        triangles.Clear();

        left = false;
        right = false;
        forward = false;
        backward = false;
        up = false;
        down = false;

        /* Triangles start with 3 :
         * 0 - 1
         *   /
         * 3   2
         * 
         * 0   1
         *     |
         * 3 - 2
         * 
         * Inverted for back / down / left :
         * 0 - 1
         * |  
         * 3   2
         * 
         * 0   1
         *   / |
         * 3   2
         */
        int ind = 0;
        if (!forward)
        {
            triangles.Add(3 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            triangles.Add(0 + 4 * ind);
            triangles.Add(3 + 4 * ind);
            triangles.Add(2 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            ind++;

            vertices.Add(new Vector3(0, 0, 0));
            vertices.Add(new Vector3(1, 0, 0));
            vertices.Add(new Vector3(1, 1, 0));
            vertices.Add(new Vector3(0, 1, 0));
        }
        if (!up)
        {
            triangles.Add(3 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            triangles.Add(0 + 4 * ind);
            triangles.Add(3 + 4 * ind);
            triangles.Add(2 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            ind++;

            vertices.Add(new Vector3(0, 1, 0));
            vertices.Add(new Vector3(1, 1, 0));
            vertices.Add(new Vector3(1, 1, 1));
            vertices.Add(new Vector3(0, 1, 1));
        }
        if (!backward)
        {
            triangles.Add(3 + 4 * ind);
            triangles.Add(0 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            triangles.Add(3 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            triangles.Add(2 + 4 * ind);
            ind++;

            vertices.Add(new Vector3(0, 0, 1));
            vertices.Add(new Vector3(1, 0, 1));
            vertices.Add(new Vector3(1, 1, 1));
            vertices.Add(new Vector3(0, 1, 1));
        }
        if (!down)
        {
            triangles.Add(3 + 4 * ind);
            triangles.Add(0 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            triangles.Add(3 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            triangles.Add(2 + 4 * ind);
            ind++;

            vertices.Add(new Vector3(0, 0, 0));
            vertices.Add(new Vector3(1, 0, 0));
            vertices.Add(new Vector3(1, 0, 1));
            vertices.Add(new Vector3(0, 0, 1));
        }

        if (!left)
        {
            triangles.Add(3 + 4 * ind);
            triangles.Add(0 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            triangles.Add(3 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            triangles.Add(2 + 4 * ind);
            ind++;

            vertices.Add(new Vector3(0, 0, 0));
            vertices.Add(new Vector3(0, 0, 1));
            vertices.Add(new Vector3(0, 1, 1));
            vertices.Add(new Vector3(0, 1, 0));
        }
        if (!right)
        {
            triangles.Add(3 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            triangles.Add(0 + 4 * ind);
            triangles.Add(3 + 4 * ind);
            triangles.Add(2 + 4 * ind);
            triangles.Add(1 + 4 * ind);
            ind++;

            vertices.Add(new Vector3(1, 0, 0));
            vertices.Add(new Vector3(1, 0, 1));
            vertices.Add(new Vector3(1, 1, 1));
            vertices.Add(new Vector3(1, 1, 0));
        }
        

        meshCreated = true;
    }
    public void BuildMesh()
    {
        Vector3 trans = new Vector3(0f, 0f, 0f);
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 0, 0) - trans,
            new Vector3(1, 0, 0) - trans,
            new Vector3(1, 0, 1) - trans,
            new Vector3(0, 0, 1) - trans,
            new Vector3(0, 1, 0) - trans,
            new Vector3(1, 1, 0) - trans,
            new Vector3(1, 1, 1) - trans,
            new Vector3(0, 1, 1) - trans,
        };


        int[] toBuild = new int[]{ 0,5,1,0,4,5,
                                1,5,2,2,5,6,
                                2,7,3,2,6,7,
                                3,4,0,3,7,4,
                                4,6,5,4,7,6,
                                0,1,2,0,2,3};

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = toBuild;
        mesh.Optimize();
        mesh.RecalculateNormals();

    }

    public void Break()
    {
        chunk.BreakBlock(this);
    }
}