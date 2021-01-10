using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

public class PaginationScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] HorizontalScrollSnap hss;
    [SerializeField] int Page;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (hss != null)
        {
            hss.GoToScreen(Page);
        }
    }
}