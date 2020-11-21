using CodersAcademy.API.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodersAcademy.API.Repository
{
    public class AlbumRepository
    {
        private MusicContext Context { get; set; }        

        public AlbumRepository(MusicContext context)
        {
            Context = context;
        }

        public async Task<IList<Album>> GetAllAsync()
            => await Context.Albuns.Include(x => x.Musics).ToListAsync();

        public async Task<Album> GetAlbumByIdAsync(Guid id)
            => await Context.Albuns.Include(x => x.Musics).Where(x => x.Id == id).FirstOrDefaultAsync();

        public async Task DeleteAsync(Album entity)
        {
            Context.Albuns.Remove(entity);
            await Context.SaveChangesAsync();
        }
        public async Task CreateAsync(Album album)
        {
            await Context.Albuns.AddAsync(album);
            await Context.SaveChangesAsync();
        }
    }
}
