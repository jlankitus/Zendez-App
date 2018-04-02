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

    AudioSource audio;
    void Awake () {
        if (instance != null)
        {
            Debug.LogError("Trying to make another instance of a singleton, not good");
        }
        instance = this;
        audio = this.GetComponent<AudioSource>();
	}

    public void StartTexturingArea()
    {
        Audio();
        textureChoosingArea.SetActive(false);
        modelChoosingArea.SetActive(false);
        texturingArea.SetActive(true);
    }

    public void StartTexturingChoosingArea()
    {
        Audio();

        textureChoosingArea.SetActive(true);
        modelChoosingArea.SetActive(false);
        texturingArea.SetActive(false);
    }

    public void StartModelChoosingArea()
    {
        Audio();

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

    public void Audio()
    {
        audio.pitch = Random.Range(0.5f, 1.5f);
        audio.Play();
    }
    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

}
