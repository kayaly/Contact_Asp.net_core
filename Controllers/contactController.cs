using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Contact.Data;
using Contact.Models;
using Contact.Models.ContactViewModel;

using System.Text.RegularExpressions;
using Contact.Pages;

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
        public async Task<IActionResult> Index(string Serching
        , bool sensitive,
          int? pageNumber,
          string sortOrder)
        {

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["PhoneSortParm"] = sortOrder == "Phone" ? "Phone_desc" : "Phone";
            ViewData["EmailSortParm"] = sortOrder == "Email" ? "Email_desc" : "Email";
            ViewData["MessageSortParm"] = sortOrder == "Message" ? "Message_desc" : "Message";
            ViewData["departementSortParm"] = sortOrder == "departement" ? "departement_desc" : "departement";

            // List<contact> contacts = _context.contact.ToList();

// select s.* from contact s
            var contacts = from contact in _context.contact
            join departemnt in _context.departments 
            on contact.DepartementId equals departemnt.DepartementId
            select contact;

            
            switch (sortOrder)
            {
                case "name_desc":
                    contacts = contacts.OrderByDescending(l => l.Name);

                    break;
                case "Phone":
                    contacts = contacts.OrderBy(l => l.Phone);
                    break;
                case "Phone_desc":
                    contacts = contacts.OrderByDescending(l => l.Phone);
                    break;
                case "Email":
                    contacts = contacts.OrderBy(l => l.Email);
                    break;
                case "Email_desc":
                    contacts = contacts.OrderByDescending(l => l.Email);
                    break;
                case "Message":
                    contacts = contacts.OrderBy(l => l.Message);
                    break;
                case "Message_desc":
                    contacts = contacts.OrderByDescending(l => l.Message);
                    break;
                case "departement":
                    contacts = contacts.OrderBy(l => l.departement);
                    break;
                case "departement_desc":
                    contacts = contacts.OrderByDescending(l => l.departement);
                    break;
                default:
                    contacts = contacts.OrderBy(l => l.Name);
                    break;
            }



            if (!string.IsNullOrEmpty(Serching))
            {
                pageNumber = 1;
                if (!sensitive)
                {
                    Serching = Serching.ToLower();
                }

                return View(await contacts.Where(x =>
                x.Name.Contains(Serching) ||
                x.Email.Contains(Serching) ||
                x.Phone.Contains(Serching) ||
                x.Message.Contains(Serching)).ToListAsync());
            }

            int pageSize = 10;

            return View(PaginatedList<ContactViewModel>.Create(FillContactViewmodel(contacts).AsQueryable(), pageNumber ?? 1, pageSize));

        }

       
        public List<ContactViewModel> FillContactViewmodel(IQueryable<contact> IQ)
        {


           
            List<ContactViewModel> liContactView = new List<ContactViewModel>();

            foreach (contact x in IQ)
            {
                ContactViewModel contactview = new ContactViewModel();
                contactview.Name = x.Name;
                contactview.Email = x.Email;
                contactview.Phone = x.Phone;
                contactview.Message = x.Message;
                contactview.departementName = x.departement.Name;
                contactview.id = x.id;
                contactview.DepartementId = x.DepartementId;


                liContactView.Add(contactview);
            }

            return liContactView;
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

            contact.Email = contact.Email.ToLower();
            var Contact = _context.contact.FirstOrDefault(a => a.Email == contact.Email);

            if (Contact != null)
            {

                ModelState.AddModelError(string.Empty, "Email already exists");
                Create();
                return View();

            }

            else if (ModelState.IsValid)
            {

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

            var contact = await _context.contact.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,Email,Phone,Message")] contact contact)
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
