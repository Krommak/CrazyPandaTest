using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Game.Components;
using Scellecs.Morpeh;
using TriInspector;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(CollisionSystem))]
    public sealed class CollisionSystem : UpdateSystem
    {
        [SerializeField]
        private float _depenetration = 0.3f;
        private Filter _filter;

        public override void OnAwake()
        {
            _filter = this.World.Filter.With<Bullet>().With<IsActive>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var item in _filter)
            {
                ref var bullet = ref item.GetComponent<Bullet>();
                var distanceForCollision = bullet.Transform.localScale.x;

                foreach (var other in _filter)
                {
                    if (other.ID == item.ID) continue;

                    ref var otherBullet = ref other.GetComponent<Bullet>();
                    var distance = (bullet.Transform.position - otherBullet.Transform.position).magnitude;
                    if (distanceForCollision > distance)
                    {
                        CollisionHandle(ref bullet, ref otherBullet);
                    }
                }
            }
        }

        private void CollisionHandle(ref Bullet firstBullet, ref Bullet secondBullet)
        {
            firstBullet.Velocity = new Vector3(-firstBullet.Velocity.x * -firstBullet.Mass * _depenetration, firstBullet.Velocity.y, 0);
            secondBullet.Velocity = new Vector3(secondBullet.Velocity.x * -secondBullet.Mass * _depenetration, secondBullet.Velocity.y, 0);
        }
    }
}