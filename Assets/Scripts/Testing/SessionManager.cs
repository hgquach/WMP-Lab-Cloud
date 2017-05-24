using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour {

    // 		this.dotRatio = new RatioStruct(2,3);
    //this.dotColor= Color.blue;
    //this.trialMax = 10;
    //this.dotSprite = "WhiteDot";
    //this.dotMax = 50;
    //      this.dotSeparation = 1;

    RoundManagerTest roundManager;
    [SerializeField]
    private int maxRound;
    [SerializeField]
    private int currentRound;
    [SerializeField]
    private int currentLevel;
    private RatioStruct tempRatio;
    void Awake()
    {
        roundManager = GameObject.FindGameObjectWithTag("RoundManager").GetComponent<RoundManagerTest>();

        this.currentRound = 1;
        this.currentLevel = 1;
        this.maxRound = 3;
        this.tempRatio = new RatioStruct(1, 3);
        UpdateRound(tempRatio, Color.white, 10, "WhiteDot", 30);
        roundManager.roundStart();
    }

    void Start()
    {

    }

    void Update()
    {
        if(!roundManager.getRoundStart() && this.currentRound != 1)
        {
            Debug.Log("adjusting next round round");
            if(this.currentRound != this.maxRound)
            {
                Debug.Log("new round");
                this.UpdateRound(tempRatio, Color.blue, 5, "WhiteDot", 50);
                roundManager.roundStart();
            }
            //look at the hypothetical trial recording manager and see if the accuracy is above a threshold and 
            //adjust the round and start it

        }
    }

    void UpdateRound(RatioStruct ratio,Color color , int trialMax , string dotSprite , int dotMax , int dotSepartaion = 1)
    {
        Debug.Log("updating round");
        this.roundManager.setColor(color);
        this.roundManager.setDotSeperation(dotSepartaion);
        this.roundManager.setTrialMax(trialMax);
        this.roundManager.setDotSprite(dotSprite);
        this.roundManager.setTrialRatio(ratio);
        this.roundManager.setDotMax(50);
    }

    public void incRound()
    {
        this.currentRound += 1;
    }
}
