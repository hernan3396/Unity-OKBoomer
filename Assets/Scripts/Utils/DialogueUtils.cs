using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueUtils : MonoBehaviour
{
    [SerializeField] private DialogueScriptable _text;
    [SerializeField] private float _speed = 0.1f;
    [SerializeField] private string _scene = "MainMenu";
    private TMP_Text _dialogue;

    private IEnumerator _coroutine;

    private void Awake()
    {
        _dialogue = GetComponent<TMP_Text>();
        _coroutine = ReadingDialogue();

        StartDialogue();
    }

    private void StartDialogue()
    {
        _dialogue.text = string.Empty;

        StartCoroutine(_coroutine);
    }

    private IEnumerator ReadingDialogue()
    {
        foreach (char item in _text.Text)
        {
            _dialogue.text += item;
            yield return new WaitForSeconds(_speed);
            EventManager.OnPlaySound(AudioManager.SFX.Dialogue);
        }

        Finish();
    }

    public void Finish()
    {
        StopCoroutine(_coroutine);
        _dialogue.text = _text.Text;
        EventManager.OnPlaySound(AudioManager.SFX.FinishDialogue);

        Invoke("ChangeLevel", 1);
    }

    private void ChangeLevel()
    {
        EventManager.OnChangeLevel(_scene);
    }
}
