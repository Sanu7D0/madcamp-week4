using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LobbyUIManager : Singleton<LobbyUIManager>
{
    [SerializeField]
    public GameObject characterSelectPannel;
    [SerializeField]
    public GameObject leaveButton;
    public UnityEvent onClickedLeaveButtonListener;
    public UnityEvent<string> onCharacterClickedListener;

    public void ShowLeaveButton() { 
        characterSelectPannel.SetActive(false);
        leaveButton.SetActive(true);
    }

    public void ShowCharacterSelectPanel() {
        characterSelectPannel.SetActive(true);
        leaveButton.SetActive(false);
    }

    public void onClickedLeaveButton() {
        onClickedLeaveButtonListener.Invoke();
    }

    public void OnCharacterClicked(BaseEventData data)
    {
        PointerEventData ped = (PointerEventData)data;
        string candidatedCharacterName = ped.pointerCurrentRaycast.gameObject.GetComponent<CandidateCharacter>().candidateCharacter.name;
        onCharacterClickedListener.Invoke(candidatedCharacterName);
    }

    public void appendChild(GameObject childObject) {
        var parentTansfrom = GetComponent<RectTransform>();
        childObject.transform.SetParent(parentTansfrom, false);
    }
}