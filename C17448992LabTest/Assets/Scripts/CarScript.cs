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
        
        CurrentTrafficTarget = this.gameObject;
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
            ||CurrentTrafficTarget.GetComponent<TrafficLight>().myState != TrafficLight.TrafficState.Green)
        {
            ChooseNewPoint();
        }

        GreenTrafficListGenerator(ActiveLights, TrafficLightParent.GetComponent<TrafficLightGenerator>().trafficPointsLightList);

        Force = (SeekForce(CurrentTrafficTarget.transform.position) * SeekSteeringWeight) + 
                (ArriveForce(CurrentTrafficTarget.transform.position, ArriveDistance) * ArriveSteeringWeight);
        
        Vector3 newAcceleration = Force / mass;
        Acc = Vector3.Lerp(Acc, newAcceleration, Time.deltaTime);
        Vel += Acc * Time.deltaTime;

        Vel = Vector3.ClampMagnitude(Vel, maxSpeed);

        if (Vel.magnitude > float.Epsilon) // allows movement above zero mark, removing issue of infinitely turning to look at transform
        {
            Vector3 tempUp = Vector3.Lerp(transform.up, Vector3.up + (Acc * banking), Time.deltaTime * 3.0f);
            transform.LookAt(transform.position + Vel, tempUp);

            transform.position += Vel * Time.deltaTime;
            Vel *= (1.0f - (damping * Time.deltaTime));
        }
    }

    void GreenTrafficListGenerator(List<GameObject> ClearList, List<GameObject> AddfromList)
    {
        foreach(GameObject inputLight in ClearList)
        {
            if (inputLight.GetComponent<TrafficLight>().myState != TrafficLight.TrafficState.Green)
            {
                ActiveLights.Remove(inputLight);
            }
        }
        foreach(GameObject trafficLight  in AddfromList)
        {
            if(trafficLight.GetComponent<TrafficLight>().myState == TrafficLight.TrafficState.Green && !ActiveLights.Contains(trafficLight))
            {
                ActiveLights.Add(trafficLight);
            }
        }
    }

    void ChooseNewPoint()
    {
        int Picker = Random.Range(0, ActiveLights.Count);
        CurrentTrafficTarget = ActiveLights[Picker];
    }

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
