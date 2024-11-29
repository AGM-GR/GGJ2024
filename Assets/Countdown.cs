using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    [Tooltip("El texto que mostrar� la cuenta atr�s.")]
    public TextMeshProUGUI countdownText;

    [Header("Events")]
    [Tooltip("Evento invocado cuando termina la cuenta atr�s.")]
    public UnityEvent onCountdownFinished;


    public void SetValue(string value)
    {
        countdownText.text = value;
    }

    public void ShowRandomGoMessage()
    {
        List<string> msgs = new List<string> { "A ZURRAR", "A POR LOS PI�OS", "PI�OS FUERA", "CUESTI�N DE MOLAR", "HINCA EL COLMILLO", "QUE SALTEN ESOS PI�OS", "LAS CARIES SALEN CARAS" };
        countdownText.text = msgs.GetRandomElement();
    }


    public void Finish()
    {
        onCountdownFinished?.Invoke();
    }


}
