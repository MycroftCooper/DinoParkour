using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartPageBtnController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    UIManager UIM;
    private void Start()
        => UIM = GameObject.Find("UI").GetComponent<UIManager>();
    public void OnPointerEnter(PointerEventData eventData)
        => UIM.setSelectBtn(gameObject.GetComponent<Image>());

    public void OnPointerExit(PointerEventData eventData)
        => UIM.setSelectBtn(null);
}
