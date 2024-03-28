using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRRayInteractor))]
public class ControlRayInteractor : MonoBehaviour
{
    
    private XRRayInteractor rayInteractor;
    private bool isEnable = false;

    // Start is called before the first frame update
    void Start()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
    }

    private void OnDisable()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
    }

    // Update is called once per frame
    public bool hayCubo()
    {
        RaycastHit raycastHit;
        ;
        if (rayInteractor.TryGetCurrent3DRaycastHit(out raycastHit))
        {
            if (raycastHit.transform.tag.Equals("cubo"))
            {
                GameObject.Find("Main Camera").GetComponent<Player>().MirarRayo(raycastHit);
            }
            return true;
        }
        else
        {
            return false;
        }
    }



}
