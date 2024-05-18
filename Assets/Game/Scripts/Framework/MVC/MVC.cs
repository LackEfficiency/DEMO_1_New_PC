using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class MVC
{
    //存储MVC
    public static Dictionary<string, Model> Models = new Dictionary<string, Model>(); //模型名字————模型
    public static Dictionary<string, View> Views = new Dictionary<string, View>(); //视图名字————视图
    public static Dictionary<string, Type> CommandMap = new Dictionary<string, Type>(); //事件名字————控制器类型

    //注册
    public static void RegisterModel(Model model)
    {
        Models[model.Name] = model;
    }

    public static void RegisterView(View view)
    {
        //如果重复注册，先删除原有 
        if (Views.ContainsKey(view.Name))
            Views.Remove(view.Name);

        view.RegisterEvents();
        Views[view.Name] = view;

    }

    public static void RegisterController(string eventName, Type controllerType)
    {
        CommandMap[eventName] = controllerType;
    }

    //获取
    public static T GetModel<T>() where T : Model
    {
        foreach (Model model in Models.Values)
        {
            if (model is T)
                return (T)model;
        }
        return null;
    }
    public static T GetView<T>() where T : View
    {
        foreach (View view in Views.Values)
        {
            if (view is T)
                return (T)view;
        }
        return null;
    }


    //发送事件

    public static void SendEvent(string eventName, object data = null)
    {
        //控制器响应事件
        if (CommandMap.ContainsKey(eventName))
        {
            Type t = CommandMap[eventName];
            Controller c = Activator.CreateInstance(t) as Controller;

            //控制器执行
            c.Execute(data);
        }

        //视图响应事件
        foreach (View v in Views.Values)
        {
            if (v.AttentionEvents.Contains(eventName))
            {
                //视图响应事件
                v.HandleEvent(eventName, data);

            }
        }


    }
}