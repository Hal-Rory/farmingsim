using System;
using UnityEngine;
using UnityEngine.AI;
namespace Entities
{
    public abstract class Entity : MonoBehaviour, IDamageable
    {
        public Vector3 Direction;
        public bool IsMoving;
        [SerializeField] protected float Speed = 3;
        public event Action OnDeathEvent;
        protected Vector3 RotateVector;
        protected NavMeshAgent NavAgent;
        public bool AutoMove;
        protected Vector3 MoveVector;

        public bool Busy { get; private set; }
        private void OnEnable()
        {
            Health = MaxHealth;
        }
        private void Start()
        {
            NavAgent = GetComponent<NavMeshAgent>();
            NavAgent.speed = Speed;
            EntityStart();            
        }
        private void OnDestroy()
        {
            EntityDestroy();            
        }
        public void DoFocus()
        {
            Busy = true;
            IsMoving = false;
        }

        public void DoUnfocus()
        {
            Busy = false;
        }
        protected abstract void EntityStart();
        protected abstract void EntityDestroy();
        public virtual void Rotation(Vector3 rotationDelta)
        {
            RotateVector = rotationDelta;
            transform.eulerAngles += RotateVector;
        }
        public void AnimateMovement(Vector3 direction)
        {
            Debug.DrawRay(transform.position, direction * 2, Color.red);
        }

        public void StopMoving()
        {
            NavAgent.velocity = Vector3.zero;
            NavAgent.isStopped = true; 
            NavAgent.ResetPath();
            AutoMove = false;
        }

        public void Movement(Vector3 position, bool auto = false)
        {
            AutoMove = auto;
            if (NavAgent.isStopped)
            {
                NavAgent.isStopped = false;
            }
            if (auto)
            {
                NavAgent.destination = position;
                Direction = transform.forward;                
            }
            else
            {
                NavAgent.Move(position);
                Direction = position.normalized;
            }
            AnimateMovement(Direction);
        }

        #region Damage
        public bool IsAlive { get => MaxHealth != 0 ? Health / MaxHealth > 0 : false; }
        [field: SerializeField] public float MaxHealth { get; private set; } = 10;
        public float Health { get; protected set; }
        public virtual void TakeDamage(float amount)
        {
            Health -= Mathf.Abs(amount);
            Health = Mathf.Clamp(Health, 0, MaxHealth);
            if (!IsAlive)
            {
                OnDeath();
                OnDeathEvent?.Invoke();
            }
        }
        protected abstract void OnDeath();
        public virtual void HealDamage(float amount)
        {
            Health += Mathf.Abs(amount);
            Health = Mathf.Clamp(Health, 0, MaxHealth);
        }
        #endregion
    }
}