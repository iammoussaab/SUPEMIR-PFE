using System;
using System.Collections.Generic;
using System.Text;
using LaserAssetPackage.Scripts.Laser.Logic.Query;
using LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.ScenarioTests;

namespace Tests.Player.ScenarioTests.TestData
{
    public class EmitterTestData : ForwarderTestData, IEquatable<EmitterTestData>
    {
        public IQueryableLaserReceiver DirectlyHitReceiver { get; set; }
        public ISet<IQueryableLaserReceiver> HitReceivers { get; } = new HashSet<IQueryableLaserReceiver>();
        public int EmitterActivatedMessages { get; set; }
        public int EmitterDeactivatedMessages { get; set; }


        public bool Equals(EmitterTestData other)
        {
            return other != null &&
                   base.Equals(other) &&
                   other.EmitterActivatedMessages == EmitterActivatedMessages &&
                   other.EmitterDeactivatedMessages == EmitterDeactivatedMessages;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append("hit receivers: " + PrintCollection(HitReceivers));
            builder.Append("activated messages: " + EmitterActivatedMessages + "\n");
            builder.Append("deactivated messages: " + EmitterDeactivatedMessages + "\n");
            return builder.ToString();
        }
    }
}