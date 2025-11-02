using Microsoft.AspNetCore.Identity;
using DataAccess.Types;

namespace DataAccess.Entities;

public class ApplicationUser : IdentityUser<int>
{
    public WriterType? WriterType { get; set; }
}