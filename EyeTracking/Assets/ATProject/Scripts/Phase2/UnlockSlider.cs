using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UnlockSlider : MonoBehaviour
{
   [SerializeField] private float fillTime;
   [SerializeField] private bool isStarting;
   private bool startTimer = false;
   
   public UnityEvent onFillComplete;
   public UnityEvent upButtonEvent;
   public UnityEvent downButtonEvent;
   public GameObject _button;
   private bool isPinUp = false;

   public UnityEvent SliderResetEvent;
   
   private Slider _slider;

   private void Start()
   {
      _slider = GetComponent<Slider>();
      upButtonEvent = _button.GetComponent<UnlockButton>().IsUpEvent;
      upButtonEvent.AddListener(UpEvent);
      downButtonEvent = _button.GetComponent<UnlockButton>().IsDownEvent;
      downButtonEvent.AddListener(DownEvent);
      
      _slider.minValue = 0f;
      _slider.maxValue = 1f;
      _slider.value = 0f;
   }

   void Update()
   {
      if (_slider.value >= 1f) return;
      
      if (isStarting || startTimer)
      {
         _slider.value += Time.deltaTime / fillTime;
      }
      

      if (_slider.value >= 0.95f)
      {
         if (isPinUp)
         {
            onFillComplete.Invoke();
         }
         else
         {
            SliderResetEvent.Invoke();
         }
      }

      

      
   }
   
   public void StartTimer()
   {
      startTimer = true;
   }

   public void UpEvent()
   {
      isPinUp = true;
   }

   public void DownEvent()
   {
      isPinUp = false;
   }

   public void SliderReset()
   {
      _slider.value = 0f;
      isPinUp = false;
      startTimer = false;
   }
  
}
