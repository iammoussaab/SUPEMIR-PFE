using LaserAssetPackage.Scripts.Laser.Dto;
using LaserAssetPackage.Scripts.Laser.Logic;
using LaserAssetPackage.Scripts.Laser.Logic.Query;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Debug
{
    /// <summary>
    /// Simple debugger for the laser receiver.
    ///
    /// Changes its color if it is being affect by a laser and reverts the change if the laser stops affecting it.
    /// </summary>
    public class LaserReceiverDebugger : MonoBehaviour
    {
        private ILaserTarget _target;
        private Material _material;
        private Color _defaultColor;

        void Awake()
        {
            _material = GetComponent<MeshRenderer>().material;
            _defaultColor = _material.color;
            _target = GetComponent<ILaserTarget>();

            _target.OnNewEmitterReceived += OnNewEmitter;
            _target.OnEmitterDetached += OnEmitterDetached;
        }

        private void OnNewEmitter(IQueryableLaserReceiver sender, LaserHit laserHit)
        {
            _material.color = Color.red;
        }

        private void OnEmitterDetached(IQueryableLaserReceiver sender, LaserEmitter laserEmitter)
        {
            if (_target.AttachedEmitterCount == 0)
            {
                _material.color = _defaultColor;
            }
        }
    }
}