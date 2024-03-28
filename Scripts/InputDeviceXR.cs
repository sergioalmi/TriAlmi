using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class InputDeviceXR : MonoBehaviour
{
    [SerializeField]
    private XRNode xRNode = XRNode.LeftHand;

    private List<InputDevice> devices = new List<InputDevice>();

    private InputDevice device;

    //CONTROL DEL JUEGO
    public GameController gameController;


    //CONTROLES
    private float grip,trigger=0;//valor del trigger,grip
    private bool triggerIsPressed;//BOTON INDICE
    private bool primaryButtonIsPressed;//BOTON A
    private bool secundaryButtonIsPressed;//BOTON B
    private bool primary2DAxisIsChosen;//JOYSTICK
    private Vector2 primary2DAxisValue = Vector2.zero; //VALORES DEL JOYSTICK
    private Vector2 prevPrimary2DAxisValue; // VALORES PREVIOS DEL JOYSTICK, PARA COMPROBAR CAMBIOS
    private bool gripIsPressed;//BOTON LATERAL

    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(xRNode, devices);
        device = devices.FirstOrDefault();
    }

    void OnEnable()
    {
        if (!device.isValid)
        {
            GetDevice();
        }
    }

    void Update()
    {
        if (!device.isValid)
        {
            GetDevice();
        }

    

        // CAPTURAR BOTON INDICE   
        bool triggerButtonValue = false;
        if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButtonValue) && triggerButtonValue && !triggerIsPressed)
        {
            triggerIsPressed = true;
           // Debug.Log($"Indice dedo pulsado {xRNode}");
            if (xRNode== XRNode.LeftHand)
            {
                //habilitar rayo
                gameController.manoIzquierda.GetComponent<XRInteractorLineVisual>().enabled = true;
                //comprobar que hemos dado al cubo
                gameController.manoIzquierda.GetComponent<ControlRayInteractor>().hayCubo();
            }
            if (xRNode == XRNode.RightHand)
            {
                //habilitar rayo
                gameController.manoDerecha.GetComponent<XRInteractorLineVisual>().enabled = true;
                //comprobar que hemos dado al cubo
                gameController.manoDerecha.GetComponent<ControlRayInteractor>().hayCubo();
            }
        }
        else if (!triggerButtonValue && triggerIsPressed)
        {
            triggerIsPressed = false;
            //Debug.Log($"Indice dedo desactivado {xRNode}");
            if (xRNode == XRNode.LeftHand)
            {
                gameController.manoIzquierda.GetComponent<XRInteractorLineVisual>().enabled = false;
            }
            if (xRNode == XRNode.RightHand)
            {
                gameController.GetComponent<GameController>().manoDerecha.GetComponent<XRInteractorLineVisual>().enabled = false;
            }
        }

        // CAPTURAR BOTON A
        bool primaryButtonValue = false;
        InputFeatureUsage<bool> primaryButtonUsage = CommonUsages.primaryButton;

        if (device.TryGetFeatureValue(primaryButtonUsage, out primaryButtonValue) && primaryButtonValue && !primaryButtonIsPressed)
        {
            primaryButtonIsPressed = true;
            //Debug.Log($"Boton A {xRNode}");
        }
        else if (!primaryButtonValue && primaryButtonIsPressed)
        {
            primaryButtonIsPressed = false;
            //Debug.Log($"Boton A levantado {xRNode}");
        }

        // CAPTURAR BOTON B
        bool secundaryButtonValue = false;
        InputFeatureUsage<bool> secundaryButtonUsage = CommonUsages.secondaryButton;

        if (device.TryGetFeatureValue(secundaryButtonUsage, out secundaryButtonValue) && secundaryButtonValue && !secundaryButtonIsPressed)
        {
            secundaryButtonIsPressed = true;
            //Debug.Log($"Boton B {xRNode}");
        }
        else if (!primaryButtonValue && primaryButtonIsPressed)
        {
            secundaryButtonIsPressed = false;
            //Debug.Log($"Boton B levantado {xRNode}");
        }


        // CAPTURAR EL JOYSTICK
        InputFeatureUsage<Vector2> primary2DAxisUsage = CommonUsages.primary2DAxis;
        // NOS ASEGURAMOS QUE EL VALOR ES DIFRENTES DE CERO PARA SABER SI ESTA PULSADO
        if (primary2DAxisValue != prevPrimary2DAxisValue)
        {
            primary2DAxisIsChosen = false;
        }
        if (device.TryGetFeatureValue(primary2DAxisUsage, out primary2DAxisValue) && primary2DAxisValue != Vector2.zero && !primary2DAxisIsChosen)
        {
            prevPrimary2DAxisValue = primary2DAxisValue;
            primary2DAxisIsChosen = true;
            //Debug.Log($"Primary2DAxis value activated {primary2DAxisValue} on {xRNode}");
        }
        else if (primary2DAxisValue == Vector2.zero && primary2DAxisIsChosen)
        {
            prevPrimary2DAxisValue = primary2DAxisValue;
            primary2DAxisIsChosen = false;
            //Debug.Log($"Primary2DAxis deactivated {primary2DAxisValue} on {xRNode}");
        }


        // CAPTURAR BOTON LATERAL
        float gripValue;
        InputFeatureUsage<float> gripUsage = CommonUsages.grip;
        

        //CUANDO EL BOTON LATERAL ESTA PULSADO DA VALORES ENTRE 0 Y 1
        //------------------------------------------------------------
        if (device.TryGetFeatureValue(gripUsage, out gripValue) && gripValue>0.8f && gripValue<1f &&  !gripIsPressed)
        {
            gripIsPressed = true;
            //Debug.Log($"Boton lateral pulsado en {xRNode}");
        }
        else if ((gripValue < 0.5 || gripValue>1) && gripIsPressed)
        {
            gripIsPressed = false;
            //Debug.Log($"Boton lateral levantado en {xRNode}");
        }

        //aqui controlamos el movimiento de las manos al pulsar los botones
        //debo crear varibles para las manos
        float tValue;
        device.TryGetFeatureValue(CommonUsages.trigger, out tValue);

        grip = gripValue;
        trigger = tValue;
    }

    public float getGrip() {
        return grip;
    }
    public float getTrigger()
    {
        return trigger;
    }


}