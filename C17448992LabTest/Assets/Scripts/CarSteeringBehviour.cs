using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CarSteeringBehviour : MonoBehaviour
{
    public Vector3 force;
    public float SteeringWeight;

    public CarScript car;

    public abstract Vector3 CalculateForce();
}
