using SimShop.Base;
using SimShop.Data;
using SimShop.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimShop.Operation;

public interface ICategoryService: IBaseService<Category, CategoryRequest, CategoryResponse>
{
    //ApiResponse<List<CategoryResponse>> GetAllCategoryWithProducts();
}
