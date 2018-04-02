using UnityEngine;
using System.Collections;

public class ShoeControls : MonoBehaviour {
    public float zoomSpeed = .05f;
    public float moveSpeed = .025f;

    public GameObject shoeSprite;
    public Camera mainCamera;
    // Use this for initialization
    void Start()
    {
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * moveSpeed * Time.deltaTime, touchDeltaPosition.y * moveSpeed * Time.deltaTime, 0);
        }
        else if (Input.touchCount == 2)
        {
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
            deltaMagnitudeDiff *= -1;
            mainCamera.orthographicSize -= deltaMagnitudeDiff * zoomSpeed * 5 * Time.deltaTime;
            shoeSprite.transform.localScale = new Vector3(shoeSprite.transform.localScale.x + deltaMagnitudeDiff * zoomSpeed * Time.deltaTime, shoeSprite.transform.localScale.x + deltaMagnitudeDiff * zoomSpeed * Time.deltaTime, shoeSprite.transform.localScale.z);


        }
        

        //Vector2 touchDeltaPosition2 = new Vector2(1, 1);
        //transform.Translate(-touchDeltaPosition2.x * moveSpeed * Time.deltaTime, -touchDeltaPosition2.y * moveSpeed * Time.deltaTime, 0);
    }
}
