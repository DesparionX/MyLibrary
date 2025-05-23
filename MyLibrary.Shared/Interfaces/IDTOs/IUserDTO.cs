﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyLibrary.Server.Data.Entities.Interfaces;

namespace MyLibrary.Shared.Interfaces.IDTOs
{
    public interface IUserDTO<TEntity,TId> : IDTO<TEntity,TId>, IUser<TId>
        where TEntity : class, IUser<TId>
        where TId : IEquatable<TId>
    {
    }
    public interface IUserDTO : IDTO<IUser<string>, string>, IUser<string>
    {
    }
}
