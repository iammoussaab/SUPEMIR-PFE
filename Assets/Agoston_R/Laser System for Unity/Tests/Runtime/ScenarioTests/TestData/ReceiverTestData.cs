using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LaserAssetPackage.Scripts.Laser.Logic.Query;

namespace Tests.Player.ScenarioTests.TestData
{
    public class ReceiverTestData : IEquatable<ReceiverTestData>
    {
        public string Name { get; set; }
        public List<IQueryableLaserEmitter> EmittersAttached { get; } = new List<IQueryableLaserEmitter>();
        public ISet<IQueryableLaserEmitter> HitsByEmitter { get; } = new HashSet<IQueryableLaserEmitter>();
        public List<IQueryableLaserEmitter> EmittersDetached { get; } = new List<IQueryableLaserEmitter>();
        public int AllHitsCeased { get; set; }

        public bool Equals(ReceiverTestData other)
        {
            return other != null &&
                   !other.EmittersAttached.Except(EmittersAttached).Any() &&
                   !other.HitsByEmitter.Except(HitsByEmitter).Any() &&
                   !other.EmittersDetached.Except(EmittersDetached).Any() &&
                   other.AllHitsCeased == AllHitsCeased;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Name + "\n");
            builder.Append("emitters attached: " + PrintCollection(EmittersAttached));
            builder.Append("hits by emitter:" + PrintCollection(HitsByEmitter));
            builder.Append("emitters detached:" + PrintCollection(EmittersDetached));
            builder.Append("hits ceased:" + AllHitsCeased + "\n");
            return builder.ToString();
        }

        private string PrintCollection<T>(IEnumerable<T> list)
        {
            StringBuilder builder = new StringBuilder();
            foreach (object o in list)
            {
                builder.Append(o);
                builder.Append(" | ");
            }

            builder.Append("\n");
            return builder.ToString();
        }
    }
}