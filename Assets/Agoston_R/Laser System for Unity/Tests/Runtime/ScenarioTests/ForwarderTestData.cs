using System;
using System.Collections.Generic;
using System.Text;

namespace LaserAssetPackage.Tests.LaserAssetPackage.Tests.Runtime.ScenarioTests
{
    public abstract class ForwarderTestData : IEquatable<ForwarderTestData>
    {
        public string Name { get; set; }
        public int MissedHits { get; set; }
        public int HitNonActors { get; set; }

        public bool Equals(ForwarderTestData other)
        {
            return other != null &&
                   other.MissedHits == MissedHits &&
                   other.HitNonActors == HitNonActors;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Name + "\n");
            builder.Append("missed hits: " + MissedHits + "\n");
            builder.Append("hit non actors: " + HitNonActors + "\n");
            return builder.ToString();
        }

        protected string PrintCollection<T>(IEnumerable<T> list)
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