using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class EnterSceneCommand : Controller
{
    public override void Execute(object data)
    {
        SceneArgs e = data as SceneArgs;

        //注册视图(view)
        switch (e.SceneIndex)
        {
            case 0:
                break;

            case 1:
                RegisterView(GameObject.Find("Canvas").transform.Find("UIOpenPackage").GetComponent<UIStore>());
                RegisterView(GameObject.Find("Canvas").transform.Find("UIOpenPackage").GetComponent<UIOpenPackage>());
                RegisterView(GameObject.Find("Canvas").transform.Find("UIStart").GetComponent<UIStart>());
                RegisterView(GameObject.Find("Canvas").transform.Find("UILibrary").GetComponent<UILibrary>());
                break;

            case 2:

                RegisterView(GameObject.Find("Canvas").transform.Find("UIBattle").GetComponent<UIBattle>());
                RegisterView(GameObject.Find("MapSelect").GetComponent<UIMap>());
                break;

            case 4:
                RegisterView(GameObject.Find("Canvas").transform.Find("DeckManager").GetComponent<DeckManager>());
                RegisterView(GameObject.Find("Canvas").transform.Find("BtnBack").GetComponent<UIBack>());
                break;


            case 3:
                RegisterView(GameObject.Find("Canvas").transform.Find("UICardsList").GetComponent<UICardList>());
                RegisterView(GameObject.Find("Canvas").transform.Find("UICountRound").GetComponent<UICountRound>());
                RegisterView(GameObject.Find("Canvas").transform.Find("UIShowRound").GetComponent<UIShowRound>());
                RegisterView(GameObject.Find("Map").GetComponent<Spawner>());
                RegisterView(GameObject.Find("Map").GetComponent<Spell>());
                RegisterView(GameObject.Find("Canvas").transform.Find("UIButtonBattle").GetComponent<UIButtonBattle>());
                RegisterView(GameObject.Find("Map").GetComponent<CardAction>());
                RegisterView(GameObject.Find("Canvas").transform.Find("UIInformationWindow").GetComponent<UIInformationWindow>());
                RegisterView(GameObject.Find("Canvas").transform.Find("UIWin").GetComponent<UIWin>());
                RegisterView(GameObject.Find("Canvas").transform.Find("UILost").GetComponent<UILost>());
                break;
            default:
                break;
        }
    }
}