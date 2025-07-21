using System.Collections.Generic;
using System.Linq;
using LaserAssetPackage.Scripts.Laser.Dto;
using LaserAssetPackage.Scripts.Laser.Logic;
using LaserAssetPackage.Scripts.Laser.Logic.Query;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Scripting
{
    /// <summary>
    /// Utility to speed up the work with Laser Actors. Contains various queries implemented about laser actors in the scene.
    /// Ideally you want to rely on the events found in interfaces and run these queries on event invocation.
    ///
    /// NOTE: only the Laser Actors that were present at the start of the scene are being taken into account. Actors instantiated during game are treated as non existent.
    /// NOTE: be careful with using <see cref="GameObject.Find"/> - many of the prefabs have a child object with a laser component on it and these child objects have the same name.
    /// 
    /// You can however name your root laser actor (object with the <see cref="LaserActorRoot"/> on it) and find a laser component by that name.
    ///
    /// Explanations:
    ///  * affected by an emitter means that the laser emitter's laser hits the given object.
    /// </summary>
    public abstract class LaserActorAware : MonoBehaviour
    {
        private readonly ISet<IQueryableLaserActor> _actors = new HashSet<IQueryableLaserActor>();
        private readonly ISet<LaserActorRoot> _actorRoots = new HashSet<LaserActorRoot>();

        /// <summary>
        /// Invoked when all the laser targets are hit by at least one emitter.
        /// </summary>
        public delegate void NotifyAllTargetsHit();

        public event NotifyAllTargetsHit OnAllTargetsHit;

        /// <summary>
        /// Invoked when a target that was unaffected before is hit by an emitter.
        /// </summary>
        public delegate void NotifyAnyTargetHit(IQueryableLaserTarget target);

        public event NotifyAnyTargetHit OnAnyTargetHit;

        /// <summary>
        /// Invoked when all the laser receivers are hit by at least one emitter.
        /// </summary>
        public delegate void NotifyAllReceiversHit();

        public event NotifyAllReceiversHit OnAllReceiversHit;

        /// <summary>
        /// Invoked when a receiver that was unaffected before is hit by an emitter.
        /// </summary>
        public delegate void NotifyAnyReceiverHit(IQueryableLaserReceiver receiver);

        public event NotifyAnyReceiverHit OnAnyReceiverHit;
        
        /// <summary>
        /// Gets the Laser Actor of given type by root name (name of the GameObject with the <see cref="LaserActorRoot"/> on it on the parent hierarchy of the LaserActor object).
        ///
        /// Finds the first root actor by this name and returns the <see cref="IQueryableLaserActor"/> using GetComponentInChildren.
        /// Use sparingly or in scenes with few laser actors. Otherwise, store the result calculated in an initializer method.
        /// </summary>
        /// <param name="actorRootName">GameObject name of the actor</param>
        /// <typeparam name="T">type of the actor</typeparam>
        /// <returns>the actor or null if no actor of given type was found by that name</returns>
        public T FindLaserActorByRootName<T>(string actorRootName) where T : class, IQueryableLaserActor
        {
            var root = _actorRoots.FirstOrDefault(r => r.name == actorRootName);
            return root != null ? root.GetComponentInChildren<T>() : null;
        }
        
        /// <summary>
        /// Gets all Laser Actors of type T.
        /// </summary>
        /// <typeparam name="T">the type of the actors to find</typeparam>
        /// <returns>all the actors of the given type.</returns>
        public ISet<T> GetLaserActorsByType<T>() where T : class, IQueryableLaserActor
        {
            return new HashSet<T>(_actors.OfType<T>());
        }

        /// <summary>
        /// Find the number of <see cref="IQueryableLaserReceiver"/>s affected by the given emitter.
        /// </summary>
        /// <param name="emitter">the laser emitter in question</param>
        /// <returns>the number of receivers the emitter currently affects.</returns>
        public int GetAffectedReceiverCount(IQueryableLaserEmitter emitter)
        {
            return emitter.AffectedReceivers.Count;
        }
        
        /// <summary>
        /// Find the number of <see cref="IQueryableLaserTarget"/>s affected by the given receiver.
        /// </summary>
        /// <param name="emitter">the emitter in question</param>
        /// <returns>the number of targets affected by the given emitter</returns>
        public int GetAffectedTargetCount(IQueryableLaserEmitter emitter)
        {
            return GetAffectedTargets(emitter).Count;
        }

        /// <summary>
        /// Find the <see cref="IQueryableLaserReceiver"/>s that are affected by the given emitter.
        /// </summary>
        /// <param name="emitter">the emitter in question</param>
        /// <returns>the receivers that are affected by the given emitter</returns>
        public ISet<IQueryableLaserReceiver> GetAffectedReceivers(IQueryableLaserEmitter emitter)
        {
            return emitter.AffectedReceivers;
        }
        
        /// <summary>
        /// Find the <see cref="IQueryableLaserTarget"/>s affected by the given emitter. 
        /// </summary>
        /// <param name="emitter">the emitter in question</param>
        /// <returns>the targets affected by the given emitter.</returns>
        public ISet<IQueryableLaserTarget> GetAffectedTargets(IQueryableLaserEmitter emitter)
        {
            return new HashSet<IQueryableLaserTarget>(GetAffectedReceivers(emitter).OfType<IQueryableLaserTarget>());
        }
        
        /// <summary>
        /// Find the number of emitters affecting a given receiver - or target.
        /// </summary>
        /// <param name="receiver">the receiver in question</param>
        /// <returns>the number of emitters that affect the receiver.</returns>
        public int GetNumberOfAffectingEmitters(IQueryableLaserReceiver receiver)
        {
            return receiver.AttachedEmitterCount;
        }
        
        /// <summary>
        /// Find the number of actors of a given type (e.g. number of laser emitters or number of laser mirrors)
        /// </summary>
        /// <typeparam name="T">The type of the given actor. Can be either an interface or a concrete class</typeparam>
        /// <returns>the number of actors of the given type.</returns>
        public int GetLaserActorCountOfType<T>() where T : IQueryableLaserActor
        {
            return _actors.OfType<T>().Count();
        }

        /// <summary>
        /// Find the total number of laser actors in the scene.
        /// </summary>
        /// <returns>the total number of laser actors in the scene.</returns>
        public int GetTotalLaserActorCount()
        {
            return GetLaserActorCountOfType<IQueryableLaserActor>();
        }

        /// <summary>
        /// Find the emitters that affect the given laser receiver (or target - targets are receivers too).
        /// </summary>
        /// <param name="receiver">the receiver whose emitters are in question</param>
        /// <returns>the emitters that affect the given receiver.</returns>
        public ISet<IQueryableLaserEmitter> GetAffectingLaserEmitters(IQueryableLaserReceiver receiver)
        {
            return new HashSet<IQueryableLaserEmitter>(receiver.AttachedEmitters);
        }
        
        /// <summary>
        /// Finds whether a given receiver (or target) is affected by a certain emitter.
        /// </summary>
        /// <param name="receiver">the receiver in question</param>
        /// <param name="emitter">the emitter in question</param>
        /// <returns>true if the emitter affects the receiver of false otherwise.</returns>
        public bool IsReceiverAffectedByEmitter(IQueryableLaserReceiver receiver, IQueryableLaserEmitter emitter)
        {
            return GetAffectingLaserEmitters(receiver).Contains(emitter);
        }
        
        /// <summary>
        /// Finds whether a given emitter affects a given receiver.
        /// </summary>
        /// <param name="emitter">the emitter in question</param>
        /// <param name="receiver">the receiver in question</param>
        /// <returns>true if the emitter affects the receiver, false otherwise.</returns>
        public bool DoesEmitterAffectReceiver(IQueryableLaserEmitter emitter, IQueryableLaserReceiver receiver)
        {
            return GetAffectedReceivers(emitter).Contains(receiver);
        }

        /// <summary>
        /// Find whether all targets are affected by an emitter.
        /// </summary>
        /// <returns>true if all targets (but not all receivers that are not targets) are affected by an emitter, false otherwise.</returns>
        public bool AreAllTargetsAffectedByAnEmitter()
        {
            return _actors.OfType<IQueryableLaserTarget>().All(t => t.AttachedEmitterCount != 0);
        }

        /// <summary>
        /// Find whether all receivers are affected by an emitter.
        /// </summary>
        /// <returns>true if all the receivers - not just, but also targets - are affected by an emitter.</returns>
        public bool AreAllReceiversAffectedByAnEmitter()
        {
            return _actors.OfType<IQueryableLaserReceiver>().All(t => t.AttachedEmitterCount != 0);
        }

        /// <summary>
        /// Find the number of receivers that are NOT affected by at least one emitter.
        /// </summary>
        /// <returns>the number of receivers (including targets) that are NOT affected by at least one receiver.</returns>
        public int GetNumberOfUnaffectedReceivers()
        {
            return _actors.OfType<IQueryableLaserReceiver>().Count(r => r.AttachedEmitterCount == 0);
        }

        /// <summary>
        /// Find the number of receivers that are affected by at least one emitter.
        /// </summary>
        /// <returns>the number of receivers (including targets) that are affected by at least one receiver.</returns>
        public int GetNumberOfAffectedReceivers()
        {
            return _actors.OfType<IQueryableLaserReceiver>().Count(r => r.AttachedEmitterCount != 0);
        }
        
        /// <summary>
        /// Find the number of targets that are affected by at least one emitter.
        /// </summary>
        /// <returns>the number of targets that are affected by at least one receiver.</returns>
        public int GetNumberOfAffectedTargets()
        {
            return _actors.OfType<IQueryableLaserTarget>().Count(r => r.AttachedEmitterCount != 0);
        }

        /// <summary>
        /// Find the number of targets (not all receivers) that are NOT affected by at least one emitter.
        /// </summary>
        /// <returns>the number of targets that are NOT affected by at least one emitter.</returns>
        public int GetNumberOfUnaffectedTargets()
        {
            return _actors.OfType<IQueryableLaserTarget>().Count(r => r.AttachedEmitterCount == 0);
        }

        /// <summary>
        /// Find the emitters that do not affect any <see cref="IQueryableLaserReceiver"/>s.
        /// </summary>
        /// <returns>the emitters that do not affect any receiver.</returns>
        public ISet<IQueryableLaserEmitter> GetEmittersWithoutReceivers()
        {
            return new HashSet<IQueryableLaserEmitter>(_actors.OfType<IQueryableLaserEmitter>().Where(e => e.AffectedReceivers.Count == 0));
        }

        /// <summary>
        /// Find the number of emitters that do not affect any <see cref="IQueryableLaserReceiver"/>s.
        /// </summary>
        /// <returns>the number of emitters that do not affect any receiver.</returns>
        public int GetNumberOfEmittersWithoutReceivers()
        {
            return _actors.OfType<IQueryableLaserEmitter>().Count(e => e.AffectedReceivers.Count == 0);
        }

        /// <summary>
        /// Find the emitters that do not affect any <see cref="IQueryableLaserTarget"/>s.
        /// </summary>
        /// <returns>the emitters that do not affect any targets.</returns>
        public ISet<IQueryableLaserEmitter> GetEmittersWithoutTargets()
        {
            return new HashSet<IQueryableLaserEmitter>(
                _actors.OfType<IQueryableLaserEmitter>().Where(e => !e.AffectedReceivers.OfType<IQueryableLaserTarget>().Any())
            );
        }

        /// <summary>
        /// Find the number of emitters that do not affect any <see cref="IQueryableLaserTarget"/>s.
        /// </summary>
        /// <returns>the number of emitters that do not affect any targets.</returns>
        public int GetNumberOfEmittersWithoutTargets()
        {
            return _actors.OfType<IQueryableLaserEmitter>().Count(e => !e.AffectedReceivers.OfType<IQueryableLaserTarget>().Any());
        }
        
        protected virtual void Awake()
        {
            SetUpActors();
            SetUpActorRoots();
            SubscribeToActors();
        }

        protected virtual void OnDestroy()
        {
            UnsubscribeFromActors();
        }

        private void SetUpActors()
        {
            foreach (var actor in FindObjectsOfType<MonoBehaviour>().OfType<IQueryableLaserActor>())
            {
                _actors.Add(actor);
            }
        }

        private void SetUpActorRoots()
        {
            foreach (var root in FindObjectsOfType<LaserActorRoot>())
            {
                _actorRoots.Add(root);
            }
        }
        
        private void SubscribeToActors()
        {
            SubscribeToAllReceivers();
            SubscribeToAllTargets();
        }
        
        private void UnsubscribeFromActors()
        {
            UnsubscribeFromAllReceivers();
            UnsubscribeFromAllTargets();
        }

        private void SubscribeToAllTargets()
        {
            foreach (var target in GetLaserActorsByType<IQueryableLaserTarget>())
            {
                target.OnNewEmitterReceived += OnTargetHit;
            }
        }

        private void UnsubscribeFromAllTargets()
        {
            foreach (var target in GetLaserActorsByType<IQueryableLaserTarget>())
            {
                target.OnNewEmitterReceived -= OnTargetHit;
            }
        }

        private void SubscribeToAllReceivers()
        {
            foreach (var receiver in GetLaserActorsByType<IQueryableLaserReceiver>())
            {
                receiver.OnNewEmitterReceived += OnReceiverHit;
            }
        }

        private void UnsubscribeFromAllReceivers()
        {
            foreach (var receiver in GetLaserActorsByType<IQueryableLaserReceiver>())
            {
                receiver.OnNewEmitterReceived -= OnReceiverHit;
            }
        }

        private void OnReceiverHit(IQueryableLaserReceiver sender, LaserHit laserHit)
        {
            OnAnyReceiverHit?.Invoke(sender);
            if (AreAllReceiversAffectedByAnEmitter())
            {
                OnAllReceiversHit?.Invoke();
            }
        }

        private void OnTargetHit(IQueryableLaserReceiver sender, LaserHit laserHit)
        {
            OnAnyTargetHit?.Invoke(sender as IQueryableLaserTarget);
            if (AreAllTargetsAffectedByAnEmitter())
            {
                OnAllTargetsHit?.Invoke();
            }
        }
    }
}