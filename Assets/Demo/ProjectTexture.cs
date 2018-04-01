using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//////////////////
// Put on an object with a sprite renderer to have it project its texture on to a mesh.
//
// pixelsPerUnit - pixels per unit of the texture we want to put on the mesh
// shoeObject    - the object that contains the mesh we want to retexture
//
// depthTest       - Test if verticies can be seen by camera 
// mainCamera    - main camera only really needs to be set if raycast == true

//////////////////

[RequireComponent(typeof(SpriteRenderer))]
public class ProjectTexture : MonoBehaviour
{
    // Pixels per unit of the source texture
    float pixelsPerUnit = 100;

    // GameObject that contains the shoe mesh
    public GameObject shoeObject;

    // set to test if verts are seeable by camera
    public bool depthTest;
    public Camera mainCamera;

    public GameObject perspectiveCamera;

    public bool whiteTransparent = false;

    Dictionary<Vector3, Vector2> vertToUv = new Dictionary<Vector3, Vector2>();
    SpriteRenderer spriteRenderer;
    Texture2D text;

    void FillVertToText()
    {
        Mesh mesh = shoeObject.GetComponent<MeshFilter>().mesh;
        Vector2[] uvs = mesh.uv;
        Vector3[] verts = mesh.vertices;
        Material mat = shoeObject.GetComponent<MeshRenderer>().material;
        Texture2D targetText = mat.GetTexture("_MainTex") as Texture2D;
        for (int i = 0; i < verts.Length; i++)
        {

            Vector2 texelCords = new Vector2(Mathf.FloorToInt(uvs[i].x * targetText.width), Mathf.FloorToInt(uvs[i].y * targetText.height));
            if (!vertToUv.ContainsKey(verts[i]))
            {
                vertToUv.Add(verts[i],texelCords);
            }
        }
    }

    public void ProjectOnToTexture()
    {
        pixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;
        // Loop through verts of mesh
        Mesh mesh = shoeObject.GetComponent<MeshFilter>().mesh;
        Vector3[] verts = mesh.vertices;
        List<Vector2> uvs = new List<Vector2>();
        foreach (Vector3 vert in verts)
        {
            Vector3 vertTrans = shoeObject.transform.TransformPoint(vert);
            Vector2 sourceTextureCords = VertToSourceTextureCord(vertTrans);
            if (sourceTextureCords.x * sourceTextureCords.y >= 0 && sourceTextureCords.x < text.width && sourceTextureCords.y < text.height)
            {
                if(depthTest)
                {
                    float scale = .001f;
                    if (!Physics.Linecast(mainCamera.transform.position, new Vector3(vertTrans.x + (mainCamera.transform.position.x - vertTrans.x) * scale, vertTrans.y + (mainCamera.transform.position.y - vertTrans.y) * scale, vertTrans.z + (mainCamera.transform.position.z - vertTrans.z) * scale)))
                    {
                        //if (text.GetPixel((int)sourceTextureCords.x, (int)sourceTextureCords.y) == Color.white)
                        //    uvs.Add(Vector2.zero);
                        uvs.Add(new Vector2((sourceTextureCords.x / text.width), (sourceTextureCords.y / text.height)));
                        //Debug.DrawLine(mainCamera.transform.position, new Vector3(vertTrans.x + (mainCamera.transform.position.x-vertTrans.x)* scale, vertTrans.y + (mainCamera.transform.position.y - vertTrans.y) * scale, vertTrans.z + (mainCamera.transform.position.z - vertTrans.z) * scale), Color.blue, 10);
                    }
                    else
                    {
                       // Debug.DrawLine(mainCamera.transform.position, new Vector3(vertTrans.x + (mainCamera.transform.position.x - vertTrans.x) * scale, vertTrans.y + (mainCamera.transform.position.y - vertTrans.y) * scale, vertTrans.z + (mainCamera.transform.position.z - vertTrans.z) * scale), Color.red, 10);
                        uvs.Add(Vector2.zero);
                    }
                }
                else
                {
                    uvs.Add(new Vector2((sourceTextureCords.x / text.width), (sourceTextureCords.y / text.height)));
                }
            }
            else
            {
                uvs.Add(Vector2.zero);
            }
        }

        Material mat = shoeObject.GetComponent<MeshRenderer>().material;
        mat.SetTexture("_MainTex", text);
        mesh.SetUVs(0,uvs);
        Color color = spriteRenderer.color;
        color.a = 0;
        spriteRenderer.color = color;
        perspectiveCamera.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        Debug.Log("Done");
    }

    Vector2 VertToDestTextureCord(Vector3 vert)
    {
        return vertToUv[vert];
    }

    Vector2 VertToSourceTextureCord(Vector3 vert)
    {
        float pixUnitsX = pixelsPerUnit /transform.localScale.x;
        float pixUnitsY = pixelsPerUnit /transform.localScale.y;
        Vector2 basePosition = new Vector2(this.transform.position.x - text.width/(2 * pixUnitsX)  , this.transform.position.y - text.height/ (2 * pixUnitsY));
        
        float xPosition = (vert.x - basePosition.x) * pixUnitsX;
        float yPosition = (vert.y - basePosition.y) * pixUnitsY;
        return new Vector2(Mathf.RoundToInt(xPosition), Mathf.RoundToInt(yPosition));
    }

    void WhiteTrans()
    {
        int height = text.height;
        int width = text.width;
        for(int x = 0; x < height; x++)
        {
            for(int y = 0; y < width; y++)
            {
                Color color = text.GetPixel(x, y);
                //color.a = 1;
                if(color.r + color.g + color.b > 2.9)
                {
                    color.a = 0;

                }
                text.SetPixel(x, y, color);
            }
        }
        text.Apply();
    }
    // Use this for initialization
    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        text = spriteRenderer.sprite.texture;
        if(whiteTransparent)
        {
            WhiteTrans();
        }
        FillVertToText();
        //ProjectOnToTexture();
    }
}
