﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Treino3.Models;
using Treino3.Repositories.Interfaces;
using Treino3.Services;

namespace Treino3.Controllers
{
    public class SalesController : Controller
    {
        private readonly ISalesRecordRepository _salesServices;
        private readonly IConfiguration _configuration;

        public SalesController(ISalesRecordRepository salesServices, IConfiguration configuration)
        {
            _salesServices = salesServices;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page, DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
                minDate = DateTime.Parse("01/01/1960");

            if(!maxDate.HasValue)
                maxDate = DateTime.Now;

            List<SalesRecord> sales = await _salesServices.GetAllPaginatedByDateAsync(minDate, maxDate, page ?? 1);

            ViewBag.ParamName = "page";
            ViewBag.ItemsPerPage = _configuration.GetValue<int>("Pagination:ItemsPerPage");
            ViewBag.MaxLinksPerPage = _configuration.GetValue<int>("Pagination:MaxLinksPerPage");
            ViewBag.TotalItems = await _salesServices.TotalItemsByDateAsync(minDate, maxDate);
            return View(sales);
        }
    }
}
