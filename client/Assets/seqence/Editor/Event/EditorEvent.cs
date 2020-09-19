using UnityEngine;

namespace UnityEditor.Seqence
{
    public enum EventT
    {
        None = 0,
        Record = 1,
        Select = 2,
    }

    public abstract class EventData
    {
        public abstract EventT e { get; }
    }

    public class EventRecordData : EventData
    {
        public override EventT e
        {
            get { return EventT.Record; }
        }
    }

    public class EventSelectData : EventData
    {
        public override EventT e
        {
            get { return EventT.Select; }
        }
    }
}
