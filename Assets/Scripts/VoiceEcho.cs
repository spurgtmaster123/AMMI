using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceEcho : MonoBehaviour
{
    public AudioSource source;
    public AudioLoudnessDetector detector;

    public float loudnessSensibility = 100;
    public float threshold = 0.1f;
    //1 is no smoothness 0.1 is full smooth
    public float smoothness = 1;

    public GameObject echo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensibility;

        if (loudness < threshold)
        {

            loudness = 0;

            Instantiate(echo, this.transform);
        }
    }
}
