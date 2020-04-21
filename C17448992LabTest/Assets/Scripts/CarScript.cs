using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    public GameObject TrafficLightParent;

    public List<GameObject> ActiveLights;

    public GameObject CurrentTrafficTarget;

    [Range(0f,10f)]
    public float SeekSteeringWeight, ArriveSteeringWeight;
    public float ArriveDistance;

    public Vector3 Force = Vector3.zero;
    public Vector3 Acc = Vector3.zero;
    public Vector3 Vel = Vector3.zero;
    public float mass = 1f;

    [Range(0.0f, 10.0f)]
    public float damping = 0.01f;

    [Range(0.0f, 1.0f)]
    public float banking = 0.1f;
    public float maxSpeed = 5.0f;
    public float maxForce = 10.0f;

    private void Start()
    {
        //Setting Up
        this.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
        CurrentTrafficTarget = this.gameObject;

        //Adding everythin to the active list in order to clear it
        foreach(GameObject trafficLight in TrafficLightParent.GetComponent<TrafficLightGenerator>().trafficPointsLightList)
        {
            ActiveLights.Add(trafficLight);
            var TL = trafficLight.GetComponent<TrafficLight>();
            if(TL.myState == TrafficLight.TrafficState.Green)
            {
                CurrentTrafficTarget = trafficLight;
            }
        }
    }

    

    private void FixedUpdate()
    {
        //Choosing new target when conditions met
        if(Vector3.Distance(this.gameObject.transform.position, CurrentTrafficTarget.transform.position)<=0.5f
            ||CurrentTrafficTarget.GetComponent<TrafficLight>().myState != TrafficLight.TrafficState.Green
            )
        {
            if (ActiveLights.Count > 0)
            {
                ChooseNewPoint();
            }
            else // setting target to the car itself when the Active List is empty
            {
                CurrentTrafficTarget = this.gameObject;
            }
        }

        GreenTrafficListGenerator(ActiveLights, TrafficLightParent.GetComponent<TrafficLightGenerator>().trafficPointsLightList);

        Force = (SeekForce(CurrentTrafficTarget.transform.position) * SeekSteeringWeight) + 
                (ArriveForce(CurrentTrafficTarget.transform.position, ArriveDistance) * ArriveSteeringWeight);

        Acc = Force / mass;
        Vel += Acc * Time.deltaTime;

        Vel = Vector3.ClampMagnitude(Vel, maxSpeed);

        if (Vel.magnitude > 0.001f)
        {
            Vector3 BankingUp = Vector3.Lerp(transform.up, Vector3.up + (Acc * banking), Time.deltaTime * 2.0f);
            transform.LookAt(transform.position + Vel, BankingUp);
        }
        transform.position += Vel * Time.deltaTime;
        Vel *= (1.0f - (damping * Time.deltaTime));
        
        
    }

    void GreenTrafficListGenerator(List<GameObject> ClearList, List<GameObject> AddfromList)
    {
        //clearing from active lights
        for(int i = 0; i< ActiveLights.Count; i++)
        {
            if(ActiveLights[i].GetComponent<TrafficLight>().myState != TrafficLight.TrafficState.Green)
            {
                ActiveLights.Remove(ActiveLights[i]);
            }
        }
        //adding green lights from traffic lights parent to active lights list
        foreach(GameObject trafficLight  in AddfromList)
        {
            if(trafficLight.GetComponent<TrafficLight>().myState == TrafficLight.TrafficState.Green && !ActiveLights.Contains(trafficLight))
            {
                ActiveLights.Add(trafficLight);
            }
        }
    }

    //Picking a random integer
    void ChooseNewPoint()
    {
        int Picker = Random.Range(0, ActiveLights.Count-1);
        CurrentTrafficTarget = ActiveLights[Picker];
    }


    //Steering Behaviours from lecture notes
    public Vector3 SeekForce(Vector3 target)
    {
        Vector3 desired = target - transform.position;
        desired.Normalize();
        desired *= maxSpeed;
        return desired - Vel;
    }

    public Vector3 ArriveForce(Vector3 target, float slowingDistance = 15.0f)
    {
        Vector3 toTarget = target - transform.position;

        float distance = toTarget.magnitude;
        if (distance < 0.1f)
        {
            return Vector3.zero;
        }
        float ramped = maxSpeed * (distance / slowingDistance);

        float clamped = Mathf.Min(ramped, maxSpeed);
        Vector3 desired = clamped * (toTarget / distance);
        return desired - Vel;
    }

    


}
