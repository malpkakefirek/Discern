using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class RoomSelect : MonoBehaviour, ISelectHandler
{
    [SerializeField] private TextMeshProUGUI selectionText;

    public void OnSelect(BaseEventData eventData)
    {
        selectionText.text = gameObject.name;
    }
}
