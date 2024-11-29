using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class IntroSkipper : MonoBehaviour
{
    [Header("Timeline Settings")]
    [Tooltip("El Playable Director asociado a la Timeline.")]
    public PlayableDirector playableDirector;

    [Tooltip("El segundo al que se saltará en la Timeline.")]
    [Min(0)]
    public double secondToJump;

    [Header("Input Settings")]
    [Tooltip("Referencia al mapa de acciones de entrada.")]
    public InputActionReference submitAction;

    private void OnEnable()
    {
        if (submitAction != null)
        {
            submitAction.action.Enable();
            submitAction.action.performed += OnSubmit;
        }
    }

    private void OnDisable()
    {
        if (submitAction != null)
        {
            submitAction.action.performed -= OnSubmit;
            submitAction.action.Disable();
        }
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        if (playableDirector != null && playableDirector.time < secondToJump)
        {
            playableDirector.time = secondToJump;
            playableDirector.Evaluate();
        }
        else
        {
            Debug.LogWarning("No se ha asignado un PlayableDirector.");
        }
    }
}
