using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoSpawner : MonoBehaviour
{

    [Header("speed")]
    public float speed = 2;

    [Header("Distance")]
    public float distance = 2;

   


   
  

   


    // Start is called before the first frame update
    void Start()
    {
        
        destroy_object();


    }

    // Update is called once per frame
    void Update()
    {
        Vector3 vectormesh = this.transform.localScale;
        float growing = this.speed * Time.deltaTime;
        this.transform.localScale = new Vector3(vectormesh.x + growing, vectormesh.y + growing, vectormesh.z + growing);



       


    }


    private void destroy_object ()
    {
        Destroy(this.gameObject, distance);
    
    }



}
