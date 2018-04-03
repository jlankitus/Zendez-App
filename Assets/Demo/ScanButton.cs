using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ScanButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    public void Scan()
    {
        DemoMaster.instance.StartModelScan();
    }
    // Update is called once per frame
    void Update () {
	}
}
