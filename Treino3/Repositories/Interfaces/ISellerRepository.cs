﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Treino3.Models;

namespace Treino3.Repositories.Interfaces
{
    public interface ISellerRepository : ICrud<Seller>, IPagination<Seller>
    {
    }
}
