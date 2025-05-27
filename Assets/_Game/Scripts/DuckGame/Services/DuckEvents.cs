using IKhom.EventBusSystem.Runtime.abstractions;
using UnityEngine;

namespace _Game.Scripts.DuckGame.Services
{
    public class DuckEvents
    {
        public struct DuckTouchedEvent : IEvent
        {
            public DuckColor DuckColor;
            public Vector3 Position;
        }

        public struct DuckStartedDraggingEvent : IEvent
        {
            public DuckColor DuckColor;
        }

        public struct DuckPlacedCorrectlyEvent : IEvent
        {
            public DuckColor DuckColor;
            public Vector3 BasketPosition;
        }

        public struct DuckPlacedIncorrectlyEvent : IEvent
        {
            public DuckColor DuckColor;
            public Vector3 AttemptedPosition;
        }

        public struct RoundCompleteEvent : IEvent
        {
            public int CompletedRound;
            public int TotalRounds;
        }

        public struct GameCompleteEvent : IEvent
        {
            public int TotalRounds;
        }

        public struct InactivityDetectedEvent : IEvent
        {
            public float InactiveTime;
        }

        public struct HelperHandRequestedEvent : IEvent
        {
            public DuckColor DuckColor;
            public Vector3 DuckPosition;
            public Vector3 TargetBasketPosition;
        }

        public struct BasketsSpawnedEvent : IEvent
        {
            public int RoundNumber;
        }

        public struct DucksSpawnedEvent : IEvent
        {
            public int RoundNumber;
            public DuckColor[] DuckColors;
        }
    }
}