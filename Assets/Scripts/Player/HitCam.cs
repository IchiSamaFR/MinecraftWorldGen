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


                    if (_vec.x == b.transform.position.x)
                    {
                        _vec = new Vector3(b.pos.x - 1, b.pos.y, b.pos.z) + _cor;
                    }
                    if (_vec.x == b.transform.position.x + 1)
                    {
                        _vec = new Vector3(b.pos.x + 1, b.pos.y, b.pos.z) + _cor;
                    }

                    if (_vec.y == b.transform.position.y)
                    {
                        _vec = new Vector3(b.pos.x, b.pos.y - 1, b.pos.z) + _cor;
                    }
                    if (_vec.y == b.transform.position.y + 1)
                    {
                        _vec = new Vector3(b.pos.x, b.pos.y + 1, b.pos.z) + _cor;
                    }

                    if (_vec.z == b.transform.position.z)
                    {
                        _vec = new Vector3(b.pos.x, b.pos.y, b.pos.z - 1) + _cor;
                    }
                    if (_vec.z == b.transform.position.z + 1)
                    {
                        _vec = new Vector3(b.pos.x, b.pos.y, b.pos.z + 1) + _cor;
                    }
                    hited.GetComponent<Block>().chunk.PlaceBlock(_vec);
                }
            }
        }
    }
}
