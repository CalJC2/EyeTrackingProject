using UnityEngine;

public class UnlockCheckManager : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    private int _pinAmountLocked;

    void Start()
    {
        continueButton.SetActive(false);
    }

    public void Unlock()
    {
        continueButton.SetActive(true);
    }
}
