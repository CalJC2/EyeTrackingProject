using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject pinCheckPhaseCanvas;
    public GameObject unlockPhaseCanvas;
    public GameObject openPhaseCanvas;

    private bool pinCheckActive;
    private bool unlockActive;
    private bool openActive;

    private void Start()
    {
        // when the rest of the game is added, set these all to false by default and true when needed
        pinCheckPhaseCanvas.SetActive(true);
        pinCheckActive = true;
        unlockPhaseCanvas.SetActive(false);
        unlockActive = false;
        openPhaseCanvas.SetActive(false);
        openActive = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Continue()
    {
        if(pinCheckActive)
        {
            pinCheckPhaseCanvas.SetActive(false);
            pinCheckActive = false;
            unlockPhaseCanvas.SetActive(true);
            unlockActive = true;
        }
        else if (unlockActive)
        {
            unlockPhaseCanvas.SetActive(false);
            unlockActive = false;
            openPhaseCanvas.SetActive(true);
            openActive = true;
        }
        else if (openActive)
        {
            //go back to the GameScreen
        }
            
    }
}
