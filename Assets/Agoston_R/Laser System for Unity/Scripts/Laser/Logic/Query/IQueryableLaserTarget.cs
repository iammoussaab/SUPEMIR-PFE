namespace LaserAssetPackage.Scripts.Laser.Logic.Query
{
    /// <summary>
    /// Differentiates between regular receivers and targets (constructs that are a final target for an emitter).
    /// </summary>
    public interface IQueryableLaserTarget : IQueryableLaserReceiver
    {
    }
}