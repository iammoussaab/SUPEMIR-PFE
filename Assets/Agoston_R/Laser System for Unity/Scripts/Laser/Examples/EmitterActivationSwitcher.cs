using System;
using System.Collections.Generic;
using LaserAssetPackage.Scripts.Laser.Logic;
using UnityEngine;

namespace LaserAssetPackage.Scripts.Laser.Examples
{
    public class EmitterActivationSwitcher : MonoBehaviour
    {
        private ISet<LaserEmitter> _emitters;

        private Action[] _actions;

        private int _interactionCount;

        private int InteractionCount
        {
            get => _interactionCount;
            set => _interactionCount = value % 2;
        }

        private void Awake()
        {
            _emitters = new HashSet<LaserEmitter>(FindObjectsOfType<LaserEmitter>());
            _actions = new Action[] {DeactivateEmitters, ActivateEmitters};
        }

        private void Update()
        {
            if (!Input.GetKeyDown("space")) return;

            SwitchEmitterState();
            InteractionCount++;
        }

        private void ActivateEmitters()
        {
            foreach (var emitter in _emitters)
            {
                emitter.Activate();
            }
        }

        private void DeactivateEmitters()
        {
            foreach (var emitter in _emitters)
            {
                emitter.Deactivate();
            }
        }

        private void SwitchEmitterState()
        {
            _actions[InteractionCount].Invoke();
        }
    }
}