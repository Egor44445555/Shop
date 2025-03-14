using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public static Hand main;
    public Transform HoldingPoint;

    [HideInInspector] public GameObject collectible;

    void Awake()
    {
        main = this;
    }
}
