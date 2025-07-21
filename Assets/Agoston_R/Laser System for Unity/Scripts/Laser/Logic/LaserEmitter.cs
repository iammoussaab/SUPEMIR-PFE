using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LaserAssetPackage.Scripts.Laser.Cache;
using LaserAssetPackage.Scripts.Laser.Dto;
using LaserAssetPackage.Scripts.Laser.Logic.Query;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Logic
{
    /// <summary>
    /// Emits laser forward that interacts with <see cref="ILaserReceiver"/>s and <see cref="ILaserTarget"/>s.
    /// </summary>
    public class LaserEmitter : MonoBehaviour, ILaserEmitter
    {
        private const float MaxRayDistance = 1000f;
        private const string LaserOriginGameObject = "laser_origin";

        [Tooltip("Laser refresh rate per second. Lower this will increase performance but the lasers are updated less frequently. /n" +
                 "Need to restart the game for the change to take effect.")]
        [SerializeField]
        private float laserRefreshRate = 30f;

        [Tooltip("The maximum distance for the raycast. Can be lowered to optimize the game a bit or to avoid unnecessary raycasts. \n " +
                 "If the value is less than or equal to 0, the value will be infinite. ")]
        [SerializeField]
        private float maxDistance = -1.0f;

        private bool _isLaserEmitting;

        private readonly LaserReceiverCache _receiverCache = LaserReceiverCache.Instance;
        private readonly ISet<ILaserReceiver> _currentReceivers = new HashSet<ILaserReceiver>();
        private readonly ISet<ILaserReceiver> _maintainedReceivers = new HashSet<ILaserReceiver>();
        private Transform _laserOrigin;

        public event IQueryableLaserForwarder.NotifyLaserHitActor LaserHitActor;
        public event IQueryableLaserForwarder.NotifyLaserHitNonActor LaserHitNonActor;
        public event IQueryableLaserForwarder.NotifyLaserMiss LaserMiss;
        public event IQueryableLaserEmitter.NotifyActivated EmitterActivated;
        public event IQueryableLaserEmitter.NotifyDeactivated EmitterDeactivated;
        public event IQueryableLaserEmitter.NotifyChainReturned ChainReturned;

        [Tooltip("The layers the emitter can interact with.")]
        public string[] supportedLayers;

        private int _supportedLayers;
        
        /// <summary>
        /// Returns a copy of the <see cref="ILaserReceiver"/>s that this emitter affects.
        ///
        /// Note that depending on the scripts' execution order this may be off by one shoot iteration.
        /// </summary>
        public ISet<IQueryableLaserReceiver> AffectedReceivers => new HashSet<IQueryableLaserReceiver>(_maintainedReceivers);

        private IEnumerator _routine;

        private void Awake()
        {
            SetUpLaserOrigin();
            SetUpSupportedLayers();
        }

        private void OnEnable()
        {
            Activate();
        }

        private void OnDisable()
        {
            Deactivate();
        }

        public void Activate()
        {
            _isLaserEmitting = true;
            _routine = EmitLaser();
            StartCoroutine(_routine);
            EmitterActivated?.Invoke(this);
        }

        public void Deactivate()
        {
            _isLaserEmitting = false;
            StopCoroutine(_routine);
            NotifyNowUnaffectedReceivers();
            ClearReceivers();
            EmitterDeactivated?.Invoke(this);
        }

        private void SetUpLaserOrigin()
        {
            _laserOrigin = transform.Find(LaserOriginGameObject);
            if (_laserOrigin == null)
            {
                _laserOrigin = transform;
            }
        }

        private IEnumerator EmitLaser()
        {
            var waitForSeconds = new WaitForSecondsRealtime(1f / laserRefreshRate);
            yield return null;
            while (_isLaserEmitting)
            {
                ClearCurrentReceivers();
                ShootRay();
                NotifyNowUnaffectedReceivers();
                SavePreviousReceiverState();
                yield return waitForSeconds;
            }
        }

        private void ClearCurrentReceivers()
        {
            _currentReceivers.Clear();
        }

        private void ShootRay()
        {
            Vector3 direction = CalculateLaserDirection();
            int layerMask = _supportedLayers;
            float rayMaxDistance = CalculateRayDistance();
            Vector3 origin = _laserOrigin.position;

            if (Physics.Raycast(origin, direction, out var hit, rayMaxDistance, layerMask, QueryTriggerInteraction.Collide))
            {
                OnRaycastHit(origin, hit, direction, rayMaxDistance, layerMask);
            }
            else
            {
                LaserMiss?.Invoke(this, LaserDirection.Create(origin, direction, this, rayMaxDistance));
            }
        }

        private float CalculateRayDistance()
        {
            return maxDistance > 0 ? maxDistance : MaxRayDistance;
        }

        private void NotifyNowUnaffectedReceivers()
        {
            foreach (var receiver in _maintainedReceivers.Except(_currentReceivers))
            {
                receiver.CeaseHit(this);
            }
        }

        private void ClearReceivers()
        {
            foreach (var receiver in _currentReceivers)
            {
                receiver.CeaseHit(this);
            }

            _maintainedReceivers.Clear();
            _currentReceivers.Clear();
        }

        private void SavePreviousReceiverState()
        {
            _maintainedReceivers.Clear();
            _maintainedReceivers.UnionWith(_currentReceivers);
        }

        private void OnRaycastHit(Vector3 origin, RaycastHit hit, Vector3 direction, float rayMaxDistance, int layers)
        {
            if (IsHitAReceiver(hit, out var receiver))
            {
                OnReceiverHit(hit, origin, direction, receiver, rayMaxDistance, layers);
                LaserHitActor?.Invoke(this, LaserHit.Create(hit, origin, direction, this, rayMaxDistance, layers), receiver);
            }
            else
            {
                LaserHitNonActor?.Invoke(this, LaserHit.Create(hit, origin, direction, this, rayMaxDistance, layers));
            }
        }

        private bool IsHitAReceiver(RaycastHit hit, out ILaserReceiver receiver)
        {
            receiver = _receiverCache.FindOrCache(hit.collider);
            return receiver != null;
        }

        private void OnReceiverHit(RaycastHit hit, Vector3 origin, Vector3 direction, ILaserReceiver receiver, float rayMaxDistance, int layers)
        {
            LaserResult result = receiver.Hit(LaserHit.Create(hit, origin, direction, this, rayMaxDistance - hit.distance, layers));
            ChainReturned?.Invoke(this, result);
            _currentReceivers.UnionWith(result.AffectedReceivers);
        }

        private Vector3 CalculateLaserDirection()
        {
            return transform.TransformDirection(Vector3.forward);
        }
        
        private void SetUpSupportedLayers()
        {
            if (supportedLayers == null || supportedLayers.Length == 0 || supportedLayers.Contains(""))
            {
                _supportedLayers = ~0;
                return;
            }
            foreach (var layer in supportedLayers)
            {
                _supportedLayers |= 1 << LayerMask.NameToLayer(layer);
            }
        }

        public bool Equals(IQueryableLaserActor other)
        {
            return other != null && this.GetInstanceID() == other.GetInstanceID();
        }

        public Transform FindLaserRoot()
        {
            return GetComponentInParent<LaserActorRoot>().transform;
        }

        public override string ToString()
        {
            return $"{nameof(LaserEmitter)} :: {FindLaserRoot().name}/{name}";
        }
    }
}