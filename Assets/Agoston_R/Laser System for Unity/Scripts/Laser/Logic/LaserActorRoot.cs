using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Logic
{
    /// <summary>
    /// Marks a complex hierarchy's root that contains a Laser Actor. Useful when we want to move e.g. the whole emitter cube
    /// (whose children contain the actual emitter script).
    /// </summary>
    public class LaserActorRoot : MonoBehaviour
    {
    }
}