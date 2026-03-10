using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ContinueButtonSlider : MonoBehaviour, IGazeTarget
{
    [SerializeField] private bool isPinCheckPhase;

    public float requiredGazeTime;
    public bool emptyWhenLookingAway = true;
    
    public UnityEvent onFillComplete;

    private Slider _slider;
    private bool isBeingLookedAt = false;
    private bool hasTriggered = false;
    
    void Start()
    {
        _slider = GetComponent<Slider>();

        _slider.minValue = 0f;
        _slider.maxValue = 1f;
        _slider.value = 0f;

    }

    void Update()
    {
        if (hasTriggered) return;

        if (isBeingLookedAt)
        {
            _slider.value += Time.deltaTime / requiredGazeTime;

            if (_slider.value >= 1f)
            {
                TriggerAction();
            }
        }
        else if (emptyWhenLookingAway && _slider.value > 0f)
        {
            _slider.value -= Time.deltaTime / requiredGazeTime;
        }

        isBeingLookedAt = false;
    }

    void TriggerAction()
    {
        hasTriggered = true;
        _slider.value = 1f;
        onFillComplete.Invoke();
    }

    public void LookAt()
    {
        isBeingLookedAt = true;
    }

    public void LookAway()
    {
        isBeingLookedAt = false;   
    }
}
