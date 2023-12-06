using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using static UnityEditor.PlayerSettings;

public class LegDataLogging : MonoBehaviour
{

    //[SerializeField] private TextMeshProUGUI debugText;
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


    // Til mean calc
    private List<float> jolts = new List<float>();
    public int meanCalcAmount;


    //// Et andet forsøg på accelerometer access
    //public Text valores;

    //private XRNode lxRNode = XRNode.RightHand;

    //private List<InputDevice> devices = new List<InputDevice>();
    //private InputDevice device;

    //private bool devicePositionChosen;
    //private Vector3 devicePositionValue = Vector3.zero;
    //private Vector3 prevdevicePositionValue;

    ////Access to the hardware device and gets its information saving it in the variable device
    //void GetDevice()
    //{
    //    InputDevices.GetDevicesAtXRNode(lxRNode, devices);
    //    device = devices[0];

    //}

    //// Checks if the device is enable, if not it takes action and calls the GetDevice function
    //void OnEnable()
    //{
    //    if (!device.isValid)
    //    {
    //        GetDevice();
    //    }
    //}


    // Start is called before the first frame update
    void Start()
    {
        oldTime = Time.time;
        rb = gameObject.GetComponent<Rigidbody>();
    }







    // Update is called once per frame
    void Update()
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
                // ... så tager vi hastigheden af controlleren
                node.TryGetVelocity(out leg_vel);
                //positionRH = handPos.position;
                //Debug.Log(positionRH);

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

                //if (jolts.Count < meanCalcAmount)
                //{
                //    jolts.Add(Mathf.Abs(jolt));
                //}
                //else if (jolts.Count == meanCalcAmount)
                //{
                //    jolts.RemoveAt(0);
                //    jolts.Add(Mathf.Abs(jolt));
                //}

            }
        }


        //if (!device.isValid)
        //{
        //    GetDevice();
        //}

        //// capturing position changes
        //InputFeatureUsage<Vector3> devicePositionsUsage = CommonUsages.devicePosition;
        //// make sure the value is not zero and that it has changed
        //if (devicePositionValue != prevdevicePositionValue)
        //{
        //    devicePositionChosen = false;
        //}

        //if (device.TryGetFeatureValue(devicePositionsUsage, out devicePositionValue) && devicePositionValue != Vector3.zero && !devicePositionChosen)
        //{
        //    valores.text = devicePositionValue.ToString("F3");
        //    prevdevicePositionValue = devicePositionValue;
        //    devicePositionChosen = true;
        //}
        //else if (devicePositionValue == Vector3.zero && devicePositionChosen)
        //{
        //    valores.text = devicePositionValue.ToString("F3");
        //    prevdevicePositionValue = devicePositionValue;
        //    devicePositionChosen = false;
        //}





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

            echoValueForFoot = Mathf.Abs(jolt);


            //for (int i = 0; i < jolts.Count; i++)
            //{
            //    echoValueForFoot += jolts[i];
            //}

            //echoValueForFoot /= jolts.Count;


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
