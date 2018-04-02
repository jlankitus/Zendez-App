using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UpdateTexture : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Sprite sprite;
    public void SetTexture()
    {
        DemoMaster.instance.SetSprite(sprite);
        DemoMaster.instance.StartTexturingArea();
    }
}
