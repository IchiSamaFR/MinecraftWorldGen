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
                if (Input.GetMouseButtonDown(0))
                {
                    hited.GetComponent<Block>().Break();
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    Vector3 _vec = hit.point;

                    if (_vec.x == hit.transform.position.x)
                    {
                        _vec.x -= 1;
                    }
                    if (_vec.y == hit.transform.position.y)
                    {
                        _vec.y -= 1;
                    }
                    if (_vec.z == hit.transform.position.z)
                    {
                        _vec.z -= 1;
                    }
                    hited.GetComponent<Block>().chunk.PlaceBlock(_vec);
                }
            }
        }
    }
}
