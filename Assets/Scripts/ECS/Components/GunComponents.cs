using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Game.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct Gun : IComponent
    {
        public float Recharge;
        public float Timer;
        public Transform ShootPosition;
        public Transform Transform;

        public Vector3 Direction => (ShootPosition.position - Transform.position).normalized;
    }
}