using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class ImageUploader : MonoBehaviour {
    public AudioSource audioSource;
	public WebCamTexture webCamTexture;
    public Text debug;
	int count = 0;
    public string device;

	void Start() 
	{
		webCamTexture = new WebCamTexture();
		GetComponent<Renderer>().material.mainTexture = webCamTexture;
		webCamTexture.Play();
        audioSource = this.GetComponent<AudioSource>();
    }

    public void Photo()
    {
        count++;
        StartCoroutine(TakePhoto());
    }

	void Update() {

	}

	IEnumerator TakePhoto()
	{
        if(audioSource != null)
        {
            audioSource.Play();
        }
		// NOTE - you almost certainly have to do this here:

		yield return new WaitForEndOfFrame();

        // it's a rare case where the Unity doco is pretty clear,
        // http://docs.unity3d.com/ScriptReference/WaitForEndOfFrame.html
        // be sure to scroll down to the SECOND long example on that doco page 

        try
        {
            Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
            photo.SetPixels(webCamTexture.GetPixels());
            photo.Apply();

            //Encode to a JPG
            byte[] bytes = photo.EncodeToJPG();

            print(count);
            string your_path = "";
            if(device == "pc")
                your_path = Application.dataPath + "/StreamingAssets/"; // Application.persistentDataPath;
            if(device == "android")
            {
                your_path = Application.persistentDataPath + "/";
            }

            //Write out the PNG. Of course you have to substitute your_path for something sensible
            File.WriteAllBytes(your_path + count + ".jpg", bytes);
            debug.text = your_path + count + ".jpg";
        }
        catch (Exception ex)
        {
            debug.text = ex.ToString();
        }
	}

	IEnumerator UploadFileCo(string localFileName, string uploadURL)
	{
		WWW localFile = new WWW("file:///" + localFileName);
		yield return localFile;
		if (localFile.error == null)
			Debug.Log("Loaded file successfully");
		else
		{
			Debug.Log("Open file error: "+localFile.error);
			yield break; // stop the coroutine here
		}
		WWWForm postForm = new WWWForm();
		// version 1
		//postForm.AddBinaryData("theFile",localFile.bytes);
		// version 2
		postForm.AddBinaryData("theFile",localFile.bytes,localFileName,"text/plain");
		WWW upload = new WWW(uploadURL,postForm);        
		yield return upload;
		if (upload.error == null)
			Debug.Log("upload done :" + upload.text);
		else
			Debug.Log("Error during upload: " + upload.error);
	}
	void UploadFile(string localFileName, string uploadURL)
	{
		StartCoroutine(UploadFileCo(localFileName, uploadURL));
	}
}


/*
 * 

curl -v 'https://developer.api.autodesk.com/photo-to-3d/v1/file'
  -X 'POST'
  -H 'Authorization: Bearer eyjhbGCIOIjIuzI1NiISimtpZCI6...'
  -F "photosceneid=hcYJcrnHUsNSPII9glhVe8lRF6lFXs4NHzGqJ3zdWMU"
  -F "type=image"
  -F "file[0]=@c:/sample_data/_MG_9026.jpg"
  -F "file[1]=@c:/sample_data/_MG_9027.jpg"


*/