using TMPro;
using UnityEngine;

public class Phase2Button : MonoBehaviour, IGazeTarget
{
    [Header("UI References")]
    public TextMeshProUGUI numberText;
    public Phase2Bar barManager;

    private int myNumber;

    public void SetUpButton(int assignedNumber)
    {
        myNumber = assignedNumber;
        if(numberText != null) numberText.text = myNumber.ToString();
    }
    
    public void LookAt()
    {
        if (barManager != null)
        {
            barManager.SetGazedNumber(myNumber);
        }
    }

    public void LookAway()
    {
        if (barManager != null)
        {
            barManager.ClearGazedNumber(myNumber);
        }
    }
}
