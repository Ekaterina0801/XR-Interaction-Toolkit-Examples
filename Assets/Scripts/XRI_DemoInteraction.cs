using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(Collider))]
public class XRI_DemoInteraction : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] Renderer targetRenderer;
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material activeMaterial;
    [SerializeField] float revertAfter = 0.25f;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip activateSfx;

    [Header("Animator")]
    [SerializeField] Animator animator;             
    [SerializeField] string triggerName = "Activate";

    [Header("Physics")]
    [SerializeField] Rigidbody rb;
    [SerializeField] float impulseForce = 5f;
    [SerializeField] Vector3 impulseDirection = Vector3.up;

    XRGrabInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<XRGrabInteractable>();
        if (!targetRenderer) targetRenderer = GetComponentInChildren<Renderer>(true);
        if (!defaultMaterial && targetRenderer) defaultMaterial = targetRenderer.sharedMaterial;
        if (!audioSource) audioSource = GetComponent<AudioSource>();
        if (!rb) rb = GetComponent<Rigidbody>();
        if (!animator) animator = GetComponent<Animator>();
    }

    void OnEnable() => interactable.activated.AddListener(OnActivated);
    void OnDisable() => interactable.activated.RemoveListener(OnActivated);

    void OnActivated(ActivateEventArgs _)
    {
 
        if (targetRenderer && activeMaterial)
        {
            targetRenderer.material = activeMaterial;
            if (revertAfter > 0f) Invoke(nameof(RevertMaterial), revertAfter);
        }


        if (audioSource && activateSfx) audioSource.PlayOneShot(activateSfx);

      
        if (animator && !string.IsNullOrEmpty(triggerName))
            animator.SetTrigger(triggerName);
        else
        
            (targetRenderer ? targetRenderer.transform : transform).localScale *= 1.5f;

   
        if (rb)
            rb.AddForce(transform.TransformDirection(impulseDirection.normalized) * impulseForce, ForceMode.Impulse);
    }

    void RevertMaterial()
    {
        if (targetRenderer && defaultMaterial)
            targetRenderer.material = defaultMaterial;
    }
}
