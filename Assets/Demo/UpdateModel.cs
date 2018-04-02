using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateModel : MonoBehaviour {

    public Mesh mesh;
    public Texture2D texture;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void UpdateMod()
    {
        DemoMaster.instance.SetObj(mesh, texture);
        DemoMaster.instance.StartTexturingArea();
    }
}
