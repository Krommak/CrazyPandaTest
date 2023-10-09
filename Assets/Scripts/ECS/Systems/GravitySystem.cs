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
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(GravitySystem))]
    public sealed class GravitySystem : UpdateSystem
    {
        private Vector3 _gravity;
        private Filter _filter;

        public override void OnAwake()
        {
            _gravity = Physics.gravity;
            this._filter = this.World.Filter.With<Bullet>().With<UpdateVelocity>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var item in _filter)
            {
                var update = item.GetComponent<UpdateVelocity>();
                ref var component = ref item.GetComponent<Bullet>();

                component.Velocity += _gravity * update.UpdateCoefficient * deltaTime;

                item.RemoveComponent<UpdateVelocity>();
            }
        }
    }
}