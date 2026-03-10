using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnlockController : MonoBehaviour
{
    [Header("Timer Event Checks")]
    public GameObject _timer1;
    public GameObject _timer2;
    public GameObject _timer3;
    public GameObject _timer4;
    public GameObject _timer5;
    public UnityEvent timer1Event;
    public UnityEvent timer2Event;
    public UnityEvent timer3Event;
    public UnityEvent timer4Event;
    public UnityEvent timer5Event;

    [Header("Timer Reset Event")] 
    private UnlockSlider _slider1;
    private UnlockSlider _slider2;
    private UnlockSlider _slider3;
    private UnlockSlider _slider4;
    private UnlockSlider _slider5;
    


   private void Start()
   {
      timer1Event = _timer1.GetComponent<UnlockSlider>().SliderResetEvent;
      timer2Event = _timer2.GetComponent<UnlockSlider>().SliderResetEvent;
      timer3Event = _timer3.GetComponent<UnlockSlider>().SliderResetEvent;
      timer4Event = _timer4.GetComponent<UnlockSlider>().SliderResetEvent;
      timer5Event = _timer5.GetComponent<UnlockSlider>().SliderResetEvent;
      
      timer1Event.AddListener(Reset);
      timer2Event.AddListener(Reset);
      timer3Event.AddListener(Reset);
      timer4Event.AddListener(Reset);
      timer5Event.AddListener(Reset);

   }

   public void Reset()
   {
       _slider1 = _timer1.GetComponent<UnlockSlider>();
       _slider2 = _timer2.GetComponent<UnlockSlider>();
       _slider3 = _timer3.GetComponent<UnlockSlider>();
       _slider4 = _timer4.GetComponent<UnlockSlider>();
       _slider5 = _timer5.GetComponent<UnlockSlider>();

       _slider1.SliderReset();
       _slider2.SliderReset();
       _slider3.SliderReset();
       _slider4.SliderReset();
       _slider5.SliderReset();

   }
}
