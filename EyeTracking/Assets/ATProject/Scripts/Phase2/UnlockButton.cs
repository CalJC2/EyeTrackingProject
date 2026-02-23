using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnluckButton : MonoBehaviour, IGazeTarget
{
    [Header("Pin Movement")]
    [SerializeField] private GameObject unlockPin;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float targetDistance;
    private RectTransform _rectTransform;
    private Vector2 _startPos;
    private Vector2 _targetPos;

    private bool isBeingLookedAt = false;
    private Button _Button;
    
    
    void Start()
    {
        _rectTransform = unlockPin.GetComponent<RectTransform>();
        _startPos = _rectTransform.anchoredPosition;
        _targetPos = _startPos + new Vector2(0, targetDistance);
        
        _Button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingLookedAt)
        {
            _rectTransform.anchoredPosition = Vector2.MoveTowards(_rectTransform.anchoredPosition, _targetPos, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(_rectTransform.anchoredPosition, _targetPos) > 0.01f)
            {
                _rectTransform.anchoredPosition = _targetPos;
            }
        }
        else
        {
            _rectTransform.anchoredPosition = Vector2.MoveTowards(_rectTransform.anchoredPosition, _startPos, moveSpeed * Time.deltaTime);
        }
        
        isBeingLookedAt = false;
    }

    public void LookAt()
    {
        isBeingLookedAt = true;
        _Button.Select();
        
    }

    public void LookAway()
    {
        isBeingLookedAt = false;
        EventSystem.current.SetSelectedGameObject(null);
    }
}
