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
    void Awake () {
        if (instance != null)
        {
            Debug.LogError("Trying to make another instance of a singleton, not good");
        }
        instance = this;
	}

    public void StartTexturingArea()
    {
        textureChoosingArea.SetActive(false);
        modelChoosingArea.SetActive(false);
        texturingArea.SetActive(true);
    }

    public void StartTexturingChoosingArea()
    {
        textureChoosingArea.SetActive(true);
        modelChoosingArea.SetActive(false);
        texturingArea.SetActive(false);
    }

    public void StartModelChoosingArea()
    {
        textureChoosingArea.SetActive(false);
        modelChoosingArea.SetActive(true);
        texturingArea.SetActive(false);
    }

    public void SetSprite(Sprite text)
    {
        spriteRenderer.sprite = text;
    }

    public void SetObj(Mesh mesh, Texture2D text)
    {
        shoeObject.GetComponent<MeshFilter>().mesh = mesh;
        shoeObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", text);
    }

    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

}
