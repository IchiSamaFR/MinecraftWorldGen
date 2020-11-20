using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCam : MonoBehaviour
{
    public LayerMask layers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Check();
    }


    void Check()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 6, layers))
        {
            GameObject hited = hit.transform.gameObject;
            
            if (hited.GetComponent<Block>())
            {
                if (Input.GetMouseButton(0))
                {
                    hited.GetComponent<Block>().Break();
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    Block b = hited.GetComponent<Block>();
                    Vector3 _vec = hit.point;
                    Vector3 _cor = new Vector3(0.1f, 0.1f, 0.1f);

                    int pX = 0;
                    int pY = 0;
                    int pZ = 0;


                    if (_vec.x == b.transform.position.x)
                    {
                        pX = b.pos[0] - 1;
                        pY = b.pos[1];
                        pZ = b.pos[2];
                    }
                    if (_vec.x == b.transform.position.x + 1)
                    {
                        pX = b.pos[0] + 1;
                        pY = b.pos[1];
                        pZ = b.pos[2];
                    }

                    if (_vec.y == b.transform.position.y)
                    {
                        pX = b.pos[0];
                        pY = b.pos[1] - 1;
                        pZ = b.pos[2];
                    }
                    if (_vec.y == b.transform.position.y + 1)
                    {
                        pX = b.pos[0];
                        pY = b.pos[1] + 1;
                        pZ = b.pos[2];
                    }

                    if (_vec.z == b.transform.position.z)
                    {
                        pX = b.pos[0];
                        pY = b.pos[1];
                        pZ = b.pos[2] - 1;
                    }
                    if (_vec.z == b.transform.position.z + 1)
                    {
                        pX = b.pos[0];
                        pY = b.pos[1];
                        pZ = b.pos[2] + 1;
                    }
                    hited.GetComponent<Block>().chunk.PlaceBlock(pX, pY, pZ);
                }
            }
        }
    }
}
