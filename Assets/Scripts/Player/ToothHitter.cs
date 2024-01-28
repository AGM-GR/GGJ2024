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
            StartCoroutine(DisableInput());
        }
    }

     IEnumerator DisableInput()
    {
        _character.CharacterMovement.IsMovementAllowed = false;
        yield return new WaitForSeconds(1.5f);
        _character.CharacterMovement.IsMovementAllowed = true;
        yield return null;
    }

    public void TryToothHit()
    {
        StartCoroutine(ToothHitEnabler());
    }

    IEnumerator ToothHitEnabler()
    {
        _isHitting = true;
        HitTrigger.enabled = true;
        yield return new WaitForSeconds(0.5f);
        HitTrigger.enabled = false;
        _isHitting = false;
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isHitting) return;
        if (other.attachedRigidbody == null) return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            HitTrigger.enabled = false;
            StartCoroutine(ToothHit(other.GetComponent<Character>()));
        }
    }

    IEnumerator ToothHit(Character character)
    {
        _isHitting = false;

        character.CharacterMovement.LookAt(_character.transform.position);
        character.Animator.SetTrigger("ToothHitReceived");
        character.SetPlayerWaitAnimation();
        TeethManager characterTeeth = character.GetComponent<TeethManager>();
        characterTeeth.DropTooth();

        yield return new WaitForSeconds(ToothHitCooldown);
    }

    public void Clear()
    {
        _isHitting = false;
        HitTrigger.enabled = false;
    }
}
