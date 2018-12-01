using System;

namespace OwinPoc
{
    public class ScopeId
    {
        public Guid Id { get; set; }

        public static ScopeId Empty => new ScopeId(Guid.Empty);

        public ScopeId(Guid id)
        {
            Id = id;
        }

        public static ScopeId NewId() => new ScopeId(Guid.NewGuid());
    }
}