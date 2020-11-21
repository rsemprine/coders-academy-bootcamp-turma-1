using CodersAcademy.API.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodersAcademy.API.Repository
{
    public class UserRepository
    {
        private readonly MusicContext Context;

        public UserRepository(MusicContext context)
        {
            Context = context;
        }

        public async Task CreateAsync(User user)
        {
            await Context.Users.AddAsync(user);
            await Context.SaveChangesAsync();
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            return await Context.Users
                                .Include(x => x.FavoriteMusics)
                                .ThenInclude(x => x.Music)
                                .ThenInclude(x => x.Album)
                                .FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
        }
    }
}
