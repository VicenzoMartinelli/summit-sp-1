using System.Collections.Generic;
using SummitApi.Domain;

namespace SummitApi.Repository
{
    public class UsuarioEmMemoriaRepository : IUsuarioRepository
  {
        private readonly List<Usuario> users; 

        public UsuarioEmMemoriaRepository()
        {
            users = new List<Usuario>(){
                new Usuario(){
                    ApelidoFofo = "1"
                },
                new Usuario(){
                    ApelidoFofo = "2"
                },
                new Usuario(){
                    ApelidoFofo = "3"
                },
            };
        }
        public List<Usuario> ObterTodos()
        {
            return users;
        }
    }
}