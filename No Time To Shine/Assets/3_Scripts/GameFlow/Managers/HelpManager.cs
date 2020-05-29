using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class HelpManager : MonoBehaviour
{
    [SerializeField] Light2D[] currentLights;
    [SerializeField] float speedOfLightOnOff = 15;
    float[] initialIntensities;
    float[] currentIntensities;

    private void Awake()
    {
        initialIntensities = new float[currentLights.Length];
        currentIntensities = new float[currentLights.Length];
    }

    private void Start()
    {
        for (int i=0; i < currentLights.Length; i++)
        {
            initialIntensities[i] = currentLights[i].intensity;
        }
    }

    private void Update()
    {
        for (int i = 0; i < currentLights.Length; i++)
        {
            currentIntensities[i] = Mathf.PingPong(Time.time * speedOfLightOnOff, initialIntensities[i]);
            currentLights[i].intensity = currentIntensities[i];
        }
    }
}
