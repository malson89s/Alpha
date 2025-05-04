using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

// Interface – gärna i egen fil om möjligt
public interface IMemberRepository : IBaseRepository<MemberEntity, Member>
{
}

// Repository-klass
public class MemberRepository : BaseRepository<MemberEntity, Member>, IMemberRepository
{
    public MemberRepository(DataContext context) : base(context)
    {
    }
}

