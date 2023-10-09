using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using Game.Components;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(BulletMovementSystem))]
    public sealed class BulletMovementSystem : UpdateSystem
    {
        [SerializeField]
        private float _slowCoefficient;

        private Filter _normalFilter;
        private Filter _slowFilter;

        public override void OnAwake()
        {
            this._normalFilter = this.World.Filter.With<Bullet>().With<IsActive>().Without<IsSlow>().Build();
            this._slowFilter = this.World.Filter.With<Bullet>().With<IsActive>().With<IsSlow>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var item in _normalFilter)
            {
                var component = item.GetComponent<Bullet>();
                component.Transform.position = component.Transform.position + (component.Velocity * deltaTime/component.Mass);
                item.SetComponent(new UpdateVelocity()
                {
                    UpdateCoefficient = 1,
                });
            }
            foreach (var item in _slowFilter)
            {
                var component = item.GetComponent<Bullet>();
                component.Transform.position = component.Transform.position + (component.Velocity * deltaTime / _slowCoefficient / component.Mass);
                item.SetComponent(new UpdateVelocity()
                {
                    UpdateCoefficient = 1/_slowCoefficient,
                });
            }
        }
    }
}