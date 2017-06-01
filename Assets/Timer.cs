using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
    [SerializeField]
    private float timeLeft;
    [SerializeField]
    private bool timeOver;
    // Use this for initialization
    void Start()
    {
        this.timeOver = false;

    }

    // Update is called once per frame
    void Update()
    {
        this.timeLeft = this.timeLeft - Time.deltaTime;
        if(Mathf.Approximately(this.timeLeft,0f))
        {
            this.timeOver = true;
        }

    }

    public float getTimeLeft()
    {
        return this.timeLeft;
    }

    public void setTimeLeft(float timeleft)
    {
        this.timeLeft = timeleft;

    }

    public bool CheckTime()
    {
        return this.timeOver;

    }
}
