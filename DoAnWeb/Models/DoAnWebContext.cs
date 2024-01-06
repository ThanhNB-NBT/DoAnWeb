using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Models;

public partial class DoAnWebContext : DbContext
{
    public DoAnWebContext()
    {
    }

    public DoAnWebContext(DbContextOptions<DoAnWebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<About> Abouts { get; set; }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AdminMenu> AdminMenus { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BlogComment> BlogComments { get; set; }

    public virtual DbSet<CategoryBlog> CategoryBlogs { get; set; }

    public virtual DbSet<CategoryProject> CategoryProjects { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-HJV1A73;Initial Catalog=DoAnWeb;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<About>(entity =>
        {
            entity.ToTable("About");

            entity.Property(e => e.AboutId).HasColumnName("AboutID");
            entity.Property(e => e.Image).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);
        });

        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UserName).HasMaxLength(50);
            entity.Property(e => e.Image).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_Account_Role1");
        });

        modelBuilder.Entity<AdminMenu>(entity =>
        {
            entity.HasKey(e => e.MenuAdminId).HasName("PK_MenuAdmin");

            entity.ToTable("AdminMenu");

            entity.Property(e => e.ActionName).HasMaxLength(250);
            entity.Property(e => e.AreaName).HasMaxLength(250);
            entity.Property(e => e.ControllerName).HasMaxLength(250);
            entity.Property(e => e.Icon).HasMaxLength(250);
            entity.Property(e => e.ItemName).HasMaxLength(250);
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.ToTable("Blog");

            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Image).HasMaxLength(500);
            entity.Property(e => e.Tags).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Blog_Account");

            entity.HasOne(d => d.Category).WithMany(p => p.Blogs)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Blog_CategoryBlog");
        });

        modelBuilder.Entity<BlogComment>(entity =>
        {
            entity.HasKey(e => e.CommentId);

            entity.ToTable("BlogComment");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.BlogId).HasColumnName("BlogID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Detail).HasMaxLength(250);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(50);

            entity.HasOne(d => d.Blog).WithMany(p => p.BlogComments)
                .HasForeignKey(d => d.BlogId)
                .HasConstraintName("FK_BlogComment_Blog");
        });

        modelBuilder.Entity<CategoryBlog>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK_Category");

            entity.ToTable("CategoryBlog");

            entity.Property(e => e.Name).HasMaxLength(500);
        });

        modelBuilder.Entity<CategoryProject>(entity =>
        {
            entity.HasKey(e => e.CategoryPid);

            entity.ToTable("CategoryProject");

            entity.Property(e => e.CategoryPid).HasColumnName("CategoryPId");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.ToTable("Contact");

            entity.Property(e => e.ContactId).HasColumnName("ContactID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Phone).HasMaxLength(50);
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK_Team");

            entity.ToTable("Member");

            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.Detail).HasMaxLength(500);
            entity.Property(e => e.Image).HasMaxLength(250);
            entity.Property(e => e.Link).HasMaxLength(250);
            entity.Property(e => e.Message).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.WorkPosition).HasMaxLength(50);
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.ToTable("Menu");

            entity.Property(e => e.MenuId).HasColumnName("MenuID");
            entity.Property(e => e.ActionName).HasMaxLength(50);
            entity.Property(e => e.ControllerName).HasMaxLength(50);
            entity.Property(e => e.Link).HasMaxLength(50);
            entity.Property(e => e.MenuName).HasMaxLength(50);
            entity.Property(e => e.ParentId).HasColumnName("ParentID");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.ToTable("Project");

            entity.Property(e => e.CategoryPid).HasColumnName("CategoryPId");
            entity.Property(e => e.Client).HasMaxLength(250);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Image).HasMaxLength(500);
            entity.Property(e => e.Link).HasMaxLength(500);
            entity.Property(e => e.ProjectName).HasMaxLength(500);

            entity.HasOne(d => d.CategoryP).WithMany(p => p.Projects)
                .HasForeignKey(d => d.CategoryPid)
                .HasConstraintName("FK_Project_CategoryProject");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
