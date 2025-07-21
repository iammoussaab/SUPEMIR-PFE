using System.Collections.Generic;
using LaserAssetPackage.Scripts.Laser.Logic.Query;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Logic
{
    /// <summary>
    /// Class that can be serialized in scripts and enables dragging references to it.
    ///
    /// Contains readonly information on the particular receiver.
    /// </summary>
    public abstract class QueryableLaserReceiver : MonoBehaviour, IQueryableLaserReceiver
    {
        public abstract bool Equals(IQueryableLaserActor other);

        public abstract Transform FindLaserRoot();

        public abstract event IQueryableLaserReceiver.LaserEmitterAttached OnNewEmitterReceived;
        public abstract event IQueryableLaserReceiver.LaserEmitterDetached OnEmitterDetached;
        public abstract event IQueryableLaserReceiver.NotifyHitByLaser HitByLaser;
        public abstract event IQueryableLaserReceiver.NotifyAllHitsCeased AllLaserHitsCeased;
        public abstract ISet<LaserEmitter> AttachedEmitters { get; }
        public abstract int AttachedEmitterCount { get; }
    }
}