using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System;
using System.IO;
using System.IO.Compression;



public class accessToken
{
	public string access_token;

	public static accessToken CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<accessToken>(jsonString);
    }
}

// {"Usage":"0.82937216758728","Resource":"\/photoscene","Photoscene":{"photosceneid":"WIErSs15wKRyl3AOyOuablfrecNksc6jLjnKV7mE4Q0"}}

public class Photoscene
{
	public string photosceneid;
}
/*
public class RootObject
{
	public Photoscene Photoscene;
}*/

[System.Serializable]
public class RootObj
{
    public Photoscene Photoscene;
}

public class forgeAPI : MonoBehaviour {
    public string[] images;
	public Text sceneText;
	public Text debug;
	private string sceneName;
	public string format;

	private string access_token;
	private string photoSceneId;
	private bool status;
    public string device;

    public GameObject emptyPrefabWithMeshRenderer;
    public ObjImporter importer;
	
	public void createScene()
	{
		status = false;
		StartCoroutine(GetToken());
	}

	IEnumerator GetToken()
	{
		sceneName = sceneText.text;
		/*
		curl -v 
		'https://developer.api.autodesk.com/authentication/v1/authenticate' 
		-X 'POST' -H 'Content-Type: application/x-www-form-urlencoded' 
		-d "client_id=yZ812Yfi6wDD1uKu3OaMgU7aa2xUIngT&client_secret=gNJYf66myNB8P4os&grant_type=client_credentials&scope=data:read%20data:write"
		 */ 

		WWWForm form = new WWWForm();
        form.AddField("client_id", "yZ812Yfi6wDD1uKu3OaMgU7aa2xUIngT");
		form.AddField("client_secret", "gNJYf66myNB8P4os");
		form.AddField("grant_type", "client_credentials");
		form.AddField("scope", "data:read data:write");

        UnityWebRequest www = UnityWebRequest.Post("https://developer.api.autodesk.com/authentication/v1/authenticate", form);
		www.SetRequestHeader("Content-Type","application/x-www-form-urlencoded");
        yield return www.Send();
 
        if(www.isNetworkError) {
            Debug.Log(www.error);
        }
        else {
            print("Token complete!");
			print(www.downloadHandler.text);
			debug.text = www.downloadHandler.text;

			string json = www.downloadHandler.text;

			// accessToken at = new accessToken();
			access_token = accessToken.CreateFromJSON(json).access_token;
            StartCoroutine(newPhotoScene());
            // StartCoroutine(Download());
            // StartCoroutine(Process());
            // ImportObject
            // StartCoroutine(ImportObject("http://adsk-rc-photofly-prod.s3.amazonaws.com/3.0.0/OUT/EqyhOByLmGBqPxwe0FTjX2CI2SFOE1Ur3bITaHTskao%3D-osBgaw1lPLgs4QtBbbBkpS4yPZWeSPYxPK1odPK0HYo/100000000/fire.obj.zip?response-content-disposition=attachment%3B%20filename%3D3.0.0%2FOUT%2FEqyhOByLmGBqPxwe0FTjX2CI2SFOE1Ur3bITaHTskao%3D-osBgaw1lPLgs4QtBbbBkpS4yPZWeSPYxPK1odPK0HYo%2F100000000%2Ffire.obj.zip&AWSAccessKeyId=AKIAISSOG3ZDTRIE6NGA&Expires=1522610025&Signature=64f36%2BY68WfOWfzRhuQE3AzI7vk%3D"));
        } 
	}

	IEnumerator newPhotoScene()
	{
		/*
		curl -v 'https://developer.api.autodesk.com/photo-to-3d/v1/photoscene' -X 'POST' 
		-H 'Content-Type: application/json' 
		-H 'Authorization: Bearer eyJhbGciO
iJIUzI1NiIsImtpZCI6Imp3dF9zeW1tZXRyaWNfa2V5In0.eyJjbGllbnRfaWQiOiJ5WjgxMllmaTZ3REQxdUt1M09hTWdVN2FhMnhVSW5nVCIsImV4cCI6MTUxNzAwMDY5MSwic2NvcGUiOlsiZGF0YTpyZWFkIiwiZGF0YTp3cml0ZSJdLCJhdWQiOiJodHRwczovL2F1dG9kZXNrLmNvbS9hdWQvand0ZXhwNjAiLCJqdGkiOiJFdmhTZjdSc2FLNHEwUDlKc0hKcW5WeFhBWTFDcTB0NkdKSlNibFFYUnZlb1hWUmlFUFhKT08xQTFOSThYcXJtIn0.kGDxTfumgmH5AcX322_AljPWYDpqX97fI8VyvpbzSeA' 
		-d 'scenename=brandonshoe2' 
		-d 'format=rcs,obj,rcm,ortho' 
		-d 'metadata_name[0]=orthogsd' 
		-d 'metadata_value[0]=0.1' 
		-d 'metadata_name[1]=targetcs' 
		-d 'metadata_value[1]=UTM84-32N'
		 */

		string url = "https://developer.api.autodesk.com/photo-to-3d/v1/photoscene";
		WWWForm form = new WWWForm();

        form.AddField("scenename", sceneName);
		form.AddField("format", format);
		form.AddField("metadata_name[0]", "orthogsd");
		form.AddField("metadata_value[0]", "0.1");
		form.AddField("metadata_name[1]", "targetcs");
		form.AddField("metadata_value[1]", "UTM84-32N");

        UnityWebRequest www = UnityWebRequest.Post(url, form);
		www.SetRequestHeader("Content-Type","application/json");
		www.SetRequestHeader("Authorization","Bearer " + access_token);
        yield return www.Send();
 
        if(www.isNetworkError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log("Photoscene complete!");
			debug.text = "Photoscene complete!";
			// photoSceneId = SceneID.CreateFromJSON(www.downloadHandler.text).photosceneid;
			// print(photoSceneId);
			photoSceneId = parsePhotoSceneID(www.downloadHandler.text);
            StartCoroutine(PostPics());
        } 
	}

	string parsePhotoSceneID(string json)
	{
		string[] split = json.Split(':');
		string ID = split[4];
		ID = ID.Remove(0,1);
		ID = ID.Remove((ID.Length - 1),1);
		ID = ID.Remove((ID.Length - 1),1);
		ID = ID.Remove((ID.Length - 1),1);
		return ID;

	}

    IEnumerator PostPics()
    {
        /*
                  curl -v 'https://developer.api.autodesk.com/photo-to-3d/v1/file' -X 'POST' 
                  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsImtpZCI6Imp3dF9zeW1tZXRyaWNfa2V5In0.eyJjbGllbnRfaWQiOiJ5WjgxMllmaTZ3REQxdUt1M09hTWdVN2FhMnhVSW5nVCIsImV4cCI6MTUxNzAwMDY5MSwic2NvcGUiOlsiZGF0YTpyZWFkIiwiZGF0YTp3cml0ZSJdLCJhdWQiOiJodHRwczovL2F1dG9kZXNrLmNvbS9hdWQvand0ZXhwNjAiLCJqdGkiOiJFdmhTZjdSc2FLNHEwUDlKc0hKcW5WeFhBWTFDcTB0NkdKSlNibFFYUnZlb1hWUmlFUFhKT08xQTFOSThYcXJtIn0.kGDxTfumgmH5AcX322_AljPWYDpqX97fI8VyvpbzSeA' 
                  -F "photosceneid=J4JsZovr9E2tRspPmf6ihe37CSZJm4nHxvZJhYStartCoroutine(newPhotoScene());mvmU4" 
                  -F "type=image" 
                  -F "file[0]=@/home/jed/Pictures/brandonShoe/1.jpg"
                   */

        string url = "https://developer.api.autodesk.com/photo-to-3d/v1/file";
        WWWForm form = new WWWForm();

        form.AddField("photosceneid", photoSceneId);
        form.AddField("type", "image");
        // string urlEncoded = WWW.EscapeURL("@/home/jed/Pictures/brandonShoe/1.jpg");
        // string [] fileEntries = Directory.GetFiles("Assets/StreamingAssets/");
        string[] files = null;

        if (device == "android")
        {
            files = Directory.GetFiles(Application.persistentDataPath + "/");
        }
        else if (device == "pc")
        {
            files = Directory.GetFiles("Assets/StreamingAssets/");
        }
        string[] fileEntries = new string[images.Length + files.Length];
        images.CopyTo(fileEntries, 0);
        files.CopyTo(fileEntries, images.Length);
        int count = 1;
        debug.text = "Posting " + fileEntries.Length + " pics";
        Debug.Log("Posting " + fileEntries.Length + " pics");

        foreach (string file in fileEntries)
        {
            if (!file.Contains(".meta"))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(file);
                form.AddBinaryData("file[0]", bytes, count.ToString() + ".jpg");

                UnityWebRequest www = UnityWebRequest.Post(url, form);
                www.SetRequestHeader("Authorization", "Bearer " + access_token);
                yield return www.Send();

                if (www.isNetworkError)
                {
                    Debug.Log(www.error);
                    debug.text = www.error;
                }
                else
                {
                    Debug.Log("photo upload complete!");
                    debug.text = www.downloadHandler.text;
                    print(www.downloadHandler.text);
                }
                count++;
            }
        }
        Debug.Log(count + " Number of photos");
        StartCoroutine(Process());
    }	

	IEnumerator Process()
	{
        /*d
		Process photoscene

		curl -v 'https://developer.api.autodesk.com/photo-to-3d/v1/photoscene/VLtjyTqPTCWq36NyLx2jdHFO6wlaqKnlqWGgFFOuHDg' 
		-X 'POST' 
		-H 'Content-Type: application/json' d
		-H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsImtpZCI6Imp3dF9zeW1tZXRyaWNfa2V5In0.eyJjbGllbnRfaWQiOiJ5WjgxMllmaTZ3REQxdUt1M09hTWdVN2FhMnhVSW5nVCIsImV4cCI6MTUxNzAwMDY5MSwic2NvcGUiOlsiZGF0YTpyZWFkIiwiZGF0YTp3cml0ZSJdLCJhdWQiOiJodHRwczovL2F1dG9kZXNrLmNvbS9hdWQvand0ZXhwNjAiLCJqdGkiOiJFdmhTZjdSc2FLNHEwUDlKc0hKcW5WeFhBWTFDcTB0NkdKSlNibFFYUnZlb1hWUmlFUFhKT
		 */
   
        var form = new WWWForm();
		// var headers = new Hashtable();
		// headers.Add ("Content-Type", "application/json");
		// headers.Add("Authorization", "Bearer " + access_token);

		string url = "https://developer.api.autodesk.com/photo-to-3d/v1/photoscene/" + photoSceneId;
		UnityWebRequest www = UnityWebRequest.Post(url, form);
		www.SetRequestHeader("Content-Type","application/json");
		www.SetRequestHeader("Authorization","Bearer " + access_token);

		// Post a request to an URL with our custom headers
		yield return www.Send();
		string statusJSON = www.downloadHandler.text;
		print (www.downloadHandler.text);
		print("PROCESSING");
		debug.text = www.downloadHandler.text;
		
		StartCoroutine(GetStatus());
	}

	IEnumerator GetStatus()
	{
		while(status != true)
		{
			var form = new WWWForm();
			var headers = new Hashtable();
			headers.Add ("Content-Type", "application/json");
			headers.Add("Authorization", "Bearer " + access_token);

			string url = "https://developer.api.autodesk.com/photo-to-3d/v1/photoscene/" + photoSceneId + "/progress";
			WWW www = new WWW(url, null, headers);

			// Post a request to an URL with our custom headers
			yield return www;
			string statusJSON = www.text;
			string[] split = statusJSON.Split(':');
			string last = split[split.Length - 1];
			last = last.Remove(0,1);
			last = last.Remove((last.Length - 1),1);
			last = last.Remove((last.Length - 1),1);
			last = last.Remove((last.Length - 1),1);

			if(last == "100")
			{
				StartCoroutine(Download());
				status = true;
			}
			else
			{
				print (www.text);
				debug.text = www.text;
				// print (www.responseHeaders);
				yield return new WaitForSeconds(6f);
			}
			
		}
	}

	IEnumerator Download()
	{
		print("downloadin...");
		var form = new WWWForm();
		var headers = new Hashtable();
		headers.Add ("Content-Type", "application/json");
		headers.Add("Authorization", "Bearer " + access_token);

        string url = "https://developer.api.autodesk.com/photo-to-3d/v1/photoscene/" + photoSceneId + "?format=obj";
		WWW www = new WWW(url, null, headers);

		// Post a request to an URL with our custom headers
		yield return www;

		print (www.text);
		print (www.responseHeaders);

		string statusJSON = www.text;
		string[] split = statusJSON.Split(':');

		string last1 = split[7];
		last1 = last1.Remove(0,1);

		string last2 = split[8];
		
		last2 = last2.Remove((last2.Length - 1),1);
		last2 = last2.Remove((last2.Length - 1),1);
		last2 = last2.Remove((last2.Length - 1),1);
		last2 = last2.Remove((last2.Length - 1),1);
		last2 = last2.Remove((last2.Length - 1),1);
		last2 = last2.Remove((last2.Length - 1),1);
		last2 = last2.Remove((last2.Length - 1),1);

		string result = (last1 + ':' + last2);
		print(result);
		string downloadURL = Regex.Unescape(result);
		debug.text = downloadURL;
        print(downloadURL);
        sceneText.text = downloadURL;

		StartCoroutine(ImportObject(downloadURL));
	}

    IEnumerator ImportObject(string url)
    {
        print("import called");
        WWW www = new WWW(url);
        yield return www;

        string zip_path = "";
        string write_path = "";

        if (!string.IsNullOrEmpty(www.error))
        {
            // debug.text = "download failed";
        }
        else
        {
            if (device == "android")
            {
                zip_path = Application.persistentDataPath + "/download.obj.zip";
                write_path = Application.persistentDataPath + "/download.obj";
            }
            else if (device == "pc")
            {
                zip_path = Application.dataPath + "download.obj.zip";
                write_path = Application.dataPath + "/download.obj";
            }

            System.IO.File.WriteAllBytes(zip_path, www.bytes);
            ZipUtil.Unzip(zip_path, write_path);

            // debug.text = "downloaded and saved";
            sceneText.text = debug.text;
            string[] objFiles = Directory.GetFiles(write_path);
            string objFile = "";
            string textFile = "";
            foreach (string file in objFiles)
            {
                string[] splitFile = file.Split('/');
                if (splitFile[splitFile.Length - 1].Contains(".obj"))
                {
                    objFile = file;
                }
                if (splitFile[splitFile.Length - 1].Contains(".jpg"))
                {
                    textFile = file;
                }
            }
            print("importing: " + objFile);
            importer.Import(objFile, textFile);

        }

        try
        {
            /*
            GameObject spawnedPrefab;
            FastObjImport importTool = new FastObjImport();
            Mesh peanuts = importTool.ImportFile(write_path);
            spawnedPrefab = Instantiate(emptyPrefabWithMeshRenderer);
            spawnedPrefab.GetComponent<MeshFilter>().mesh = peanuts;
            spawnedPrefab.transform.position = new Vector3(0, 0, 0);
            debug.text = "done";
            */
        }
        
        catch(Exception e)
        {
            debug.text = "saved, but..." + e.ToString();
        }
        
        /*
        Mesh importedMesh = objImporter.ImportFile(Application.dataPath + "/Objects/" + modelName);
 
        spawnedPrefab = Instantiate(emptyPrefabWithMeshRenderer);
        spawnedPrefab.transform.position = new Vector3(0, 0, 0);
        spawnedPrefab.GetComponent<MeshFilter>().mesh = importedMesh;
		*/
    }
}
