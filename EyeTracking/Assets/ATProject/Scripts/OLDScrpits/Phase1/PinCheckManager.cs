using System;
using UnityEngine;

public class PinCheckManager : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    private int _pinAmountLocked;

    void Start()
    {
        continueButton.SetActive(false);
    }

    public void PhaseUpdate()
    {
        _pinAmountLocked++;

        if (_pinAmountLocked >= 5)
        {
            continueButton.SetActive(true);
        }
    }
}
