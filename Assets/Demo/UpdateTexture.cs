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

    public void SetTexture()
    {
        Texture2D tex = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().mainTexture as Texture2D;
        Sprite mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        DemoMaster.instance.SetSprite(mySprite);
        DemoMaster.instance.StartTexturingArea();
    }
}
