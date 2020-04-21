using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightGenerator : MonoBehaviour
{
    public float radius, visualiserScaleSpeed, visualizerScaleFactor;
    int AmountOfLights = 8; // changing the value of it breaks the visualizer, couldnt figure out why
    public GameObject primitivePrefab;
    public List<GameObject> trafficPointsLightList;

    AudioSource _AS;
    public static float[] samplesArray = new float[256];
    //public static float[] freqBand= new float[8];
    public static List<float> freqBand = new List<float>();



    void Awake()
    {
        for(int i = 0; i< AmountOfLights;i++)
        {
            freqBand.Add(new float());
        }
        Generate();
        _AS = GetComponent<AudioSource>();
    }
    void Update()
    {
        GetAudioSpectrum();
        GetFrequencyBands();
        Visualization();
    }

    void Generate()
    {
        
        for (int i = 0; i <= AmountOfLights - 1; i++)
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

    void Visualization()
    {
        for(int i = 0; i< freqBand.Count; i++)
        {
            trafficPointsLightList[i].transform.localScale = Vector3.Lerp(trafficPointsLightList[i].transform.localScale,
                new Vector3(trafficPointsLightList[i].transform.localScale.x,
                (visualizerScaleFactor * freqBand[i])+0.2f,
                trafficPointsLightList[i].transform.localScale.z)
                ,Time.deltaTime * visualiserScaleSpeed);
        }
    }

    void GetAudioSpectrum()
    {
        _AS.GetSpectrumData(samplesArray, 0, FFTWindow.Blackman);
    }
    void GetFrequencyBands()
    {
        for (int i = 0; i < freqBand.Count; i++)
            //for (int i = 0; i < freqBand.Length - 1; i++)
        {
            int start = (int)Mathf.Pow(2, i) - 1;
            int width = (int)Mathf.Pow(2, i);
            int end = start + width;
            float average = 0;
            for (int j = start; j < end; j++)
            {
                average += samplesArray[j] * (j + 1);
            }
            average /= (float)width;
            freqBand[i] = average;
        }

    }
}
