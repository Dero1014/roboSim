﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//assign names to the axises
public class MoveTool_Script : MonoBehaviour
{
    public Transform target;
    public GameObject holder;
    public bool holdToHand;

    public Transform[] graphic;

    [Space(10)]
    public LayerMask moveLayer;

    Vector3 distance = Vector3.zero;

    bool moveActive = false;
    bool xAxis = false;
    bool yAxis = false;
    bool zAxis = false;
    bool distanceChecked = false;
    //Patrik
    [Header("The XYZ Planes:")]
    public GameObject x_Plane;
    public GameObject y_Plane;
    public GameObject z_Plane;

    private void Start() //Patrik
    {

        if (x_Plane != null && y_Plane != null && z_Plane != null)
        {
            x_Plane.SetActive(false);
            y_Plane.SetActive(false);
            z_Plane.SetActive(false);
        }

    }

    void Update()
    {
        if (!holdToHand)
            FindTargetedObject();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitObject;

        if (Input.GetKeyDown(KeyCode.Mouse0)) //Check for what axis has been selected
        {
            if (Physics.Raycast(ray, out hitObject, Mathf.Infinity, moveLayer))
            {
                if (hitObject.transform.tag == "Move Tool")
                {
                    moveActive = true;

                    if (hitObject.transform.name == "X")
                        xAxis = true;
                    else if (hitObject.transform.name == "Y")
                        yAxis = true;
                    else if (hitObject.transform.name == "Z")
                        zAxis = true;

                }
            }
        }


        if (Input.GetKey(KeyCode.Mouse0)) //IF HELD YOU CAN MOVE IT
            MoveTool();
        else //IF ITS NOT HELD THEN NOTHING IS PICKED
        {
            xAxis = false;
            yAxis = false;
            zAxis = false;
            moveActive = false;
            distanceChecked = false;
        }

    }

    void FindTargetedObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitObject;

        if (Input.GetKeyDown(KeyCode.Mouse0)) //Check for what axis has been selected
        {
            if (Physics.Raycast(ray, out hitObject))
            {
                if (hitObject.transform.gameObject != null && hitObject.transform.name != "X" && hitObject.transform.name != "Y" && hitObject.transform.name != "Z" && !hitObject.transform.GetComponentInParent<RobotCommands>())
                {
                    if (hitObject.transform.tag != "Ground")
                    {
                        holder.SetActive(true);
                        target = hitObject.transform;
                        transform.position = target.position;
                    }
                    else
                    {
                        holder.SetActive(false); //if it touches the ground
                        target = null;
                    }

                }
                
                if (hitObject.transform.GetComponentInParent<RobotCommands>())
                {
                    holder.SetActive(false);
                    target = null;
                }

            }
            else
            {
                holder.SetActive(false);
            }

            if (Input.GetMouseButtonDown(1))
            {
                target = null;
            }

        }

        if (target!=null)
        {
            target.position = transform.position;
        }


    }

    void MoveTool()
    {
        if (moveActive)
        {
            //move on X axis
            if (xAxis)
            {
                x_Plane.SetActive(true); // -
                y_Plane.SetActive(false);// - - > Patrik
                z_Plane.SetActive(false);// -

                Vector3 mousePosition = GetMousePositionX(); //get the mouse position

                if (!distanceChecked) //set the difference between the mouse and the origin point
                {
                    distanceChecked = true;
                    distance = mousePosition - transform.position;
                }
                
                transform.position = new Vector3(mousePosition.x - distance.x, transform.position.y, transform.position.z); //apply the movement
            }

            if (yAxis)
            {
                x_Plane.SetActive(false);// -
                y_Plane.SetActive(true); // - - > Patrik
                z_Plane.SetActive(false);// -

                Vector3 mousePosition = GetMousePositionY(); //get the mouse position

                if (!distanceChecked) //set the difference between the mouse and the origin point
                {
                    distanceChecked = true;
                    distance = mousePosition - transform.position;
                }

                transform.position = new Vector3(transform.position.x, mousePosition.y - distance.y, transform.position.z); //apply the movement
            }

            if (zAxis)
            {
                x_Plane.SetActive(false);// -
                y_Plane.SetActive(false);// - - > Patrik
                z_Plane.SetActive(true); // -

                Vector3 mousePosition = GetMousePositionZ(); //get the mouse position

                if (!distanceChecked) //set the difference between the mouse and the origin point
                {
                    distanceChecked = true;
                    distance = mousePosition - transform.position;
                }

                transform.position = new Vector3(transform.position.x, transform.position.y, mousePosition.z - distance.z); //apply the movement
            }

        }
    }

    #region mousePosition

    Vector3 pos;
    [Space(10)]
    int MaxAngle = 45;
    float _zAngle = 0;

    Vector3 _camAngle;

    Vector3 GetMousePositionX()
    {
        ////Use of planes to determain the position of the mouse relative to the axis we are using
        //Plane planeY = new Plane(Vector3.up, transform.position);
        //Plane planeZ = new Plane(Vector3.forward, transform.position);

        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        ////for more effective results we use two planes that cover the parts that it needs to calculate for that axis 

        //float distanceToPlane;

        //if (planeY.Raycast(ray,out distanceToPlane))
        //    pos = ray.GetPoint(distanceToPlane);

        Plane planeX;

        _camAngle = transform.position - Camera.main.transform.position;

        _zAngle = Vector3.Angle(_camAngle, graphic[2].transform.forward);
 

        if (_zAngle >= MaxAngle) //POD ODREĐENIM KUTEM KORISTI Z ILI Y OS
        {
            planeX = new Plane(Vector3.forward, transform.position);
        }
        else
        {
            planeX = new Plane(Vector3.up, transform.position);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distanceToPlane;

        if (planeX.Raycast(ray, out distanceToPlane))
            pos = ray.GetPoint(distanceToPlane);

        return pos;
    }

    Vector3 GetMousePositionY()
    {
        //Use of planes to determain the position of the mouse relative to the axis we are using
        Plane planeY = new Plane(Vector3.forward, transform.position);
        Plane planeZ = new Plane(Vector3.right, transform.position);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //for more effective results we use two planes that cover the parts that it needs to calculate for that axis 

        float distanceToPlane;

        if (planeY.Raycast(ray, out distanceToPlane))
            pos = ray.GetPoint(distanceToPlane);

        if (planeZ.Raycast(ray, out distanceToPlane))
            pos = ray.GetPoint(distanceToPlane);

        return pos;
    }

    Vector3 GetMousePositionZ()
    {
        //Use of planes to determain the position of the mouse relative to the axis we are using
        Plane planeZ = new Plane(Vector3.right, transform.position);
        Plane planeX = new Plane(Vector3.up, transform.position);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //for more effective results we use two planes that cover the parts that it needs to calculate for that axis 

        float distanceToPlane;

        if (planeZ.Raycast(ray, out distanceToPlane))
            pos = ray.GetPoint(distanceToPlane);

        if (planeX.Raycast(ray, out distanceToPlane))
            pos = ray.GetPoint(distanceToPlane);

        return pos;
    }
    #endregion
}
