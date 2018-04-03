using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoMaster : MonoBehaviour {

    public static DemoMaster instance;

    public SpriteRenderer spriteRenderer;
    public GameObject shoeObject;

    public GameObject texturingArea;
    public GameObject textureChoosingArea;
    public GameObject modelChoosingArea;
    public GameObject modelScan;

    public ObjImporter objImporter;

    void Awake () {
        if (instance != null)
        {
            Debug.LogError("Trying to make another instance of a singleton, not good");
        }
        instance = this;
        //audio = this.GetComponent<AudioSource>();
    }

    public void UpdateFromUploadedModel(GameObject model)
    {

        MeshFilter meshFilter = model.GetComponent(typeof(MeshFilter)) as MeshFilter;
       // meshFilter.sharedMesh = importedMesh;
        MeshRenderer meshRender = model.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
        Texture2D text = meshRender.material.GetTexture("_MainTex") as Texture2D;
        SetObj(meshFilter.mesh, text, Vector3.zero, Vector3.zero, new Vector3(1,1,1));
        model.SetActive(false);
        StartTexturingArea();
    }
    public void StartTexturingArea()
    {
        Audio();
        textureChoosingArea.SetActive(false);
        modelChoosingArea.SetActive(false);
        texturingArea.SetActive(true);
        modelScan.SetActive(false);
    }

    public void StartTexturingChoosingArea()
    {
        Audio();

        textureChoosingArea.SetActive(true);
        modelChoosingArea.SetActive(false);
        texturingArea.SetActive(false);
        modelScan.SetActive(false);

    }

    public void StartModelChoosingArea()
    {
        Audio();

        textureChoosingArea.SetActive(false);
        modelChoosingArea.SetActive(true);
        texturingArea.SetActive(false);
        modelScan.SetActive(false);

    }

    public void StartModelScan()
    {
        Audio();

        textureChoosingArea.SetActive(false);
        modelChoosingArea.SetActive(false);
        texturingArea.SetActive(false);
        modelScan.SetActive(true);

    }

    public void SetSprite(Sprite text)
    {
        spriteRenderer.sprite = text;
    }

    public void SetObj(Mesh mesh, Texture2D text, Vector3 trans, Vector3 rotation, Vector3 Scale)
    {
        shoeObject.GetComponent<MeshFilter>().mesh = mesh;
        shoeObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", text);
        shoeObject.transform.localPosition = trans;
        shoeObject.transform.localScale = Scale;
        shoeObject.transform.eulerAngles = rotation;
    }

    public void Audio()
    {
        //audio.pitch = Random.Range(0.5f, 1.5f);
        //audio.Play();
    }
    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void FlipShoe()
    {
        Vector3 scale = shoeObject.transform.localScale;
        shoeObject.transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
    }

}
