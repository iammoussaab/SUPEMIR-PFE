using System;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Logic.Query
{
    /// <summary>
    /// Contains the most basic information about Laser Actors and serves as a base.
    /// </summary>
    public interface IQueryableLaserActor : IEquatable<IQueryableLaserActor>
    {
        /// <summary>
        /// The Unity Instance ID.
        /// </summary>
        /// <returns>The Unity instance ID</returns>
        int GetInstanceID();

        /// <summary>
        /// The name of the Laser Actor GameObject.
        /// </summary>
        /// <returns>The object's name</returns>
        string name { get; }

        /// <summary>
        /// The tag of the Laser Actor GameObject.
        /// </summary>
        /// <returns>The object's tag</returns>
        string tag { get; }

        /// <summary>
        /// Finds the compound Laser Actor's root. Put this on e.g. a laser emitter cube so that when moving the cube the emitter doesn't fall apart.
        ///
        /// Relatively expensive to execute so make sure to cache the result at startup.
        /// </summary>
        /// <returns>the root of the laser actor as marked by the developer or null if no such script is added to any of the parents.</returns>
        Transform FindLaserRoot();

        /// <summary>
        /// Overrides ToString method to help diagnose problems.
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}