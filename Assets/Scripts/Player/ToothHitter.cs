using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToothHitter : MonoBehaviour
{
    public Collider HitTrigger;

    public bool _isHitting;

    [Header("Tooth Hit Settings")]
    public float ToothHitCooldown = 2f;

    private Character _character;

    private void Awake()
    {
        _character = GetComponentInParent<Character>();
    }

    void OnSecondaryAttack(InputValue value)
    {
        if (!_isHitting && value.isPressed)
        {
            _character.Animator.SetTrigger("ToothHit");
        }
    }

    public void TryToothHit()
    {
        StartCoroutine(ToothHitEnabler());
    }

    IEnumerator ToothHitEnabler()
    {
        HitTrigger.enabled = true;
        yield return new WaitForSeconds(0.5f);
        HitTrigger.enabled = false;
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isHitting) return;
        if (other.attachedRigidbody == null) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            HitTrigger.enabled = false;
            StartCoroutine(ToothHit(other.GetComponent<Character>()));
        }
    }

    IEnumerator ToothHit(Character character)
    {
        _isHitting = true;
        
        TeethManager characterTeeth = character.GetComponent<TeethManager>();
        characterTeeth.DropTooth();

        yield return new WaitForSeconds(ToothHitCooldown);

        _isHitting = false;
    }
}
