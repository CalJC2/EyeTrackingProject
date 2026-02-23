using System;
using System.Collections.Generic;
using Eyeware.BeamEyeTracker;
using Eyeware.BeamEyeTracker.Unity;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LockpickController : BeamEyeTrackerMonoBehaviour
{
    private IGazeTarget _targetPin;
    private PointerEventData _eventData;
    private EventSystem _eventSystem;
    private Vector2 _GazePos;
    private Vector2 _screenPos;

    private void Start()
    {
        // get the event system that spawns with a canvas
        _eventSystem = EventSystem.current;
        _eventData = new PointerEventData(EventSystem.current);
    }

    private void Update()
    {
        //checks if the BeamEyeTracker is working
        if (betInputDevice == null) return;
        
        //get the viewport gaze
       _GazePos = betInputDevice.viewportGazePosition.ReadValue();
       // convert viewport to Screen pixels for UI support
       _screenPos = new Vector2(_GazePos.x * Screen.width, _GazePos.y * Screen.height);
       // set pointer to gaze position
       _eventData.position = _screenPos;
       
       // raycast into the UI canvas
       List<RaycastResult> results = new List<RaycastResult>();
       _eventSystem.RaycastAll(_eventData, results);
       
       IGazeTarget foundPin = null;

       //Loop through everything the gaze position hits ( Pierces through everything)
       foreach (RaycastResult result in results)
       {
           //check if the object hit is has the IGazeTarget interface
           foundPin = result.gameObject.GetComponent<IGazeTarget>();
           if (foundPin != null) break;
       }

       // if the raycast hit an IGazeTarget
       if (foundPin != null)
       {
           // if it is set as the target pin
           if (_targetPin != foundPin)
           {
               // if the target pin has something in it, look away from that pin and set the target pin to the one found
               if(_targetPin != null) _targetPin.LookAway();
               _targetPin = foundPin;
           }
           
           _targetPin.LookAt();
       }
       else
       {
           // if the target pin is set but the raycast returns no found pin
           if (_targetPin != null)
           {
               // set the target pin to look away and reset it to null
               _targetPin.LookAway();
               _targetPin = null;
           }
       }
    }
}
