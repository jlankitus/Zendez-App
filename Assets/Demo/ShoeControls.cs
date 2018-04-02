using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShoeControls : MonoBehaviour {
    public float zoomSpeed = .05f;
    public float moveSpeed = .025f;
    public float rotateSpeed = 1;
    public GameObject shoeSprite;
    public Camera mainCamera;
    public Text text;
    int mode = 0;
    bool modeSet = false;
    // Use this for initialization
    void Start()
    {
    }
	
	// Update is called once per frame
	void Update () {

        if (mode == 0 && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchDeltaPosition.x * moveSpeed * Time.deltaTime, touchDeltaPosition.y * moveSpeed * Time.deltaTime, 0);
            modeSet = false;
        }
        else if (mode == 1 && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Rotate(-touchDeltaPosition.x * rotateSpeed * Time.deltaTime, touchDeltaPosition.y * rotateSpeed * Time.deltaTime, 0);
            modeSet = false;

        }
        else if (mode == 2 && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Rotate(0, 0, touchDeltaPosition.y * rotateSpeed * Time.deltaTime);
            modeSet = false;

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
            modeSet = false;

        }
        else if(Input.touchCount >= 5   && !modeSet)
        {
            //text.text = mode.ToString();
            mode = mode + 1;
            if (mode >= 3)
                mode = 0;
            modeSet = true;
        }
    }

    private float _sensitivity = 0.4f;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation  = Vector3.zero;
    private bool _isRotating;
    void OnMouseDown()
    {
        // rotating flag
        _isRotating = true;

        // store mouse
        _mouseReference = Input.mousePosition;
    }

    void OnMouseUp()
    {
        // rotating flag
        _isRotating = false;
    }
}
