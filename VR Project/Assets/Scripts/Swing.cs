using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    public Transform startSwingHand;
    public float maxDistance = 35;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetSwingPoint();
    }

    public void GetSwingPoint()
    {
        RaycastHit raycastHit;

        Physics.Raycast()
    }
}
