using System;

namespace Domain
{
    public record MinoId
    {
        readonly Guid _id = Guid.NewGuid();
    }
}