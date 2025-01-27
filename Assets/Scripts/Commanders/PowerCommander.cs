using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class PowerCommander : MonoBehaviour {
    public static PowerCommander Instance { get; private set; }
    // Represents the hologram that is currently being gazed at.
    public GameObject FocusedObject { get; private set; }
    GestureRecognizer recognizer;

    void Awake(){
        Instance = this;

        // Set up a GestureRecognizer to detect Select gestures.
        recognizer = new GestureRecognizer();
        recognizer.Tapped += (args) =>
        {
            // Send an OnSelect message to the focused object and its ancestors.
            if (FocusedObject.transform.root.gameObject != null)
            {
                FocusedObject.transform.root.gameObject.SendMessageUpwards("power", SendMessageOptions.DontRequireReceiver);
                Debug.Log("TV_key Setting = "+CustomizeData.Device[0].key);
            }
        };
        recognizer.StartCapturingGestures();
    }

    void Update(){
        // Figure out which hologram is focused this frame.
        GameObject oldFocusObject = FocusedObject;
        // Do a raycast into the world based on the user's
        // head position and orientation.
        var headPosition = Camera.main.transform.position;
        var gazeDirection = Camera.main.transform.forward;
        RaycastHit hitInfo;
        if (Physics.Raycast(headPosition, gazeDirection, out hitInfo)){
            // If the raycast hit a hologram, use that as the focused object.
            FocusedObject = hitInfo.collider.gameObject;
        }else{
            // If the raycast did not hit a hologram, clear the focused object.
            FocusedObject = null;
        }
        // If the focused object changed this frame,
        // start detecting fresh gestures again.
        if (FocusedObject != oldFocusObject){
            recognizer.CancelGestures();
            recognizer.StartCapturingGestures();
        }
    }
}
