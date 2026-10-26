using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GuardActionType
{
    Benar,
    Salah,
    BuangSampah
}

[System.Serializable]
public class DialogueVOData
{
    [TextArea(2, 4)]
    public string text;
    
    [Tooltip("Isi dengan FMOD Event Path, contoh: event:/VoiceOver/VOLvl1/Salah1")]
    public string voEventPath; 
}

public class GuardManager : MonoBehaviour
{
    public static GuardManager Instance { get; private set; }

    [Header("UI & Expression Settings")]
    [SerializeField] private Image guardImage;
    [SerializeField] private Sprite happySprite;
    [SerializeField] private Sprite angrySprite;

    [Header("UI Text Component")]
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Dialogue & VO Lists")]
    [SerializeField] private List<DialogueVOData> benarList = new List<DialogueVOData>();
    [SerializeField] private List<DialogueVOData> salahList = new List<DialogueVOData>();
    [SerializeField] private List<DialogueVOData> buangSampahList = new List<DialogueVOData>();

    private int lastBenarIndex = -1;
    private int lastSalahIndex = -1;
    private int lastBuangSampahIndex = -1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void TriggerGuardReaction(GuardActionType actionType)
    {
        switch (actionType)
        {
            case GuardActionType.Benar:
                SetGuardExpression(happySprite);
                PlayRandomDialogue(benarList, ref lastBenarIndex);
                break;

            case GuardActionType.Salah:
                SetGuardExpression(angrySprite);
                PlayRandomDialogue(salahList, ref lastSalahIndex);
                break;

            case GuardActionType.BuangSampah:
                SetGuardExpression(angrySprite);
                PlayRandomDialogue(buangSampahList, ref lastBuangSampahIndex);
                break;
        }
    }

    private void SetGuardExpression(Sprite newSprite)
    {
        if (guardImage != null && newSprite != null)
        {
            guardImage.sprite = newSprite;
        }
    }

    private void PlayRandomDialogue(List<DialogueVOData> list, ref int lastIndex)
    {
        if (list == null || list.Count == 0) return;

        int randomIndex = lastIndex;

        if (list.Count > 1)
        {
            while (randomIndex == lastIndex)
            {
                randomIndex = Random.Range(0, list.Count);
            }
        }
        else
        {
            randomIndex = 0;
        }

        lastIndex = randomIndex;
        DialogueVOData selectedData = list[randomIndex];

        // 1. Update Teks UI Dialogue
        if (dialogueText != null)
        {
            dialogueText.text = selectedData.text;
        }

        // 2. Memutar VoiceOver FMOD menggunakan Path Event
        if (!string.IsNullOrEmpty(selectedData.voEventPath))
        {
            AudioManager.Instance?.PlayVoiceOver(selectedData.voEventPath);
        }
    }
}