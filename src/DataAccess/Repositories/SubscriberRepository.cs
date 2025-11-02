using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class SubscriberRepository(AppDbContext context) : Repository<Subscriber>(context), ISubscriberRepository
{
    
}