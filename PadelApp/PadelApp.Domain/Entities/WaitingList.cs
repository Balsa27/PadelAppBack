    using PadelApp.Domain.Primitives;

    namespace PadelApp.Domain.ValueObjects;

    public class WaitingList : Entity
    {
        public List<Guid> UserIds { get; private set; } = new();

        public WaitingList()
        {
            
        }
        
        public WaitingList( List<Guid> userIds)
        {
            UserIds = userIds;
        }

        public void AddToWaitingList(Guid userId)
        {
            if(!UserIds.Contains(userId))
                throw new InvalidOperationException("User is already in the waiting list");
            
            UserIds.Add(userId);
        }
        
        public void RemoveFromWaitingList(Guid userId)
        {
            if(!UserIds.Contains(userId))
                throw new InvalidOperationException("User is not in the waiting list");
            
            UserIds.Remove(userId);
        }
        
        public bool IsEmpty() => !UserIds.Any(); 
        
        public Guid? GetNextUser() => UserIds.FirstOrDefault();
    }