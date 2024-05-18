using System.Collections;
using UnityEngine;

public abstract class Reusable : MonoBehaviour, IReusable
{
    public abstract void OnSpawn();

    public abstract void OnUnspawn();

}