
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Chunk chunk;
    public Vector3 pos;

    List<int> triangles = new List<int>();
    List<Vector3> vertices = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();

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

    /* After Calculate build block
     * 
     */
    void Build()
    {
        Mesh mesh = new Mesh();

        GetComponent<MeshFilter>().sharedMesh = mesh;

        MeshRenderer meshR = GetComponent<MeshRenderer>();

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();


        mesh.RecalculateNormals();

        isBuilded = true;
    }

    public void BuildMesh()
    {
        BuildMesh(false, false, false, false, false, false);
    }

    /* Calculate of blocks
     * 
     */
    public void BuildMesh(bool left, bool right, bool forward, bool backward, bool up, bool down)
    {
        isBuilded = false;
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();

        /*
        left = false;
        right = false;
        forward = false;
        backward = false;
        up = false;
        down = false;
        */
        /* Triangles start with 3 :
         * 0 - 1  |  0   1
         *   /    |      |
         * 3   2  |  3 - 2
         * 
         * Inverted for back / down / left :
         * 0 - 1  |  0   1
         * |      |    / |
         * 3   2  |  3   2
         * 
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

            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(0.33f, 0));
            uvs.Add(new Vector2(0.33f, 1f));
            uvs.Add(new Vector2(0, 1));
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

            uvs.Add(new Vector2(0.33f, 0));
            uvs.Add(new Vector2(0.66f, 0));
            uvs.Add(new Vector2(0.66f, 0.99f));
            uvs.Add(new Vector2(0.33f, 0.99f));

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

            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(0.33f, 0));
            uvs.Add(new Vector2(0.33f, 1f));
            uvs.Add(new Vector2(0, 1));

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

            uvs.Add(new Vector2(0.66f, 0));
            uvs.Add(new Vector2(1f, 0));
            uvs.Add(new Vector2(1f, 1f));
            uvs.Add(new Vector2(0.66f, 1f));

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

            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(0.33f, 0));
            uvs.Add(new Vector2(0.33f, 1f));
            uvs.Add(new Vector2(0, 1));

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

            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(0.33f, 0));
            uvs.Add(new Vector2(0.33f, 1f));
            uvs.Add(new Vector2(0, 1));

        }
        

        meshCreated = true;
    }

    public void Break()
    {
        chunk.BreakBlock(this);
    }
}