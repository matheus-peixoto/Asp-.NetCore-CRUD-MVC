using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Treino3.Models;
using Treino3.Services;
using Treino3.Services.Exceptions;
using Treino3.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;

namespace Treino3.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IConfiguration _configuration;
        public DepartmentsController(IDepartmentRepository departmentRepository, IConfiguration configuration)
        {
            _departmentRepository = departmentRepository;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(int? page, string search)
        {
            List<Department> departments;
            if (!string.IsNullOrEmpty(search))
            {
                Expression<Func<Department, bool>> filter = s => s.Name.Trim().ToLower().Contains(search.Trim().ToLower());
                ViewBag.TotalItems = await _departmentRepository.TotalItemsWithFilterAsync(filter);
                departments = await _departmentRepository.GetAllPaginatedWithFilterAsync
                (
                    filter: s => s.Name.Trim().ToLower().Contains(search.Trim().ToLower()),
                    page: page ?? 1
                );
            }
            else
            {
                ViewBag.TotalItems = await _departmentRepository.TotalItemsAsync();
                departments = await _departmentRepository.GetAllPaginatedAsync(page ?? 1);
            }

            ViewBag.ParamName = "page";
            ViewBag.ItemsPerPage = _configuration.GetValue<int>("Pagination:ItemsPerPage");
            ViewBag.MaxLinksPerPage = _configuration.GetValue<int>("Pagination:MaxLinksPerPage");

            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if(ModelState.IsValid)
            {
                await _departmentRepository.InsertAsync(department);
                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { msg = "Id not provided" });

            Department department = await _departmentRepository.FindByIdAsync(id.Value);
            if (department == null)
                return RedirectToAction(nameof(Error), new { msg = "Id not founded" });

            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _departmentRepository.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch(IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { msg = e.Message });
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { msg = "Id not provided" });

            Department department = await _departmentRepository.FindByIdAsync(id.Value);
            if (department == null)
                return RedirectToAction(nameof(Error), new { msg = "Id not founded" });

            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Department department)
        {
            try
            {
                await _departmentRepository.UpdateAsync(department);
                return RedirectToAction(nameof(Index));
            }
            catch(ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { msg = e.Message });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { msg = "Id not provided" });

            Department department = await _departmentRepository.FindByIdAsync(id.Value);
            if(department == null)
                return RedirectToAction(nameof(Error), new { msg = "Id not founded" });

            return View(department);
        }

        public IActionResult Error(string msg)
        {
            ErrorViewModel viewModel = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = msg };
            return View(viewModel);
        }
    }
}
