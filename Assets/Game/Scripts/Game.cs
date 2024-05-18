using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ObjectPool))]
[RequireComponent(typeof(Sound))]
[RequireComponent(typeof(StaticData))]
public class Game : ApplicationBase<Game>//游戏起点，初始化框架
{
    //全局访问的功能
    [HideInInspector] //在编辑器里隐藏
    public ObjectPool ObjectPool = null; //对象池
    [HideInInspector]
    public Sound Sound = null; //声音控制
    [HideInInspector]
    public StaticData StaticData = null; //全局访问的静态数据

    //全局方法
    public void LoadScene(int level)
    {
        //退出旧场景

        //事件参数
        SceneArgs e = new SceneArgs();
        e.SceneIndex = SceneManager.GetActiveScene().buildIndex;

        //发布退出事件
        SendEvent(Consts.E_ExitScene, e);

        //加载新场景
        SceneManager.LoadScene(level, LoadSceneMode.Single); //加载场景的方法
    }

    private void OnLevelWasLoaded(int level)
    {
        Debug.Log("OnLevelWasLoaded:" + level);

        SceneArgs e = new SceneArgs();
        e.SceneIndex = level;

        //发布进入场景事件
        SendEvent(Consts.E_EnterScene, e);
    }

    //游戏入口
    void Start()
    {

        //确保Game对象一直存在，即使场景跳转
        Object.DontDestroyOnLoad(this.gameObject);


        //单例模式初始化
        ObjectPool = ObjectPool.Instance;
        Sound = Sound.Instance;
        StaticData = StaticData.Instance;

        //注册启动命令
        RegisterController(Consts.E_StartUp, typeof(StartUpCommand));

        //启动游戏
        SendEvent(Consts.E_StartUp);

    }

}
