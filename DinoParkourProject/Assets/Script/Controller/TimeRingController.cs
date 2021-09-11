using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRingController : MonoBehaviour
{
    public LightController LC;
    public Vector3 playSpeed;
    private bool isDayTime;
    private bool isPlaying;

    public bool IsDayTime { get => isDayTime; set => setTime(value); }
    public bool IsPlaying { get => isPlaying; set => isPlaying = value; }

    void Start()
    {
        LC = GameObject.Find("Light").GetComponent<LightController>();
        isDayTime = true;
        isPlaying = false;
    }
    private void FixedUpdate()
    {
        if (isPlaying)
        {
            transform.Rotate(playSpeed, Space.Self);
            LC.setTimeByAngle(transform.rotation.eulerAngles.z);
        }
            
    }
    private void setTime(bool isDayTime)
    {
        this.isDayTime = isDayTime;
        if (isDayTime)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            LC.setTime(true);
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            LC.setTime(false);
        }
            
    }
}
