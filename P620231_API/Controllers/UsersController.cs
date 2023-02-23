﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P620231_API.Models;
using P620231_API.Attributes;
using P620231_API.ModelsDTOs;

namespace P620231_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ApiKey] // EL APIKEY para seguridad 
    public class UsersController : ControllerBase
    {
        private readonly P620231_AutoAppoContext _context;

        public UsersController(P620231_AutoAppoContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet("ValidateUserLogin")]
        public async Task<ActionResult<User>> ValidateUserLogin(string pUserName, string pPassword)
        {
            //TODO: encriptar el password para validar contra el password encriptado en DB

            var user = await _context.Users.SingleOrDefaultAsync(e => e.Email == pUserName && e.LoginPassword == pPassword);
            if(user == null)
            {
                return NotFound();
            }

            return user;


        }




        //Ejemplo  GET: /api/Users/GetUserData?Correo=a%40gmail.com
        //Este Get permite obtener la info de un usuario 
        //recibiendo por el Email de parametro de busqueda
        [HttpGet("GetUserData")]
        public ActionResult<IEnumerable<UserDTO>> GetUserData(string Correo)
        {
            //aca usaremos una consulta linq que une informacion de 
            // 3 tablas (user - userRole - UserStatus)
            //Para asignar esos valores al DTO de usuario y entregarlos en formato json

            var query = (from u in _context.Users
                         join ur in _context.UserRoles on u.UserRoleId equals ur.UserRoleId
                         join us in _context.UserStatuses on u.UserStatusId equals us.UserStatusId
                         where u.Email == Correo && u.UserStatusId != 2
                         select new
                         {
                             idusuario = u.UserId,
                             nombre = u.Name,
                             correo = u.Email,
                             telefono = u.PhoneNumber,
                             contrasennia = u.LoginPassword,
                             cedula = u.CardId,
                             direccion = u.Address,
                             idrol = ur.UserRoleId,
                             roldescripcion = ur.UserRoleDescription,
                             idestado = us.UserStatusId,
                             estadodescripcion = us.UserStatuDescription
                         }).ToList();

            //crear un objeto de tipo de DTO de retorno
            List<UserDTO> list = new List<UserDTO>();

            foreach (var item in query)
            {
                UserDTO NewItem = new UserDTO()
                {
                    IDUsuario = item.idusuario,
                    Nombre = item.nombre,
                    Correo = item.correo,
                    NumeroTelefono = item.telefono,
                    Contrasennia = item.contrasennia,
                    Cedula = item.cedula,
                    Direccion = item.direccion,
                    IdRol = item.idrol,
                    RolDescripcion = item.roldescripcion,
                    IdEstado = item.idestado,
                    EstadoDescripcion = item.estadodescripcion,
                };
                list.Add(NewItem);
            }
            if(list == null)
            {
                return NotFound();
            }
            return list;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
