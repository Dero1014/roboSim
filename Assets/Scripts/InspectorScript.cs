﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InspectorScript : MonoBehaviour
{
    //za aktiviranje inspektor komponenata
    public GameObject itemsInspector;
    public GameObject proxyInspector;
    public GameObject robotInspector;
    public GameObject treadInspector;

    [Space(20)]
    public new TextMeshProUGUI name; //prikazuje ime objekta
    private GameObject interest; // pamti izabrani objekt

    // prikazuju vrijednosti objekta
    public TMP_InputField[] pos;
    public TMP_InputField[] scal;
    public TMP_InputField[] rot;
    //

    [Space(10)]// ignore
    public TMP_InputField proxyDistanceDetection;
    public TMP_InputField treadSpeed;
    public TMP_InputField mass;
    [Space(10)]
    public Toggle phyToggle;
    public Toggle simToggle;

    private Rigidbody rb;
    //ignore end

    private bool itemIns = false;
    private bool proxIns = false;
    private bool laseIns = false;
    private bool roboIns = false;
    private bool treadIns = false;
    private bool physicsToggle = true; //ignore

    Vector3 rotatio = Vector3.zero;

    private void Start()
    {
        rot[0].text = 0.ToString();
        rot[1].text = 0.ToString();
        rot[2].text = 0.ToString();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitIns;
        
        //selection process
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hitIns))
            {
                if (hitIns.transform.gameObject.layer == 8)
                {
                    interest = hitIns.transform.gameObject;
                    name.text = interest.name;
                    rb = interest.GetComponent<Rigidbody>();
                    phyToggle.isOn = rb.useGravity;
                    treadIns = false;
                    laseIns = false;
                    proxIns = false;
                    itemIns = true;
                    roboIns = false;
                }

                if (hitIns.transform.gameObject.tag == "Proxy")
                {
                    interest = hitIns.transform.gameObject;
                    name.text = interest.name;
                    treadIns = false;
                    laseIns = false;
                    proxIns = true;
                    itemIns = false;
                    roboIns = false;
                }

                if (hitIns.transform.gameObject.tag == "Laser")
                {
                    interest = hitIns.transform.gameObject;
                    name.text = interest.name;
                    treadIns = false;
                    laseIns = true;
                    proxIns = false;
                    itemIns = false;
                    roboIns = false;
                }

                if (hitIns.transform.gameObject.tag == "TreadMill")
                {
                    interest = hitIns.transform.gameObject;
                    while (interest.transform.parent!=null)
                    {
                        interest = interest.transform.parent.gameObject;
                    }
                    name.text = interest.name;
                    treadIns = true;
                    laseIns = false;
                    proxIns = false;
                    itemIns = false;
                    roboIns = false;
                }

                if (hitIns.transform.gameObject.layer == 9)
                {
                    interest = hitIns.transform.GetComponentInParent<RobotKinematicsTypeB>().gameObject;
                    name.text = interest.name;
                    //simToggle.isOn = interest.GetComponent<RobotManager>().sim[0].activeSelf;
                    treadIns = false;
                    laseIns = false;
                    proxIns = false;
                    itemIns = false;
                    roboIns = true;
                }

            }
        }

        if (interest!=null)
            name.text = interest.name;

         //inspectors
        if (interest!=null && itemIns)
        {
            SetComponents();
            
            //positions
            if (!pos[0].isFocused)
                pos[0].text = interest.transform.position.x.ToString();

            if (!pos[1].isFocused)
                pos[1].text = interest.transform.position.y.ToString();

            if (!pos[2].isFocused)
                pos[2].text = interest.transform.position.z.ToString();

            //scale
            if (!scal[0].isFocused)
                scal[0].text = interest.transform.localScale.x.ToString();

            if (!scal[1].isFocused)
                scal[1].text = interest.transform.localScale.y.ToString();

            if (!scal[2].isFocused)
                scal[2].text = interest.transform.localScale.z.ToString();

            //rotation
            if (!rot[0].isFocused)
                rot[0].text = interest.transform.rotation.x.ToString();

            if (!rot[1].isFocused)
                rot[1].text = interest.transform.eulerAngles.y.ToString();

            if (!rot[2].isFocused)
                rot[2].text = interest.transform.localEulerAngles.z.ToString();

            interest.transform.position = new Vector3(float.Parse(pos[0].text), float.Parse(pos[1].text), float.Parse(pos[2].text));
            interest.transform.localScale = new Vector3(float.Parse(scal[0].text), float.Parse(scal[1].text), float.Parse(scal[2].text));
            interest.transform.rotation = Quaternion.Euler(float.Parse(rot[0].text), float.Parse(rot[1].text), float.Parse(rot[2].text));

            //component
            if (!mass.isFocused)
                mass.text = rb.mass.ToString();

            float i = 0;

            if (float.TryParse(mass.text, out i))
            {
                rb.mass = float.Parse(mass.text);
            }

        }

        if (interest != null && proxIns)
        {
            SetComponents();

            //positions
            if (!pos[0].isFocused)
                pos[0].text = interest.transform.position.x.ToString();

            if (!pos[1].isFocused)
                pos[1].text = interest.transform.position.y.ToString();

            if (!pos[2].isFocused)
                pos[2].text = interest.transform.position.z.ToString();

            if (!proxyDistanceDetection.isFocused)
                proxyDistanceDetection.text = interest.transform.GetComponent<ProxySensor_Script>().distanceDetection.ToString();

            interest.transform.position = new Vector3(float.Parse(pos[0].text), float.Parse(pos[1].text), float.Parse(pos[2].text));
            interest.transform.rotation = Quaternion.Euler(float.Parse(rot[0].text), float.Parse(rot[1].text), float.Parse(rot[2].text));

            //component
            interest.transform.GetComponent<ProxySensor_Script>().distanceDetection = float.Parse(proxyDistanceDetection.text);
        }

        if (interest != null && treadIns)
        {
            SetComponents();

            //positions
            if (!pos[0].isFocused)
                pos[0].text = interest.transform.position.x.ToString();

            if (!pos[1].isFocused)
                pos[1].text = interest.transform.position.y.ToString();

            if (!pos[2].isFocused)
                pos[2].text = interest.transform.position.z.ToString();

            if (!treadSpeed.isFocused)
                treadSpeed.text = interest.transform.GetComponent<Treadmill_Script>().treadSpeed.ToString();

            //scale
            if (!scal[0].isFocused)
                scal[0].text = interest.transform.localScale.x.ToString();

            if (!scal[1].isFocused)
                scal[1].text = interest.transform.localScale.y.ToString();

            if (!scal[2].isFocused)
                scal[2].text = interest.transform.localScale.z.ToString();

            interest.transform.position = new Vector3(float.Parse(pos[0].text), float.Parse(pos[1].text), float.Parse(pos[2].text));
            interest.transform.rotation = Quaternion.Euler(float.Parse(rot[0].text), float.Parse(rot[1].text), float.Parse(rot[2].text));
            interest.transform.localScale = new Vector3(float.Parse(scal[0].text), float.Parse(scal[1].text), float.Parse(scal[2].text));
            
            //component
            interest.transform.GetComponent<Treadmill_Script>().treadSpeed = float.Parse(treadSpeed.text);
        }


        if (interest != null && laseIns)
        {
            SetComponents();

            if (!pos[0].isFocused)
                pos[0].text = interest.transform.position.x.ToString();

            if (!pos[1].isFocused)
                pos[1].text = interest.transform.position.y.ToString();

            if (!pos[2].isFocused)
                pos[2].text = interest.transform.position.z.ToString();

            interest.transform.position = new Vector3(float.Parse(pos[0].text), float.Parse(pos[1].text), float.Parse(pos[2].text));
            interest.transform.rotation = Quaternion.Euler(float.Parse(rot[0].text), float.Parse(rot[1].text), float.Parse(rot[2].text));
        }

        if (interest != null && roboIns)
        {
            SetComponents();

            if (!pos[0].isFocused)
                pos[0].text = interest.transform.position.x.ToString();

            if (!pos[1].isFocused)
                pos[1].text = interest.transform.position.y.ToString();

            if (!pos[2].isFocused)
                pos[2].text = interest.transform.position.z.ToString();

            interest.transform.position = new Vector3(float.Parse(pos[0].text), float.Parse(pos[1].text), float.Parse(pos[2].text));
        }


    }

    void SetComponents()
    {
        itemsInspector.SetActive(itemIns);
        proxyInspector.SetActive(proxIns);
        robotInspector.SetActive(roboIns);
        treadInspector.SetActive(treadIns);
    }

    public void PhysicsToggle()
    {
        if (itemIns)
        {
            physicsToggle = !physicsToggle;

            rb.useGravity = !physicsToggle;
            rb.isKinematic = physicsToggle;
        }
    }

    public void SimulationToggle(bool isOn)
    {
        if (roboIns)
            interest.GetComponent<RobotManager>().ShowSim = !isOn;
    }
}
