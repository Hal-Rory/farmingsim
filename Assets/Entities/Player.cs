using Items;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class Player : Character
    {               
        private enum PlayerStates { Idle, inMotion};
        private PlayerStates State;
        private IInputManager InputManager => GameManager.Instance.InputManager;
        [SerializeField] private Transform View;
        private Vector3 ViewRotateVector;
        [SerializeField]
        private float LookSpeed = 5;
        protected override void EntityStart()
        {
            foreach (var t in StartingStats)
            {
                Stats.SetStat(t);
            }
            ViewRotateVector = View.localEulerAngles;
        }
        public float dist = 10;
        private void Update()
        {
            if (!Busy)
            {                
                MoveVector = transform.rotation * InputManager.GetMovementVector().FlattenY() * Time.deltaTime * Speed;
                if (!AutoMove)
                {
                    Rotation(InputManager.GetLookVector() * Time.deltaTime* LookSpeed);
                    Movement(MoveVector);
                    SetState(PlayerStates.inMotion);
                }
                else
                {
                    if (MoveVector.magnitude > 0)
                    {
                        switch (State)
                        {
                            case PlayerStates.inMotion:                               
                                StopMoving();
                                break;                            
                        }                        
                    }                    
                }

                CheckState();
            }
        }

        public override void Rotation(Vector3 rotationDelta)
        {
            base.Rotation(Vector3.up * rotationDelta.x);
            ViewRotateVector -= Vector3.right * rotationDelta.y;
            ViewRotateVector.x = Mathf.Clamp(ViewRotateVector.x, - 75, 80);
            View.localEulerAngles = ViewRotateVector;
        }

        #region States
        private void CheckState()
        {            
        }
        private void SetState(PlayerStates state)
        {
            State = state;
        }
        #endregion
        
        #region Stats
        public Stats Stats { get; private set; } = new Stats();
        public Stats.Value[] StartingStats;
        public IEnumerable<string> LevelUpStats(params Stats.Value[] values)
        {
            IEnumerable<string> stats = Stats.LevelUpStats(values);
            return stats;
        }
        #endregion                  

    }
}