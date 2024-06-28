using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


//鼠标悬停在卡上时显示卡的信息，buff和技能
public class UIShowInformation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    UIInformationWindow uIInformationWindow = null;
    bool hasEntered = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!hasEntered)
        {
            hasEntered = true;
            uIInformationWindow = GameObject.Find("Canvas").transform.Find("UIInformationWindow").GetComponent<UIInformationWindow>();
            Vector3 pos = Input.mousePosition;
            MonsterCard monsterCard = GetComponent<MonsterCard>();
            uIInformationWindow.Show(pos, monsterCard);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        uIInformationWindow.Hide();
        hasEntered = false;
    }
}