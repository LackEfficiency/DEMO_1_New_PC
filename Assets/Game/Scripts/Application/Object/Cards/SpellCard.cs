using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpellCard : Card
{
    #region ����
    #endregion

    #region �¼�
    #endregion

    #region �ֶ�
    string m_Effect;

    #endregion

    #region ����
    public string Effect 
    { 
        get => m_Effect; 
    }
    #endregion

    #region ����
    #endregion

    #region Unity�ص�
    #endregion

    #region �¼��ص�
    public override void OnSpawn()
    {
        base.OnSpawn();
    }

    public override void OnUnspawn()
    {
        base.OnUnspawn();
    }
    #endregion

    #region ��������
    #endregion
}
