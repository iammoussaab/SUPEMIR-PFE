using LaserAssetPackage.Scripts.Laser.Dto;

namespace LaserAssetPackage.Scripts.Laser.Drawing.Helper
{
    /// <summary>
    /// Utility class to transform between laser dto-s.
    ///
    /// Note - transform as not in a 3D environment but convert one dto to another.
    /// </summary>
    public class LaserDtoTransformer
    {
        /// <summary>
        /// Create the start and endpoints of the laser beam based on the laser hit data.
        /// </summary>
        /// <param name="laserHit">the hit data</param>
        /// <returns>the start and endpoints of the beam</returns>
        public LaserLineEndpoints Transform(LaserHit laserHit)
        {
            return LaserLineEndpoints.Create(laserHit.Origin, laserHit.Hit.point);
        }

        /// <summary>
        /// Create the start and endpoints of the laser beam based on the laser's origin and distance (if it didn't connect).
        /// </summary>
        /// <param name="laserDirection">the direction and distance data of the laser</param>
        /// <returns>the start and endpoints of the beam</returns>
        public LaserLineEndpoints Transform(LaserDirection laserDirection)
        {
            return LaserLineEndpoints.Create(laserDirection.Origin, laserDirection.Origin + laserDirection.Direction.normalized * laserDirection.Distance);
        }
    }
}