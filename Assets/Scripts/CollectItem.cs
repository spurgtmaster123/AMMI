using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private int collectCount = 0;
    public GameObject door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Collect")
        {
            collectCount++;
            Destroy(other.gameObject);
        }
    }

    private void Update()
    {
        if (collectCount >= 3)
        {
            OpenDoor();
            collectCount = 0;
        }
    }

    private void OpenDoor()
    {
        door.SetActive(true);
    }
}