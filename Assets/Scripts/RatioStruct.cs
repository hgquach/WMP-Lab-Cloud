using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RatioStruct 
{
	public int cloud1, cloud2;

	public RatioStruct(int cloudOneRatio , int cloudTwoRatio)
	{
		this.cloud1 = cloudOneRatio;
		this.cloud2 = cloudTwoRatio;
	}
}
