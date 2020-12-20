using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryFire : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        // fire and last for 10 seconds
        //then destroy
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "enemy" == true)
        {
            Destroy(other.gameObject);
        }
    }
}
