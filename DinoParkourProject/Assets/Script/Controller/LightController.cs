using UnityEngine;

public class LightController : MonoBehaviour
{
    private Light theLight;
    public float maxInstensity;
    public float minInstensity;
    public Color dayTimeColor;
    public Color nightTimeColor;
    public float speed;

    private void Start()
    {
        theLight = gameObject.GetComponent<Light>();
    }
    private float trigonometric(float min, float max,float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float A = (max - min) / 2.0f;
        float fai = min + A;
        return A * Mathf.Cos(rad) + fai;
    }
    public void setTimeByAngle(float angle)
    {
        theLight.intensity = trigonometric(minInstensity,maxInstensity,angle);
        if(100<angle && angle<200)
            theLight.color = nightTimeColor;
        else
            theLight.color = dayTimeColor;
    }
    public void setTime(bool isDayTime)
    {
        if (isDayTime)
        {
            theLight.intensity = maxInstensity;
            theLight.color = dayTimeColor;
        }
        else
        {
            theLight.intensity = minInstensity;
            theLight.color = nightTimeColor;
        }
    }
}
