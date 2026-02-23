using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnlockController : MonoBehaviour
{
   [SerializeField] private GameObject _pin1;
   [SerializeField] private GameObject _pin2;
   [SerializeField] private GameObject _pin3;
   [SerializeField] private GameObject _pin4;
   [SerializeField] private GameObject _pin5;
   
   private RectTransform _rectTransformPin1;
   private Vector3 _pin1Position;
   
   private RectTransform _rectTransformPin2; 
   private Vector3 _pin2Position;

   private RectTransform _rectTransformPin3;
   private Vector3 _pin3Position;

   private RectTransform _rectTransformPin4;
   private Vector3 _pin4Position;

   private RectTransform _rectTransformPin5;
   private Vector3 _pin5Position;


   private float _timer = 0f;
   public UnlockSlider _slider;

   private void Start()
   {
      _rectTransformPin1 = _pin1.GetComponent<RectTransform>();
      _rectTransformPin2 = _pin2.GetComponent<RectTransform>();
      _rectTransformPin3 = _pin3.GetComponent<RectTransform>();
      _rectTransformPin4 = _pin4.GetComponent<RectTransform>();
      _rectTransformPin5 = _pin5.GetComponent<RectTransform>();
      
      _pin1Position = _rectTransformPin1.position;
      _pin2Position = _rectTransformPin2.position;
      _pin3Position = _rectTransformPin3.position;
      _pin4Position = _rectTransformPin4.position;
      _pin5Position = _rectTransformPin5.position;
   }

   private void Update()
   {
      _timer += Time.deltaTime;

      if (_timer > 0 && _timer <= 5)
      {
         if (_pin1Position != _pin1.GetComponent<RectTransform>().position)
         {
            _slider.Reset();
            _timer = 0f;
         }
      }
      else if (_timer > 5 && _timer <= 10)
      {
         if (_pin2Position != _pin2.GetComponent<RectTransform>().position)
         {
            _slider.Reset();
            _timer = 0f;
         }
      }
      else if (_timer > 10 && _timer <= 15)
      {
         if (_pin3Position != _pin3.GetComponent<RectTransform>().position)
         {
            _slider.Reset();
            _timer = 0f;
         }
      }
      else if (_timer > 15 && _timer <= 20)
      {
         if (_pin4Position != _pin4.GetComponent<RectTransform>().position)
         {
            _slider.Reset();
            _timer = 0f;
         }
      }
      else if (_timer > 20 && _timer <= 25)
      {
         if (_pin5Position != _pin5.GetComponent<RectTransform>().position)
         {
            _slider.Reset();
            _timer = 0f;
         }
      }
   }
}
