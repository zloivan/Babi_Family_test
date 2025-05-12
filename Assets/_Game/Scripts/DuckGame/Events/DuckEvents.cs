using IKhom.EventBusSystem.Runtime.abstractions;

namespace BabiFamily.DuckGame.Events
{
    public struct GameStartEvent : IEvent {}
    
    public struct GameEndEvent : IEvent 
    {
        public bool Success;
    }
    
    public struct LevelCompleteEvent : IEvent 
    {
        public int LevelIndex;
    }
    
    public struct ProgressUpdateEvent : IEvent 
    {
        public int CurrentProgress;
        public int MaxProgress;
    }
    
    public struct ShowHintEvent : IEvent
    {
        public int DuckIndex;
    }
    
    // События, связанные с утками
    public struct DuckSpawnedEvent : IEvent 
    {
        public int DuckIndex;
        public int BasketIndex;
    }
    
    public struct DuckDragStartEvent : IEvent 
    {
        public int DuckIndex;
    }
    
    public struct DuckDragEndEvent : IEvent 
    {
        public int DuckIndex;
        public bool PlacedCorrectly;
    }
    
    public struct AllDucksPlacedEvent : IEvent {}
}