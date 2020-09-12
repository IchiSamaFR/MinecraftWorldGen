
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Chunk chunk;
    public Vector3 pos;
    public static List<int> triangles = new List<int>();

    void Start()
    {
        if (!chunk)
        {
            BuildMesh();
        }
    }

    public void BuildMesh(bool left, bool right, bool forward, bool backward, bool up, bool down)
    {

        Mesh mesh = new Mesh();

        mesh.vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, 0, 1),
            new Vector3(0, 0, 1),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 1, 1),
            new Vector3(0, 1, 1),
        };

        List<int> triangles = new List<int>();

        if (left)
        {
            triangles.Add(3);
            triangles.Add(4);
            triangles.Add(0);
            triangles.Add(3);
            triangles.Add(7);
            triangles.Add(4);
        }
        if (right)
        {
            triangles.Add(1);
            triangles.Add(5);
            triangles.Add(2);
            triangles.Add(2);
            triangles.Add(5);
            triangles.Add(6);
        }
        if (forward)
        {
            triangles.Add(0);
            triangles.Add(5);
            triangles.Add(1);
            triangles.Add(0);
            triangles.Add(4);
            triangles.Add(5);
        }
        if (backward)
        {
            triangles.Add(2);
            triangles.Add(7);
            triangles.Add(3);
            triangles.Add(2);
            triangles.Add(6);
            triangles.Add(7);
        }
        if (up)
        {
            triangles.Add(4);
            triangles.Add(6);
            triangles.Add(5);
            triangles.Add(4);
            triangles.Add(7);
            triangles.Add(6);
        }
        if (down)
        {
            triangles.Add(0);
            triangles.Add(1);
            triangles.Add(2);
            triangles.Add(0);
            triangles.Add(2);
            triangles.Add(3);
        }


        int[] test = new int[]{0,1,5,0,5,4,
                                   1,2,5,2,6,5,
                                   2,3,7,2,7,6,
                                   3,0,4,3,4,7,
                                   4,5,6,4,6,7,
                                   0,2,1,0,3,2};

        mesh.triangles = triangles.ToArray();

        GetComponent<MeshFilter>().mesh = mesh;

    }
    public void BuildMesh()
    {
        Vector3 trans = new Vector3(0.5f, 0.5f, 0.5f);
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

}