using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Contact.Data;

using Contact.Models;

using System.Text.RegularExpressions;
using Contact.Pages;
using System.Collections;

namespace Contact.Controllers
{
    public class contactController : Controller
    {
        private readonly ContactContext _context;


        public contactController(ContactContext context)
        {
            _context = context;
        }

        // GET: contact
        // public void Sorting(int FirstSorting, int SecondSorting, int ThirdSorting, IEnumerable<ContactViewModel> contacts)
        // {
        //     ContactViewModel contact = new ContactViewModel();
        //     string[] array = { "", contact.Name, contact.Email, contact.Message, contact.Phone, contact.departementName };


        //     contacts = contacts.OrderBy(l => array[FirstSorting]).ThenBy().ThenBy();
        //     if (FirstSorting != null)
        //     {
        //         contacts = contacts.OrderBy(l => array[FirstSorting]);
        //         switch (SecondSorting)
        //         {
        //             case 1:
        //                 contacts = contacts.OrderBy(l => l.Name).ThenBy)();
        //                 switch (ThirdSorting)
        //                 {

        //                     case 2:
        //                         contacts = contacts.OrderBy(l => l.Name).ThenBy(l => l.Email);

        //                         break;
        //                     case 3:
        //                         contacts = contacts.OrderBy(l => l.Name).ThenBy(l => l.Message);
        //                         break;
        //                     case 4:
        //                         contacts = contacts.OrderBy(l => l.Name).ThenBy(l => l.Phone);
        //                         break;
        //                     case 5:
        //                         contacts = contacts.OrderBy(l => l.Name).ThenBy(l => l.departementName);
        //                         break;
        //                 }
        //                 break;
        //         }
        //     }

        // }

        public IQueryable<ContactViewModel> CustomSorting(int NumSorting, int levelSorting, IQueryable<ContactViewModel> contacts)
        {


            switch (NumSorting)
            {
                case 1:
                    if (levelSorting == 1)
                    {
                        contacts = contacts.OrderBy(x => x.Name);
                        return contacts;
                    }

                    else
                    {
                        contacts = ((IOrderedQueryable<ContactViewModel>)contacts).ThenBy(x => x.Name);
                        return contacts;
                    }

                case 2:
                    if (levelSorting == 1)
                    {
                        contacts = contacts.OrderBy(x => x.Email);
                        return contacts;
                    }

                    else
                    {
                        contacts = ((IOrderedQueryable<ContactViewModel>)contacts).ThenBy(x => x.Email);
                        return contacts;
                    }

                case 3:
                    if (levelSorting == 1)
                    {
                        contacts = contacts.OrderBy(x => x.Message);
                        return contacts;
                    }

                    else
                    {
                        contacts = ((IOrderedQueryable<ContactViewModel>)contacts).ThenBy(x => x.Message);
                        return contacts;
                    }

                case 4:
                    if (levelSorting == 1)
                    {
                        contacts = contacts.OrderBy(x => x.Phone);
                        return contacts;
                    }

                    else
                    {
                        contacts = ((IOrderedQueryable<ContactViewModel>)contacts).ThenBy(x => x.Phone);
                        return contacts;
                    }

                case 5:
                    if (levelSorting == 1)
                    {
                        contacts = contacts.OrderBy(x => x.departementName);
                        return contacts;
                    }

                    else
                    {
                        contacts = ((IOrderedQueryable<ContactViewModel>)contacts).ThenBy(x => x.departementName);
                        return contacts;
                    }



            }
            return contacts;
        }



        public async Task<IActionResult> Index(string Serching
        , bool sensitive,
          int? pageNumber,
          string sortOrder,
          int FirstSorting,
          int SecondSorting,
          int ThirdSorting)
        {
            int pageSize = 30;


            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PhoneSortParm"] = sortOrder == "Phone" ? "Phone_desc" : "Phone";
            ViewData["EmailSortParm"] = sortOrder == "Email" ? "Email_desc" : "Email";
            ViewData["MessageSortParm"] = sortOrder == "Message" ? "Message_desc" : "Message";
            ViewData["departementSortParm"] = sortOrder == "departement" ? "departement_desc" : "departement";



            var contacts = from c in _context.contact
                           join d in _context.departments
                           on c.DepartementId equals d.DepartementId
                           select new ContactViewModel
                           {
                               id = c.id,
                               Name = c.Name,
                               Email = c.Email,
                               Phone = c.Phone,
                               Message = c.Message,
                               departementName = d.Name


                           };


                            
            contacts = CustomSorting(FirstSorting, 1, contacts);
            contacts = CustomSorting(SecondSorting, 2, contacts);
            contacts = CustomSorting(ThirdSorting, 3, contacts);


            if (!string.IsNullOrEmpty(Serching))
            {
                pageNumber = 1;
                if (!sensitive)
                {
                    Serching = Serching.ToLower();
                }

                return View(PaginatedList<ContactViewModel>.Create(contacts.Where(x =>
                        x.Name.Contains(Serching) ||
                        x.Email.Contains(Serching) ||
                        x.Phone.Contains(Serching) ||
                        x.Message.Contains(Serching)), pageNumber ?? 1, pageSize));

            }
            return View(PaginatedList<ContactViewModel>.Create(contacts, pageNumber ?? 1, pageSize));

        }




        // GET: contact/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.contact
                .FirstOrDefaultAsync(m => m.id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: contact/Create
        public IActionResult Create()
        {


            List<Departement> li = new List<Departement>();
            li = _context.departments.ToList();
            ViewBag.listofitems = li;
            return View();


        }


        // POST: contact/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,Email,Phone,Message,DepartementId")] contact contact)
        {

            contact.departement = _context.departments.FirstOrDefault(a => a.DepartementId == contact.DepartementId);

            if (contact.Email == null)
            {
                ModelState.AddModelError(string.Empty, "The Email can not be empty");
                Create();
                return View();
            }


            //contact.Email = contact.Email.ToLower();

            // var Contact = _context.contact.FirstOrDefault(a => a.Email == contact.Email);

            // if (Contact != null)
            // {

            //     ModelState.AddModelError(string.Empty, "Email already exists");
            //     Create();
            //     return View();

            // }

            else if (ModelState.IsValid)
            {
                contact.Email = contact.Email.ToLower();
                _context.Add(contact);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }



            return View(contact);
        }

        // GET: contact/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = _context.contact.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: contact/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,Email,Phone,Message,DepartementId")] contact contact)
        {
            if (id != contact.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!contactExists(contact.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contact);
        }

        // GET: contact/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.contact
                .FirstOrDefaultAsync(m => m.id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.contact.FindAsync(id);
            _context.contact.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool contactExists(int id)
        {
            return _context.contact.Any(e => e.id == id);
        }
    }
}
