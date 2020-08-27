using Microsoft.EntityFrameworkCore;

namespace Snail.Face.Test
{
    public class SnailFaceTestDb:DbContext
    {
        public SnailFaceTestDb(DbContextOptions<SnailFaceTestDb> options)
         : base(options)
        {
        }
        public DbSet<SnailFaceModel> FaceModels { get; set; }
    }
}
