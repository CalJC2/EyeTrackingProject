using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//Holds data phase 1 makes and phase 2 needs
[Serializable]
public class PinData
{
    public float targetHeight;
    public int assignedNumber;
    public bool isSet;
}

public class LockPickManager : MonoBehaviour
{
    // Singleton instance
    // so other scripts can easily talk to the lockpickingmanager using LockPickManager.Instance
    public static LockPickManager Instance { get; private set; }
    
    public enum Phase{None, Phase1, Phase2, Phase3, Complete}

    [Header("Game State")] 
    public Phase currentPhase = Phase.None;
    public PinData[] lockPins = new PinData[5];

    [Header("Player Control Events")] 
    public UnityEvent OnMiniGameStart;
    public UnityEvent OnMiniGameEnd;
    
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
    
    [Header("Audio References")]
    public AudioSource audioSource;
    public AudioClip[] pinSetSounds;
    public AudioClip pinMoveSound;
    public AudioClip barPassSound;
    public AudioClip barFailSound;
    public AudioClip doorUnlockSound;
    public AudioSource loopingAudioSource;

    private DoorController targetDoor;
    private Slider Continue1Slider;
    private Slider Continue2Slider;
    private int[] phase2AssignedNumbers;

    private void Awake()
    {
        // set up singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
 
    }

    private void Start()
    {
        if (phase1ContinueButton != null) Continue1Slider = phase1ContinueButton.GetComponent<Slider>();
        if (phase2ContinueButton != null) Continue2Slider = phase2ContinueButton.GetComponent<Slider>();
        StartPhase(Phase.None);
        
    }

    public void StartMiniGame(DoorController doorToUnlock)
    {
        targetDoor = doorToUnlock;
        
        //disable player controls
        if(OnMiniGameStart != null) OnMiniGameStart.Invoke();
        
        phase2AssignedNumbers = null;
        
        StartPhase(Phase.Phase1);

    }

    public void StartPhase(Phase newPhase)
    {
        currentPhase = newPhase;
        // reset all canvases and buttons
        if (Phase1Canvas != null) Phase1Canvas.SetActive(false);
        if (Phase2Canvas != null) Phase2Canvas.SetActive(false);
        if (Phase3Canvas != null) Phase3Canvas.SetActive(false);
        phase1ContinueButton.SetActive(false);
        phase2ContinueButton.SetActive(false);
        
        if (Continue1Slider != null) Continue1Slider.value = 0f;
        if (Continue1Slider != null) Continue2Slider.value = 0f;

        //switch based on which phase the player will be in
        switch (currentPhase)
        {
            case Phase.None:
                break;
            case Phase.Phase1:
                Phase1Canvas.SetActive(true);
                InitialisePhase1RandomData();
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

    private void InitialisePhase1RandomData()
    {
        // minimum and max height each pin could move to
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
            // sets the assigned numbers for phase 2
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

        // shuffle the indices used to set each pin
        int[] indices = { 0, 1, 2, 3, 4 };
        for (int i = indices.Length - 1; i >= 0; i--)
        {
            int randomIndex = Random.Range(0, i+1);
            int temp = indices[i];
            indices[i] = indices[randomIndex];
            indices[randomIndex] = temp;
        }

        // set each pin within phase 1
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
        // called everytime a pin is set
        lockPins[pinIndex].isSet = true;
        PlayRandomPinSetSound();
        CheckPhase1Complete();

    }

    public void CheckPhase1Complete()
    {
        // checks each pin to see if they are all in the set position
        foreach (PinData pin in lockPins)
        {
            if (!pin.isSet) return;
        }
        
        phase1ContinueButton.SetActive(true);
    }


    private void InitialisePhase2Data()
    {
        phase2AssignedNumbers = new int[5];

        for (int i = 0; i < phase1Pins.Length; i++)
        {
            if (phase1Pins[i] != null)
            {
                phase2AssignedNumbers[i] = phase1Pins[i].GetAssignedNumber();
            }
        }
        
        Array.Reverse(phase2AssignedNumbers);

        string debugString = "Sequence sent to phase 2 bar: ";
        for (int i = 0; i < phase2AssignedNumbers.Length; i++)
        {
            debugString += phase2AssignedNumbers[i] + ", ";
        }
        Debug.Log(debugString);
        
        int[] randomButtonLayout = new int[] { 1, 2, 3, 4, 5 };
        for (int i = randomButtonLayout.Length - 1; i >= 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = randomButtonLayout[i];
            randomButtonLayout[i] = randomButtonLayout[randomIndex];
            randomButtonLayout[randomIndex] = temp;
        }
        
        
        for(int i = 0; i < phase2Buttons.Length; i++)
        {
            if (phase2Buttons[i] != null)
            {
                phase2Buttons[i].SetUpButton(randomButtonLayout[i]);
            }
        }

        if (Phase2Bar != null)
        {
            Phase2Bar.InitialisePhase2(phase2AssignedNumbers);
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

        if (targetDoor != null)
        {
            targetDoor.UnlockAndOpenDoor();
            targetDoor = null;
        }

        if (OnMiniGameEnd != null) OnMiniGameEnd.Invoke();
    }

    public PinData GetPinData(int index)
    {
        return lockPins[index];
    }

    public void PlayRandomPinSetSound()
    {
        if (audioSource != null && pinSetSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, pinSetSounds.Length);
            audioSource.PlayOneShot(pinSetSounds[randomIndex]);
        }
    }

    public void PlayPinMoveSound()
    {
        if (audioSource != null && pinMoveSound != null) audioSource.PlayOneShot(pinMoveSound);
    }

    public void PlayBarPassSound()
    {
        if (audioSource != null && barPassSound != null) audioSource.PlayOneShot(barPassSound);
    }

    public void PlayBarFailSound()
    {
        if (audioSource != null && barFailSound != null) audioSource.PlayOneShot(barFailSound);  
    }

    public void Phase1PinStartMoveSound()
    {
        if (loopingAudioSource != null && pinMoveSound != null && !loopingAudioSource.isPlaying)
        {
            loopingAudioSource.clip = pinMoveSound;
            loopingAudioSource.loop = true;
            loopingAudioSource.Play();
        }
    }

    public void Phase1PinStopMoveSound()
    {
        if (loopingAudioSource != null && loopingAudioSource.isPlaying)
        {
            loopingAudioSource.Stop();
        }
    }
    
}
