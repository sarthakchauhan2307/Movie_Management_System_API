using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheatreMaster.Api.Data;
using TheatreMaster.Api.Models;
using TheatreMasterService.Api.Service;

namespace TheatreMasterService.Api.Controllers
{
    [Route("/api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class TheatreController : ControllerBase
    {
        #region configuration
        private readonly ITheatreService _theatreservice;

        public TheatreController(ITheatreService theatreservice)
        {
            _theatreservice = theatreservice;
        }
        #endregion

        #region GetTheatre
        [HttpGet]

        public async Task<IActionResult> GetTheatre()
             => Ok(await _theatreservice.GetTheatresAsync());
        #endregion

        #region AddTheatre
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddTheatre(Theatre theatre)
            => Ok(await _theatreservice.CreateTheatreAsync(theatre));
        #endregion

        #region GetTheatreById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTheatreById(int id)
             => Ok(await _theatreservice.GetTheatreByIdAsync(id));
        #endregion

        #region UpdateTheatre
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateTheatre(int id, Theatre theatre)
                => Ok(await _theatreservice.UpdateTheatreAsync(id, theatre));
        #endregion

        #region DeleteTheatre
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteTheatre(int id)
              => Ok(await _theatreservice.DeleteTheatreAsync(id));
        #endregion



    }
}
