using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce.Api.Search.Controllers
{
    /*
     * Course:          Web Programming 3
     * Assessment:      Milestone 3
     * Created by:      Breanna de Forest - 2145494
     * Date:            25 November, 2023
     * Class Name:      SearchController.cs
     * Description:     This class handles search calls. It has one method that calls the SearchAsync function
     *                  to get a result. If the search was successful, its posts an ok response with the
     *                  retrieved data, otherwise it sends a not found response.
     */

    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        [HttpPost]
        public async Task<IActionResult> SearchAsync(SearchTerm term)
        {
            var result = await searchService.SearchAsync(term.CustomerId);
            if (result.IsSuccess)
            {
                return Ok(result.SearchResults);
            }
            return NotFound();
        }
    }
}
