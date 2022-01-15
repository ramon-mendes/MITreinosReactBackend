using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace MITreinosReact.Models
{
	public class UserManagerModel
	{
		[Key]
		public int Id  { get; set; }
		public string Name { get; set; }
		[StringLength(150)]
		public string Email { get; set; }
		public string PWD { get; set; }

		[NotMapped]
		public bool Authorized { get; set; }
		[NotMapped]
		public string ReturnUrl { get; set; }
	}

	public class UserManagerValidator : AbstractValidator<UserManagerModel>
	{
		public UserManagerValidator()
		{
			RuleFor(x => x.Email).EmailAddress().NotEmpty();
			RuleFor(x => x.PWD).NotEmpty();
			RuleFor(x => x.Authorized).Must(x => x).WithMessage("Login failed.");
		}
	}
}