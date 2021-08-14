using UnityEngine;
using System.Collections;
// a simple gesture script so as to detect some swipe action
public class SwipeGesture : MonoBehaviour {
    // vector 2 - start and end
    private Vector2 startPosition = Vector2.zero;
    private Vector2 endPosition = Vector2.zero;
    // defines screen size for our orthographic camera
    private float _height;
    private float _width;
    // this boolean is used so as to do a combination X and Y when the gesture goes in diagonal
    public bool comboXY = true;

    void Start() {
        // catches the main camera
        Camera cam = Camera.main;
        // checks if the camera is orthographic
        if (cam.orthographic == false) {
            Debug.LogError("This script must be used on an orthographic camera");
            this.enabled = false;
        }
        // defines screen size
        _height = 2f * cam.orthographicSize;
        _width = _height * cam.aspect;
    }
    // update for mobile device so as to detect some gesture on screen
    void Update() {
#if UNITY_ANDROID || UNITY_IOS
        // first event - mouse button down (we got the first point - start)
        if (Input.GetMouseButtonDown(0))
            startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // second event - mouse button up (we got the second point - end)
        if (Input.GetMouseButtonUp(0)) {
            endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // calls the main function
            Swipe(startPosition, endPosition, comboXY);
        }
# endif
    }
    // function so as to translate gesture as an event
    void Swipe(Vector2 startPosition, Vector2 endPosition, bool comboXY) {
        // checks movement
        if (startPosition != endPosition && startPosition != Vector2.zero && endPosition != Vector2.zero) {
            // defines the delta values for each axis
            float deltaX = endPosition.x - startPosition.x;
            float deltaY = endPosition.y - startPosition.y;
            // the movement on X axis is larger than the quater size screen
            if (deltaX >= _width / 4 || deltaX <= -_width / 4) {
                // our movement goes right
                if (startPosition.x < endPosition.x)
                    GetComponent<Manager>().Move(3);
                else // ...or left
                    GetComponent<Manager>().Move(2);
                // continues if we want a combination X and Y
                if (comboXY == false)
                    return; // no? bye !
            }
            // the movement on Y axis is larger than the third size screen
            if (deltaY >= _height / 3 || deltaY <= -_height / 3) {
                // our movement goes up
                if (startPosition.y < endPosition.y)
                    GetComponent<Manager>().Move(0);
                else // ...or down
                    GetComponent<Manager>().Move(1);
            }
            // vector2 null so as to prepare the next gesture
            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
        }
    }
}