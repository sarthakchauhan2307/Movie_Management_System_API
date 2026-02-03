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
    public class ScreenController : ControllerBase
    {
        #region configuration
        private readonly IScreenService _service;

        public ScreenController(IScreenService service)
        {
            _service = service;
        }
        #endregion

        #region GetScreens
        [HttpGet]
        public async Task<IActionResult> GetScreens()
                  => Ok(await _service.GetScreensAsync());
        #endregion

        #region GetScreenById
        [HttpGet("{id}")]
        public async Task<IActionResult> GetScreenById(int id)
        {
           return Ok(await _service.GetScreenByIdAsync(id));
        }
        #endregion

        #region AddScreen
        [HttpPost]
        public async Task<IActionResult> AddScreen(Screen screen)
        {
           return Ok( await _service.CreateScreenAsync(screen));
        }
        #endregion

        #region UpdateScreen
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateScreen(int id, Screen screen)
        {
            return Ok( await _service.UpdateScreenAsync(id, screen));
        }
        #endregion

        #region DeleteScreen
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteScreen(int id)
        {
           return Ok( await _service.DeleteScreenAsync(id));
        }
        #endregion



    }
}
