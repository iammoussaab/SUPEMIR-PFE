using LaserAssetPackage.Scripts.Laser.Dto;
using LaserAssetPackage.Scripts.Laser.Logic.Query;

namespace LaserAssetPackage.Scripts.Laser.Scripting
{
    /// <summary>
    /// Example to show that by extending the <see cref="LaserActorAware"/> class all the queries and events inside it are made available.
    /// To see the full list of available queries, see <see cref="LaserActorAware"/>. 
    /// </summary>
    public class ScriptingExample : LaserActorAware
    {
        private IQueryableLaserTarget _mySelectedTarget;
        
        private void Start()
        {
            SubscribeToSomeRandomEvents();
            FindMyTargetAndSubscribeToIt();
        }

        private void FindMyTargetAndSubscribeToIt()
        {
            _mySelectedTarget = FindLaserActorByRootName<IQueryableLaserTarget>("nonblocking_receiver");
            _mySelectedTarget.OnNewEmitterReceived += Method_To_Run_When_New_Emitter_Hits_My_Target;
        }

        private void Method_To_Run_When_New_Emitter_Hits_My_Target(IQueryableLaserReceiver sender, LaserHit laserHit)
        {
            UnityEngine.Debug.Log($"My favourite target, {_mySelectedTarget.name} was hit by an emitter: {laserHit.Emitter.name}");
        }

        private void SubscribeToSomeRandomEvents()
        {
            OnAllTargetsHit += LevelComplete;
            OnAnyReceiverHit += ReceiverHit;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnAllTargetsHit -= LevelComplete;
            OnAnyReceiverHit -= ReceiverHit;
        }

        private void LevelComplete()
        {
            UnityEngine.Debug.Log("yay level complete");
        }

        private void ReceiverHit(IQueryableLaserReceiver receiver)
        {
            int emittersHittingThisReceiver = GetNumberOfAffectingEmitters(receiver);
            if (emittersHittingThisReceiver > 1)
            {
                UnityEngine.Debug.Log("that receiver will burn");
            }
        }
    }
}