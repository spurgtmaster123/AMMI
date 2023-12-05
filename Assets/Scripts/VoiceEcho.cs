using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceEcho : MonoBehaviour
{
    // Script is based on the video "How to Use Your Voice as Input in Unity - Microphone and Audio Loudness Detection" by Valem Tutorials 
    // https://www.youtube.com/watch?v=dzD0qP8viLw&list=PLrIb7eNPK270sL7U7174k7xNBLKV70Rlo&index=3
    // And  "3D Scanner effect Tutorial -Unity Shader Graph" by Game Slave
    // https://www.youtube.com/watch?v=yiTF4rJu6tY&list=PLrIb7eNPK270sL7U7174k7xNBLKV70Rlo&index=4


    //1 is no smoothness 0.1 is full smooth
    // public float smoothness = 1;

    //Calling the AudioLoudnessDetector script
    public AudioLoudnessDetector detector;

    public float loudnessBooster = 100;
    public float threshold = 0.1f;

    public float cooldown = 1;

    public bool startEcho = true;

    public GameObject echo;

    // Update is called once per frame
    void Update()
    {
        //The script will continously look for the input from the microphone and when the threshold is reached,
        //then the timer routine will start, spawning the echosphere.
        //Loudness is then reset to zero until the next michrophone input
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessBooster;
        
        if (loudness > threshold)
        {
            if (startEcho) { StartCoroutine(timer()); }
            loudness = 0;
        }
    }

    //This limits the amount of echospheres that can be spawned even while continously making noise.
    public IEnumerator timer()
    {
        Instantiate(echo, this.transform);

        startEcho = false;

        yield return new WaitForSeconds(cooldown);

        startEcho = true;


    }

}
