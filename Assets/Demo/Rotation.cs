using UnityEngine;

public class Rotation : MonoBehaviour {
    public float xSpeed = 1;
    public float ySpeed = 1;
    public float zSpeed = 1;
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(new Vector3(xSpeed, ySpeed, zSpeed));
	}
}
