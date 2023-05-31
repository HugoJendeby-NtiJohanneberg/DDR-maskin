using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatScroller : MonoBehaviour
{

    public float beatTempo;

    public bool hasStarted;

    // Start is called before the first frame update
    void Start()
    {
        // units moved per second
        beatTempo = beatTempo / 60f;
    }

    // Update is called once per frame
    void Update()
    {
        

        if(!hasStarted)
        {
            // We don't want to start the game from BeatScroller
            /* if(Input.anyKeyDown)
            {
                hasStarted = true;
            } */
            
        } else
        {
            // move position on x, y, z axis Time.deltaTime stores time difference
            transform.position -= new Vector3(0f, beatTempo * Time.deltaTime, 0f);
        }
    }
}
