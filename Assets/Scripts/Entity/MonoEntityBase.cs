using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using GameFramework;

namespace Entity
{
	public class MonoEntityBase : MonoBehaviour, IEntity
	{
        private List<IEntityComponent> entityComponents = new List<IEntityComponent>();

        public int EntityID { get; protected set; } = -1;

        public virtual Vector3 Position { get; set; }
        public virtual Vector3 Rotation { get; set; }
        public virtual Vector3 Velocity { get; set; }
        public virtual Vector3 AngularVelocity { get; set; }

        public T AttachEntityComponent<T>(T component) where T : IEntityComponent
        {
            entityComponents.Add(component);

            component.OnAttached(this);

            return component;
        }

        public T DetachEntityComponent<T>(T component) where T : IEntityComponent
        {
            entityComponents.Remove(component);

            component.OnDetached();

            return component;
        }

        public T GetEntityComponent<T>() where T : IEntityComponent
        {
            var found = entityComponents.Find(x => x is T);

            if (found == null)
                return default;

            return (T)found;
        }

        public List<T> GetEntityComponents<T>() where T : IEntityComponent
        {
            var found = entityComponents.FindAll(x => x is T);

            if (found == null)
                return null;

            return found.Cast<T>().ToList();
        }

        public void SendCommandToAll(ICommand command)
        {
            List<IEntityComponent> temp = new List<IEntityComponent>(entityComponents);

            foreach (var component in temp)
            {
                if (!entityComponents.Contains(component))
                    continue;

                component.OnCommand(command);
            }
        }

        public void SendCommand(ICommand command, List<Type> cullings)
        {
            List<IEntityComponent> temp = new List<IEntityComponent>(entityComponents);

            foreach (var component in temp)
            {
                if (!entityComponents.Contains(component))
                    continue;

                if (cullings.Exists(x => x.IsAssignableFrom(component.GetType())))
                {
                    component.OnCommand(command);
                }
            }
        }

        public void SendCommandToViews(ICommand command)
        {
            SendCommand(command, new List<Type> { typeof(ViewComponentBase), typeof(MonoViewComponentBase) });
        }
    }
}
