using LaserAssetPackage.Scripts.Laser.Drawing.Coloring;
using LaserAssetPackage.Scripts.Laser.Dto;
using LaserAssetPackage.Scripts.Laser.Exceptions;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Drawing.Controller
{
    /// <summary>
    /// Encapsulates the logic needed to interact with the laser beam's shader.
    /// </summary>
    public class LaserBeamController : MonoBehaviour
    {
        private const float Eps = 0.001f;

        private LineRenderer _beam;

        private readonly Vector3[] _linePositions = new Vector3[2];
        private static readonly int LineDistance = Shader.PropertyToID("_lineDistance");
        private static readonly int WidthConstant = Shader.PropertyToID("_widthConstant");
        private static readonly int ColorConstant = Shader.PropertyToID("_Color");

        private LaserColorRegistry ColorRegistry => LaserColorRegistry.Instance;

        private void Awake()
        {
            _beam = GetComponent<LineRenderer>();
            ValidateBeamWithMaterialNotNull();
        }

        private void Start()
        {
            SetUVScaleBasedOnWidth();
            EraseBeam();
        }

        /// <summary>
        /// Makes the laser beam disappear.
        /// </summary>
        public void EraseBeam()
        {
            _beam.positionCount = 0;
        }

        /// <summary>
        /// Draws the laser beam between the given endpoints and designates it to the emitter it comes from.
        /// </summary>
        /// <param name="endpoints">the start and end points of the laser beam</param>
        /// <param name="emitterId">the emitter the beam belongs to</param>
        public void DrawLaserBeam(LaserLineEndpoints endpoints, int emitterId)
        {
            SetLineColor(ColorRegistry.GetBeamColor(emitterId));
            SetLineRendererPositions(endpoints);
            UpdateEdgeTaperByLineDistance(_linePositions);
        }

        private void SetLineColor(Color tint)
        {
            _beam.material.SetColor(ColorConstant, tint);
        }

        private void SetLineRendererPositions(LaserLineEndpoints endpoints)
        {
            _beam.positionCount = _linePositions.Length;
            _linePositions[0] = transform.InverseTransformPoint(endpoints.Origin);
            _linePositions[1] = transform.InverseTransformPoint(endpoints.Destination);
            _beam.SetPositions(_linePositions);
        }

        private void UpdateEdgeTaperByLineDistance(Vector3[] linePositions)
        {
            float distance = Vector3.Distance(linePositions[0], linePositions[1]);
            if (distance < Eps)
            {
                distance = 1f;
            }

            _beam.material.SetFloat(LineDistance, distance);
        }

        private void SetUVScaleBasedOnWidth()
        {
            _beam.material.SetFloat(WidthConstant, _beam.widthCurve.keys[0].value);
        }

        private void ValidateBeamWithMaterialNotNull()
        {
            if (_beam != null && _beam.material != null)
            {
                return;
            }

            throw new MissingLaserAssetException($"LaserDrawer on GameObject {name}: particle drawer, line renderer or its material is missing.");
        }
    }
}