using System.Collections.Generic;
using LaserAssetPackage.Scripts.Laser.Dto;

namespace LaserAssetPackage.Scripts.Laser.Logic.Query
{
    /// <summary>
    /// Provides information about a <see cref="LaserEmitter"/>. 
    /// </summary>
    public interface IQueryableLaserEmitter : IQueryableLaserForwarder
    {
        /// <summary>
        /// The set of receivers (including relays) that are affected by this emitter.
        ///
        /// Creates a copy of the set when invoked.
        /// </summary>
        public ISet<IQueryableLaserReceiver> AffectedReceivers { get; }

        /// <summary>
        /// Invoked when the <see cref="LaserEmitter"/> is activated.
        /// </summary>
        delegate void NotifyActivated(IQueryableLaserEmitter sender);

        event NotifyActivated EmitterActivated;

        /// <summary>
        /// Invoked when the <see cref="LaserEmitter"/> is deactivated.
        /// </summary>
        delegate void NotifyDeactivated(IQueryableLaserEmitter sender);

        event NotifyDeactivated EmitterDeactivated;

        /// <summary>
        /// Invoked when the laser chain through the forwarders (if any) has returned. Contains information on the result of the hits and forwarded hits. 
        /// </summary>
        delegate void NotifyChainReturned(IQueryableLaserEmitter sender, LaserResult result);

        event NotifyChainReturned ChainReturned;
    }
}