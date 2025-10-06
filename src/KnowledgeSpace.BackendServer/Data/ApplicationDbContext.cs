using KnowledgeSpace.BackendServer.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeSpace.BackendServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
            builder.Entity<User>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
            
            // Combination keys
            //builder.Entity<LabelInKnowledgeBase>()
            //    .HasKey(c => new { c.LabelId, c.KnowledgeBaseId });
            builder.Entity<Permission>()
                .HasKey(c => new { c.RoleId, c.FunctionId, c.CommandId });
            //builder.Entity<Vote>()
            //    .HasKey(c => new { c.KnowledgeBaseId, c.UserId });
            builder.Entity<CommandInFunction>()
                .HasKey(c => new { c.CommandId, c.FunctionId });

            // =================== VOTE =================
            builder.Entity<Vote>(entity =>
            {
                entity.HasKey(v => new { v.KnowledgeBaseId, v.UserId });

                // Cấu hình kiểu dữ liệu cho UserId (do bạn giới hạn varchar(50))
                entity.Property(v => v.UserId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                // Thiết lập quan hệ với KnowledgeBase
                entity.HasOne(v => v.KnowledgeBase)        // <-- navigation property
                    .WithMany(kb => kb.Votes)
                    .HasForeignKey(v => v.KnowledgeBaseId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Thiết lập quan hệ với User
                entity.HasOne(v => v.User)                 // <-- navigation property
                    .WithMany(u => u.Votes)
                    .HasForeignKey(v => v.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // =================== COMMENT =================
            builder.Entity<Comment>(entity =>
            {
                entity.HasOne(c => c.KnowledgeBase)
                    .WithMany(kb => kb.Comments)
                    .HasForeignKey(c => c.KnowledgeBaseId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.OwnerUser)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(c => c.OwnerUserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // =================== ATTACHMENT =================
            builder.Entity<Attachment>(entity =>
            {
                entity.HasOne(a => a.KnowledgeBase)
                    .WithMany(kb => kb.Attachments)
                    .HasForeignKey(a => a.KnowledgeBaseId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(a => a.Comment)
                    .WithMany(c => c.Attachments)
                    .HasForeignKey(a => a.CommentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            // ============= KnowledgeBase ================
            builder.Entity<KnowledgeBase>(entity =>
            {
                entity.HasOne(kb => kb.Category)
                    .WithMany(c => c.KnowledgeBases)
                    .HasForeignKey(kb => kb.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(kb => kb.OwnerUser)
                    .WithMany(u => u.KnowledgeBases)
                    .HasForeignKey(kb => kb.OwnerUserId)
                    .OnDelete(DeleteBehavior.NoAction); // tránh vòng xóa User
            });

            builder.Entity<Category>(entity =>
            {
                // Quan hệ 1-n: Category → KnowledgeBase
                entity.HasMany(c => c.KnowledgeBases)
                    .WithOne(kb => kb.Category)
                    .HasForeignKey(kb => kb.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });



            // ================ LabelInKnowledgeBase =================
            builder.Entity<LabelInKnowledgeBase>(entity =>
            {
                entity.HasKey(x => new { x.LabelId, x.KnowledgeBaseId });

                entity.HasOne(x => x.Label)
                    .WithMany(l => l.LabelInKnowledgeBases)
                    .HasForeignKey(x => x.LabelId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.KnowledgeBase)
                    .WithMany(kb => kb.LabelInKnowledgeBases)
                    .HasForeignKey(x => x.KnowledgeBaseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // =================== REPORT ====================
            builder.Entity<Report>(entity =>
            {
                // Report → KnowledgeBase (1-n)
                entity.HasOne(r => r.KnowledgeBase)
                    .WithMany(kb => kb.Reports)
                    .HasForeignKey(r => r.KnowledgeBaseId)
                    .OnDelete(DeleteBehavior.Cascade); // Giữ nguyên

                // Report → Comment (1-n)
                entity.HasOne(r => r.Comment)
                    .WithMany(c => c.Reports)
                    .HasForeignKey(r => r.CommentId)
                    .OnDelete(DeleteBehavior.NoAction); // ❗ Không cascade để tránh vòng xóa

                // Report → User (1-n)
                entity.HasOne(r => r.ReportUser)
                    .WithMany(u => u.Reports)
                    .HasForeignKey(r => r.ReportUserId)
                    .OnDelete(DeleteBehavior.NoAction); // ❗ Không cascade
            });



            // Giúp giảm thiểu sự phụ thuộc vào SQL Server
            builder.HasSequence("KnowledgeBaseSequence");
        }

        public DbSet<Command> Commands { set; get; }
        public DbSet<CommandInFunction> CommandInFunctions { set; get; }
        public DbSet<ActivityLog> ActivityLogs { set; get; }
        public DbSet<Category> Categories { set; get; }
        public DbSet<Comment> Comments { set; get; }
        public DbSet<Function> Functions { set; get; }
        public DbSet<KnowledgeBase> KnowledgeBases { set; get; }
        public DbSet<Label> Labels { set; get; }
        public DbSet<LabelInKnowledgeBase> LabelInKnowledgeBases { set; get; }
        public DbSet<Permission> Permissions { set; get; }
        public DbSet<Report> Reports { set; get; }
        public DbSet<Vote> Votes { set; get; }
        public DbSet<Attachment> Attachments { get; set; }
    }
}
