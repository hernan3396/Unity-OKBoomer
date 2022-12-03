using UnityEngine;
using TMPro;
using System.Collections;

public class Clock : MonoBehaviour
{
    [SerializeField] private int _delay = 60;
    private TMP_Text _clockText;

    private void Awake()
    {
        _clockText = GetComponent<TextMeshProUGUI>();
        StartCoroutine(UpdateTime());
    }

    private IEnumerator UpdateTime()
    {
        var today = System.DateTime.Now;
        _clockText.text = today.ToString("HH:mm");

        yield return new WaitForSeconds(_delay);
        StartCoroutine(UpdateTime());
    }
}
