    using PadelApp.Domain.Primitives;

    namespace PadelApp.Domain.ValueObjects;

    public class WaitingList : Entity
    {
        private readonly List<Guid> _userIds = new();
        public Guid? CurrentUserId { get; private set; }

        public WaitingList()
        {
            
        }
        
        public void NotifyNextUser()
        {
            CurrentUserId = GetNextUser();

            if (CurrentUserId.HasValue)
            {
                //todo: raise an event to notify the user
            }
        }

        public void Accept(Guid userId)
        {
            if (!CurrentUserId.HasValue || 
                CurrentUserId.Value != userId)
                throw new InvalidOperationException("This user cannot accept the waiting list at this time.");
            
            //todo: raise an event to notify the user
            _userIds.Remove(userId);
            CurrentUserId = null;
        }

        public void Reject(Guid userId)
        {
            if (!CurrentUserId.HasValue || CurrentUserId.Value != userId)
            {
                throw new InvalidOperationException("This user cannot reject the waiting list at this time.");
            }
            
            _userIds.Remove(userId);
            CurrentUserId = null;
        }

        public void AddToWaitingList(Guid userId)
        {
            if(!_userIds.Contains(userId))
                throw new InvalidOperationException("User is already in the waiting list");
            
            _userIds.Add(userId);
        }
        
        public bool IsEmpty() => !_userIds.Any(); 
        
        private Guid? GetNextUser() => _userIds.FirstOrDefault();
    }