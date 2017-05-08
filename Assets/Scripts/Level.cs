using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level {

	// spawned object or object of interest (OI) will be refered to as dots
	private string levelName; 
	private RatioStruct cloudRatio;
	private int maxDot;
	private Color dotColor;
	private int totalSession;

	public Level( string name , RatioStruct ratio , int totaldot , Color color , int sessions)
	{
		this.levelName = name;
		this.cloudRatio = ratio;
		this.maxDot = totaldot; 
		this.dotColor = color;
		this.totalSession = sessions;
		
	}



	public string getLevelName()
	{
		return this.levelName;
	}

	public  RatioStruct getCloudRatio()
	{
		return this.cloudRatio;
	}

	public int getMaxDot()
	{
		return this.maxDot;
	}

	public Color getColor()
	{
		return this.dotColor;
	}

	public  int getTotalSession()
	{
		return this.totalSession;
	}
}
