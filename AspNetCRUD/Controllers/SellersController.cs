using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Treino3.Models;
using Treino3.Models.ViewModel;
using Treino3.Repositories.Interfaces;
using Treino3.Services;
using Treino3.Services.Exceptions;

namespace Treino3.Controllers
{
    public class SellersController : Controller
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IConfiguration _configuration;

        public SellersController(ISellerRepository sellerRepository, IDepartmentRepository departmentRepository, IConfiguration configuration)
        {
            _sellerRepository = sellerRepository;
            _departmentRepository = departmentRepository;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(int? page, string search)
        {
            List<Seller> sellers;

            if (!string.IsNullOrEmpty(search)) 
            {
                Expression<Func<Seller, bool>> filter = s => s.Name.Trim().ToLower().Contains(search.Trim().ToLower());
                ViewBag.TotalItems = await _sellerRepository.TotalItemsWithFilterAsync(filter);
                sellers = await _sellerRepository.GetAllPaginatedWithFilterAsync
                (
                    filter: filter,
                    page: page ?? 1
                );
            }
            else
            {
                ViewBag.TotalItems = await _sellerRepository.TotalItemsAsync();
                sellers = await _sellerRepository.GetAllPaginatedAsync(page ?? 1);
            }

            ViewBag.ParamName = "page";
            ViewBag.ItemsPerPage = _configuration.GetValue<int>("Pagination:ItemsPerPage");
            ViewBag.MaxLinksPerPage = _configuration.GetValue<int>("Pagination:MaxLinksPerPage");

            return View(sellers);
        }

        public async Task<IActionResult> Create()
        {
            SellerFormViewModel viewModel = new SellerFormViewModel { Departments = await _departmentRepository.FindAllAsync() };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Seller seller)
        {
            if(ModelState.IsValid)
            {
                await _sellerRepository.InsertAsync(seller);
                return RedirectToAction(nameof(Index));
            }

            SellerFormViewModel viewModel = new SellerFormViewModel { Departments = await _departmentRepository.FindAllAsync(), Seller = seller };
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });

            Seller seller = await _sellerRepository.FindByIdAsync(id.Value);
            if (seller == null)
                return RedirectToAction(nameof(Error), new { message = "Id not founded" });

            return View(seller);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerRepository.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch(IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Id not founded" });

            Seller seller = await _sellerRepository.FindByIdAsync(id.Value);
            if (seller == null)
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });

            SellerFormViewModel viewModel = new SellerFormViewModel() { Seller = seller, Departments = await _departmentRepository.FindAllAsync() };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if (id != seller.Id)
                return RedirectToAction(nameof(Error), new { message = "Id missmatch" });

            try
            {
                await _sellerRepository.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            catch(ApplicationException e)
            {
                return RedirectToAction(nameof(Index), new { message = e.Message });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });

            Seller seller = await _sellerRepository.FindByIdAsync(id.Value);
            if (seller == null)
                return RedirectToAction(nameof(Error), new { message = "Id not founded"});

            return View(seller);
        }

        public IActionResult Error(string message)
        {
            ErrorViewModel viewModel = new ErrorViewModel() { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Message = message };
            return View(viewModel);
        }
    }
}
