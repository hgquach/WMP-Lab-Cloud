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

        if(!(this.timeLeft < 0f))
        {
            this.timeLeft = this.timeLeft - Time.deltaTime;

        }
        else
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
