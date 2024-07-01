using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//鼠标悬停在卡上时显示卡的信息，buff和技能
public class UIShowInformation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    UIInformationWindow uIInformationWindow = null;
    bool hasEntered = false;

    private void Start()
    {
        uIInformationWindow = GameObject.Find("Canvas").transform.Find("UIInformationWindow").GetComponent<UIInformationWindow>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!hasEntered)
        {
            hasEntered = true;
            Vector3 pos = Input.mousePosition;
            MonsterCard monsterCard = GetComponent<MonsterCard>();
            uIInformationWindow.Show(pos, monsterCard);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hasEntered)
        {
            uIInformationWindow.Hide();
            hasEntered = false;
        }
    }

}