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
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(SlowControllSystem))]
    public sealed class SlowControllSystem : UpdateSystem
    {
        private Vector3 _zonePosition;
        private float _zoneRadius;
        private Filter _normalFilter;
        private Filter _slowFilter;

        public override void OnAwake()
        {
            var filter = this.World.Filter.With<SlowZone>().Build();
            foreach (var item in filter)
            {
                var component = item.GetComponent<SlowZone>();
                _zonePosition = component.Transform.position;
                _zoneRadius = component.ZoneRadius;
            }

            this._normalFilter = this.World.Filter.With<Bullet>().With<IsActive>().Without<IsSlow>().Build();
            this._slowFilter = this.World.Filter.With<Bullet>().With<IsActive>().With<IsSlow>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var item in _normalFilter)
            {
                var component = item.GetComponent<Bullet>();
                var distance = (component.Transform.position - _zonePosition).magnitude;
                if (distance <= _zoneRadius)
                    item.SetComponent(new IsSlow());
            }
            foreach (var item in _slowFilter)
            {
                var component = item.GetComponent<Bullet>();
                var distance = (component.Transform.position - _zonePosition).magnitude;
                if (distance > _zoneRadius)
                    item.RemoveComponent<IsSlow>();
            }
        }
    }
}