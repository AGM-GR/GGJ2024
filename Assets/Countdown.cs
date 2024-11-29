using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    [Tooltip("El texto que mostrará la cuenta atrás.")]
    public TextMeshProUGUI countdownText;

    [Header("Events")]
    [Tooltip("Evento invocado cuando termina la cuenta atrás.")]
    public UnityEvent onCountdownFinished;


    public void SetValue(string value)
    {
        countdownText.text = value;
    }

    public void ShowRandomGoMessage()
    {
        List<string> msgs = new List<string> { "A ZURRAR", "A POR LOS PIÑOS", "PIÑOS FUERA", "CUESTIÓN DE MOLAR", "HINCA EL COLMILLO", "QUE SALTEN ESOS PIÑOS", "LAS CARIES SALEN CARAS" };
        countdownText.text = msgs.GetRandomElement();
    }


    public void Finish()
    {
        onCountdownFinished?.Invoke();
    }


}
