using UnityEngine;
using System.Collections;
using System.IO;
using System.IO.Compression;

public class DownloadObj : MonoBehaviour {

    public string link;
    public string modelName = "New_Model";

    public delegate void ObjectImportedEventHandler(GameObject gameObject);
    public event ObjectImportedEventHandler ObjectImported;


    // Use this for initialization
    void Start()
    {
        StartCoroutine(ImportObject());
    }

    // Update is called once per frame
    IEnumerator ImportObject()
    {

        //link = "http://images.earthcam.com/ec_metros/ourcams/fridays.jpg";
        //link = "http://adsk-rc-photofly-prod.s3.amazonaws.com/3.0.0/OUT/LzT1os0PPaM5JvV0A8176DNs5NuVc6Fr9UWMb6PYgyk%3D-rF5sRX8krJtylKB3UXrPUtdi8nDCkU99gLVU3NGgvFg/100000000/3295.obj.zip?response-content-disposition=attachment%3B%20filename%3D3.0.0%2FOUT%2FLzT1os0PPaM5JvV0A8176DNs5NuVc6Fr9UWMb6PYgyk%3D-rF5sRX8krJtylKB3UXrPUtdi8nDCkU99gLVU3NGgvFg%2F100000000%2F3295.obj.zip&AWSAccessKeyId=AKIAISSOG3ZDTRIE6NGA&Expires=1522431524&Signature=F8FRmlKn31TX1LLzmI4kb43N8PE%3D";

        WWW www = new WWW(link);
        yield return www;

        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
            Debug.Log("Download Error");
        }
        else
        {
            string write_path = Application.dataPath + "/Objects/" + modelName + ".zip";
            System.IO.File.WriteAllBytes(write_path, www.bytes);
            Debug.Log("Wrote to path");
        }

        // Decompress
        //Decompress(new FileInfo(Application.dataPath + "/Objects/" + modelName + ".zip"));

        //Mesh importedMesh = objImporter.ImportFile(Application.dataPath + "/Objects/" + modelName);
        //GameObject emptyGameObject = new GameObject();
        //emptyGameObject.transform.position = new Vector3(0, 0, 0);
        //MeshFilter meshFilter = emptyGameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
        //meshFilter.mesh = importedMesh;

        //if(ObjectImported != null)
        //{
        //    ObjectImported(emptyGameObject);
        //}
    }



    /// None of this decompress stuff works!!!!
    
    //public void Decompress(FileInfo fileToDecompress)
    //{
    //    using (FileStream originalFileStream = fileToDecompress.OpenRead())
    //    {
    //        string currentFileName = fileToDecompress.FullName;
    //        string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

    //        using (FileStream decompressedFileStream = File.Create(newFileName))
    //        {
    //            using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
    //            {
    //                CopyTo(decompressionStream, decompressedFileStream);
    //                //decompressionStream.CopyTo(decompressedFileStream);
    //            }
    //        }
    //    }
    //}

    //public static void CopyTo(Stream input, Stream output)
    //{
    //    byte[] buffer = new byte[16 * 1024];
    //    int bytesRead;
    //    while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
    //    {
    //        output.Write(buffer, 0, bytesRead);
    //    }
    //}
}
