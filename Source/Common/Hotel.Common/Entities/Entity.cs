using System;

namespace Hotel.Common.Entities
{
    public abstract class Entity : IEntity
    {
        protected Entity() { Create(); }

        public string Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }
        public bool IsDeleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public void Create()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.Now;
        }

        public void Update()
        {
            LastUpdatedAt = DateTime.Now;
        }

        public void Delete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.Now;
        }
    }

    public interface IEntity
    {
        string Id { get; }
        DateTime CreatedAt { get; }
        DateTime LastUpdatedAt { get; }
        bool IsDeleted { get; }
        DateTime? DeletedAt { get; }

        void Create();
        void Update();
        void Delete();
    }
}