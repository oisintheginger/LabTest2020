using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightGenerator : MonoBehaviour
{
    public float radius;
    public float AmountOfLights;
    public GameObject primitivePrefab;
    public List<GameObject> trafficPointsLightList;


    void Generate()
    {
        for (int i = 0; i <= AmountOfLights-1; i++)
        {
            float Angle = i * Mathf.PI * 2f / AmountOfLights;
            Vector3 newPos = new Vector3(Mathf.Cos(Angle) * radius, this.transform.position.y, Mathf.Sin(Angle) * radius);
            GameObject NewTrafficLight = Instantiate(primitivePrefab, newPos, Quaternion.identity);
            NewTrafficLight.transform.parent = this.transform;
            NewTrafficLight.name = "Light" + i;
            NewTrafficLight.AddComponent<TrafficLight>();
            trafficPointsLightList.Add(NewTrafficLight);
            
        }
        
    }
    void Awake()
    {
        Generate();
    }
}
