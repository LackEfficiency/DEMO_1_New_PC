using System.Collections;
using UnityEngine;

//Unity脚本 实现渐进渐出效果
public class FadeController : MonoBehaviour
{
    #region 常量
    #endregion

    #region 事件
    #endregion

    #region 字段
    //渐进渐出的时间
    float fadeDuration = 1.0f;
    #endregion

    #region 属性
    #endregion

    #region 方法
    //渐进实现
    public IEnumerator FadeIn(GameObject go)
    {
        SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component missing on " + go.name);
            yield break;
        }

        //设置初始颜色透明
        Color startColor = spriteRenderer.color;
        startColor.a = 0f;

        //目标颜色不透明
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = endColor;
        go.SetActive(true);
    }

    //渐出实现
    public IEnumerator FadeOut(GameObject go)
    {
        SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component missing on " + go.name);
            yield break;
        }

        Color startColor = spriteRenderer.color;

        //目标颜色透明
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            spriteRenderer.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = endColor;
        go.SetActive(false);
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    #endregion

    #region 帮助方法
    #endregion
}