using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//很简单的一个脚本，基本就是调包，也不放进框架里了
//挂载到gameObject后,鼠标放在gameObject上会放大
class UIZoom : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    public float zoomSize = 1.2f;
    #endregion

    #region 属性
    #endregion

    #region 方法
    #endregion

    #region Unity回调
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(zoomSize, zoomSize, 1.0f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }

    #endregion

    #region 事件回调
    #endregion

    #region 帮助方法
    #endregion
}