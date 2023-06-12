using UnityEngine;
using System;
using System.Collections;

namespace UwU.Example
{
    using UwU.DI;
    using UwU.DI.Container;

    public class ShapeManager : MonoBehaviour
    {
        // Field injection
        [Inject] private readonly IDependencyContainer container;

        // Property injection
        [Inject] public SquareBehaviour squareBehaviour { get; internal set; }

        private CircleBehaviour circleBehaviour;

        private IAwake[] awakers;
        private IStart[] starters;
        private IUpdate[] updaters;

        private bool isInitialized;

        // Method injection
        [Inject]
        private void MethodInjection(CircleBehaviour circleBehaviour)
        {
            this.circleBehaviour = circleBehaviour;
        }

        public void Initialize()
        {
            this.awakers = this.container.All<IAwake>();
            this.starters = this.container.All<IStart>();
            this.updaters = this.container.All<IUpdate>();

            Array.ForEach(this.awakers, awaker => awaker.Awake());
            Array.ForEach(this.starters, starter => starter.Start());

            Debug.Log($"<color=magenta>Sample FieldInjection: {this.container}</color>");
            Debug.Log($"<color=magenta>Sample PropertyInjection: {this.squareBehaviour}</color>");
            Debug.Log($"<color=magenta>Sample MethodInjection: {this.circleBehaviour}</color>");

            this.isInitialized = true;
        }

        private void Update()
        {
            if (!this.isInitialized)
                return;

            var deltaTime = Time.deltaTime;
            Array.ForEach(this.updaters, updater => updater.Update(deltaTime));
        }
    }
}