using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unzip : MonoBehaviour {

	// Use this for initialization
	void Start () {

        // string zip_path = Application.persistentDataPath + "download.obj.zip";
        string zip_path = Application.dataPath + '/' + "download.obj.zip";
        string write_path = Application.dataPath + '/' + "download.obj";
        ZipUtil.Unzip(zip_path, write_path);
        print(write_path);
        // System.IO.File.WriteAllBytes(write_path, www.bytes);
        // debug.text = "downloaded and saved";
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
