using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnInput : MonoBehaviour
{
    [SerializeField] private InputActionProperty pinchAction;
    [SerializeField] private InputActionProperty gripAction;

    [SerializeField] private Animator handAnimator;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        handAnimator.SetFloat("Trigger", 
                                pinchAction.action.ReadValue<float>());
        handAnimator.SetFloat("Grip", gripAction.action.ReadValue<float>());

    }
}
