
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Chunk chunk;
    public int[] pos = new int[3];

    List<int> triangles = new List<int>();
    List<Vector3> vertices = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    List<Vector3> normales = new List<Vector3>();

    void Start()
    {

    }

    bool meshCreated = false;
    bool isBuilded = false;
    /*
    private void Update()
    {
        if (!isBuilded && meshCreated)
        {
            Build();
        }
    }
    */

    /* After Calculate build block
     * 
     */
    void Build()
    {
        if (!chunk.GetAt(pos[0], pos[1], pos[2]))
        {
            return;
        }
        Mesh mesh = new Mesh();

        GetComponent<MeshFilter>().sharedMesh = mesh;

        MeshRenderer meshR = GetComponent<MeshRenderer>();

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.normals = normales.ToArray();

        isBuilded = true;
    }

    public void BuildMeshEditor()
    {
        BuildMesh(false, false, false, false, false, false);
        Build();
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
        normales.Clear();

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

            uvs.Add(new Vector2(0.125f * 0, 0));
            uvs.Add(new Vector2(0.125f * 1, 0));
            uvs.Add(new Vector2(0.125f * 1, 1f));
            uvs.Add(new Vector2(0.125f * 0, 1));

            normales.Add(Vector3.forward);
            normales.Add(Vector3.forward);
            normales.Add(Vector3.forward);
            normales.Add(Vector3.forward);
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

            uvs.Add(new Vector2(0.125f * 1, 0));
            uvs.Add(new Vector2(0.125f * 2, 0));
            uvs.Add(new Vector2(0.125f * 2, 1f));
            uvs.Add(new Vector2(0.125f * 1, 1));

            normales.Add(Vector3.right);
            normales.Add(Vector3.right);
            normales.Add(Vector3.right);
            normales.Add(Vector3.right);

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

            uvs.Add(new Vector2(0.125f * 2, 0));
            uvs.Add(new Vector2(0.125f * 3, 0));
            uvs.Add(new Vector2(0.125f * 3, 1f));
            uvs.Add(new Vector2(0.125f * 2, 1));

            normales.Add(Vector3.back);
            normales.Add(Vector3.back);
            normales.Add(Vector3.back);
            normales.Add(Vector3.back);
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

            uvs.Add(new Vector2(0.125f * 3, 0));
            uvs.Add(new Vector2(0.125f * 4, 0));
            uvs.Add(new Vector2(0.125f * 4, 1f));
            uvs.Add(new Vector2(0.125f * 3, 1));

            normales.Add(Vector3.left);
            normales.Add(Vector3.left);
            normales.Add(Vector3.left);
            normales.Add(Vector3.left);
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

            uvs.Add(new Vector2(0.125f * 4, 0));
            uvs.Add(new Vector2(0.125f * 5, 0));
            uvs.Add(new Vector2(0.125f * 5, 1f));
            uvs.Add(new Vector2(0.125f * 4, 1));

            normales.Add(Vector3.up);
            normales.Add(Vector3.up);
            normales.Add(Vector3.up);
            normales.Add(Vector3.up);
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

            uvs.Add(new Vector2(0.125f * 5, 0));
            uvs.Add(new Vector2(0.125f * 6, 0));
            uvs.Add(new Vector2(0.125f * 6, 1f));
            uvs.Add(new Vector2(0.125f * 5, 1));

            normales.Add(Vector3.down);
            normales.Add(Vector3.down);
            normales.Add(Vector3.down);
            normales.Add(Vector3.down);
        }

        Build();
    }

    public void Break()
    {
        chunk.BreakBlock(this);
    }
}