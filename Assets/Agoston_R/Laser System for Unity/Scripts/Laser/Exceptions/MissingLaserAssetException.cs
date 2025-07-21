using System;

namespace LaserAssetPackage.Scripts.Laser.Exceptions
{
    /// <summary>
    /// Thrown when a laser asset count not be located.
    /// </summary>
    public class MissingLaserAssetException : Exception
    {
        public MissingLaserAssetException(string message) : base(message)
        {
        }
    }
}