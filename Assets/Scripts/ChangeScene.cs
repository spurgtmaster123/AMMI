using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "newScene")
        {
            // Debug.Log("tag found");
            SceneManager.LoadScene("LevelTwo");
        }
    }
}
