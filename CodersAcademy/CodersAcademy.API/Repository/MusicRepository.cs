using CodersAcademy.API.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodersAcademy.API.Repository
{    
    public class MusicRepository
    {
        private readonly MusicContext Context;

        public MusicRepository(MusicContext context)
        {
            Context = context;
        }

        public async Task<IList<Music>> GetMusics()
            => await Context.Musics.ToListAsync();

        public async Task<Music> GetMusicByIdAsync(Guid id)
           => await Context.Musics.Where(x => x.Id == id).FirstOrDefaultAsync();

        public async Task DeleteAsync(Music entity)
        {
            Context.Musics.Remove(entity);
            await Context.SaveChangesAsync();
        }
        public async Task CreateAsync(Music album)
        {
            await Context.Musics.AddAsync(album);
            await Context.SaveChangesAsync();
        }

    }
}
