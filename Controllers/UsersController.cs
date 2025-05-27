using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(DataContext context) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await context.AppUsers.ToListAsync();
            return users;
        }



        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user =  await context.AppUsers.FindAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            return user;
        }

    }
}
