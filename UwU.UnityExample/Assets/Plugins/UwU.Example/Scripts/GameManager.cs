using UnityEngine;

namespace UwU.Example
{
    using UwU.Unity.DI;

    public class GameManager : DIContext
    {
        private ShapeManager shapeManager;
        private SquareBehaviour squareBehaviour;
        private CircleBehaviour circleBehaviour1;
        private CircleBehaviour circleBehaviour2;

        public override void Setup()
        {
            this.shapeManager = GameObject.Find("ShapeManager").GetComponent<ShapeManager>();
            this.squareBehaviour = GameObject.Find("Square").GetComponent<SquareBehaviour>();
            this.circleBehaviour1 = GameObject.Find("Circle").GetComponent<CircleBehaviour>();
            this.circleBehaviour2 = GameObject.Find("Circle (1)").GetComponent<CircleBehaviour>();

            this.binder.BindCommand<ShapeManager, ShapeManager>(this.shapeManager);

            this.binder.BindCommand<IAwake, SquareBehaviour>(this.squareBehaviour);
            this.binder.BindCommand<IStart, SquareBehaviour>(this.squareBehaviour);

            // square will call Update()
            this.binder.BindCommand<IUpdate, SquareBehaviour>(this.squareBehaviour);

            // circle1 will call Update()
            this.binder.BindRelevantsTypeCommand(this.circleBehaviour1, new string[] { });    // Bind all relevant types

            // circle2 will not call Update()
            this.binder.BindCommand<IAwake, CircleBehaviour>(this.circleBehaviour2);
            this.binder.BindCommand<IStart, CircleBehaviour>(this.circleBehaviour2);

            // This method will execute all registered binding commands.
            this.binder.ExecuteBindingCommand();

            this.Inject();
        }

        public override void Initialize()
        {
            this.shapeManager.Inject();
            this.shapeManager.Initialize();
        }
    }
}