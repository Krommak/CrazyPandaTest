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
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(DeactivateBulletSystem))]
    public sealed class DeactivateBulletSystem : UpdateSystem
    {
        [SerializeField]
        private float _deactivateOnY = -10;
        private Filter _filter;
        public override void OnAwake()
        {
            _filter = this.World.Filter.With<Bullet>().With<IsActive>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var item in _filter)
            {
                var position = item.GetComponent<Bullet>().Transform.position;
                if (position.y < _deactivateOnY)
                {
                    RefreshBullet(item);
                }
            }
        }

        private void RefreshBullet(Entity entity)
        {
            ref var component = ref entity.GetComponent<Bullet>();
            var transform = component.Transform;
            transform.eulerAngles = Vector3.zero;
            var rigibody = component.Rigidbody;
            rigibody.velocity = Vector2.zero;
            rigibody.angularVelocity = 0;
            var mass = component.Mass;
            this.World.RemoveEntity(entity);
            var newEntity = this.World.CreateEntity();
            newEntity.SetComponent(new Bullet()
            {
                Transform = transform,
                Rigidbody = rigibody,
                Mass = mass
            });
            transform.gameObject.SetActive(false);
        }
    }
}