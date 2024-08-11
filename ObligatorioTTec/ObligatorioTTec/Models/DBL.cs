using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ObligatorioTTec.Models
{
    public class DBL
    {
        string path;
        private  SQLiteAsyncConnection _database;
        public DBL(string Path)
        {
            path = Path;
        }
        private void Init()
        {
            if(_database == null)
            {
                //var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CineDBL.db3");
                _database = new(path);
                _database.CreateTableAsync<Usuario>().Wait();
            }
        }
        public async Task<List<Usuario>> GetUsuarios()
        {
            Init();
            return await _database.Table<Usuario>().ToListAsync();
        }
        public Task<Usuario> IniciarSession(string correo, string password)
        {
            Init();
            var user = _database.Table<Usuario>().Where(u => u.Correo == correo && u.Password == password).FirstOrDefaultAsync();
            return user;
        }
        public async Task<bool> UsuarioExiste(Usuario usuario)
        {
            var res = _database.Table<Usuario>().Where(u => u.Correo == usuario.Correo).CountAsync();
            return res.Result > 0;
        }
        public async Task<bool> SetUsuario(Usuario usuario)
        {
            Init();
            if (!UsuarioExiste(usuario).Result)
            {
                await _database.InsertAsync(usuario);
                return true;
            }
            return false;
        }
        public Task<Usuario> GetUsuario(string correo)
        {
            var user = _database.Table<Usuario>().Where(u => u.Correo == correo).FirstOrDefaultAsync();
            return user;
        }
        public Task<int> BorrarUsuario(Usuario usuario)
        {
            return _database.DeleteAsync(usuario);
        }
    }
}
