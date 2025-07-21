using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LaserAssetPackage.Scripts.Laser.Logic.Query;
using LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.ScenarioTests;

namespace Tests.Player.ScenarioTests.TestData
{
    public class RelayTestData : ForwarderTestData, IEquatable<RelayTestData>
    {
        public List<IQueryableLaserEmitter> EmittersAttached { get; } = new List<IQueryableLaserEmitter>();
        public ISet<IQueryableLaserEmitter> HitsByEmitter { get; } = new HashSet<IQueryableLaserEmitter>();
        public List<IQueryableLaserEmitter> EmittersDetached { get; } = new List<IQueryableLaserEmitter>();
        public int AllHitsCeased { get; set; }

        public bool Equals(RelayTestData other)
        {
            return other != null &&
                   base.Equals(other) &&
                   !other.EmittersAttached.Except(EmittersAttached).Any() &&
                   !other.HitsByEmitter.Except(HitsByEmitter).Any() &&
                   !other.EmittersDetached.Except(EmittersDetached).Any() &&
                   other.AllHitsCeased == AllHitsCeased;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append("emitters attached: " + PrintCollection(EmittersAttached));
            builder.Append("hits by emitter: " + PrintCollection(HitsByEmitter));
            builder.Append("emitters detached: " + PrintCollection(EmittersDetached));
            builder.Append("all hits ceased: " + AllHitsCeased + "\n");
            return builder.ToString();
        }
    }
}