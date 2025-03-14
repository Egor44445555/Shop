using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class UI : MonoBehaviour
{
    public static UI main;
    public LayerMask interactableLayer;
    public float interactionRange = 100f;
    public GameObject dropButton;
    public GameObject[] collectibles;

    Camera cam;
    GraphicRaycaster graphicRaycaster;
    GraphicRaycaster graphicRaycasterOther;
    PointerEventData pointerEventData;
    EventSystem eventSystem;
    Vector2 startPoint = new Vector2();
    GameObject thrownObjectHold;

    void Awake()
    {
        main = this;
    }
    
    void Start()
    {
        cam = Camera.main;
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);

            if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactableLayer))
            {                
                if (hit.collider && hit.collider.GetComponent<Collectible>() != null && PlayerControllerPerson.main.collectibleName == "")
                {
                    GameObject throwableObject = Array.Find(collectibles, (item) => item.GetComponent<Collectible>().name == hit.collider.gameObject.GetComponent<Collectible>().name);
                    
                    throwableObject.GetComponent<Rigidbody>().isKinematic = true;
                    throwableObject.GetComponent<Rigidbody>().detectCollisions = false;
                    throwableObject.GetComponent<Rigidbody>().useGravity = false;
                    throwableObject.GetComponent<Collectible>().inHand = true;

                    thrownObjectHold = Instantiate(throwableObject, Hand.main.HoldingPoint.position, Hand.main.HoldingPoint.rotation, cam.transform);
                    Hand.main.collectible = thrownObjectHold;

                    PlayerControllerPerson.main.collectibleName = hit.collider.gameObject.GetComponent<Collectible>().name;
                    dropButton.SetActive(true);
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    public void DropCollectible()
    {
        GameObject throwableObject = Array.Find(collectibles, (item) => item.GetComponent<Collectible>().name == PlayerControllerPerson.main.collectibleName);

        if (throwableObject)
        {
            GameObject thrownObject = Instantiate(throwableObject, Hand.main.HoldingPoint.position, Hand.main.HoldingPoint.rotation);
            Rigidbody rb = thrownObject.GetComponent<Rigidbody>();
            float throwForce = thrownObject.GetComponent<Collectible>().throwForce;

            if (rb != null)
            {
                Destroy(thrownObjectHold);
                rb.isKinematic = false;
                rb.detectCollisions = true;
                rb.useGravity = true;

                rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
                PlayerControllerPerson.main.collectibleName = "";
                dropButton.SetActive(false);
            }
        }        
    }
}
