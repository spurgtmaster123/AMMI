using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceEcho : MonoBehaviour
{
    //public AudioSource source;
    public AudioLoudnessDetector detector;

    public float loudnessSensibility = 100;
    public float threshold = 0.1f;
    //1 is no smoothness 0.1 is full smooth
    public float smoothness = 1;

    public float cooldown = 1;

    private bool startEcho = true;

    public GameObject echo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;

        Debug.Log(loudness);

     

        if (loudness > threshold)
        {
            if (startEcho) { StartCoroutine(timer()); }
            loudness = 0;

            
        }
    }

    IEnumerator timer()
    {
        
        Instantiate(echo, this.transform);

        startEcho = false;

        yield return new WaitForSeconds(cooldown);

        startEcho = true;


    }

}
