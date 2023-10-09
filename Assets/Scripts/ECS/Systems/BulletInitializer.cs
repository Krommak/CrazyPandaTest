using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Scellecs.Morpeh;
using Game.Components;
using TriInspector;

namespace Game.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Initializers/" + nameof(BulletInitializer))]
    public sealed class BulletInitializer : Initializer
    {
        [SerializeField]
        private bool _randomizeMass;
        [SerializeField, ShowIf("_randomizeMass")]
        private Vector2 _randomizeMinMax;
        [SerializeField]
        private GameObject _bulletPrefab;
        [SerializeField]
        private int _bulletCount;

        public override void OnAwake()
        {
            for (int i = 0; i < _bulletCount; i++)
            {
                CreateBullet();
            }
        }

        public override void Dispose()
        {
        }

        private void CreateBullet()
        {
            var go = Instantiate(_bulletPrefab);
            var entity = this.World.CreateEntity();
            var mass = _randomizeMass ? Random.Range(_randomizeMinMax.x, _randomizeMinMax.y) : 1;
            entity.SetComponent(new Bullet()
            {
                Transform = go.transform,
                Velocity = new Vector2(),
                Mass = mass
            });
            go.transform.localScale = new Vector3(mass, mass, mass);
            go.SetActive(false);
        }
    }
}