using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UnlockSlider : MonoBehaviour
{
   [SerializeField] private float fillTime = 20f;
   private bool emptyWhenHit = false;
   
   public UnityEvent onFillComplete;
   
   private Slider _slider;

   private void Start()
   {
      _slider = GetComponent<Slider>();
      
      _slider.minValue = 0f;
      _slider.maxValue = 1f;
      _slider.value = 0f;
   }

   void Update()
   {
      _slider.value += Time.deltaTime / fillTime;

      if (_slider.value >= 1f)
      {
         onFillComplete.Invoke();
      }
      
   }

   public void Reset()
   {
      _slider.value = 0f;
   }
}
