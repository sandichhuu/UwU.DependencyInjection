using UnityEngine;
using UwU.Demo;

public class BoxBehaviour : MonoBehaviour, ILoop
{
    public void Setup()
    {
        Debug.Log("hello, i'm a cube, not a box.");
    }

    public void Loop(float dt)
    {
        var rotationX = Mathf.Sin(Time.time);
        this.transform.rotation = Quaternion.Euler(rotationX * 100, 0, 0);
    }
}
