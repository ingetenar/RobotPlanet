using System;
using System.Collections.Generic;
using System.Linq;

namespace RobotPlanet.Core
{
    public class Repository<T> : IRepository<T>
        where T : Robot
    {
        private readonly List<T> items = new();

        private readonly Dictionary<T, EventSubscriptions>
            subscriptions = new();

        public event Action<T>? RobotAdded;

        public event Action<T>? RobotRemoved;

        public event Action<T>? CrazyActionPerformed;

        public void Add(T item)
        {
            ArgumentNullException.ThrowIfNull(item);

            if (items.Contains(item))
            {
                return;
            }

            items.Add(item);

            RobotAdded?.Invoke(item);

            Subscribe(item);
        }

        public void Remove(T item)
        {
            ArgumentNullException.ThrowIfNull(item);

            if (!items.Remove(item))
            {
                return;
            }

            Unsubscribe(item);

            RobotRemoved?.Invoke(item);
        }

        public IEnumerable<T> GetAll()
        {
            return items.ToList();
        }

        public T? Find(Func<T, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);

            return items.FirstOrDefault(predicate);
        }

        public string TriggerCrazyAction(T actor)
        {
            ArgumentNullException.ThrowIfNull(actor);

            if (!items.Contains(actor))
            {
                throw new InvalidOperationException(
                    "Robot ei kuulu sellesse hoidlasse.");
            }

            string result = actor.CrazyAction();

            CrazyActionPerformed?.Invoke(actor);

            return result;
        }

        private void Subscribe(T robot)
        {
            Action<T> addedHandler = addedRobot =>
            {
                if (!ReferenceEquals(robot, addedRobot))
                {
                    robot.Welcome(addedRobot);
                }
            };

            Action<T> removedHandler = removedRobot =>
            {
                if (!ReferenceEquals(robot, removedRobot))
                {
                    robot.Goodbye(removedRobot);
                }
            };

            Action<T> crazyHandler = actor =>
            {
                if (!ReferenceEquals(robot, actor))
                {
                    robot.ReactToCrazyAction(actor);
                }
            };

            RobotAdded += addedHandler;
            RobotRemoved += removedHandler;
            CrazyActionPerformed += crazyHandler;

            subscriptions[robot] = new EventSubscriptions(
                addedHandler,
                removedHandler,
                crazyHandler);
        }

        private void Unsubscribe(T robot)
        {
            if (!subscriptions.TryGetValue(
                    robot,
                    out EventSubscriptions? handlers))
            {
                return;
            }

            RobotAdded -= handlers.AddedHandler;
            RobotRemoved -= handlers.RemovedHandler;
            CrazyActionPerformed -= handlers.CrazyHandler;

            subscriptions.Remove(robot);
        }

        private sealed class EventSubscriptions
        {
            public EventSubscriptions(
                Action<T> addedHandler,
                Action<T> removedHandler,
                Action<T> crazyHandler)
            {
                AddedHandler = addedHandler;
                RemovedHandler = removedHandler;
                CrazyHandler = crazyHandler;
            }

            public Action<T> AddedHandler { get; }

            public Action<T> RemovedHandler { get; }

            public Action<T> CrazyHandler { get; }
        }
    }
}