// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Devpendent.Areas.Identity.Data;
using Devpendent.Data;
using Devpendent.Data.Validation;
using Devpendent.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartBreadcrumbs.Attributes;

namespace Devpendent.Areas.Identity.Pages.Account.Manage
{
    [Breadcrumb("Manage your account", AreaName = "Identity")]
    public class IndexModel : PageModel
    {
        private readonly DevpendentContext _context;
        private readonly UserManager<DevpendentUser> _userManager;
        private readonly SignInManager<DevpendentUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public IndexModel(
            DevpendentContext context,
            UserManager<DevpendentUser> userManager,
            SignInManager<DevpendentUser> signInManager,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public string Email { get; set; }

        public DateTime RegisterDate { get; set; }

        public string Image { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public string ContactText { get; set; }

        public string Specialties { get; set; }

        public string Website { get; set; }

        public ICollection<Job> Jobs { get; set; }

        public ICollection<Education> Educations { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string Username { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last name")]
            public string LastName { get; set; }

            public string Location { get; set; }

            public string Description { get; set; }

            public string ContactText { get; set; }

            public string Specialties { get; set; }

            public string Website { get; set; }

            [FileExtension]
            public IFormFile ImageUpload { get; set; }
        }

        private async Task LoadAsync(DevpendentUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var firstName = user.FirstName;
            var lastName = user.LastName;
            var email = user.Email;
            var location = user.Location;
            var description = user.Description;
            var registerDate = user.RegisterDate;
            var image = user.Image;
            var contactText = user.ContactText;
            var specialties = user.Specialties;
            var website = user.Website;

            var jobs = _context.Jobs.Where(j => j.UserId == user.Id).ToList();
            var educations = _context.Educations.Where(e => e.UserId == user.Id).ToList();

            user.Jobs = jobs;
            user.Educations = educations;

            Email = email;
            RegisterDate = registerDate;
            Image = image;
            Username = userName;
            FirstName = firstName;
            LastName = lastName;
            Location = location;
            Description = description;
            ContactText = contactText;
            Specialties = specialties;
            Jobs = jobs;
            Educations = educations;
            Website = website;

            Input = new InputModel
            {
                Username = userName,
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                LastName = lastName,
                Location = location,
                Description = description,
                ContactText = contactText,
                Specialties = specialties,
                Website = website
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (Input.Username != user.UserName)
            {
                user.UserName = Input.Username;
                var setUserNameResult = await _userManager.UpdateAsync(user);
                if (!setUserNameResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set username.";
                    return RedirectToPage();
                }
            }

            if (Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;
                var setFirstNameResult = await _userManager.UpdateAsync(user);
                if (!setFirstNameResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set first name.";
                    return RedirectToPage();
                }
            }

            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
                var setLastNameResult = await _userManager.UpdateAsync(user);
                if (!setLastNameResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set last name.";
                    return RedirectToPage();
                }
            }

            if (Input.Location != user.Location)
            {
                user.Location = Input.Location;
                var setLocationResult = await _userManager.UpdateAsync(user);
                if (!setLocationResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set location.";
                    return RedirectToPage();
                }
            }

            if (Input.Description != user.Description)
            {
                user.Description = Input.Description;
                var setDescriptionResult = await _userManager.UpdateAsync(user);
                if (!setDescriptionResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set description.";
                    return RedirectToPage();
                }
            }

            if (Input.ContactText != user.ContactText)
            {
                user.ContactText = Input.ContactText;
                var setContactTextResult = await _userManager.UpdateAsync(user);
                if (!setContactTextResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set contact text.";
                    return RedirectToPage();
                }
            }

            if (Input.Specialties != user.Specialties)
            {
                user.Specialties = Input.Specialties;
                var setSpecialtiesResult = await _userManager.UpdateAsync(user);
                if (!setSpecialtiesResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set specialties.";
                    return RedirectToPage();
                }
            }

            if (Input.Website != user.Website)
            {
                user.Website = Input.Website;
                var setWebsiteResult = await _userManager.UpdateAsync(user);
                if (!setWebsiteResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set website.";
                    return RedirectToPage();
                }
            }

            if (Input.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/users");
                var extension = Path.GetExtension(Input.ImageUpload.FileName);
                string imageName = user.UserName + extension;
                string filePath = Path.Combine(uploadsDir, imageName);

                if (user.Image != null)
                {
                    string oldImagePath = Path.Combine(uploadsDir, user.Image);

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                FileStream fs = new FileStream(filePath, FileMode.Create);
                await Input.ImageUpload.CopyToAsync(fs);
                fs.Close();

                user.Image = imageName;
                var setImageResult = await _userManager.UpdateAsync(user);
                if (!setImageResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set a profile picture.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
