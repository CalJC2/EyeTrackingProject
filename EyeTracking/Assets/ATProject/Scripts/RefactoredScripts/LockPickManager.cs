using UnityEngine;
using System.Collections.Generic;

//Holds data phase 1 makes and phase 2 needs
[System.Serializable]
public class PinData
{
    public float targetHeight;
    public int assignedNumber;
    public bool isSet;
}

public class LockPickManager : MonoBehaviour
{
    // Singleton instance so other scripts can easily talk to the lockpickingmanager
    public static LockPickManager Instance { get; private set; }
    
    public enum Phase{None, Phase1, Phase2, Phase3, Complete}

    [Header("Game State")] 
    public Phase currentPhase = Phase.None;
    public PinData[] lockPins = new PinData[5];
    
    [Header("Phase 1 References")]
    public GameObject Phase1Canvas;
    public GameObject phase1ContinueButton;
    public Phase1PinInteractable[] phase1Pins;
    
    [Header("Phase 2 References")]
    public GameObject Phase2Canvas;
    public GameObject phase2ContinueButton;
    public Phase2Bar Phase2Bar;
    public Phase2Button[]  phase2Buttons;
    
    [Header("Phase 3 References")]
    public GameObject Phase3Canvas;
    public Phase3Rotator Phase3Rotator;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // for testing, will be called when a player walks up to a door
        StartPhase(Phase.Phase3);
        
    }

    public void StartPhase(Phase newPhase)
    {
        currentPhase = newPhase;
        // reset all canvases and buttons
        Phase1Canvas.SetActive(false);
        Phase2Canvas.SetActive(false);
        Phase3Canvas.SetActive(false);
        phase1ContinueButton.SetActive(false);
        phase2ContinueButton.SetActive(false);

        //switch based on which phase the player will be in
        switch (currentPhase)
        {
            case Phase.Phase1:
                Phase1Canvas.SetActive(true);
                InitialisePhase1RandomDate();
                break;
            case Phase.Phase2:
                Phase2Canvas.SetActive(true);
                InitialisePhase2Data();
                break;
            case Phase.Phase3:
                Phase3Canvas.SetActive(true);
                Phase3Rotator.InitialisePhase3();
                break;
            case Phase.Complete:
                Debug.Log("Return to 3d environment");
                break;
            
        }
    }

    private void InitialisePhase1RandomDate()
    {
        float minHeight = 10f;
        float maxHeight = 200f;
        
        // creates a pool of numbers from 1 to 5 and shuffles them
        List<int> availableNumbers = new List<int> {1,2,3,4,5};
        for (int i = availableNumbers.Count - 1; i >= 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            int temp = availableNumbers[i];
            availableNumbers[i] = availableNumbers[rnd];
            availableNumbers[rnd] = temp;
        }
        
        //assign the numbers from above for loop 
        for(int i = 0; i < lockPins.Length; i++)
        {
            int uniqueNumber = availableNumbers[i];
            //makes a percentage based off the unique number and then calculates height off that percentage
            float heightPercentage = (uniqueNumber - 1f )/ 4f;
            float calculatedHeight = Mathf.Lerp(minHeight, maxHeight, heightPercentage);
            
            // assigns the data to the specific pin
            lockPins[i] = new PinData
            {
                targetHeight = calculatedHeight,
                assignedNumber = uniqueNumber,
                isSet = false

            };
            
            Debug.Log($"Data Generated -> Logic Slot {i} holds Number {uniqueNumber} at Height {calculatedHeight:F1}");
        }

        // 
        int[] indices = { 0, 1, 2, 3, 4 };
        for (int i = indices.Length - 1; i >= 0; i--)
        {
            int randomIndex = Random.Range(0, i+1);
            int temp = indices[i];
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        for (int i = 0; i < phase1Pins.Length; i++)
        {
            if (phase1Pins[i] != null)
            {
                phase1Pins[i].SetUpPin(indices[i]);
            }
            else
            {
                Debug.Log("Pin " + indices[i] + " not found");
            }
        }
    }

    public void ReportPinSet(int pinIndex)
    {
        lockPins[pinIndex].isSet = true;
        CheckPhase1Complete();

    }

    public void CheckPhase1Complete()
    {
        foreach (PinData pin in lockPins)
        {
            if (!pin.isSet) return;
        }
        
        phase1ContinueButton.SetActive(true);
    }


    private void InitialisePhase2Data()
    {
        int[] numbersToAssign = new int[5];
        for (int i = 0; i < 5; i++)
        {
            numbersToAssign[i] = lockPins[i].assignedNumber;
        }

        for (int i = numbersToAssign.Length - 1; i >= 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = numbersToAssign[i];
            numbersToAssign[i] = numbersToAssign[randomIndex];
            numbersToAssign[randomIndex] = temp;
        }
        
        for(int i = 0; i < phase2Buttons.Length; i++)
        {
            if (phase2Buttons[i] != null)
            {
                phase2Buttons[i].SetUpButton(numbersToAssign[i]);
            }
        }

        if (Phase2Bar != null)
        {
            Phase2Bar.InitialisePhase2();
        }
    }

    public void CompletePhase2()
    {
        if(phase2ContinueButton != null) phase2ContinueButton.SetActive(true);
        
    }



    public void TriggerPhase3()
    {
        StartPhase(Phase.Phase3);
    }
    
    public void TriggerPhase2()
    {
        StartPhase(Phase.Phase2);
    }

    public void CompleteGame()
    {
        StartPhase(Phase.Complete);
    }
}
