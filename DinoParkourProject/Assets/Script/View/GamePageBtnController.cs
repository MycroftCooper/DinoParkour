using UnityEngine;
using UnityEngine.EventSystems;

public class GamePageBtnController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private DinoController DC;
    private bool isDown = false;
    private void Start()
    => DC = GameObject.Find("DinoPrefab").GetComponent<DinoController>();

    public void OnPointerDown(PointerEventData eventData)
        => isDown = true;

    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
        DC.State = DinoController.DinoState.Run;
    }

    private void Update()
    {
        if(isDown)
        {
            if (gameObject.name == "UpBtn")
                DC.State = DinoController.DinoState.Jump;
            if (gameObject.name == "DownBtn")
                DC.State = DinoController.DinoState.Down;
        }
    }
}
