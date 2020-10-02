using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class rrt : MonoBehaviour
{

    private int i, j, k;
    private float[,] theta;
    private float[] rand;
    private int[,] pair;
    private float px, py, pz;
    private float distance, d_min;
    private float[,] jrange;
    private float[] gconf;
    private int ITE_NUM = 500;
    private int ite = 0;
    private float l_branch = 0.5f;
    private int DIM = 3;
    //   private float EPS = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Debug.Log("RRT");

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        theta = new float[ITE_NUM, DIM];
        jrange = new float[DIM, 2];
        pair = new int[ITE_NUM, 2];
        gconf = new float[DIM];
        rand = new float[DIM];

        jrange[0, 0] = -10.0f;
        jrange[0, 1] = 10.0f;
        jrange[1, 0] = -10.0f;
        jrange[1, 1] = 10.0f;
        jrange[2, 0] = -10.0f;
        jrange[2, 1] = 10.0f;

        theta[ite, 0] = 0.0f;
        theta[ite, 1] = 0.0f;
        theta[ite, 2] = 0.0f;

        px = theta[0, 0];
        py = theta[0, 1];
        pz = theta[0, 2];

        gconf[0] = 1.0f;
        gconf[1] = 1.0f;
        gconf[2] = 1.0f;

        UnityEngine.Debug.Log("px:" + px + "py:" + py + "pz:" + pz);

        sphere.transform.position = new Vector3(theta[ite, 0], theta[ite, 1], theta[ite, 2]);

    }

    // Update is called once per frame
    void Update()
    {
        GameObject node = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        GameObject link = new GameObject();
        Vector3 connect1, connect2;

        ite++;
        pair[ite - 1, 1] = ite;
        for (k = 0; k < DIM; k++)
        {
            rand[k] = UnityEngine.Random.Range(jrange[k, 0], jrange[k, 1]);
        }

        for (i = 0; i < ite; i++)
        {
            distance = 0.0f;
            for (k = 0; k < DIM; k++)
            {
                distance = distance + (rand[k] - theta[i, k]) * (rand[k] - theta[i, k]);
            }

            if (i == 0)
            {
                d_min = distance;
                pair[ite - 1, 0] = i;
            }
            else if (d_min > distance)
            {
                d_min = distance;
                pair[ite - 1, 0] = i;
            }
        }

        for (k = 0; k < DIM; k++)
        {
            theta[ite, k] = l_branch * rand[k] + (1.0f - l_branch) * theta[pair[ite - 1, 0], k];
        }
        UnityEngine.Debug.Log("theta: " + theta[ite, 0] + ", " + theta[ite, 1] + ", " + theta[ite, 2] + ", ");
        UnityEngine.Debug.Log("pair: " + pair[ite - 1, 0] + ", " + pair[ite - 1, 1]);

        node.AddComponent<SphereCollider>();
        SphereCollider sc = node.GetComponent<SphereCollider>();
        sc.radius = 0.1f;

        connect1 = new Vector3(theta[ite, 0], theta[ite, 1], theta[ite, 2]);
        node.transform.position = connect1;
 
        connect2 = new Vector3(theta[pair[ite -1, 0], 0], theta[pair[ite - 1, 0], 1], theta[pair[ite - 1, 0], 2]);
        link.AddComponent<LineRenderer>();
        LineRenderer lr = link.GetComponent<LineRenderer>();

        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, connect1);
        lr.SetPosition(1, connect2);
            
        if(ite > ITE_NUM)
        {
            UnityEngine.Debug.Break();
        }

    }
}
