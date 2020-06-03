using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using EntityCommand;
using GameFramework;

namespace Entity
{
	public abstract class MonoEntityBase : MonoBehaviour, IEntity
	{
        public int EntityID { get; protected set; } = -1;

        public EntityType EntityType { get; protected set; } = EntityType.None;

        public bool IsValid
        {
            get { return EntityManager.Instance.IsRegistered(EntityID); }
        }

        private Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set
            {
                position = value;
                SendCommandToViews(new PositionChanged());
            }
        }

        private Vector3 rotation;
        public Vector3 Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                SendCommandToViews(new RotationChanged());
            }
        }

        public Vector3 Velocity { get; set; }
        public Vector3 AngularVelocity { get; set; }

        private List<IComponent> m_listComponent = new List<IComponent>();

        #region Interface For Convenience
        public abstract float MovementSpeed { get; }

        public Vector3 Forward { get { return (Quaternion.Euler(Rotation) * Vector3.forward).normalized; } }
        #endregion

        protected virtual void Awake()
        {
            InitComponents();
        }

        protected virtual void InitComponents()
        {
        }

        public virtual void Initialize(params object[] param)
        {
        }

        public virtual void Tick(int tick)
        {
            GetComponents<State.StateBase>().ForEach(state =>
            {
                //  Iterating중에 Entity가 Destroy 안되었는지 체크
                if (IsValid)
                {
                    state.Tick(tick);
                }
            });

            GetComponents<Behavior.BehaviorBase>().ForEach(behavior =>
            {
                if (IsValid)
                {
                    behavior.Tick(tick);
                }
            });
        }

        public T AttachComponent<T>(T component) where T : IComponent
        {
            m_listComponent.Add(component);

            component.OnAttached(this);

            return component;
        }

        public T DetachComponent<T>(T component) where T : IComponent
        {
            m_listComponent.Remove(component);

            component.OnDetached();

            return component;
        }

        public T GetComponent<T>() where T : IComponent
        {
            var found = m_listComponent.Find(x => x is T);

            if (found == null)
                return default;

            return (T)found;
        }

        public List<T> GetComponents<T>() where T : IComponent
        {
            var found = m_listComponent.FindAll(x => x is T);

            if (found == null)
                return null;

            return found.Cast<T>().ToList();
        }

        public void SendCommandToAll(ICommand command)
        {
            List<IComponent> components = new List<IComponent>(m_listComponent);

            foreach (var component in components)
            {
                if (!m_listComponent.Contains(component))
                    continue;

                component.OnCommand(command);
            }
        }

        public void SendCommand(ICommand command, List<Type> cullings)
        {
            List<IComponent> components = new List<IComponent>(m_listComponent);

            foreach (var component in components)
            {
                if (!m_listComponent.Contains(component))
                    continue;

                if (cullings.Exists(x => x.IsAssignableFrom(component.GetType())))
                {
                    component.OnCommand(command);
                }
            }
        }

        public void SendCommandToViews(ICommand command)
        {
            SendCommand(command, new List<Type> { typeof(IViewComponent) });
        }

        public void SendCommandToModels(ICommand command)
        {
            SendCommand(command, new List<Type> { typeof(IModelComponent) });
        }

        public void SendCommandToControllers(ICommand command)
        {
            SendCommand(command, new List<Type> { typeof(IControllerComponent) });
        }

        #region PhysicsSimulation
        public virtual void OnBeforePhysicsSimulation(int tick)
        {
            BasicView basicView = GetComponent<BasicView>();

            basicView.ModelTransform.hasChanged = false;
            basicView.ModelTransform.GetComponent<Rigidbody>().isKinematic = false;
        }

        public virtual void OnAfterPhysicsSimulation(int tick)
        {
            BasicView basicView = GetComponent<BasicView>();

            if (basicView.ModelTransform.hasChanged)
            {
                if (Position != basicView.Position)
                {
                    Position = basicView.Position;
                }

                if (Rotation != basicView.Rotation)
                {
                    Rotation = basicView.Rotation;
                }
            }

            basicView.ModelTransform.GetComponent<Rigidbody>().isKinematic = true;
        }
        #endregion
    }
}
