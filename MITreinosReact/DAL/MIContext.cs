using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MITreinosReact.Models;

namespace MITreinosReact.DAL
{
	public class MIContext : DbContext
	{
		public const string DEFAULT_USER_EMAIL = "test@test.com";
		public const string DEFAULT_USER_PWD = "123456";

		public MIContext(DbContextOptions<MIContext> options)
			: base(options)
		{
		}

		public DbSet<UserModel> Users { get; set; }
		public DbSet<UserManagerModel> UserManagers { get; set; }
		public DbSet<CourseModel> Courses { get; set; }
		public DbSet<CourseModuleModel> CourseModules { get; set; }
		public DbSet<CourseLessonModel> CourseLessons { get; set; }
		public DbSet<UserCourseModel> UserCourse { get; set; }
		public DbSet<UserLessonWatchModel> UserWatchs { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseLazyLoadingProxies();
			base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserManagerModel>(b =>
			{
				b.HasIndex(b => b.Email).IsUnique();

				b.HasData(new UserManagerModel()
				{
					Id = 1,
					Name = "Ramon",
					Email = DEFAULT_USER_EMAIL,
					PWD = DEFAULT_USER_PWD,
				});
			});

			modelBuilder.Entity<UserModel>(b =>
			{
				b.HasIndex(b => b.Email).IsUnique();

				b.HasData(new UserModel()
				{
					Id = 1,
					Name = "Ramon",
					Email = DEFAULT_USER_EMAIL,
					PWD = DEFAULT_USER_PWD,
				});
			});


			// Import courses
			modelBuilder.Entity<CourseModel>(b =>
			{
				b.HasData(new CourseModel()
				{
					Id = 1,
					Title = "IE Homens",
					Slug = "ie-homens",
					LogoURL = "https://storagemvc.blob.core.windows.net/videos/pro4-23.jpg",
				});
				b.HasData(new CourseModel()
				{
					Id = 2,
					Title = "IE Mulheres",
					Slug = "ie-mulheres",
					LogoURL = "https://storagemvc.blob.core.windows.net/videos/pro4-23.jpg",
				});
			});
			modelBuilder.Entity<UserCourseModel>(b =>
			{
				b.HasData(new UserCourseModel()
				{
					Id = 1,
					CourseId = 1,
					UserId = 1,
				});
				b.HasData(new UserCourseModel()
				{
					Id = 2,
					CourseId = 2,
					UserId = 1,
				});
			});
		}
	}
}
