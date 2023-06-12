using UnityEngine;
using UwU.Demo.Shapes;
using UwU.IFS;

namespace UwU.Demo
{
    public class ShapeManager : MonoBehaviour, ILoop
    {
        [GetComponentInChildren] private readonly BoxBehaviour box;
        [GetComponentInChildren] private readonly SphereBehaviour sphere;

        public void Setup()
        {
            this.box.transform.position = new Vector3(-5.5f, 0, 0);
            this.sphere.transform.position = new Vector3(+5.5f, 0, 0);

            this.box.Setup();
            this.sphere.Setup();
        }

        void ILoop.Loop(float dt)
        {
            this.box.Loop(dt);
            this.sphere.Loop(dt);
        }
    }
}