using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundResize : MonoBehaviour {
    SpriteRenderer sr;
    Camera backgroundCamera;
    void Awake()
    {
        backgroundCamera = GameObject.FindGameObjectWithTag("BackgroundCamera").GetComponent<Camera>();
    }
    void Start()
    {
        resizeBackGround();
 
    }

    public void resizeBackGround()
    {
        sr = GetComponent<SpriteRenderer>();
        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        transform.localScale = new Vector3(
            worldScreenWidth / sr.sprite.bounds.size.x,
            worldScreenHeight / sr.sprite.bounds.size.y, 1);
    }

}
