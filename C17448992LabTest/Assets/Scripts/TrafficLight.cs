using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public enum TrafficState
    {
        Green ,
        Yellow ,
        Red
    };

    //allows car to reference the state of the traffic light
    public TrafficState myState;


    float timer = 0f;
    private void Awake()
    {
        int Picker = Random.Range(0, 3);
        switch (Picker)
        {
            case 0:
                myState = TrafficState.Green;
                this.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                timer = (int)Random.Range(5, 10f);
                break;
            case 1:
                myState = TrafficState.Yellow;
                this.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
                timer = 4f;
                break;
            case 2:
                myState = TrafficState.Red;
                this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                timer = (int)Random.Range(5, 10f);
                break;
        }
    }

    void AdvanceState(float lengthToWait, TrafficState currentState)
    {
        timer -= Time.deltaTime;
        if(timer<=0)
        {
            switch (currentState)
            {
                case TrafficState.Green:
                    myState = TrafficState.Yellow;
                    this.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    timer = 4f;
                    break;

                case TrafficState.Yellow:
                    myState = TrafficState.Red;
                    this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                    timer =(int)Random.Range(5, 10f);
                    break;

                case TrafficState.Red:
                    myState = TrafficState.Green;
                    this.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                    timer = (int)Random.Range(5, 10f);
                    break;
            }

        }
    }

    private void Update()
    {
        AdvanceState(timer, myState);
    }
}
