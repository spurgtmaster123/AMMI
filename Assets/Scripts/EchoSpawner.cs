using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoSpawner : MonoBehaviour
{

    //Script based the video "3D Scanner effect Tutorial -Unity Shader Graph" by Game Slave
    // https://www.youtube.com/watch?v=yiTF4rJu6tY&list=PLrIb7eNPK270sL7U7174k7xNBLKV70Rlo&index=4


    //Speed determines the rate of upscaling the sphere on which the shadergraph script is located. 
    // Timer determines the time before the sphere is destroyed again

    public float speed = 2;
    public float timer = 2;

    // Start is called before the first frame update
    void Start()
    {
        destroy_object();
    }

    // Update is called once per frame
    // 
    void Update()
    {
        echoGrow();

    }

    // This changes the scale of the sphere over time
    private void echoGrow()
    {
        Vector3 vectormesh = this.transform.localScale;
        float growing = this.speed * Time.deltaTime;
        this.transform.localScale = new Vector3(vectormesh.x + growing, vectormesh.y + growing, vectormesh.z + growing);
    }

    // This removes the sphere from the game after the set time has passed
    private void destroy_object ()
    {
        Destroy(this.gameObject, timer);
    
    }

}
