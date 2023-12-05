using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudnessDetector : MonoBehaviour
{
    // Script is based on the video "How to Use Your Voice as Input in Unity - Microphone and Audio Loudness Detection" by Valem Tutorials 
    // https://www.youtube.com/watch?v=dzD0qP8viLw&list=PLrIb7eNPK270sL7U7174k7xNBLKV70Rlo&index=3

    public int sampleWindow = 64;
    private AudioClip microphoneClip;
    public int deviceNmr;
    
    float totalVolume = 0;


    // Start is called before the first frame update
    void Start()
    {
        //Initialize the microphone audioclip
        MicrophoneToAudioClip();
    }


    public void MicrophoneToAudioClip()
    {
        //Get the first microphone in device list
        string microphoneName = Microphone.devices[deviceNmr];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }


    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[deviceNmr]), microphoneClip);
    }

    
    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        // Samplewindow is used to determine the samples "before" the clip which will be the data that the volume will be detected on.
        // Though the start should not be less than 0, as no data will be found
        int startPosition = clipPosition - sampleWindow;

        if (startPosition < 0)
        {
            return 0;
        }

        //Wavedata is then created based on the startpostion and then with the size of the sampleWindow

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);


        // This for loop is used to return the mean value of the volume in the sample size of the sampleWindow

        for (int i = 0; i < sampleWindow; i++)
        {
            totalVolume += Mathf.Abs(waveData[i]);
        }

        return totalVolume / sampleWindow;
    }

}


