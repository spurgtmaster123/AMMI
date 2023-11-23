using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using static UnityEditor.PlayerSettings;

public class LegDataLogging : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI debugText;
    private Vector3 leg_vel;
    public Vector3 leg_vel_threshold;

    private Vector3 positionRH;
    public Vector3 leg_acc_threshold;

    private List<float> leg_velocity = new List<float>();

    float OldMax;
    float OldMin;
    float NewMax = 1;
    float NewMin = 0;

    float OldValue;
    float NewValue;
    float NewRange;

    Vector3 newVel;

    Vector3 oldVel = new Vector3 (0,0,0);

    float oldTime;
    float newTime;

    float accel;

    float newAccel;

    float oldAccel = 0;

    float jolt;

    public float echoValueForFoot;

    public Transform handPos;

    Rigidbody rb;

    public VoiceEcho voiceEcho;
    private bool startEcho = true;
    private float cooldown;
    public GameObject echo;
    private float threshold;

    // Start is called before the first frame update
    void Start()
    {
        oldTime = Time.time;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Laver list over nodes
        List<XRNodeState> nodes = new List<XRNodeState>();
        // Vi tager alle node states som HMD'en har og tilføjer dem til listen
        InputTracking.GetNodeStates(nodes);
        // Et foreach loop hvor vi kører igennem alle nodes
        foreach (XRNodeState node in nodes)
        {
            // Hvis nodetypen er den controller der sidder på benet...
            if (node.nodeType == XRNode.RightHand)
            {
                // ... så tager vi hastigheden af controlleren, debugger
                node.TryGetVelocity(out leg_vel);
                positionRH = handPos.position;
                Debug.Log(positionRH);
                //node.TryGetAcceleration(out leg_acc);
                //debugText.SetText(leg_vel.ToString());
                //Debug.Log("Leg vel: " + leg_vel);
                //Debug.Log(leg_acc);
                //VelToJolt(leg_vel);

                // Start med at sætte de nyhentede værdier til "newVel" og "newTime"
                newVel = leg_vel;
                newTime = Time.time;

                //Debug.Log("New vel: " + newVel);
                // Derefter bliver accelerationen beregnet for denne frame, via differentiering (forskellen af velocity fra denne frame til sidste frame over forskellen af tiden for nuværende frame og sidste frame.)
                accel = (newVel.z - oldVel.z) / (newTime - oldTime);

                //Debug.Log("Accel: " + accel);

                // Så bliver newAccel defineret som den nuværende beregnet accel
                newAccel = accel;

                // Nu kan jolt bliver berenget ved at differentiere accelerationen
                jolt = (newAccel - oldAccel) / (newTime - oldTime);

                //Debug.Log("Jolt: " + jolt);

                // Til sidst sættes oldVel, oldTime og oldAccel til værdierne for den nuværende frame, da det skal bruges
                // til næste frames beregning
                oldVel = leg_vel;
                oldTime = Time.time;

                oldAccel = accel;



                // Send data script ting
                // echolocation.LegSignal(leg_vel.z);

                // Og hvis hastigheden nedad er højere end threshold for signal, så sendes et signal
                if (jolt <= leg_vel_threshold.z)
                {
                    // Her er kode der sender et signal til script om at de skal lave texture-ting

                    //Debug.Log("LEG");

                }

            }
        }



    }

    private void VelToJolt(Vector3 leg_vel)
    {
        // Start med at sætte de nyhentede værdier til "newVel" og "newTime"
        leg_vel = newVel;
        newTime = Time.time;

        Debug.Log("New vel: " + newVel);
        // Derefter bliver accelerationen beregnet for denne frame
        accel = (newVel.z - oldVel.z) / (newTime - oldTime);

        Debug.Log("Accel: " + accel);

        // Så bliver newAccel defineret som den nuværende beregnet accel
        newAccel = accel;
        
        // New
        jolt = (newAccel - oldAccel) / (newTime - oldTime);

        //Debug.Log(jolt);

        leg_vel = oldVel;
        oldTime = Time.time;

        oldAccel = accel;
    }



    void AdjustLegValue(float z_value)
    {

        float newValue = Mathf.InverseLerp(z_value, OldMax, 0);


        float OldRange = (OldMax - OldMin);
        if (OldRange == 0)
            NewValue = NewMin;
        else
        {
            NewRange = (NewMax - NewMin);
            NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
        }



    }


    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("Floor"))
        {
            echoValueForFoot = rb.velocity.y;

            echoValueForFoot = jolt;

            //if (voiceEcho.startEcho)
            //{
            //    StartCoroutine(voiceEcho.timer());
            //}

            if (echoValueForFoot > threshold)
            {
                if (startEcho) { StartCoroutine(timer()); }
                echoValueForFoot = 0;


            }



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
